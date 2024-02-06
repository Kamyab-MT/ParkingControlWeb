using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class Transaction
    {

        [Key]
        public string Id { get; set; }
        public int Amount { get; set; }
        public string CardNumber { get; set; }
        public string OwnerName { get; set; }
        public string TrackingCode { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }

        [ForeignKey("Parking")]
        public string ParkingId { get; set; }
        public Parking Parking { get; set; }

        [ForeignKey("Car")]
        public string CarId { get; set; }
        public Car Car { get; set; }

        public DateTime DateCreated { get; set; }
        public string Username { get; set; }

    }
}
