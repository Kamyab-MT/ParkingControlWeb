using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class Transaction
    {

        [Key]
        public string Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int Amount { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public AppUser User { get; set; }


    }
}
