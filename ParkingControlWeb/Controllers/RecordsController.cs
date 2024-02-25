using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingControlWeb.Data.Enum;
using ParkingControlWeb.Data.Extensions;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Helpers;
using ParkingControlWeb.Models;
using ParkingControlWeb.ViewModels.Add;
using ParkingControlWeb.ViewModels.List;
using ParkingControlWeb.ViewModels.Wrappers;
using Python.Runtime;

namespace ParkingControlWeb.Controllers
{
    public class RecordsController : Controller
    {

        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly IRecord _recordRepository;
        readonly IParking _parkingRepository;
        readonly ITransaction _transactionRepository;
        readonly IBallance _ballanceRepository;

        public RecordsController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IRecord recordRepository, IParking parkingRepository, ITransaction transactionRepository, IBallance ballanceRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _recordRepository = recordRepository;
            _parkingRepository = parkingRepository;
            _transactionRepository = transactionRepository;
            _ballanceRepository = ballanceRepository;
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

            var records = await _recordRepository.GetAllPendingFromParking(parking);
            List<Record> recordsList;
/*
            if (User.IsInRole("SystemAdmin"))
                recordsList = records.ToList();
            else
                recordsList = records.Where(s => s.Creator == User.GetUserId()).ToList();*/

            recordsList = records.ToList();

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
                    PhoneNumber = Helper.ShowNumber(currentUser.UserName.Decrypt()),
                    PlateNumber = recordsList[i].PlateNumber.Decrypt(),
                    Date = recordsList[i].EntranceTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                };

                Ballance ballance = await _ballanceRepository.Get(recordsList[i].ParkingId, currentUser.Id);

                int balAmount = ballance == null ? 0 : ballance.Amount;

                if (vm.Status == -1)
                {
                    TimeSpan timePassed = recordsList[i].ExitTime.Subtract(recordsList[i].EntranceTime);
                    Expense expense = new Expense(parking.EntranceRate, parking.HourlyRate, parking.DailyRate, timePassed.Minutes, timePassed.Hours, timePassed.Days);
                    vm.IsMoneyEnough = balAmount >= Helper.CalculateExpense(expense);
                    vm.PassedTime = Helper.TimeBetween(recordsList[i].ExitTime, recordsList[i].EntranceTime);
                    vm.Ballance = Helper.DottedPriceShow(balAmount);
                    vm.Price = Helper.DottedPriceShow(Helper.CalculateExpense(expense));
                    vm.Diffrence = Helper.DottedPriceShow(Math.Abs(balAmount - Helper.CalculateExpense(expense)));
                }

                vmRecords.Add(vm);
            }

            ActiveRecordsViewModel activeRecordsViewModel = new ActiveRecordsViewModel()
            {
                ActiveRecordsList = new ActiveRecordsListViewModel() 
                { 
                    ActiveRecords = vmRecords,
                    RemToCap = parking.Capacity + " / " + parking.PlaceTaken,
                }
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
                    //TempData["Error"] = (ModelState.Values.SelectMany(v => v.Errors).ToList()[0].ErrorMessage.ToString());
					return RedirectToAction("Index", "Records");
                }
            }

            AppUser user = await _userManager.FindByIdAsync(User.GetUserId());
            Parking parking = await _parkingRepository.GetById(user.ParkingId);

            bool additive = false;

            if (parking.PlaceTaken >= parking.Capacity)
                additive = true;

            string PlateNumber = recordViewModel.AddRecord.PlateNumber2 + recordViewModel.AddRecord.PlateNumber1 + recordViewModel.AddRecord.PlateNumber3 + recordViewModel.AddRecord.PlateNumber4;

            var rec = await _recordRepository.GetAllActiveFromParking(parking);
            var plate = rec.FirstOrDefault(s=> s.PlateNumber == PlateNumber.Encrypt());

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

                record.Creator = User.GetUserId();
                record.Username = newUser.UserName;

                parking.PlaceTaken++;

                _recordRepository.Add(record);

                if(!additive)
                    TempData["Success"] = "ورود ماشین با موفقیت ثبت شد";
                else
                    TempData["Info"] = "ورود ماشین مازاد بر ظرفیت پارکینگ با موفقیت ثبت شد";

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
            Parking parking = await _parkingRepository.GetById(record.ParkingId);

            parking.PlaceTaken--;

            record.ExitTime = DateTime.Now;
            record.Status = -1;

            _recordRepository.Save();

            return RedirectToAction("Index", "Records");
        }

        public IActionResult ChargeDriverModal()
        {
            return PartialView("_ChargeDriver");
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
                    Username = user.UserName,
                };

                Ballance ballance = await _ballanceRepository.Get(record.ParkingId, user.Id);

                ballance.Amount += amount;
                
                var result = _ballanceRepository.Update(ballance);
                var transResult = _transactionRepository.Add(transaction);

                if (result && transResult)
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

            Ballance ballance = await _ballanceRepository.Get(parking.Id, user.Id);

            if (expenseAmount <= ballance.Amount)
            {
                ballance.Amount -= expenseAmount;
                _ballanceRepository.Update(ballance);

                record.Status = 1;
                
                _recordRepository.Save();

                TempData["Success"] = "خروج راننده با موفقیت ثبت شد";
            }
            else
                TempData["Error"] = "موجودی کاربر کافی نمی‌باشد.";

            return RedirectToAction("Index", "Records");
        }

        public void RecognizePlateNumber()
        {
            using (Py.GIL())
            {
                PythonEngine.Initialize();
                PythonEngine.Exec("print('Hello, Python!')");
                PythonEngine.Shutdown();
            }
        }

        public async Task<IActionResult> RecordsHistory()
        {
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
                TimeSpan timePassed = recordsList[i].ExitTime.Subtract(recordsList[i].EntranceTime);
                parking = await _parkingRepository.GetById(recordsList[i].ParkingId);

                Expense expense = new Expense(parking.EntranceRate, parking.HourlyRate, parking.DailyRate, timePassed.Minutes, timePassed.Hours, timePassed.Days);
                vmRecords.Add(
                new RecordsHistoryViewModel()
                {
                    
                    EntranceTime = Helper.DateShow(recordsList[i].EntranceTime),
                    PhoneNumber = Helper.ShowNumber(recordsList[i].Username.Decrypt()),
                    PlateNumber = recordsList[i].PlateNumber.Decrypt(),
                    ExitTime = Helper.DateShow(recordsList[i].ExitTime),
                    PassedTime = Helper.TimeBetween(recordsList[i].ExitTime, recordsList[i].EntranceTime),
                    Price = Helper.DottedPriceShow(Helper.CalculateExpense(expense)),
                    Parking = parking.Name.Decrypt(),
                    Date = recordsList[i].ExitTime.ToString("yyyy-MM-ddTHH:mm:ss"),
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

                AppUser newUser = new AppUser() { Id = Guid.NewGuid().ToString(), UserName = Username.Encrypt(), PhoneNumber = Username.Encrypt(), SuperiorUserId = sup, Active = 1, RegisterDate = DateTime.Now };

                var result = await _userManager.CreateAsync(newUser);

                Ballance ballance = new Ballance()
                {
                    Id = Guid.NewGuid().ToString(),
                    ParkingId = parkingId,
                    UserId = newUser.Id,
                    Amount = 0,
                    DateJoined = DateTime.Now,
                };

                _ballanceRepository.Add(ballance);

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
            else // it does exist
            {
                var conflict = await _ballanceRepository.Get(ParkingId, response.Id);

                if (conflict == null)
                {

                    Ballance ballance = new Ballance()
                    {
                        Id = Guid.NewGuid().ToString(),
                        ParkingId = ParkingId,
                        UserId = response.Id,
                        Amount = 0,
                        DateJoined = DateTime.Now,
                    };

                    _ballanceRepository.Add(ballance);
                }

                return response;

            }
        }

        [Authorize(Roles = "GlobalAdmin, SystemAdmin")]
        public async Task<IActionResult> TransactionsHistory()
        {
            List<TransactionViewModel> vm = new List<TransactionViewModel>();

            IEnumerable<Transaction> trs = null;

            if (User.IsInRole(Role.GlobalAdmin))
                trs = await _transactionRepository.GetAll();
            else
            {
                AppUser user = await _userManager.FindByIdAsync(User.GetUserId());
                trs = await _transactionRepository.GetAllFromAParking(user.ParkingId);
            }

            var list = trs.ToList();

            for (int i = 0; i < list.Count; i++)
            {

                vm.Add(new TransactionViewModel()
                {
                    Amount = Helper.DottedPriceShow(list[i].Amount) + " تومان",
                    CardNumber = Helper.ShowCardNumber(list[i].CardNumber.Decrypt()),
                    DateCreated = Helper.DateShow(list[i].DateCreated),
                    PhoneNumber = Helper.ShowNumber(list[i].Username.Decrypt()),
                    TrackingCode = list[i].TrackingCode.Decrypt(),
                    Owner = list[i].OwnerName.Decrypt(),
                    Date = list[i].DateCreated.ToString("yyyy-MM-ddTHH:mm:ss"),
                });
            }

            return View(vm);
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
            if (User.IsInRole(Role.GlobalAdmin) || User.IsInRole(Role.Driver)) return null;

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
            if (User.IsInRole(Role.GlobalAdmin) || User.IsInRole(Role.Driver)) return null;

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
