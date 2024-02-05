namespace ParkingControlWeb.ViewModels.List
{
    public class ActiveRecordsListViewModel
    {
        public List<ActiveRecordViewModel> ActiveRecords { get; set; }
    }

    public class ActiveRecordViewModel
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string PlateNumber { get; set; }
        public string EntranceTime { get; set; }
        public string ExitTime { get; set; }
        public string PassedTime { get; set; }
        public int Status { get; set; }
        public string Ballance { get; set; }
        public string Price { get; set; }
        public string Diffrence { get; set; }
        public bool IsMoneyEnough { get; set; }
    }
}