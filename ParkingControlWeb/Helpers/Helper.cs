using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using ParkingControlWeb.Models;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ParkingControlWeb.Helpers
{
    public static class Helper
    {

        static AesCryptography aesCryptography = new AesCryptography();
        static LocalDate localDate = new LocalDate();

        public static int CalculateExpense(Expense expense) => expense.Calculate();

        public static string DateShow(DateTime date) => localDate.ShowWeekDayAndMonth(date);

        public static string TimeBetween(DateTime first, DateTime second)
        {
            var sub = first.Subtract(second);
            
            if(sub.Days > 0)
                return sub.Days + "روز⠀و" + sub.Hours + "⠀ساعت";
            else if(sub.Hours > 0)
                return sub.Hours + "ساعت⠀و" + sub.Minutes + "⠀دقیقه";
            else
                return sub.Minutes + "⠀دقیقه";
        }

        public static string ShowCardNumber(string number) => number.Insert(4,"-").Insert(9,"-").Insert(14,"-");

        public static string ShowNumber(string number) => number.Insert(4, "-").Insert(8, "-");

        public static string DottedPriceShow(int number)
        {
            NumberFormatInfo numberFormat = new NumberFormatInfo
            {
                NumberGroupSeparator = ",",
                NumberGroupSizes = new[] { 3 }
            };

            string formattedNumber = number.ToString("N0", numberFormat);

            return formattedNumber;
        }

        public static string Encrypt(string data) => aesCryptography.Encrypt(data);

        public static string Decrypt(string encryptedData) => aesCryptography.Decrypt(encryptedData);
    }

    public class Expense
    {

        public int expense;

        public Expense(int enterPrice, int hourlyPrice, int dailyPrice, int minutesSpent, int hoursSpent, int daysSpent)
        {
            expense += enterPrice +
                (daysSpent * dailyPrice) + (daysSpent * 24 * hourlyPrice) +
                (hoursSpent * hourlyPrice) +
                (hourlyPrice * (minutesSpent / 60));
        }

        public int Calculate() => expense;
    }
}