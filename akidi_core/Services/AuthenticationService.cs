using BackEndServices.Models;
using BackEndServices.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndServices.Services
{
    public class AuthenticationService : BaseService
    {
        DatabaseConnector databaseConnector = new DatabaseConnector();

        AccountService accountService = new AccountService();
        public AuthenticateResponse IsValidCredentials(UserCredentials credentials)
        {
            Customer customer = new Customer();
            AuthenticateResponse authenticateResponse = new AuthenticateResponse();
            customer = GetCustomerDetailsStored(credentials.userName, credentials.userType);
            if (customer.userName == null)
            {
                authenticateResponse.code = 1000;
                authenticateResponse.message = "The user does not exist !!";
                authenticateResponse.customerInfos = customer;
                return authenticateResponse;
            }

            string hashedPassword = customer.password;
            string hashedInput = Utils.Utils.CalculateSHA256(credentials.password);

            bool isValid = string.Equals(hashedPassword, hashedInput, StringComparison.OrdinalIgnoreCase);


            // get userDetail from DB and check if valid

            //isValid = true;// byPassed

            if (isValid)
            {
                authenticateResponse.code = 1000;
                authenticateResponse.message = "Success !!";
                authenticateResponse.customerInfos = customer;
                //return customer;
            }

            if (! isValid)
            {
                authenticateResponse.code = 1010;
                authenticateResponse.message = "Wrong Password!!";
                authenticateResponse.customerInfos = null;
                //return customer;
            }

            return authenticateResponse;

        }



        public AuthenticateResponse resetUserPassword(UserPasswordEditPayload userPasswordEditPayload)
        {
            bool hasBeenUpdated = false;
            Customer customer = new Customer();
            AuthenticateResponse authenticateResponse = new AuthenticateResponse();
            customer = GetCustomerDetailsStored(userPasswordEditPayload.userName, userPasswordEditPayload.userType);
            if (customer.userName == null)
            {
                authenticateResponse.code = 1000;
                authenticateResponse.message = "The user does not exist !!";
                authenticateResponse.customerInfos = customer;
                return authenticateResponse;
            }


            string hashedInput = Utils.Utils.CalculateSHA256(userPasswordEditPayload.newPassword);

            if (userPasswordEditPayload.userType == "customer")
                hasBeenUpdated = this.updatePassword("customers", userPasswordEditPayload.userName, hashedInput);

            else if (userPasswordEditPayload.userType == "agent")
                hasBeenUpdated = this.updatePassword("agents", userPasswordEditPayload.userName, hashedInput);

            else if (userPasswordEditPayload.userType == "bank")
                hasBeenUpdated = this.updatePassword("bank", userPasswordEditPayload.userName, hashedInput);

            // get userDetail from DB and check if valid

            //isValid = true;// byPassed

            if (hasBeenUpdated)
            {
                authenticateResponse.code = 1000;
                authenticateResponse.message = "Password Updated Successfully !!";
                authenticateResponse.customerInfos = customer;
                //return customer;
            }

            

            return authenticateResponse;

        }


        private bool updatePassword(string tableName, string username,string password)
        {
            bool updateSuccessful = false;        

            using (MySqlConnection connection = new MySqlConnection(databaseConnector.connectionString))
            {
                databaseConnector.OpenConnection();
                string query = null;
                query = "update "+ tableName + " set password = '" + password + "' where userName = '" + username + "';";

                MySqlCommand command = new MySqlCommand(query, databaseConnector.connection);
                int rowsAffected = command.ExecuteNonQuery();
                databaseConnector.CloseConnection();
                if (rowsAffected > 0)
                {
                    updateSuccessful = true;
                }

            }

            return updateSuccessful;

        }





        //string connectionString = "server=localhost;database=YourDatabase;user=YourUsername;password=YourPassword";
        private Customer GetCustomerDetailsStored(string username, string userType)
        {

            Customer customer = new Customer();
            Agent agent = new Agent();


            if (databaseConnector.OpenConnection())
            {
                string query = null;
                if (userType == "customer")
                    //query = "SELECT * FROM customers where userName = '" + username+"';";
                    query = "SELECT * FROM customers a, customer_account b where a.userName = '" + username + "' AND a.customerId = b.userId AND b.accountType = 'WALLET';";
                else if (userType == "merchant")
                    query = "SELECT * FROM bank where userName = '" + username + "';";
                else if (userType == "agent")
                    query = "SELECT * FROM agents where userName = '" + username + "';";

                using (MySqlCommand command = new MySqlCommand(query, databaseConnector.connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            while (reader.Read())
                            {
                                if (userType == "customer")
                                    customer = new Customer()
                                    {
                                        userId = (int)reader["customerId"],
                                        //customerBankCode = (string)reader["customerBankCode"],
                                        customerEmail = (string)reader["customerEmail"],
                                        customerName = (string)reader["customerName"],
                                        customerPhone = (string)reader["customerPhone"],
                                        //datecreated =  reader["datecreated"],
                                        userName = (string)reader["userName"],
                                        password = (string)reader["password"],
                                        status = reader["status"].ToString(),
                                        //lastLogin = (string)reader["lastLogin"],
                                        tryCount = (int)reader["tryCount"],
                                        deviceId = (string)reader["deviceId"],
                                        current_balance = int.Parse(reader["current_balance"].ToString()),
                                        userType = "customer"
                                        //isActivated = 0,
                                    };
                                else if (userType == "agent")
                                    agent = new Agent()
                                    {
                                        userId = (int)reader["agentId"],
                                        agentBankCode = (string)reader["agentBankCode"],
                                        agentEmail = (string)reader["agentEmail"],
                                        agentName = (string)reader["agentName"],
                                        agentPhone = (string)reader["agentPhone"],
                                        //datecreated =  reader["datecreated"],
                                        userName = (string)reader["userName"],
                                        password = (string)reader["password"],
                                        status = reader["status"].ToString(),
                                        //lastLogin = (string)reader["lastLogin"],
                                        tryCount = (int)reader["tryCount"],
                                        deviceId = (string)reader["deviceId"],
                                        current_balance = int.Parse(reader["current_balance"].ToString()),
                                        userType = "agent"
                                        //isActivated = 0,
                                    };
                            }
                        }
                    }
                }

                databaseConnector.CloseConnection();
            }

            return customer;

        }


        public  string getTheCorrectDb(string userType)
        {
            switch(userType.ToLower())
            {
                case "customer":
                    return "customers";
                case "business":
                    return "bank";
                case "bank":
                    return "bank";
                case "agent":
                    return "agents";
                default:
                    return "customer";
            }

        }

        public ReturnMessage checkIfUserExists(string username , string userType = "customer")
        {
            ReturnMessage message = new ReturnMessage();
            string connectionString = databaseConnector.connectionString;

            try
            {
                using(MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string database  = getTheCorrectDb(userType);
                    string query = $"SELECT count(*) FROM {database} WHERE userName = '{username}';";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    try
                    {

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        message.code = System.Net.HttpStatusCode.OK;
                        message.returnObject = count;
                        return message;

                    }
                    catch (Exception ex)
                    {
                        message.code = System.Net.HttpStatusCode.BadRequest;
                        message.message = ex.Message;
                        return message;
                    }

                }

            }catch(Exception ex)
            {
                message.message = ex.Message;
                message.code = System.Net.HttpStatusCode.InternalServerError;
            }

            return message;
        }



        private string GetStoredHashedPassword(string username)
        {
            return Utils.Utils.CalculateSHA256("test");
        }

        //query = $"insert into customer_account(userId,accountType,accountProvider,customerName,customerBankCode,accountNumber,STATUS,bankName)
        //values(19, "MOBILE MONEY", "ORANGE MONEY", NULL, NULL, "2250709279464", "ACTIVATED", NULL)"

        private int generateUserWallet(Customer customer)
        {
            UserAccount userAccount = new UserAccount()
            {
                customerName = customer.customerName,
                userId = customer.userId,
                accountType = "WALLET",
                accountProvider = "AKIDI",
                customerBankCode = customer.customerBankCode,
                accountNumber = customer.customerPhone,
                status = "ACTIVATED",
                bankName = null,
                userType = customer.userType,
                current_balance = 0
            };

            return accountService.createWallet(userAccount);
        }




    public ReturnMessage Register(dynamic person)
        {

            ReturnMessage message = new ReturnMessage();
            int rowsAffected = 0;
            string query = null;
            if (person.userType == "customer")
            {
                
                query = $"INSERT INTO customers (customerName,customerPhone,customerEmail,customerBankCode,status,tryCount,deviceId,datecreated,lastLogin,userName,password, pinCode, saltPinData) VALUES (@customerName,@customerPhone,@customerEmail,@customerBankCode,@status,@tryCount,@deviceId,@datecreated,@lastLogin,@userName,@password, @pinCode, @saltPinData); SELECT LAST_INSERT_ID();";
                if (databaseConnector.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                    cmd.Parameters.AddWithValue("@customerName", person.customerName);
                    cmd.Parameters.AddWithValue("@customerPhone", person.customerPhone);
                    cmd.Parameters.AddWithValue("@customerEmail", person.customerEmail);
                    cmd.Parameters.AddWithValue("@customerBankCode", person.customerBankCode);
                    cmd.Parameters.AddWithValue("@status", "1");
                    cmd.Parameters.AddWithValue("@tryCount", "0");
                    cmd.Parameters.AddWithValue("@deviceId", person.deviceId);
                    cmd.Parameters.AddWithValue("@datecreated", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));//YYYY - MM - DD HH: MM: SS
                    cmd.Parameters.AddWithValue("@lastLogin", null);
                    cmd.Parameters.AddWithValue("@userName", person.userName);
                    cmd.Parameters.AddWithValue("@password", person.password);
                    cmd.Parameters.AddWithValue("@pinCode", person.hashedPin);
                    cmd.Parameters.AddWithValue("@saltPinData", person.saltedPin);
                    //cmd.Parameters.AddWithValue("@current_balance", 0);


                    try
                    {
                        rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
                        message.code = System.Net.HttpStatusCode.OK;
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Error occured during insertion : " + ex.Message);
                        message.code = System.Net.HttpStatusCode.InternalServerError;
                        message.message = ex.Message;
                        return message;
                    }
                }
            }
            else if (person.userType == "merchant")
            {
                query = $"INSERT INTO merchant (customerName,customerPhone,customerEmail,customerBankCode,status,tryCount,deviceId,datecreated,lastLogin,userName,password,current_balance) VALUES (@customerName,@customerPhone,@customerEmail,@customerBankCode,@status,@tryCount,@deviceId,@datecreated,@lastLogin,@userName,@password,@current_balance); SELECT LAST_INSERT_ID();";
                if (databaseConnector.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                    cmd.Parameters.AddWithValue("@customerName", person.customerName);
                    cmd.Parameters.AddWithValue("@customerPhone", person.customerPhone);
                    cmd.Parameters.AddWithValue("@customerEmail", person.customerEmail);
                    cmd.Parameters.AddWithValue("@customerBankCode", person.customerBankCode);
                    cmd.Parameters.AddWithValue("@status", person.status);
                    cmd.Parameters.AddWithValue("@tryCount", person.tryCount);
                    cmd.Parameters.AddWithValue("@deviceId", person.deviceId);
                    cmd.Parameters.AddWithValue("@datecreated", person.datecreated);
                    cmd.Parameters.AddWithValue("@lastLogin", person.lastLogin);
                    cmd.Parameters.AddWithValue("@userName", person.userName);
                    cmd.Parameters.AddWithValue("@password", person.password);
                    cmd.Parameters.AddWithValue("@current_balance", 0);
                    cmd.Parameters.AddWithValue("@pinCode", person.hashedPin);
                    cmd.Parameters.AddWithValue("@saltPinData", person.saltedPin);



                    try
                    {
                        rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
                        message.code = System.Net.HttpStatusCode.OK;

                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Error occured during insertion : " + ex.Message);
                        message.code = System.Net.HttpStatusCode.InternalServerError;
                        message.message = ex.Message;
                        return message;
                    }
                }
            }
            else if (person.userType == "agent")
            {
                query = $"INSERT INTO agents (agentName,agentrPhone,agentEmail,agentrBankCode,status,tryCount,deviceId,datecreated,lastLogin,userName,password,current_balance) VALUES (@agentName,@agentPhone,@agentEmail,@agentBankCode,@status,@tryCount,@deviceId,@datecreated,@lastLogin,@userName,@password,@current_balance); SELECT LAST_INSERT_ID();";
                if (databaseConnector.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                    cmd.Parameters.AddWithValue("@agentName", person.agentName);
                    cmd.Parameters.AddWithValue("@agentPhone", person.agentPhone);
                    cmd.Parameters.AddWithValue("@agentEmail", person.agentEmail);
                    cmd.Parameters.AddWithValue("@agentBankCode", person.agentBankCode);
                    cmd.Parameters.AddWithValue("@status", person.status);
                    cmd.Parameters.AddWithValue("@tryCount", person.tryCount);
                    cmd.Parameters.AddWithValue("@deviceId", person.deviceId);
                    cmd.Parameters.AddWithValue("@datecreated", person.datecreated);
                    cmd.Parameters.AddWithValue("@lastLogin", person.lastLogin);
                    cmd.Parameters.AddWithValue("@userName", person.userName);
                    cmd.Parameters.AddWithValue("@password", person.password);
                    cmd.Parameters.AddWithValue("@current_balance", 0);
                    cmd.Parameters.AddWithValue("@pinCode", person.hashedPin);
                    cmd.Parameters.AddWithValue("@saltPinData", person.saltedPin); 


                    try
                    {
                        rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
                        message.code = System.Net.HttpStatusCode.OK;
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Error occured during insertion : " + ex.Message);
                        message.code = System.Net.HttpStatusCode.InternalServerError;
                        message.message = ex.Message;
                        return message;
                    }
                }

            }



            databaseConnector.CloseConnection();


            // generate the wallet begin

            Customer customerResponse = new Customer()
            {
                userId = rowsAffected,
                customerPhone = person.customerPhone,
                userType = person.userType
            };

            int resp = this.generateUserWallet(customerResponse);

            person.userId = rowsAffected;
            person.walletId = resp;
            person.status = "ACTIVATED";

            message.returnObject = person;

            // generate the wallet end

            if (resp > 0)
                return message;
            else
            {
                message.code = System.Net.HttpStatusCode.InternalServerError;
                message.message = "WALLET CREATION FAILED";
                return message;
            }
        }
    }
    
}