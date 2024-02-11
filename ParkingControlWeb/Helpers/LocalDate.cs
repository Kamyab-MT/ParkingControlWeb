using System.Globalization;

namespace ParkingControlWeb.Helpers
{
    public class LocalDate
    {
        static string[] WeekDays =
        {
            "شنبه",
            "یکشنبه",
            "دوشنبه",
            "سه‌شنبه",
            "چهارشنبه",
            "پنجشنبه",
            "جمعه",
        };

        static string[] Month =
        {
            "فروردین",
            "اردیبهشت",
            "خرداد",
            "تیر",
            "مرداد",
            "شهریور",
            "مهر",
            "آبان",
            "آذر",
            "دی",
            "بهمن",
            "اسفند",
        };

        public string ShowFormalDateAndTime(DateTime date)
        {
            PersianCalendar persianCalendar = new PersianCalendar();
            return string.Format("{0}/{1}/{2}", persianCalendar.GetYear(date), persianCalendar.GetMonth(date), persianCalendar.GetDayOfMonth(date));
        }

        public string ShowWeekDayAndMonth(DateTime date)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            int index = (int)persianCalendar.GetDayOfWeek(date) + 1;
            if (index > 6) index = 0;
            string weekDay = WeekDays[index];

            int mIndex = persianCalendar.GetMonth(date) - 1;
            string month = Month[mIndex];

            return string.Format("{0} {1} {2} {3} - ساعت {4}:{5}", weekDay, persianCalendar.GetDayOfMonth(date), month, persianCalendar.GetYear(date), date.Hour, date.Minute);
        }

    }
}
