using ParkingControlWeb.Helpers;

namespace ParkingControlWeb.Data.Extensions
{
    public static class Utilities
    {

        public static string Decrypt(this string txt)
        {
            return Helper.Decrypt(txt);
        }

        public static string Encrypt(this string txt)
        {
            return Helper.Encrypt(txt);
        }
    }
}
