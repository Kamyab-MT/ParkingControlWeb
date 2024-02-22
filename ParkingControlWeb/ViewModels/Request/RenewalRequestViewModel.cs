using System.ComponentModel.DataAnnotations;

namespace ParkingControlWeb.ViewModels.Request
{
    public class RenewalRequestViewModel
    {
        public string Id { get; set; }

        public string CardName;
        public string CardNumber;

        public bool Pending;

        public List<RenewalVM> renewalVMs;

        public string OneMonthPrice;
        public string ThreeMonthPrice;
        public string SixMonthPrice;
        public string OneYearPrice;

        public string UntilOneMonth;
        public string UntilThreeMonth;
        public string UntilSixMonth;
        public string UntilOneYear;

        [Display(Name="بازه زمانی تمدید")]
        public string OptionSelected { get; set; }
        [Display(Name = "4 رقم آخر کارت")]
        [Required(ErrorMessage = "وارد کنید")]
        public string CardLast4Number { get; set; }
        [Display(Name = "زمان واریز")]
        [Required(ErrorMessage = "وارد کنید")]
        public string Date { get; set; }

    }

    public class RenewalVM
    {
        public string Date;
        public string Service;
        public string Status; //0_Nothing // 1_done // 2_Proccess // -1_Failed
        public string Desc;
        public DateTime DateCreated;
    }
}
