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
        public string RepeatPassword { get; set; }

    }
}
