using ParkingControlWeb.Helpers;

namespace ParkingControlWeb.Data.Extensions
{
    public static class Utilities
    {

        public static string Decrypt(this string txt)
        {
            var output = txt == "-" ? "-" : Helper.Decrypt(txt);
            return output;
        }

        public static string Encrypt(this string txt)
        {
            var output = txt == null ? "-" : Helper.Encrypt(txt);
            return output;
        }
    }
}
