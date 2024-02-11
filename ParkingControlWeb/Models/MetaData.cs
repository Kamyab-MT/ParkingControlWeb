using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.Models
{
    public class MetaData
    {

        [Key]
        public string Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
