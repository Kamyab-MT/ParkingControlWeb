using ParkingControlWeb.ViewModels.Add;
using ParkingControlWeb.ViewModels.List;

namespace ParkingControlWeb.ViewModels.Wrappers
{
    public class ActiveRecordsViewModel
    {
        public ActiveRecordsListViewModel ActiveRecordsList { get; set; }
        public AddRecordViewModel AddRecord { get; set; }
    }
}
