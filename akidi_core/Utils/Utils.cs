using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BackEndServices.Utils
{

    
    public class Utils
    {
        public static string CalculateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {

                byte[] inputBytes = Encoding.UTF8.GetBytes(input);


                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convertir le tableau d'octets en une chaîne hexadécimale
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }


        public static int SaveLog(string controller, string end_point_name, string data)
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();

            databaseConnector.OpenConnection();

            int rowsAffected = 0;
            string query = null;


            query = $"INSERT INTO app_log (controller,end_point_name,data,insert_date) VALUES (@controller,@end_point_name,@data,@insert_date)";

            MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
            cmd.Parameters.AddWithValue("@controller", controller);
            cmd.Parameters.AddWithValue("@end_point_name", end_point_name);
            cmd.Parameters.AddWithValue("@data", data);
            cmd.Parameters.AddWithValue("@insert_date", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));//YYYY - MM - DD HH: MM: SS
            


            try
            {
                rowsAffected = cmd.ExecuteNonQuery();

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error occured during insertion : " + ex.Message);
            }



            databaseConnector.CloseConnection();

            return rowsAffected;


        }

    }




}