namespace ParkingControlWeb.ViewModels.Request
{
    public class RenewalViewModel
    {
        public string Id { get; set; }
        public float OneMonth { get; set; }
        public float ThreeMonth { get; set; }
        public float SixMonth { get; set; }
        public float OneYear { get; set; }
        public string OptionSelected { get; set; }
    }
}
