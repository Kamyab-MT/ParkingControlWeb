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

        readonly UserManager<IdentityUser> _userManager;
        readonly SignInManager<IdentityUser> _signInManager;
        readonly ApplicationDbContext _context;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View("Error", new ErrorViewModel() { RequestId = "ER-1"});
        }
        
        public IActionResult Login()
        {
            LoginViewModel loginVM = new LoginViewModel();
			return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            IdentityUser user = await _userManager.FindByNameAsync(loginViewModel.UserName); // first we need to check that the user actually exist
            
            if (user != null)
            {
                //User has been found

                bool passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

                if (passwordCheck)
                {
                    //Password is correct

                    var result = _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Result == Microsoft.AspNetCore.Identity.SignInResult.Success)
                    {
						TempData["Success"] = "با موفقیت وارد شدید";
						return RedirectToAction("Index", "Home");
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

        public async Task<IActionResult> Register()
        {
            RegisterViewModel model = new RegisterViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "ورود ها نامعتبر هستند";
                return View(registerViewModel);
            }

            IdentityUser response = await _userManager.FindByNameAsync(registerViewModel.UserName);

            if (response == null) // it does not exist
            {
                IdentityUser newUser = new IdentityUser() { UserName = registerViewModel.UserName, PhoneNumber = registerViewModel.UserName };

                string Role = "Driver";


                var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);

                if (result.Succeeded) // user created successfully
                {
                    await _userManager.AddToRoleAsync(newUser, Role);
                    return RedirectToAction("Index", "Home");
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

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            TempData["Success"] = "با موفقیت از حساب کاربری خود خارج شدید";

            return RedirectToAction("Index", "Home");
        }

    }
}
