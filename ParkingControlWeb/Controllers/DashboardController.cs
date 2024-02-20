using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Enum;
using ParkingControlWeb.Data.Extensions;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Helpers;
using ParkingControlWeb.Models;
using ParkingControlWeb.ViewModels;
using ParkingControlWeb.ViewModels.Account;
using ParkingControlWeb.ViewModels.Edit;
using ParkingControlWeb.ViewModels.List;
using ParkingControlWeb.ViewModels.Request;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ParkingControlWeb.Controllers
{
    public class DashboardController : Controller
    {

        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signManager;
        readonly ApplicationDbContext _context;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IInfo _infoRepository;
        readonly IParking _parkingRepository;
        readonly IBallance _ballanceRepository;

        Role role = new Role();

        public DashboardController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IInfo infoRepository, IParking parkingRepository, IBallance ballanceRepository)
        {
            _userManager = userManager;
            _signManager = signInManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _infoRepository = infoRepository;
            _parkingRepository = parkingRepository;
            _ballanceRepository = ballanceRepository;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Driver"))
                    return RedirectToAction("Charge", "Dashboard");
                else if (User.IsInRole("GlobalAdmin"))
                    return RedirectToAction("UsersList", "Dashboard");
                else
                    return RedirectToAction("Index", "Records");
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UsersList()
        {

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

            var Subscription = await SubscriptionCheck();
            if (Subscription != null) return Subscription;

            if (!_httpContextAccessor.HttpContext.User.IsInRole("Driver") && !_httpContextAccessor.HttpContext.User.IsInRole("Expert"))
            {
                string role = _httpContextAccessor.HttpContext.User.IsInRole("GlobalAdmin") ? "SystemAdmin" : "Expert";
                var usersList = await _userManager.GetUsersInRoleAsync(role);
                var limitedList = usersList.Where(t => t.SuperiorUserId == _httpContextAccessor.HttpContext.User.GetUserId());

                List<InfoViewModel> infos = new List<InfoViewModel>();

                if (limitedList.ToList().Count > 0)
                {

                    foreach (var user in limitedList)
                    {
                        var info = await _infoRepository.GetById(user.InfoId);

                        InfoViewModel infoVM = new InfoViewModel()
                        {
                            FullName = info.FullName.Decrypt(),
                            RegisterDate = Helper.DateShow((DateTime)user.RegisterDate),
                            ExpireDate = Helper.DateShow(user.SubscriptionExpiry),
                            ParkingName = _parkingRepository.GetById(user.ParkingId).GetAwaiter().GetResult().Name.Decrypt(),
                            Username = Helper.ShowNumber(user.UserName.Decrypt()),
                            Date = ((DateTime)user.RegisterDate).ToString("yyyy-MM-ddTHH:mm:ss"),
                    };


                        infos.Add(infoVM);
                    }

                    UsersListViewModel usersListView = new UsersListViewModel()
                    {
                        Users = limitedList.ToList(),
                        Infos = infos
                    };

                    return View(usersListView);

                }

            }

            return View(null);
        }

        [Authorize()]
        public async Task<IActionResult> Register()
        {

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

            var Subscription = await SubscriptionCheck();
            if (Subscription != null) return Subscription;

            if (User.IsInRole(Role.Driver) || User.IsInRole(Role.Expert))
                return RedirectToAction("Index", "Home");

            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            if (!ModelState.IsValid)
            {

                if (User.IsInRole(Role.GlobalAdmin))
                {
                    TempData["Error"] = "ورودی ها نامعتبر هستند";
                    //TempData["Error"] = (ModelState.Values.SelectMany(v => v.Errors).ToList()[0].ErrorMessage);

                    return View(registerViewModel);
                }
                else // SystemAdmin
                {
                    if (ModelState.Values.SelectMany(v => v.Errors).ToList().Count > 8)
                    {
                        TempData["Error"] = "ورودی ها نامعتبر هستند";
                        return View(registerViewModel);
                    }
                }
            }

            IdentityUser response = await _userManager.FindByNameAsync(registerViewModel.UserName.Encrypt());

            if (response == null) // it does not exist
            {
                string infoId = Guid.NewGuid().ToString();
                string parkingId = "";

                AppUser user = null;
                if (User.IsInRole(Role.GlobalAdmin))
                    parkingId = Guid.NewGuid().ToString();
                else
                {
                    user = await _userManager.GetUserAsync(User);
                    parkingId = user.ParkingId;
                }

                AppUser newUser = new AppUser() { UserName = registerViewModel.UserName.Encrypt(), PhoneNumber = registerViewModel.UserName.Encrypt(), SuperiorUserId = User.GetUserId(), Active = 1, RegisterDate = DateTime.Now };

                if (User.IsInRole("GlobalAdmin"))
                {
                    int[] values = [1, 3, 6, 12];
                    int index = int.Parse(registerViewModel.RenewalIndex);

                    newUser.SubscriptionExpiry = DateTime.Now.AddMonths(values[index]);
                }
                else
                    newUser.SubscriptionExpiry = user.SubscriptionExpiry;

                string selectedRole = _httpContextAccessor.HttpContext.User.IsInRole(Role.GlobalAdmin) ? Role.SystemAdmin : Role.Expert;

                var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);

                if (result.Succeeded) // user created successfully
                {
                    await _userManager.AddToRoleAsync(newUser, selectedRole);

                    if (await _userManager.IsInRoleAsync(newUser, Role.SystemAdmin))
                    {

                        Parking parking = new Parking()
                        {
                            Id = parkingId,
                            Address = registerViewModel.ParkingAddress.Encrypt(),
                            City = registerViewModel.City.Encrypt(),
                            State = registerViewModel.State.Encrypt(),
                            Name = registerViewModel.ParkingName.Encrypt(),
                            DailyRate = registerViewModel.DailyRate,
                            EntranceRate = registerViewModel.EntranceRate,
                            HourlyRate = registerViewModel.HourlyRate,
                            OwnerId = newUser.Id
                        };

                        _parkingRepository.Add(parking);
                        newUser.ParkingId = parkingId;
                        _context.SaveChanges();
                    }
                    else
                    {
                        AppUser superiorUser = await _userManager.FindByIdAsync(newUser.SuperiorUserId);
                        Parking parking = await _parkingRepository.GetById(superiorUser.ParkingId);
                        newUser.ParkingId = parking.Id;
                    }

                    Info info = new Info()
                    {
                        Id = infoId,
                        FullName = registerViewModel.FullName.Encrypt(),
                        Address = registerViewModel.Address.Encrypt(),
                        NationalCode = registerViewModel.NationalCode.Encrypt(),
                        LandlineTel = registerViewModel.LandlineTel.Encrypt(),
                        UserId = newUser.Id,
                    };

                    _infoRepository.Add(info);

                    newUser.InfoId = infoId;

                    await _userManager.UpdateAsync(newUser);

                    TempData["Success"] = "کاربر جدید با موفقیت ساخته شد";

                    return RedirectToAction("UsersList", "Dashboard");
                }
                else
                {
                    TempData["Error"] = "ساخت کاربر جدید با خطا رو به رو شد";
                    return View(registerViewModel);
                }
            }
            else // already exist
            {
                TempData["Error"] = "شماره وارد شده قبلا در سیستم ثبت شده است";
                return View(registerViewModel);
            }

        }

        public async Task<IActionResult> Edit(string id)
        {

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

            var Subscription = await SubscriptionCheck();
            if (Subscription != null) return Subscription;

            var user = await _userManager.FindByIdAsync(id);
            var info = await _infoRepository.GetById(user.InfoId);
            var parking = await _parkingRepository.GetById(user.ParkingId);

            EditViewModel editVM = new EditViewModel()
            {
                Id = info.UserId,
                UserName = user.UserName.Decrypt(),
                FullName = info.FullName.Decrypt(),
                NationalCode = info.NationalCode.Decrypt(),
                LandlineTel = info.LandlineTel.Decrypt(),
                Address = info.Address.Decrypt(),
                ParkingName = parking.Name.Decrypt(),
                State = parking.State.Decrypt(),
                City = parking.City.Decrypt(),
                ParkingAddress = parking.Address.Decrypt(),
                EntranceRate = parking.EntranceRate,
                HourlyRate = parking.HourlyRate,
                DailyRate = parking.DailyRate,
                InfoId = user.InfoId,
                ParkingId = user.ParkingId
            };

            return View(editVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel editViewModel)
        {

            if (!ModelState.IsValid)
            {
                if (ModelState.Values.SelectMany(v => v.Errors).ToList().Count != 7)
                {
                    TempData["Error"] = "ورودی ها نامعتبر هستند";
                    //TempData["Error"] = (ModelState.Values.SelectMany(v => v.Errors).ToList().Count);
                    return View(editViewModel);
                }
            }

            var user = await _userManager.FindByIdAsync(editViewModel.Id);
            var info = await _infoRepository.GetById(user.InfoId);
            var parking = await _parkingRepository.GetById(user.ParkingId);

            info.FullName = editViewModel.FullName.Encrypt();
            info.Address = editViewModel.Address.Encrypt();
            info.LandlineTel = editViewModel.LandlineTel.Encrypt();
            info.NationalCode = editViewModel.NationalCode.Encrypt();

            parking.Address = editViewModel.ParkingAddress.Encrypt();
            parking.City = editViewModel.City.Encrypt();
            parking.State = editViewModel.State.Encrypt();
            parking.Name = editViewModel.ParkingName.Encrypt();
            parking.DailyRate = editViewModel.DailyRate;
            parking.EntranceRate = editViewModel.EntranceRate;
            parking.HourlyRate = editViewModel.HourlyRate;

            var conflict = await _userManager.FindByNameAsync(editViewModel.UserName.Encrypt());

            if (conflict == null || conflict.UserName == user.UserName)
            {

                if (_infoRepository.Update(info) && _parkingRepository.Update(parking))
                {
                    user.UserName = editViewModel.UserName.Encrypt();

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        TempData["Success"] = "ویرایش حساب کاربری با موفقیت انجام شد";
                        return RedirectToAction("UsersList", "Dashboard");
                    }
                }
                else
                {
                    TempData["Error"] = "ویرایش حساب کاربری با خطا رو به رو شد";
                    return View(editViewModel);
                }
            }

            TempData["Error"] = "شماره وارد شده قبلا در سیستم ثبت شده";
            return View(editViewModel);

        }

        [Authorize(Roles = "GlobalAdmin,SystemAdmin")]
        public async Task<IActionResult> AllUsers()
        {

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

            var Subscription = await SubscriptionCheck();
            if (Subscription != null) return Subscription;

            var user = await _userManager.FindByIdAsync(User.GetUserId());
            var userParking = await _parkingRepository.GetById(user.ParkingId);

            var users = await _userManager.Users.ToListAsync();
            List<UserVM> usersVM = new List<UserVM>();

            for (int i = 0; i < users.Count; i++)
            {
                var roles = await _userManager.GetRolesAsync(users[i]);
                var info = await _infoRepository.GetById(users[i].InfoId);
                var parking = await _parkingRepository.GetById(users[i].ParkingId);

                string parkingName = parking != null ? parking.Name.Decrypt() : "-";
                string name = info != null ? info.FullName.Decrypt() : "-";

                usersVM.Add(new UserVM()
                {
                    PhoneNumber = Helper.ShowNumber(users[i].UserName.Decrypt()),
                    DateJoined = Helper.DateShow((DateTime)users[i].RegisterDate),
                    Role = roles[0],
                    Name = name,
                    Parking = parkingName,
                    Date = ((DateTime)users[i].RegisterDate).ToString("yyyy-MM-ddTHH:mm:ss"),
                });
            }

            var globalAdmin = usersVM.FirstOrDefault(s => s.Role == "GlobalAdmin");
            var parkingOwners = usersVM.Where(s => s.Role == "SystemAdmin").ToList();
            var experts = usersVM.Where(s => s.Role == "Expert").ToList();
            List<UserVM> drivers = new List<UserVM>();

            List<UserVM> userVMs = new List<UserVM>();
            if (User.IsInRole(Role.GlobalAdmin))
            {
                userVMs.Add(globalAdmin);
                drivers = usersVM.Where(s => s.Role == "Driver").ToList();
            }
            else
            {
                parkingOwners = parkingOwners.Where(s => s.Parking == userParking.Name.Decrypt()).ToList();
                experts = experts.Where(s => s.Parking == userParking.Name.Decrypt()).ToList();
                
                var bal = await _ballanceRepository.GetAllFromParking(userParking.Id);
                var balList = bal.ToList();

                for (int i = 0; i < balList.Count; i++)
                {
                    AppUser u = await _userManager.FindByIdAsync(balList[i].UserId);

                    drivers.Add(new UserVM()
                    {
                        DateJoined = Helper.DateShow(balList[i].DateJoined),
                        Name = "-",
                        Parking = userParking.Name.Decrypt(),
                        PhoneNumber = Helper.ShowNumber(u.UserName.Decrypt()),
                        Role = "Driver",
                        Date = balList[i].DateJoined.ToString("yyyy-MM-dd"),
                    });
                }
            }

            userVMs.AddRange(parkingOwners);
            userVMs.AddRange(experts);
            userVMs.AddRange(drivers);

            AllUsersViewModel VM = new AllUsersViewModel()
            {
                Users = userVMs
            };

            return View(VM);
        }
        
        [Authorize(Roles = "GlobalAdmin")]
        public async Task<IActionResult> Active(string id)
        {

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

            var Subscription = await SubscriptionCheck();
            if (Subscription != null) return Subscription;

            AppUser user = await _userManager.FindByIdAsync(id);
            List<AppUser> ownedUsers = new List<AppUser>();

            if (User.IsInRole("GlobalAdmin"))
            {
                var allSubUsers = await _userManager.GetUsersInRoleAsync("Expert");
                ownedUsers = allSubUsers.Where(s => s.SuperiorUserId == user.Id).ToList();
            }
            user.Active = user.Active == 0 ? 1 : 0;
            for (int i = 0; i < ownedUsers.Count; i++)
                ownedUsers[i].Active = ownedUsers[i].Active == 0 ? 1 : 0;

            string txt = user.Active == 1 ? "فعال" : "غیر فعال";
            if (_context.SaveChanges() > 0)
                TempData["Success"] = string.Format("{0} کردن کاربر با موفقیت انجام شد", txt);
            else
                TempData["Error"] = string.Format("{0} کردن کاربر با موفقیت انجام شد", txt);

            return RedirectToAction("UsersList", "Dashboard");
        }

        public async Task<IActionResult> Delete(string id)
        {

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

            var Subscription = await SubscriptionCheck();
            if (Subscription != null) return Subscription;

            AppUser user = await _userManager.FindByIdAsync(id);

            Info info = await _infoRepository.GetById(user.InfoId);
            _infoRepository.Delete(info);

            var roles = await _userManager.GetRolesAsync(user);

            if (roles[0] == "SystemAdmin")
            {
                Parking parking = await _parkingRepository.GetById(user.ParkingId);
                if (parking != null)
                    _parkingRepository.Delete(parking);
            }

            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.DeleteAsync(user);

            return RedirectToAction("UsersList", "Dashboard");
        }

        [Authorize(Roles = "GlobalAdmin")]
        public async Task<IActionResult> Renewal(string id)
        {

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

            var Subscription = await SubscriptionCheck();
            if (Subscription != null) return Subscription;
            RenewalViewModel renewalViewModel = new RenewalViewModel();

            var firstPrice = await _context.Pricings.FirstOrDefaultAsync(s => s.Title == "OneMonth");
            var secondPrice = await _context.Pricings.FirstOrDefaultAsync(s => s.Title == "ThreeMonth");
            var thirdPrice = await _context.Pricings.FirstOrDefaultAsync(s => s.Title == "SixMonth");
            var fourthPrice = await _context.Pricings.FirstOrDefaultAsync(s => s.Title == "OneYear");

            renewalViewModel.OneMonth = firstPrice.Price;
            renewalViewModel.ThreeMonth = secondPrice.Price;
            renewalViewModel.SixMonth = thirdPrice.Price;
            renewalViewModel.OneYear = fourthPrice.Price;

            renewalViewModel.Id = id;

            return View(renewalViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Renewal(RenewalViewModel renewalViewModel)
        {
            int[] values = [1, 3, 6, 12];
            int index = int.Parse(renewalViewModel.OptionSelected);

            AppUser user = await _userManager.FindByIdAsync(renewalViewModel.Id);

            if (user.SubscriptionExpiry >= DateTime.Now)
                user.SubscriptionExpiry = user.SubscriptionExpiry.AddMonths(values[index]);
            else
                user.SubscriptionExpiry = DateTime.Now.AddMonths(values[index]);

            if (_context.SaveChanges() > 0)
            {

                TempData["Success"] = "اشتراک کاربر با موفقیت تمدید شد";
                return RedirectToAction("UsersList", "Dashboard");
            }
            else
            {
                TempData["Error"] = "تمدید اشتراک کاربر با خطا رو به رو شد";
                return View(renewalViewModel);
            }

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

                await _signManager.SignOutAsync();

                return RedirectToAction("Index", "Home");
            }

            return null;
        }

        [Authorize(Roles = "SystemAdmin")]
        public async Task<IActionResult> RenewalRequest()
        {
            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

            var Subscription = await SubscriptionCheck();
            if (Subscription != null) return Subscription;

            AppUser user = await _userManager.FindByIdAsync(User.GetUserId());

            RenewalRequestViewModel renewalViewModel = new RenewalRequestViewModel();

            var firstPrice = await _context.Pricings.FirstOrDefaultAsync(s => s.Title == "OneMonth");
            var secondPrice = await _context.Pricings.FirstOrDefaultAsync(s => s.Title == "ThreeMonth");
            var thirdPrice = await _context.Pricings.FirstOrDefaultAsync(s => s.Title == "SixMonth");
            var fourthPrice = await _context.Pricings.FirstOrDefaultAsync(s => s.Title == "OneYear");

            renewalViewModel.OneMonthPrice = Helper.DottedPriceShow(firstPrice.Price);
            renewalViewModel.ThreeMonthPrice = Helper.DottedPriceShow(secondPrice.Price);
            renewalViewModel.SixMonthPrice = Helper.DottedPriceShow(thirdPrice.Price);
            renewalViewModel.OneYearPrice = Helper.DottedPriceShow(fourthPrice.Price);

            renewalViewModel.UntilOneMonth = Helper.DateShow(user.SubscriptionExpiry.AddMonths(1));
            renewalViewModel.UntilThreeMonth = Helper.DateShow(user.SubscriptionExpiry.AddMonths(3));
            renewalViewModel.UntilSixMonth = Helper.DateShow(user.SubscriptionExpiry.AddMonths(6));
            renewalViewModel.UntilOneYear = Helper.DateShow(user.SubscriptionExpiry.AddMonths(12));

            var cardName = await _context.MetaDatas.FirstOrDefaultAsync(s => s.Key == "RenewalCardName");
            var cardNumber = await _context.MetaDatas.FirstOrDefaultAsync(s => s.Key == "RenewalCardNumber");

            var reqs = _context.RenewalRequests.Where(s => s.UserId == User.GetUserId()).ToList();
            
            for (int i = 0; i < reqs.Count; i++)
            {
                if (reqs[i].Status == 2)
                    renewalViewModel.Pending = true;
            }

            renewalViewModel.renewalVMs = new List<RenewalVM>();

            string[] serviceTexts = { "یک ماهه", "سه ماهه", "شش ماهه", "یک ساله" };

            for (int i = 0; i < reqs.Count; i++)
            {
                string statusText = "";
                
                if (reqs[i].Status == 1)
                    statusText = "موفق";
                else if (reqs[i].Status == -1)
                    statusText = "ناموفق";
                else if (reqs[i].Status == 2)
                    statusText = "درحال پردازش";

                renewalViewModel.renewalVMs.Add(new RenewalVM()
                {
                    Date = reqs[i].Time,
                    Service = serviceTexts[i],
                    Status = statusText
                });
            }

            renewalViewModel.CardName = cardName.Value.Decrypt();
            renewalViewModel.CardNumber = cardNumber.Value.Decrypt();

            renewalViewModel.Id = User.GetUserId();

            return View(renewalViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> RenewalRequest(RenewalRequestViewModel vm)
        {
            
            _context.RenewalRequests.Add(new RenewalRequest()
            {
                Id = Guid.NewGuid().ToString(),
                CardNumber = vm.CardLast4Number.Encrypt(),
                Time = vm.Date,
                ServiceIndex = int.Parse(vm.OptionSelected),
                Status = 2,
                UserId = User.GetUserId(),
                Description = "-"
            });

            var result = _context.SaveChanges() > 0;

            if (result)
                TempData["Success"] = "درخواست شما با موفقیت ارسال شد";
            else
                TempData["Error"] = "ارسال درخواست شما با خطا رو به رو شد";

            return RedirectToAction("RenewalRequest", "Dashboard");
        }

        public async Task<IActionResult> RenewalRequests()
        {
            List<RenewalRequestReceivedViewModel> vm = new List<RenewalRequestReceivedViewModel>();

            var pending = await _context.RenewalRequests.Where(s=> s.Status == 2).ToListAsync();

            string[] serviceText = { "یک ماهه", "سه ماهه", "شش ماهه", "یک ساله"};

            for (int i = 0; i < pending.Count; i++)
            {
                AppUser user = await _userManager.FindByIdAsync(pending[i].UserId);
                Info info = await _infoRepository.GetById(user.InfoId);

                vm.Add(new RenewalRequestReceivedViewModel()
                {
                    Id = pending[i].Id,
                    Card = pending[i].CardNumber.Decrypt(),
                    Service = serviceText[pending[i].ServiceIndex],
                    PhoneNumber = user.UserName.Decrypt(),
                    Name = info.FullName.Decrypt(),
                    Time = pending[i].Time,
                    Description = pending[i].Description
                });
            }

            return View(vm);
        }

        public async Task<IActionResult> AcceptRenewal(string id)
        {

            string idText = id.Substring(0,id.IndexOf('|'));
            string descText = id.Substring(id.IndexOf('|')+1);

            var req = await _context.RenewalRequests.FirstOrDefaultAsync(s => s.Id == idText);
            var user = await _userManager.FindByIdAsync(req.UserId);

            req.Description = descText;
            req.Status = 1;

            int[] months = { 1, 3, 6, 12 };

            user.SubscriptionExpiry = user.SubscriptionExpiry.AddMonths(months[req.ServiceIndex]);

            if (_context.SaveChanges() > 0)
                TempData["Success"] = "اشتراک کاربر با موفقیت تمدید شد";
            else
                TempData["Error"] = "فرآیند با خطا رو به رو شد";

            return RedirectToAction("RenewalRequests", "Dashboard");
        }

        public async Task<IActionResult> RejectRenewal(string id)
        {

            string idText = id.Substring(0, id.IndexOf('|'));
            string descText = id.Substring(id.IndexOf('|') + 1);

            var req = await _context.RenewalRequests.FirstOrDefaultAsync(s => s.Id == idText);

            req.Description = descText;
            req.Status = -1;

            if (_context.SaveChanges() > 0)
                TempData["Success"] = "درخواست کاربر رد شد";
            else
                TempData["Error"] = "فرآیند با خطا رو به رو شد";

            return RedirectToAction("RenewalRequests", "Dashboard");
        }

        public IActionResult Charge()
        {
            return View();
        }

        //___________________ Create GLobal Admin User
        /*
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            AppUser newUser = new AppUser() { UserName = registerViewModel.UserName, PhoneNumber = registerViewModel.UserName, SuperiorUserId = "None" };

            string selectedRole = "GlobalAdmin";
            
            var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (result.Succeeded) // user created successfully
            {
                await _userManager.AddToRoleAsync(newUser, selectedRole);
                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                TempData["Error"] = "ساخت کاربر جدید با خطا رو به رو شد";
                return View(registerViewModel);
            }

        }
        */

    }
}
