using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Enum;
using ParkingControlWeb.Models;
using ParkingControlWeb.ViewModels;

namespace ParkingControlWeb.Controllers
{
	public class AccountController : Controller
    {

        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;
        readonly ApplicationDbContext _context;
        readonly IHttpContextAccessor _httpContextAccessor;

        Role role = new Role();

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View("Error", new ErrorViewModel() { RequestId = "ER-1"});
        }
        
        public IActionResult Login()
        {
            if (User != null && User.Identity.IsAuthenticated) return RedirectToAction("Index", "Dashboard");

            LoginViewModel loginVM = new LoginViewModel();
			return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByNameAsync(loginViewModel.UserName); // first we need to check that the user actually exist
            
            if (user != null)
            {
                //User has been found
                if(user.Active == 0)
                {
                    TempData["Error"] = "این حساب غیر فعال شده است";
                    return View();
                }

                bool passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (passwordCheck)
                {
                    //Password is correct

                    var result = _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, true);
                    if (result.Result == Microsoft.AspNetCore.Identity.SignInResult.Success)
                    {
						TempData["Success"] = "با موفقیت وارد شدید";

                        if(User.IsInRole("Driver"))
                            return RedirectToAction("Charge", "Dashboard");
                        else if (User.IsInRole("GlobalAdmin"))
                            return RedirectToAction("UsersList", "Dashboard");
                        else
                            return RedirectToAction("Records", "Dashboard");
                    }

                    TempData["Error"] = "ورود به حساب انجام نشد، لطفا مجدد تلاش کنید";
                    return View(loginViewModel);
                }

                TempData["Error"] = "رمز عبور اشتباه است، لطفا مجدد تلاش کنید";
                return View(loginViewModel);
            }

            TempData["Error"] = "شماره همراه وارد شده در سیستم وجود ندارد";
            return View(loginViewModel);
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = "با موفقیت از حساب کاربری خود خارج شدید";

            return RedirectToAction("Index", "Home");
        }

    }
}
