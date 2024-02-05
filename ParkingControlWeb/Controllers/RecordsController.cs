using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingControlWeb.Data.Enum;
using ParkingControlWeb.Data.Extensions;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Helpers;
using ParkingControlWeb.Models;
using ParkingControlWeb.Repository;
using ParkingControlWeb.ViewModels.Add;
using ParkingControlWeb.ViewModels.List;
using ParkingControlWeb.ViewModels.Wrappers;

namespace ParkingControlWeb.Controllers
{
    public class RecordsController : Controller
    {

        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly IRecord _recordRepository;
        readonly ICar _carRepository;
        readonly IParking _parkingRepository;
        readonly ITransaction _transactionRepository;

        public RecordsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IRecord recordRepository, ICar carRepository, IParking parkingRepository, ITransaction transactionRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _recordRepository = recordRepository;
            _carRepository = carRepository;
            _parkingRepository = parkingRepository;
            _transactionRepository = transactionRepository;
        }

        [Authorize(Roles = "SystemAdmin,Expert")]
        public async Task<IActionResult> Index()
        {

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

            var Subscription = await SubscriptionCheck();
            if (Subscription != null) return Subscription;

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
                
                if (recordsList[i].Status == 1)
                    continue;

                AppUser currentUser = await _userManager.FindByIdAsync(recordsList[i].UserId);
                ActiveRecordViewModel vm = new ActiveRecordViewModel()
                {
                    Id = recordsList[i].Id,
                    EntranceTime = Helper.DateShow(recordsList[i].EntranceTime),
                    Status = recordsList[i].Status,
                    ExitTime = recordsList[i].Status == -1 ? Helper.DateShow(recordsList[i].ExitTime) : "-",
                    PassedTime = Helper.TimeBetween(DateTime.Now, recordsList[i].EntranceTime),
                    PhoneNumber = currentUser.UserName.Decrypt(),
                    PlateNumber = recordsList[i].PlateNumber.Decrypt(),
                };

                if (vm.Status == -1)
                {
                    TimeSpan timePassed = recordsList[i].ExitTime.Subtract(recordsList[i].EntranceTime);
                    Expense expense = new Expense(parking.EntranceRate, parking.HourlyRate, parking.DailyRate, timePassed.Minutes, timePassed.Hours, timePassed.Days);
                    vm.IsMoneyEnough = currentUser.Ballance >= Helper.CalculateExpense(expense);
                    vm.PassedTime = Helper.TimeBetween(recordsList[i].ExitTime, recordsList[i].EntranceTime);
                    vm.Ballance = Helper.DottedPriceShow((int)currentUser.Ballance);
                    vm.Price = Helper.DottedPriceShow(Helper.CalculateExpense(expense));
                    vm.Diffrence = Helper.DottedPriceShow(Math.Abs((int)currentUser.Ballance - Helper.CalculateExpense(expense)));
                }

                vmRecords.Add(vm);
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

            AppUser user = await _userManager.FindByIdAsync(User.GetUserId());
            Parking parking = await _parkingRepository.GetById(user.ParkingId);

            string PlateNumber = recordViewModel.AddRecord.PlateNumber2 + recordViewModel.AddRecord.PlateNumber1 + recordViewModel.AddRecord.PlateNumber3 + recordViewModel.AddRecord.PlateNumber4;

            var rec = await _recordRepository.GetAllActiveFromParking(parking);
            var plate = rec.FirstOrDefault(s=> s.PlateNumber.Decrypt() == PlateNumber);
            
            if(plate == null || (plate != null && plate.Status != 0) )
            {

                Record record = new Record();

                record.Id = Guid.NewGuid().ToString();
                record.ParkingId = user.ParkingId;
                record.EntranceTime = DateTime.Now;
                record.Status = 0;

                record.PlateNumber = PlateNumber.Encrypt();
                
                var newUser = await RegisterOrGetDriverUser(recordViewModel.AddRecord.PhoneNumber, record.ParkingId, user.Id, user.SuperiorUserId);
                
                record.UserId = newUser.Id;

                record.CarId = await RegisterOrGetUserCar(PlateNumber, record.UserId);
                record.Creator = User.GetUserId();

                _recordRepository.Add(record);

                return RedirectToAction("Index", "Records");
            }
            else
            {
                TempData["Error"] = "پلاک وارد شده در سیستم جاری موجود است،\nامکان ثبت ورود جدید وجود ندارد.";
                return RedirectToAction("Index", "Records");
            }
        }

        public async Task<IActionResult> StopARecord(string id)
        {

            Record record = await _recordRepository.GetAsync(id);

            record.ExitTime = DateTime.Now;
            record.Status = -1;

            _recordRepository.Save();

            return RedirectToAction("Index", "Records");
        }

        public IActionResult ChargeDriver(string id)
        {
            ChargeDriverViewModel vm = new ChargeDriverViewModel();
            vm.DriverId = id;
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ChargeDriver(ChargeDriverViewModel vm)
        {
            Record record = await _recordRepository.GetAsync(vm.DriverId);
            AppUser user = await _userManager.FindByIdAsync(record.UserId);

            string right = vm.Amount.Replace(",", "");
            int amount = int.Parse(right);

            if(amount >= 1000 && amount <= 10000000)
            {

                Transaction transaction = new Transaction()
                {
                    Id = Guid.NewGuid().ToString(),
                    Amount = amount,
                    CardNumber = vm.CardNumber.Encrypt(),
                    DateCreated = DateTime.Now,
                    OwnerName = vm.OwnerName.Encrypt(),
                    TrackingCode = vm.TrackingCode.Encrypt(),
                    UserId = user.Id,
                    ParkingId = record.ParkingId,
                    CarId = record.CarId,
                };

                user.Ballance += amount;
                var result = await _userManager.UpdateAsync(user);
                var transResult = _transactionRepository.Add(transaction);

                if (result.Succeeded && transResult)
                    TempData["Success"] = "حساب کاربر با موفقیت شارژ شد";
                else
                    TempData["Error"] = "شارژ کردن حساب کاربر با\nخطا رو به رو شد";

                return RedirectToAction("Index", "Records");
            }
            else
            {
                TempData["Error"] = "مبلغ شارژ باید بین 10 هزار تومان و\n10 میلیون تومان باشد";
                return View(vm);
            }
        }

        public async Task<IActionResult> FinalizeARecord(string id)
        {
            Record record = await _recordRepository.GetAsync(id);
            Parking parking = await _parkingRepository.GetById(record.ParkingId);
            AppUser user = await _userManager.FindByIdAsync(record.UserId);

            TimeSpan timePassed = record.ExitTime.Subtract(record.EntranceTime);

            Expense expense = new Expense(parking.EntranceRate, parking.HourlyRate, parking.DailyRate, timePassed.Minutes, timePassed.Hours, timePassed.Days);

            int expenseAmount = Helper.CalculateExpense(expense);

            if (expenseAmount <= user.Ballance)
            {
                user.Ballance -= expenseAmount;
                record.Status = 1;
                
                _userManager.UpdateAsync(user);
                _recordRepository.Save();
            }
            else
                TempData["Error"] = "موجودی کاربر کافی نمی‌باشد.";

            return RedirectToAction("Index", "Records");
        }



        public async Task<IActionResult> RecordsHistory()
        {

            if (User.IsInRole(Role.GlobalAdmin))
                return RedirectToAction("Index", "Dashboard");

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

            var Subscription = await SubscriptionCheck();
            if (Subscription != null) return Subscription;

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
                TimeSpan timePassed = recordsList[i].ExitTime.Subtract(recordsList[i].EntranceTime);
                Expense expense = new Expense(parking.EntranceRate, parking.HourlyRate, parking.DailyRate, timePassed.Minutes, timePassed.Hours, timePassed.Days);
                vmRecords.Add(
                new RecordsHistoryViewModel()
                {
                    
                    EntranceTime = Helper.DateShow(recordsList[i].EntranceTime),
                    PhoneNumber = currentUser.UserName.Decrypt(),
                    PlateNumber = recordsList[i].PlateNumber.Decrypt(),
                    ExitTime = Helper.DateShow(recordsList[i].ExitTime),
                    PassedTime = Helper.TimeBetween(recordsList[i].ExitTime, recordsList[i].EntranceTime),
                    Price = Helper.DottedPriceShow(Helper.CalculateExpense(expense))
                });
            }

            return View(vmRecords);
        }

        public async Task<AppUser> RegisterOrGetDriverUser(string Username, string ParkingId, string UserId, string SuperiorId)
        {

            var response = await _userManager.FindByNameAsync(Username.Encrypt());

            if (response == null) // it does not exist
            {
                string parkingId = ParkingId;
                string sup = User.IsInRole("SystemAdmin") ? UserId : SuperiorId;
                AppUser user = await _userManager.FindByIdAsync(User.GetUserId());

                AppUser newUser = new AppUser() { Id = Guid.NewGuid().ToString(), UserName = Username.Encrypt(), PhoneNumber = Username.Encrypt(), SuperiorUserId = sup, Active = 1, ParkingId = parkingId, SubscriptionExpiry = user.SubscriptionExpiry, RegisterDate = DateTime.Now };

                var result = await _userManager.CreateAsync(newUser);

                if (result.Succeeded) // user created successfully
                {
                    await _userManager.AddToRoleAsync(newUser, Role.Driver);
                    return newUser;
                }
                else
                {
                    TempData["Error"] = "ثبت ورود با خطا مواجه شد";
                    return null;
                }
            }
            else // already exist
                return response;
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


        public async Task<IActionResult> SessionCheck()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Error"] = "از حساب خود خارج شدید، لطفا مجدد وارد شوید";
                return RedirectToAction("Index", "Home");
            }

            return null;
        }

        public async Task<IActionResult> ActivityCheck()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user.Active == 0)
            {
                TempData["Error"] = "این حساب غیر فعال شده است";
                return RedirectToAction("Index", "Home");
            }

            return null;
        }

        public async Task<IActionResult> SubscriptionCheck()
        {
            if (User.IsInRole(Role.GlobalAdmin)) return null;

            var user = await _userManager.GetUserAsync(User);

            if (user.SubscriptionExpiry < DateTime.Now)
            {
                if (User.IsInRole(Role.Driver))
                    TempData["Error"] = "این پارکینگ در حال حاضر\nبه سامانه دسترسی ندارد";
                else
                    TempData["Error"] = "اشتراک این پارکینگ به اتمام رسیده\nجهت تمدید با پشتیبانی تماس بگیرید";

                await _signInManager.SignOutAsync();

                return RedirectToAction("Index", "Home");
            }

            return null;
        }
    }
}
