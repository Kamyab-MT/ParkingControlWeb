using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingControlWeb.Data.Enum;
using ParkingControlWeb.Helpers;
using ParkingControlWeb.Models;
using System.Diagnostics;

namespace ParkingControlWeb.Controllers
{
    public class HomeController : Controller
    {

        readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {

        /*  TimeSpan F = new TimeSpan(25, 30, 0);
            Console.WriteLine(F.Days * 12000);
            Console.WriteLine(F.Hours * 6000);
            Console.WriteLine(6000 * ((float)F.Minutes / 60));*/

            if (User.Identity.IsAuthenticated)
            {
                if(User.IsInRole("GlobalAdmin"))
                    return RedirectToAction("UsersList", "Dashboard");
                else if (User.IsInRole("Driver"))
                    return RedirectToAction("Charge", "Dashboard");
                else
                    return RedirectToAction("Index", "Records");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
