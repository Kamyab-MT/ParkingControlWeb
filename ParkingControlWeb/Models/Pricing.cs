using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.Models
{
    public class Pricing
    {
        [Key]
        public string Id { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
    }
}
