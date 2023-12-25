using ParkingControlWeb.Data.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public Role? Role { get; set; }
        public int Ballance { get; set; }
        public DateTime RegisterDate {  get; set; }
        [ForeignKey("Info")]
        public int InfoId { get; set; }
        public Info Info { get; set; }

    }
}
