using Microsoft.AspNetCore.Identity;

namespace ParkingControlWeb.ViewModels
{
    public class UsersListViewModel
    {
        public string Role { get; set; }
        public List<IdentityUser> Users { get; set; }
    }
}
