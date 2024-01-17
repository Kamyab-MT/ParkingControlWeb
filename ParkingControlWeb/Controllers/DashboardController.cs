using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Enum;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Models;
using ParkingControlWeb.ViewModels;
using System.Globalization;

namespace ParkingControlWeb.Controllers
{
    public class DashboardController : Controller
    {

        readonly UserManager<AppUser> _userManager;
        readonly ApplicationDbContext _context;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IInfo _infoRepository;
        readonly IParking _parkingRepository;

        Role role = new Role();

        public DashboardController(UserManager<AppUser> userManager, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IInfo infoRepository, IParking parkingRepository)
        {
            _userManager = userManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _infoRepository = infoRepository;
            _parkingRepository = parkingRepository;
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
                    return RedirectToAction("Records", "Dashboard");
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


            if (!_httpContextAccessor.HttpContext.User.IsInRole("Driver") && !_httpContextAccessor.HttpContext.User.IsInRole("Expert"))
            {
                string role = _httpContextAccessor.HttpContext.User.IsInRole("GlobalAdmin") ? "SystemAdmin" : "Expert";
                var usersList = await _userManager.GetUsersInRoleAsync(role);
                var limitedList = usersList.Where(t => t.SuperiorUserId == _httpContextAccessor.HttpContext.User.GetUserId());

                List<InfoViewModel> infos = new List<InfoViewModel>();
                PersianCalendar persianCalendar = new PersianCalendar();

                if (limitedList.ToList().Count > 0)
                {

                    foreach (var user in limitedList)
                    {
                        var info = await _infoRepository.GetById(user.InfoId);

                        InfoViewModel infoVM = new InfoViewModel()
                        {
                            FullName = info.FullName
                        };

                        infoVM.RegisterDate = string.Format("{0}/{1}/{2}", persianCalendar.GetYear((DateTime)info.RegisterDate), persianCalendar.GetMonth((DateTime)info.RegisterDate), persianCalendar.GetDayOfMonth((DateTime)info.RegisterDate));

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

        public async Task<IActionResult> Register()
        {

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;


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
                    //TempData["Error"] = (ModelState.Values.SelectMany(v => v.Errors).ToList()[2].ErrorMessage);

                    return View(registerViewModel);
                }
                else // SystemAdmin
                {
                    if (ModelState.Values.SelectMany(v => v.Errors).ToList().Count > 7)
                    {
                        TempData["Error"] = "ورودی ها نامعتبر هستند";
                        return View(registerViewModel);
                    }
                }
            }

            IdentityUser response = await _userManager.FindByNameAsync(registerViewModel.UserName);

            if (response == null) // it does not exist
            {
                string infoId = Guid.NewGuid().ToString();
                string parkingId = "";

                if (User.IsInRole(Role.GlobalAdmin))
                    parkingId = Guid.NewGuid().ToString();
                else
                {
                    var user = await _userManager.GetUserAsync(User);
                    parkingId = user.ParkingId;
                }

                AppUser newUser = new AppUser() { UserName = registerViewModel.UserName, PhoneNumber = registerViewModel.UserName, SuperiorUserId = _httpContextAccessor.HttpContext.User.GetUserId(), Active = 1, ParkingId = parkingId };

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
                            Address = registerViewModel.ParkingAddress,
                            City = registerViewModel.City,
                            State = registerViewModel.State,
                            Name = registerViewModel.ParkingName,
                            DailyRate = registerViewModel.DailyRate,
                            EntranceRate = registerViewModel.EntranceRate,
                            HourlyRate = registerViewModel.HourlyRate,
                            OwnerId = newUser.Id
                        };

                        _parkingRepository.Add(parking);

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
                        FullName = registerViewModel.FullName,
                        Address = registerViewModel.Address,
                        NationalCode = registerViewModel.NationalCode,
                        LandlineTel = registerViewModel.LandlineTel,
                        UserId = newUser.Id,
                        RegisterDate = DateTime.Now
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

            var user = await _userManager.FindByIdAsync(id);
            var info = await _infoRepository.GetById(user.InfoId);
            var parking = await _parkingRepository.GetById(user.ParkingId);

            EditViewModel editVM = new EditViewModel()
            {
                Id = info.UserId,
                UserName = user.UserName,
                FullName = info.FullName,
                NationalCode = info.NationalCode,
                LandlineTel = info.LandlineTel,
                Address = info.Address,
                ParkingName = parking.Name,
                State = parking.State,
                City = parking.City,
                ParkingAddress = parking.Address,
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

            var user = await _userManager.FindByNameAsync(editViewModel.UserName);
            var info = await _infoRepository.GetById(user.InfoId);
            var parking = await _parkingRepository.GetById(user.ParkingId);

            info.FullName = editViewModel.FullName;
            info.Address = editViewModel.Address;
            info.LandlineTel = editViewModel.LandlineTel;
            info.NationalCode = editViewModel.NationalCode;

            parking.Address = editViewModel.ParkingAddress;
            parking.City = editViewModel.City;
            parking.State = editViewModel.State;
            parking.Name = editViewModel.ParkingName;
            parking.DailyRate = editViewModel.DailyRate;
            parking.EntranceRate = editViewModel.EntranceRate;
            parking.HourlyRate = editViewModel.HourlyRate;

            if (_infoRepository.Update(info) && _parkingRepository.Update(parking))
            {
                user.UserName = editViewModel.UserName;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    TempData["Success"] = "ویرایش حساب کاربری با موفقیت انجام شد";
                    return RedirectToAction("UsersList", "Dashboard");
                }
            }

            TempData["Error"] = "ویرایش حساب کاربری با خطا رو به رو شد";
            return View(editViewModel);
        }

        public async Task<IActionResult> Active(string id)
        {

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

            AppUser user = await _userManager.FindByIdAsync(id);

            user.Active = user.Active == 0 ? 1 : 0;

            string txt = user.Active == 1 ? "فعال" : "غیر فعال";
            if (_context.SaveChanges() > 0)
                TempData["Success"] = string.Format("{0} کردن کاربر با موفقیت انجام شد", txt);
            else
                TempData["Error"] = string.Format("{0} کردن کاربر با موفقیت انجام شد", txt);

            return RedirectToAction("UsersList", "Dashboard");
        }

        public IActionResult Records()
        {
            return View();
        }

        public IActionResult Reporting()
        {
            return View();
        }

        public IActionResult Charge()
        {
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {

            var Session = await SessionCheck();
            if (Session != null) return Session;

            var Activity = await ActivityCheck();
            if (Activity != null) return Activity;

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

        [HttpPost]
        public async Task<IActionResult> AddRecord(AddRecordViewModel addRecordViewModel)
        {
            return RedirectToAction("Records", "Dashboard");
        }

        public IActionResult RecordsHistory()
        {
            return View();
        }

        //___________________ Create GLobal Admin User
        /*
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "ورودی ها نامعتبر هستند";
                return View(registerViewModel);
            }

            IdentityUser response = await _userManager.FindByNameAsync(registerViewModel.UserName);

            if (response == null) // it does not exist
            {
                AppUser newUser = new AppUser() { UserName = registerViewModel.UserName, PhoneNumber = registerViewModel.UserName, SuperiorUserId = "None" };

                string selectedRole = "Driver";

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
            else // already exist
            {
                TempData["Error"] = "شماره وارد شده قبلا در سیستم ثبت شده است";
                return View(registerViewModel);
            }

        }
        */

    }
}
