using ParkingControlWeb.Models;

namespace ParkingControlWeb.ViewModels
{
    public class UsersListViewModel
    {
        public List<AppUser> Users { get; set; }
        public List<InfoViewModel> Infos { get; set; }
    }

    public class InfoViewModel
    {
        public string FullName { get; set; }
        public string RegisterDate { get; set; }
        public string ExpireDate { get; set; }
        public string ParkingName { get; set; }
        public string Username { get; set; }

    }
}
