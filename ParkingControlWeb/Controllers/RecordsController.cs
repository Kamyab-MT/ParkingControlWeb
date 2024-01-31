using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Enum;
using ParkingControlWeb.Data.Extensions;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Helpers;
using ParkingControlWeb.Models;
using ParkingControlWeb.ViewModels.List;
using ParkingControlWeb.ViewModels.Wrappers;

namespace ParkingControlWeb.Controllers
{
    public class RecordsController : Controller
    {

        readonly ApplicationDbContext _dbContext;
        readonly UserManager<AppUser> _userManager;
        readonly IRecord _recordRepository;
        readonly ICar _carRepository;
        readonly IParking _parkingRepository;

        public RecordsController(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IRecord recordRepository, ICar carRepository, IParking parkingRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _recordRepository = recordRepository;
            _carRepository = carRepository;
            _parkingRepository = parkingRepository;
        }

        [Authorize(Roles = "SystemAdmin,Expert")]
        public async Task<IActionResult> Index()
        {

            AppUser user = await _userManager.FindByIdAsync(User.GetUserId());
            Parking parking = await _parkingRepository.GetById(user.ParkingId);

            var records = await _recordRepository.GetAllActiveFromParking(parking);
            List<Record> recordsList;

            if (User.IsInRole("SystemAdmin"))
                recordsList = records.ToList();
            else
                recordsList = records.Where(s => s.Creator == User.GetUserId()).ToList();

            List<ActiveRecordViewModel> vmRecords = new List<ActiveRecordViewModel>();

            for (int i = 0; i < recordsList.Count; i++)
            {
                AppUser currentUser = await _userManager.FindByIdAsync(recordsList[i].UserId);

                vmRecords.Add(
                new ActiveRecordViewModel()
                {
                    EntranceTime = Helper.DateShow(recordsList[i].EntranceTime),
                    Status = recordsList[i].Status,
                    PhoneNumber = currentUser.UserName,
                    PlateNumber = recordsList[i].PlateNumber,
                    IsMoneyEnough = currentUser.Ballance >= Helper.CalculateExpense(parking.EntranceRate,parking.HourlyRate,parking.DailyRate),
                });
            }

            ActiveRecordsViewModel activeRecordsViewModel = new ActiveRecordsViewModel()
            {
                ActiveRecordsList = new ActiveRecordsListViewModel() { ActiveRecords = vmRecords }
            };

            return View(activeRecordsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRecord(ActiveRecordsViewModel recordViewModel)
        {
            if (!ModelState.IsValid)
            {
                if(ModelState.Values.SelectMany(v => v.Errors).ToList().Count != 1)
                {
                    TempData["Error"] = "ورودی ها نامعتبر هستند";
                    return RedirectToAction("Index", "Records");
                }
            }

            var user = await _userManager.FindByNameAsync(recordViewModel.AddRecord.PhoneNumber.Encrypt());

            if(user == null)
            {

                Record record = new Record();

                record.Id = Guid.NewGuid().ToString();
                record.ParkingId = user.ParkingId;
                record.EntranceTime = DateTime.Now;
                record.Status = 0;

                string PlateNumber = recordViewModel.AddRecord.PlateNumber1 + recordViewModel.AddRecord.PlateNumber2 + recordViewModel.AddRecord.PlateNumber3 + recordViewModel.AddRecord.PlateNumber4;

                record.PlateNumber = PlateNumber.Encrypt();
                record.UserId = await RegisterOrGetDriverUser(recordViewModel.AddRecord.PhoneNumber, record.ParkingId, user.Id, user.SuperiorUserId);
                record.CarId = await RegisterOrGetUserCar(PlateNumber, record.UserId);
                record.Creator = User.GetUserId();

                _recordRepository.Add(record);

                return RedirectToAction("Index", "Records");
            }
            else
            {
                TempData["Error"] = "شماره وارد شده در سیستم جاری موجود است،\nامکان ثبت رکورد جدید وجود ندارد.";
                return RedirectToAction("Index", "Records");
            }
        }

        public async Task<IActionResult> RecordsHistory()
        {

            if (User.IsInRole(Role.GlobalAdmin))
                return RedirectToAction("Index", "Dashboard");

            IEnumerable<Record> records;
            AppUser user;
            Parking parking = null;

            if (User.IsInRole(Role.GlobalAdmin))
                records = await _recordRepository.GetAll();
            else
            {

                user = await _userManager.FindByIdAsync(User.GetUserId());
                parking = await _parkingRepository.GetById(user.ParkingId);

                records = await _recordRepository.GetAllCompletedFromParking(parking);
            }

            List<Record> recordsList;
            List<RecordsHistoryViewModel> vmRecords = new List<RecordsHistoryViewModel>();

            if (User.IsInRole(Role.GlobalAdmin) || User.IsInRole(Role.SystemAdmin))
                recordsList = records.ToList();
            else
                recordsList = records.Where(s => s.Creator == User.GetUserId()).ToList();

            for (int i = 0; i < recordsList.Count; i++)
            {
                AppUser currentUser = await _userManager.FindByIdAsync(recordsList[i].UserId);

                vmRecords.Add(
                new RecordsHistoryViewModel()
                {
                    
                    EntranceTime = Helper.DateShow(recordsList[i].EntranceTime),
                    PhoneNumber = currentUser.UserName.Decrypt(),
                    PlateNumber = recordsList[i].PlateNumber.Decrypt(),
                    ExitTime = Helper.DateShow(recordsList[i].ExitTime),
                    PassedTime = Helper.TimeBetween(recordsList[i].ExitTime, recordsList[i].EntranceTime),
                    Price = Helper.DottedPriceShow(Helper.CalculateExpense(parking.EntranceRate, parking.HourlyRate, parking.DailyRate))
                });
            }

            return View(vmRecords);
        }

        public async Task<string> RegisterOrGetDriverUser(string Username, string ParkingId, string UserId, string SuperiorId)
        {

            AppUser? response = await _userManager.FindByNameAsync(Username);

            if (response == null) // it does not exist
            {
                string parkingId = ParkingId;
                string sup = User.IsInRole("SystemAdmin") ? UserId : SuperiorId;
                AppUser user = await _userManager.FindByIdAsync(User.GetUserId());

                AppUser newUser = new AppUser() { Id = Guid.NewGuid().ToString(), UserName = Username.Encrypt(), PhoneNumber = Username.Encrypt(), SuperiorUserId = sup, Active = 1, ParkingId = parkingId, SubscriptionExpiry = user.SubscriptionExpiry };

                var result = await _userManager.CreateAsync(newUser);

                if (result.Succeeded) // user created successfully
                {
                    await _userManager.AddToRoleAsync(newUser, "Driver");
                    return newUser.Id;
                }
                else
                {
                    TempData["Error"] = "ثبت ورود با خطا مواجه شد";
                    return null;
                }
            }
            else // already exist
                return response.Id;
        }

        public async Task<string> RegisterOrGetUserCar(string PlateNumber, string UserId)
        {
            Car response = await _carRepository.GetByPlateNumber(PlateNumber);

            if (response == null)
            {
                Car car = new Car()
                {
                    Id = Guid.NewGuid().ToString(),
                    PlateNumber = PlateNumber.Encrypt(),
                    VisitCount = 1,
                    OwnerId = UserId
                };

                _carRepository.Add(car);
                return car.Id;
            }
            else
            {
                _carRepository.AddVisitCountToCar(response);
                return response.Id;
            }
        }

        public IActionResult TransactionsHistory()
        {
            return View();
        }
    }
}
