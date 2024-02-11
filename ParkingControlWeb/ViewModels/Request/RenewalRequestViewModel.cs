namespace ParkingControlWeb.ViewModels.Request
{
    public class RenewalRequestViewModel
    {
        public string Id { get; set; }
        
        public string OneMonthPrice { get; set; }
        public string ThreeMonthPrice { get; set; }
        public string SixMonthPrice { get; set; }
        public string OneYearPrice { get; set; }

        public string UntilOneMonth { get; set; }
        public string UntilThreeMonth { get; set; }
        public string UntilSixMonth { get; set; }
        public string UntilOneYear { get; set; }
        
        public string OptionSelected { get; set; }
    }
}
