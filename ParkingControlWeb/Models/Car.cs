using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class Car
    {
        [Key]
        public string? Id { get; set; }
        public string? Model { get; set; }
        public string? PlateNumber { get; set; }
        public int VisitCount { get; set; }
        [ForeignKey("Owner")]
        public string? OwnerId { get; set; }
        public IdentityUser? Owner { get; set; }
    }
}
