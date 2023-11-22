using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BackEndServices.Models;
using BackEndServices.Utils;
using MySql.Data.MySqlClient;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace BackEndServices.Services
{
    public class PinHashingService
    {
        private static string GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        private static string HashPin(string pin, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(pin + salt));
                return Convert.ToBase64String(hashedBytes);
            }
        }


        public static string SavePinCode(string pin) 
        {
            // Generate a salt (random value) to add to the PIN before hashing
            string salt = GenerateSalt();
            string hashedPin = HashPin(pin, salt);
            // Store salt and hashedPin in the database
            // Ensure you have columns for salt and hashedPin in your user table
            return salt + "|" + hashedPin;


        }

        public static bool validatePin(string storedSalt, string storedhashedPin, string inputtedPin) {
            string inputHashedPin = HashPin(inputtedPin, storedSalt);
            // Compare inputHashedPin with storedHashedPin
            bool isValidPin = inputHashedPin == storedhashedPin;
            return isValidPin;

        
        }
       

    }
}