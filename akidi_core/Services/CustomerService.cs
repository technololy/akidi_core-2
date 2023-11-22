using BackEndServices.Models;
using BackEndServices.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndServices.Services
{
    public class CustomerService : BaseService
    {
        DatabaseConnector databaseConnector = new DatabaseConnector();
        public AuthenticateResponse IsValidCredentials(UserCredentials credentials)
        {
            Customer customer = new Customer();
            AuthenticateResponse authenticateResponse = new AuthenticateResponse();
            customer = GetCustomerDetailsStored(credentials.userName);

            string hashedPassword = customer.password;
            string hashedInput = Utils.Utils.CalculateSHA256(credentials.password);

            bool isValid = string.Equals(hashedPassword, hashedInput, StringComparison.OrdinalIgnoreCase);


            // get userDetail from DB and check if valid

           //isValid = true;// byPassed

            if (isValid)
            {
                authenticateResponse.code = 1000;
                authenticateResponse.message = "Customer registered!!";
                authenticateResponse.customerInfos = customer;
                //return customer;
            }

            return authenticateResponse;

        }

        //string connectionString = "server=localhost;database=YourDatabase;user=YourUsername;password=YourPassword";
        private Customer GetCustomerDetailsStored(string username)
        {
            string connectionString = databaseConnector.connectionString;
            Customer customer = new Customer ();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM customers where userName = "+username;

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            
                            while (reader.Read())
                            {
                                customer = new Customer()
                                {
                                    customerBankCode = (string)reader["customerBankCode"],
                                    customerEmail = (string)reader["customerEmail"],
                                    customerName = (string)reader["customerName"],
                                    customerPhone = (string)reader["customerPhone"],
                                    datecreated = (string)reader["datecreated"],
                                    userName = (string)reader["userName"],
                                    password = (string)reader["password"],
                                    status = reader["status"].ToString(),
                                    lastLogin = (string)reader["lastLogin"],
                                    tryCount = (int)reader["tryCount"],
                                    deviceId = (string)reader["deviceId"],
                                    //isActivated = 0,
                                };
                                
                            }
                        }
                    }
                }
            }

            return customer;
            
        }

        public List<Customer> FetchAllCustomers()
        {
            string connectionString = databaseConnector.connectionString;
            List<Customer> customers = new List<Customer>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM customers";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer()
                            {
                                customerBankCode = (string)reader["customerBankCode"],
                                customerEmail = (string)reader["customerEmail"],
                                customerName = (string)reader["customerName"],
                                customerPhone = (string)reader["customerPhone"],
                                datecreated = (string)reader["datecreated"],
                                userName = (string)reader["userName"],
                                password = (string)reader["password"],
                                status = reader["status"].ToString(),
                                lastLogin = (string)reader["lastLogin"],
                                tryCount = (int)reader["tryCount"],
                                deviceId = (string)reader["deviceId"],
                                //isActivated = 0,
                            };
                            customers.Add(customer);
                        }
                    }
                }
            }

            return customers;
        }


        private string GetStoredHashedPassword(string username)
        {
            return Utils.Utils.CalculateSHA256("test");
        }

        public int Register(Customer customer)
        {
            int rowsAffected = 0;
            string query = $"INSERT INTO customers (customerName,customerPhone,customerEmail,customerBankCode,status,tryCount,deviceId,datecreated,lastLogin) VALUES (@customerName,@customerPhone,@customerEmail,@customerBankCode,@status,@tryCount,@deviceId,@datecreated,@lastLogin)";

            if (databaseConnector.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                cmd.Parameters.AddWithValue("@customerName", customer.customerName);
                cmd.Parameters.AddWithValue("@customerPhone", customer.customerPhone);
                cmd.Parameters.AddWithValue("@customerEmail", customer.customerEmail);
                cmd.Parameters.AddWithValue("@customerBankCode", customer.customerBankCode);
                cmd.Parameters.AddWithValue("@status", customer.status);
                cmd.Parameters.AddWithValue("@tryCount", customer.tryCount);
                cmd.Parameters.AddWithValue("@deviceId", customer.deviceId);
                cmd.Parameters.AddWithValue("@datecreated", customer.datecreated);
                cmd.Parameters.AddWithValue("@lastLogin", customer.lastLogin);
                cmd.Parameters.AddWithValue("@userName", customer.userName);
                cmd.Parameters.AddWithValue("@password", customer.password);


                try
                {
                    rowsAffected = cmd.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error occured during insertion : " + ex.Message);
                }

                databaseConnector.CloseConnection();
            }
            return rowsAffected;
        }


    }
}