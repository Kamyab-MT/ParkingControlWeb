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

        Role role = new Role();

        public DashboardController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IInfo infoRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _infoRepository = infoRepository;
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
                UsersListViewModel usersListView = new UsersListViewModel()
                {
                    Role = role,
                    Users = limitedList.ToList()
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
                //TempData["Error"] = "ورودی ها نامعتبر هستند";
                var list = ModelState.Values.SelectMany(v => v.Errors).ToList();
                TempData["Error"] = JsonConvert.SerializeObject(list);
                return View(registerViewModel);
            }

            IdentityUser response = await _userManager.FindByNameAsync(registerViewModel.UserName);

            if (response == null) // it does not exist
            {
                AppUser newUser = new AppUser() { UserName = registerViewModel.UserName, PhoneNumber = registerViewModel.UserName , SuperiorUserId = _httpContextAccessor.HttpContext.User.GetUserId() };

                string selectedRole = _httpContextAccessor.HttpContext.User.IsInRole(Role.GlobalAdmin) ? Role.SystemAdmin : Role.Expert;

                var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);

                if (result.Succeeded) // user created successfully
                {
                    await _userManager.AddToRoleAsync(newUser, selectedRole);
                    Info info = new Info()
                    {
                        FullName = registerViewModel.FullName,
                        Address = registerViewModel.Address,
                        NationalCode = registerViewModel.NationalCode,
                        LandlineTel = registerViewModel.LandlineTel,
                        UserId = newUser.Id,
                        RegisterDate = DateTime.Now
                    };

                    _infoRepository.Add(info);

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

    }
}
