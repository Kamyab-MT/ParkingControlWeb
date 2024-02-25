using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class Parking
    {
        [Key]
        public string? Id { get; set; }
        public string? Name { get; set; }
        [ForeignKey("Owner")]
        public string? OwnerId;
        public AppUser Owner;
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public int EntranceRate { get; set; }
        public int HourlyRate { get; set; }
        public int DailyRate { get; set; }
        public int Capacity { get; set; }
        public int PlaceTaken { get; set; }
    }
}
