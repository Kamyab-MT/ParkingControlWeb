using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.ViewModels
{
	public class LoginViewModel
	{
        [Required(ErrorMessage = "ورود شماره همراه الزامی است")]
        [Display(Name ="شماره همراه")]
        [DataType(DataType.PhoneNumber)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "ورود رمز عبور الزامی است")]
        [Display(Name = "رمز عبور")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
