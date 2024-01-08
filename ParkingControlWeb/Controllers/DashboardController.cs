using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Enum;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Models;
using ParkingControlWeb.ViewModels;
using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace ParkingControlWeb.Controllers
{
    public class DashboardController : Controller
    {

        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly ApplicationDbContext _context;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly IInfo _infoRepository;
        readonly IParking _parkingRepository;
        Role role = new Role();

        public DashboardController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IInfo infoRepository, IParking parkingRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _infoRepository = infoRepository;
            _parkingRepository = parkingRepository;
        }

        public async Task<IActionResult> Index()
        {
            if(!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if(!_httpContextAccessor.HttpContext.User.IsInRole("Driver") && !_httpContextAccessor.HttpContext.User.IsInRole("Expert"))
            {
                string role = _httpContextAccessor.HttpContext.User.IsInRole("GlobalAdmin") ? "SystemAdmin" : "Expert";
                var usersList = await _userManager.GetUsersInRoleAsync(role);
                var limitedList = usersList.Where(t => t.SuperiorUserId == _httpContextAccessor.HttpContext.User.GetUserId());

                List<InfoViewModel> infos = new List<InfoViewModel>();
                PersianCalendar persianCalendar = new PersianCalendar();

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
                    Role = role,
                    Users = limitedList.ToList(),
                    Infos = infos
                };

                return View(usersListView);
            }

            return View(null);
        }

        public async Task<IActionResult> Register()
        {
            if (User.IsInRole(Role.Driver) || User.IsInRole(Role.Expert) || !User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "ورودی ها نامعتبر هستند";

                //var list = ModelState.Values.SelectMany(v => v.Errors).ToList();
                //TempData["Error"] = JsonConvert.SerializeObject(list);

                return View(registerViewModel);
            }

            IdentityUser response = await _userManager.FindByNameAsync(registerViewModel.UserName);

            if (response == null) // it does not exist
            {
                string infoId = Guid.NewGuid().ToString();

                AppUser newUser = new AppUser() { UserName = registerViewModel.UserName, PhoneNumber = registerViewModel.UserName , SuperiorUserId = _httpContextAccessor.HttpContext.User.GetUserId() , Active = 1 };

                string selectedRole = _httpContextAccessor.HttpContext.User.IsInRole(Role.GlobalAdmin) ? Role.SystemAdmin : Role.Expert;

                var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);

                if (result.Succeeded) // user created successfully
                {
                    await _userManager.AddToRoleAsync(newUser, selectedRole);

                    if(await _userManager.IsInRoleAsync(newUser, Role.SystemAdmin))
                    {

                        Parking parking = new Parking()
                        {
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
        
        public IActionResult Edit(string id)
        {
            
            return View();
        }

        public async Task<IActionResult> Active(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            user.Active = user.Active == 0 ? 1 : 0;

            string txt = user.Active == 1 ? "فعال": "غیر فعال";
            if (_context.SaveChanges() > 0)
                TempData["Success"] = string.Format("{0} کردن کاربر با موفقیت انجام شد", txt);
            else
                TempData["Error"] = string.Format("{0} کردن کاربر با موفقیت انجام شد", txt);

            return RedirectToAction("Index", "Dashboard");
        }

        public async Task<IActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);

            Info info = await _infoRepository.GetById(user.InfoId);
            _infoRepository.Delete(info);

            var roles = await _userManager.GetRolesAsync(user);
            
            if (roles[0] == "SystemAdmin")
            {
                Parking parking = await _parkingRepository.GetById(user.ParkingId);
                if(parking != null)
                    _parkingRepository.Delete(parking);
            }

            await _userManager.RemoveFromRolesAsync(user, roles);
            await _userManager.DeleteAsync(user);

            return RedirectToAction("Index", "Dashboard");
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
