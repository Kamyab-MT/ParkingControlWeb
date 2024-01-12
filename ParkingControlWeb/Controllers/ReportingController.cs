using Microsoft.AspNetCore.Mvc;

namespace ParkingControlWeb.Controllers
{
    public class ReportingController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
