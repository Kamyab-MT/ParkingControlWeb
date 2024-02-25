using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.ViewModels.Account
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "ورود شماره همراه الزامی است")]
        [Display(Name = "شماره همراه")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "ورود رمز عبور الزامی است")]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        [MinLength(5, ErrorMessage = "رمز عبور باید حداقل دارای 5 کاراکتر باشد")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "رمز عبور ها با یکدیگر همخوانی ندارند")]
        [Display(Name = "تکرار رمز عبور")]
        [Required(ErrorMessage = "تکرار رمز عبور را وارد کنید")]
        public string RepeatPassword { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "نام و نام خانوادگی خود را وارد کنید")]
        public string FullName { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "کد ملی خود را وارد کنید")]
        public string NationalCode { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "تلفن ثابت")]
        [Required(ErrorMessage = "تلفن ثابت خود را وارد کنید")]
        public string LandlineTel { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "آدرس خود را وارد کنید")]
        public string Address { get; set; }


        [DataType(DataType.Text)]
        [Display(Name = "نام پارکینگ")]
        [Required(ErrorMessage = "نام پارکینگ را وارد کنید")]
        public string ParkingName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "استان")]
        [Required(ErrorMessage = "استان را وارد کنید")]
        public string State { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "شهر")]
        [Required(ErrorMessage = "شهر را وارد کنید")]
        public string City { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "آدرس پارکینگ را وارد کنید")]
        public string ParkingAddress { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "نرخ ورودی")]
        [Range(minimum: 1000, maximum: 10000000, ErrorMessage = "رقم وارد شده باید بین 1000 تومان الی 10 میلیون تومان باشد")]
        [Required(ErrorMessage = "نرخ ورودی را وارد کنید")]
        public int EntranceRate { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "نرخ ساعتی")]
        [Range(minimum: 1000, maximum: 10000000, ErrorMessage = "رقم وارد شده باید بین 1000 تومان الی 10 میلیون تومان باشد")]
        [Required(ErrorMessage = "نرخ ساعتی را وارد کنید")]
        public int HourlyRate { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "نرخ روزانه")]
        [Range(minimum: 1000, maximum: 10000000, ErrorMessage = "رقم وارد شده باید بین 1000 تومان الی 10 میلیون تومان باشد")]
        [Required(ErrorMessage = "نرخ روزانه را وارد کنید")]
        public int DailyRate { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "ظرفیت پارکینگ")]
        [Range(minimum: 1, maximum: 1000000, ErrorMessage = "رقم وارد شده باید بین 1 تومان الی 1 میلیون تومان باشد")]
        [Required(ErrorMessage = "ظرفیت پارکینگ را وارد کنید")]
        public int Capacity { get; set; }

        [Display(Name = "تمدید اولیه")]
        public string RenewalIndex { get; set; }

    }
}
