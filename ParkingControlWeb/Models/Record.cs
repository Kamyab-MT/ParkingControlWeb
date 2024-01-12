using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class Record
    {
        [Key]
        public string Id { get; set; }

        public DateTime EntranceTime { get; set; }
        public DateTime ExitTime { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [ForeignKey("Parking")]
        public string ParkingId { get; set; }
        public Parking Pakring { get; set; }

        public int Status { get; set; }
    }
}
