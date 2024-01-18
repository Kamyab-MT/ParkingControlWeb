using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.ViewModels.Add
{
    public class AddRecordViewModel
    {
        [DisplayName("شماره همراه")]
        [Required(ErrorMessage = "شماره همراه الزامی است")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [DisplayName("شماره پلاک")]
        [Required(ErrorMessage = "شماره پلاک الزامی است")]
        public string PlateNumber { get; set; }
    }
}
