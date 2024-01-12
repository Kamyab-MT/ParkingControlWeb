using Microsoft.AspNetCore.Mvc;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Models;

namespace ParkingControlWeb.Controllers
{
    public class RecordController : Controller
    {

        readonly ApplicationDbContext _dbContext;
        readonly IRecord _recordRepository;

        public RecordController(ApplicationDbContext dbContext, IRecord recordRepository)
        {
            _dbContext = dbContext;
            _recordRepository = recordRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddNewRecord()
        {
            Record record = new Record();
            return View(record);
        }

        [HttpPost]
        public Task<IActionResult> AddNewRecord(Record record)
        {
            return null;
        }
    }
}
