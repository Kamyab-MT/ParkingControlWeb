using System.ComponentModel;

namespace ParkingControlWeb.ViewModels
{
    public class AddRecordViewModel
    {
        [DisplayName("شماره همراه")]
        public string PhoneNumber { get; set; }
        [DisplayName("مدل ماشین")]
        public string CarModel { get; set; }
        [DisplayName("شماره پلاک")]
        public string PlateNumber { get; set; }
    }
}
