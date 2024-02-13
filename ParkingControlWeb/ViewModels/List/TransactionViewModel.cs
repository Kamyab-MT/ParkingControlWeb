using ParkingControlWeb.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParkingControlWeb.ViewModels.List
{
    public class TransactionViewModel
    {
        public string PhoneNumber { get; set; }
        public string CardNumber { get; set; }
        public string Amount { get; set; }
        public string TrackingCode { get; set; }
        public string DateCreated { get; set; }
        public string Owner { get; set; }
        public string Date { get; set; }
    }
}
