using ParkingControlWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.ViewModels
{
    public class RegisterViewModel
    {

        [Required(ErrorMessage = "ورود شماره همراه الزامی است")]
        [Display(Name = "شماره همراه")]
        public string UserName{ get; set; }

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
        public string? FullName { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "کد ملی خود را وارد کنید")]
        public string? NationalCode { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "تلفن ثابت")]
        [Required(ErrorMessage = "تلفن ثابت خود را وارد کنید")]
        public string? LandlineTel { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "آدرس خود را وارد کنید")]
        public string? Address { get; set; }

    }
}
