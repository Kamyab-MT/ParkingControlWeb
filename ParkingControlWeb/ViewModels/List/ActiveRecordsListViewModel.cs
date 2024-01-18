namespace ParkingControlWeb.ViewModels.List
{
    public class ActiveRecordsListViewModel
    {
        public List<ActiveRecordViewModel> ActiveRecords { get; set; }
    }

    public class ActiveRecordViewModel
    {
        public string PhoneNumber { get; set; }
        public string PlateNumber { get; set; }
        public DateTime EntranceTime { get; set; }
        public int Status { get; set; }
        public bool IsMoneyEnough { get; set; }
    }
}