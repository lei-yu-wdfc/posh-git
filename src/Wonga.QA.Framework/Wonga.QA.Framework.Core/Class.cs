using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Wonga.QA.Framework.Core
{
    public abstract class Class
    {
        private static byte[] KEY = new byte[] { 90, 22, 35, 10, 233, 194, 5, 118, 28, 13, 153, 39, 22, 165, 23, 200, 132, 21, 17, 155, 97, 31, 179, 36, 200, 167, 34, 136, 252, 130, 119, 50 };
        private static byte[] IV = new byte[] { 18, 230, 69, 209, 5, 184, 39, 165, 27, 255, 241, 136, 83, 204, 210, 199 };
        public static string Encrypt(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                throw new ArgumentException("Input string should be defined");
            }
            string result = "";
            using (Rijndael rd = Rijndael.Create())
            {
                rd.Key = KEY;
                rd.IV = IV;
                ICryptoTransform encryptor = rd.CreateEncryptor(rd.Key, rd.IV);
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(inputString);
                        }
                        var encryptedArray = memoryStream.ToArray();
                        foreach (var b in encryptedArray)
                        {
                            result += b.ToString() + "-";
                        }
                    }
                }
            }
            return result.Remove(result.Length - 1);
        }

        public static string Decrypt(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                throw new ArgumentException("Input string should be defined");
            }
            string result = "";
            var stringArray = inputString.Split('-');
            byte[] byteArray = new byte[stringArray.Length];
            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Byte.Parse(stringArray[i]);
            }
            using (Rijndael rd = Rijndael.Create())
            {
                rd.Key = KEY;
                rd.IV = IV;

                ICryptoTransform decryptor = rd.CreateDecryptor(rd.Key, rd.IV);

                using (var memoryStream = new MemoryStream(byteArray))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            result = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return result;
        }
    }
}
