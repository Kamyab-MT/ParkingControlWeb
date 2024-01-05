using Microsoft.AspNetCore.Identity;
using ParkingControlWeb.Models;

namespace ParkingControlWeb.ViewModels
{
    public class UsersListViewModel
    {
        public string Role { get; set; }
        public List<AppUser> Users { get; set; }
        public List<InfoViewModel> Infos { get; set; }
    }
}
