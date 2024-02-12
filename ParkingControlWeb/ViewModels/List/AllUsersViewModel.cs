namespace ParkingControlWeb.ViewModels.List
{
    public class AllUsersViewModel
    {
        public List<UserVM> Users { get; set; }
    }

    public class UserVM
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Parking { get; set; }
        public string DateJoined { get; set; }
        public string Role { get; set; }
        public string Date { get; set; }
    }
}
