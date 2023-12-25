using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class Parking
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Owner")]
        public int OwnerId;
        public User Owner;
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int EntranceRate { get; set; }
        public int HourlyRate { get; set; }
        public int DailyRate { get; set; }
    }
}
