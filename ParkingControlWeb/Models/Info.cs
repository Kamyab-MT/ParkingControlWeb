using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.Models
{
    public class Info
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }
        public string NationalCode { get; set; }
        public string LandlineTel { get; set; }
        public string Address { get; set; }
    }
}
