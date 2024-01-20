using ParkingControlWeb.Models;
using System.Globalization;

namespace ParkingControlWeb.Helpers
{
    public static class Helper
    {

        public static float CalculateExpense()
        {

            return 0;
        }

        public static string DateShow(DateTime date)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            return string.Format("{3}:{4} - {0}/{1}/{2}", persianCalendar.GetYear(date), persianCalendar.GetMonth(date), persianCalendar.GetDayOfMonth(date), persianCalendar.GetHour(date), persianCalendar.GetMinute(date));
        }
    }
}
