using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.ViewModels.Add
{
    public class ChargeDriverViewModel
    {

        public string DriverId { get; set; }
        [MinLength(1000, ErrorMessage = "حداقل مبلغ شارژ 1000 تومان می‌باشد"), MaxLength(10000000, ErrorMessage = "حداکثر مبلغ شارژ 10 میلیون تومان می‌باشد")]
        [Required(ErrorMessage = "وارد کردن مبلغ الزامی است")]
        public float Amount { get; set; }
    }
}
