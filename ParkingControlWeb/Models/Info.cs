using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class Info
    {
        [Key]
        public string? Id { get; set; }
        [ForeignKey("User")]
        public string? UserId { get; set; }
        public AppUser? User { get; set; }
        public string? FullName { get; set; }
        public string? NationalCode { get; set; }
        public string? LandlineTel { get; set; }
        public string? Address { get; set; }
        public DateTime? RegisterDate { get; set; }
    }
}
