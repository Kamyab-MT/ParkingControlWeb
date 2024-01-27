using ParkingControlWeb.Models;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ParkingControlWeb.Helpers
{
    public static class Helper
    {

        static AesCryptography aesCryptography = new AesCryptography();

        public static float CalculateExpense(float entranceExpense, float hourlyExpense, float dailyExpense)
        {

            return 0;
        }

        public static string DateShow(DateTime date)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            return string.Format("{3}:{4} - {0}/{1}/{2}", persianCalendar.GetYear(date), persianCalendar.GetMonth(date), persianCalendar.GetDayOfMonth(date), persianCalendar.GetHour(date), persianCalendar.GetMinute(date));
        }

        public static string DottedPriceShow()
        {
            return null;
        }

        public static string Encrypt(string data) => aesCryptography.Encrypt(data);

        public static string Decrypt(string encryptedData) => aesCryptography.Decrypt(encryptedData);
    }
}