using ParkingControlWeb.Models;

namespace ParkingControlWeb.ViewModels
{
    public class UsersListViewModel
    {
        public List<AppUser> Users { get; set; }
        public List<InfoViewModel> Infos { get; set; }
    }
}
