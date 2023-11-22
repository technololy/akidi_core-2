using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BackEndServices.Services
{
    public static class EncryptionService
    {
        static byte[] key = ConvertBase64StringToByteArray(ConfigurationManager.AppSettings["aesAlg.Key"]);
        static byte[] iv = ConvertBase64StringToByteArray(ConfigurationManager.AppSettings["aesAlg.IV"]);
        private static string EncryptJsonData<T>(T data)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            byte[] encryptedBytes;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key ;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);
                        csEncrypt.Write(dataBytes, 0, dataBytes.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    encryptedBytes = msEncrypt.ToArray();
                }
            }

            return Convert.ToBase64String(encryptedBytes);
        }

        public static byte[] ConvertBase64StringToByteArray(string base64String)
        {
            try
            {
                byte[] byteArray = Convert.FromBase64String(base64String);
                return byteArray;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid Base64 data.");
                return null;
            }
        }


        public static T DecryptJsonDatas<T>(byte[] encryptedBytes)
        {
            //byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            string decryptedJson;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.KeySize = 256;
                aesAlg.BlockSize = 128;
                aesAlg.Padding = PaddingMode.None;
                aesAlg.Mode = CipherMode.ECB;
                //aesAlg.Mode = CipherMode.CBC;


                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decryptedJson = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return JsonConvert.DeserializeObject<T>(decryptedJson);
        }

        public static T DecryptJsonData<T>(string encryptedData)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedData);
            string decryptedJson;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                aesAlg.KeySize = 256;
                aesAlg.BlockSize = 128;
                aesAlg.Padding = PaddingMode.None;
                aesAlg.Mode = CipherMode.CBC;
                

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decryptedJson = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return JsonConvert.DeserializeObject<T>(decryptedJson);
        }



        public static T Decrypt<T> (byte[] cipherText)
        {

            string decryptedText;

            using (Aes aes = Aes.Create())
            {
                //aes.Key = key;
                //aes.IV = iv;
                //aes.Padding = PaddingMode.PKCS7;
                //aes.Mode = CipherMode.CBC; // Specify the cipher mode here

                aes.Key = key;
                aes.IV = iv;
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.None;
                aes.Mode = CipherMode.CBC;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new System.IO.MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(cipherText, 0, cipherText.Length);
                    }
                    byte[] decryptedBytes = ms.ToArray();
                    decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                }
            }

            return JsonConvert.DeserializeObject<T>(decryptedText);
        }


    }
}