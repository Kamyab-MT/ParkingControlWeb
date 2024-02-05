using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.ViewModels.Add
{
    public class ChargeDriverViewModel
    {

        public string? DriverId { get; set; }
        [DataType(DataType.Currency, ErrorMessage = "لطفا مقدار عددی وارد کنید")]
        [RegularExpression("^[0-9,]+", ErrorMessage = "لطفا مقدار عددی وارد کنید")]
        [Required(ErrorMessage = "وارد کردن مبلغ الزامی است")]
        [Display(Name = "مبلغ شارژ")]
        public string? Amount { get; set; }
        [Required(ErrorMessage = "وارد کردن شماره کارت الزامی است")]
        [DataType(DataType.CreditCard)]
        [RegularExpression("^[0-9]+$", ErrorMessage = "لطفا مقدار عددی وارد کنید")]
        [Display(Name = "شماره کارت")]
        public string? CardNumber { get; set; }
        [Required(ErrorMessage = "وارد کردن نام صاحب کارت الزامی است")]
        [DataType(DataType.Text, ErrorMessage = "مقادیر درح شده معتبر نمی‌باشند")]
        [Display(Name = "نام صاحب کارت")]
        public string? OwnerName { get; set; }
        [Display(Name = "کد رهگیری (اختیاری)")]
        [Required(ErrorMessage = "درصورتی که کد رهگیری ندارید \"-\" را وارد کنید")]
        [DataType(DataType.Text)]
        public string? TrackingCode { get; set; }
    }
}
