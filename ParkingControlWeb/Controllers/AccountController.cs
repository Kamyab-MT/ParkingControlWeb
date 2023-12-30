using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingControlWeb.Data;
using ParkingControlWeb.Models;
using ParkingControlWeb.ViewModels;

namespace ParkingControlWeb.Controllers
{
	public class AccountController : Controller
    {

        readonly UserManager<User> _userManager;
        readonly SignInManager<User> _signInManager;
        readonly ApplicationDbContext _context;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ApplicationDbContext context)
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

            User user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress); // first we need to check that the user actually exist

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
						TempData["success"] = "با موفقیت وارد شدید";
						return RedirectToAction("Index", "Home");
					}

					TempData["Error"] = "ورود به حساب انجام نشد، لطفا مجدد تلاش کنید";
                    return View(loginViewModel);
                }

                TempData["Error"] = "رمز عبور اشتباه است، لطفا مجدد تلاش کنید";
                return View(loginViewModel);
            }

            TempData["Error"] = "اطلاعات وارد شده نادرست است، لطفا مجدد تلاش کنید";
            return View(loginViewModel);
        }
	}
}
