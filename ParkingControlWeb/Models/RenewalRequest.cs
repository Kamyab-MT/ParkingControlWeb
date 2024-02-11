using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class RenewalRequest
    {

        [Key]
        public string Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public int ServiceIndex { get; set; }
        public string CardNumber { get; set; }
        public string Time { get; set; }
        public int Status { get; set; }
    }
}
