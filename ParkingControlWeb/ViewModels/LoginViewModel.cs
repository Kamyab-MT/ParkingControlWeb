using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.ViewModels
{
	public class LoginViewModel
	{
        [Required(ErrorMessage = "ورود ایمیل الزامی است")]
        [Display(Name ="ایمیل")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "ورود رمز عبور الزامی است")]
        [Display(Name = "رمز عبور")]
        public string Password { get; set; }
    }
}
