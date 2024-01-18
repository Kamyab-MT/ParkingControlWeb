using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParkingControlWeb.Data;
using ParkingControlWeb.Data.Interface;
using ParkingControlWeb.Helpers;
using ParkingControlWeb.Models;
using ParkingControlWeb.ViewModels.Add;
using ParkingControlWeb.ViewModels.List;
using ParkingControlWeb.ViewModels.Wrappers;

namespace ParkingControlWeb.Controllers
{
    public class RecordsController : Controller
    {

        readonly ApplicationDbContext _dbContext;
        readonly UserManager<AppUser> _userManager;
        readonly IRecord _recordRepository;

        public RecordsController(ApplicationDbContext dbContext, UserManager<AppUser> userManager, IRecord recordRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _recordRepository = recordRepository;
        }

        public async Task<IActionResult> Index()
        {
            Info info = new Info();
            Parking parking = new Parking();

            var records = await _recordRepository.GetAllActiveFromParking(parking);
            var recordsList = records.ToList();

            List<ActiveRecordViewModel> vmRecords = new List<ActiveRecordViewModel>();
            for (int i = 0; i < recordsList.Count; i++)
            {
                vmRecords.Add(
                new ActiveRecordViewModel()
                {
                    EntranceTime = recordsList[i].EntranceTime,
                    Status = recordsList[i].Status,
                    PhoneNumber = recordsList[i].User.PhoneNumber,
                    PlateNumber = recordsList[i].Car.PlateNumber,
                    IsMoneyEnough = recordsList[i].User.Ballance >= ExpenseCalculator.CalculateExpense(),
                });
            }

            ActiveRecordsViewModel activeRecordsViewModel = new ActiveRecordsViewModel()
            {
                ActiveRecordsList = new ActiveRecordsListViewModel() { ActiveRecords = vmRecords }
            };

            return View(activeRecordsViewModel);
        }

        public IActionResult AddNewRecord()
        {
            AddRecordViewModel record = new AddRecordViewModel();
            return View(record);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRecord(Record record)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "ورودی ها نامعتبر هستند";
                return View(record);
            }

            var user = await _userManager.FindByIdAsync(User.GetUserId());

            record.Id = Guid.NewGuid().ToString();
            record.ParkingId = user.ParkingId;
            record.EntranceTime = DateTime.Now;
            record.Status = 0;
            
            record.UserId = ""; //TODO
            record.CarId = ""; // TODO

            _recordRepository.Add(record);

            return null;
        }

        public async Task<IActionResult> RecordsHistory()
        {
            return View();
        }
    }
}
