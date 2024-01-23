using Microsoft.AspNetCore.Mvc;
using ParkingControlWeb.Helpers;
using ParkingControlWeb.Models;
using System.Diagnostics;

namespace ParkingControlWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string enc = Helper.Encrypt("09939827916");
            Console.WriteLine("\n\n" + enc);
            Console.WriteLine(Helper.Decrypt(enc));

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
