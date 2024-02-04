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
        static LocalDate localDate = new LocalDate();

        public static float CalculateExpense(Expense expense) => expense.Calculate();

        public static string DateShow(DateTime date)
        {
            return localDate.ShowWeekDayAndMonth(date);
        }

        public static string TimeBetween(DateTime first, DateTime second)
        {
            var sub = first.Subtract(second);
            
            if(sub.TotalDays > 0)
                return sub.TotalDays + "روز و " + sub.TotalHours + "ساعت";
            else
                return sub.TotalHours + " ساعت و " + sub.TotalMinutes + " دقیقه";
        }

        public static string DottedPriceShow(float number)
        {
            float price = float.Parse(number.ToString(), NumberStyles.Currency);
            return price.ToString("#,#");

            return null;
        }

        public static string Encrypt(string data) => aesCryptography.Encrypt(data);

        public static string Decrypt(string encryptedData) => aesCryptography.Decrypt(encryptedData);
    }

    public class Expense
    {

        public float expense;

        public Expense(float enterPrice, float hourlyPrice, float dailyPrice, float minutesSpent,float hoursSpent, float daysSpent)
        {
            expense += enterPrice;
        }

        public float Calculate() => expense;
    }
}