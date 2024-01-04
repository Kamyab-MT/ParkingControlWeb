using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class AppUser : IdentityUser
    {
        public string? SuperiorUserId { get; set; }
        [ForeignKey("Info")]
        public string? InfoId { get; set; }
        public Info? Info { get; set; }
    }
}
