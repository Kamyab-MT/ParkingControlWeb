using ParkingControlWeb.Models;
using System.Globalization;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ParkingControlWeb.Helpers
{
    public static class Helper
    {
        static readonly byte[] KEY = Convert.FromBase64String("b7K38UCQ3Tttrho0Mb0DFA==");
        static readonly byte[] IV = Convert.FromBase64String("/9RFXy+cqyyIM50YZAP+Ug==");

        public static float CalculateExpense()
        {

            return 0;
        }

        public static string DateShow(DateTime date)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            return string.Format("{3}:{4} - {0}/{1}/{2}", persianCalendar.GetYear(date), persianCalendar.GetMonth(date), persianCalendar.GetDayOfMonth(date), persianCalendar.GetHour(date), persianCalendar.GetMinute(date));
        }

        public static string Encrypt(string data)
        {
            byte[] cipheredText;
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(KEY, IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream,encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(data);
                        }

                        cipheredText = memoryStream.ToArray(); 
                    }
                }
            }

            return Convert.ToBase64String(cipheredText);
        }

        public static string Decrypt(string encryptedData)
        {
            using (Aes aes = Aes.Create())
            {
                ICryptoTransform encryptor = aes.CreateDecryptor(KEY, IV);

                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(encryptedData)))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }

                    }
                }
            }
        }
    }
}
