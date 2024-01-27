using System.Security.Cryptography;
using System.Text;

namespace ParkingControlWeb.Helpers
{

    public class AesCryptography
    {
        const int KeySize = 256;
        const int BlockSize = 128;

        static readonly byte[] FixedKey = Encoding.UTF8.GetBytes("00112233445566778899aabbccddeeff");
        static readonly byte[] FixedIV = Encoding.UTF8.GetBytes("0102030405060708");

        public string Encrypt(string plainText)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.KeySize = KeySize;
                aesAlg.BlockSize = BlockSize;
                aesAlg.Key = FixedKey;
                aesAlg.IV = FixedIV;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string cipherText)
        {
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.KeySize = KeySize;
                aesAlg.BlockSize = BlockSize;
                aesAlg.Key = FixedKey;
                aesAlg.IV = FixedIV;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }

}
