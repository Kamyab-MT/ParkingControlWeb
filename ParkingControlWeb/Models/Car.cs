using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public string Model { get; set; }
        public string PlateNumber { get; set; }
        public int VisitCount { get; set; }
        [ForeignKey("Owner")]
        public int OwnerId { get; set; }
        public User? Owner { get; set; }
    }
}
