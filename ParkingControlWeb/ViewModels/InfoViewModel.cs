using ParkingControlWeb.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.ViewModels
{
    public class InfoViewModel
    {
        public string FullName { get; set; }
        public string RegisterDate { get; set; }
    }
}
