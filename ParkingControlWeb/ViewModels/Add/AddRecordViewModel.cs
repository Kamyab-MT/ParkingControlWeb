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

        [Required(ErrorMessage = "وارد کنید")]
        public string PlateNumber1 { get; set; }
        [Required(ErrorMessage = "وارد کنید")]
        public string PlateNumber2 { get; set; }
        [Required(ErrorMessage = "وارد کنید")]
        public string PlateNumber3 { get; set; }
        [Required(ErrorMessage = "وارد کنید")]
        public string PlateNumber4 { get; set; }
    }
}
