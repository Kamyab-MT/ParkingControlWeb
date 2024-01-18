using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class AppUser : IdentityUser
    {
        public int Active { get; set; }
        public string? SuperiorUserId { get; set; }
        [ForeignKey("Info")]
        public string? InfoId { get; set; }
        public Info? Info { get; set; }
        [ForeignKey("Parking")]
        public string? ParkingId { get; set; }
        public Parking? Parking { get; set; }
        public DateTime SubscriptionExpiry { get; set; }
        public float Ballance { get; set; }
    }
}
