using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.Models
{
    public class Pricing
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public int Price { get; set; }
    }
}
