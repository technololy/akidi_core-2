using BackEndServices.Models;
using BackEndServices.Utils;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Extensions.Configuration;
using akidi_core.Utils;

namespace BackEndServices.Services
{
    public class AccountService : BaseService
    {
        Config config = new Config();

        DatabaseConnector databaseConnector = new DatabaseConnector();
        //Utils Utils = new Utils();

        // string connectionString;

        public AccountService()
        {
            string connectionString = databaseConnector.connectionString;
        }
        /* public List<Account> GetUserAccount(AccountFilter accountFilter)
         {
             string connectionString = databaseConnector.connectionString;

             List<Account> accounts = new List<Account>();

             using (MySqlConnection connection = new MySqlConnection(connectionString))
             {
                 connection.Open();
                 string query = null;
                 if (accountFilter.userType == "agent")
                     query = "SELECT * FROM agent_account where agentId = '" + accountFilter.userId + "';";
                 else if (accountFilter.userType == "customer")
                     query = "SELECT * FROM customer_account where customerId = '" + accountFilter.userId + "';";

                 using (MySqlCommand command = new MySqlCommand(query, connection))
                 {
                     using (MySqlDataReader reader = command.ExecuteReader())
                     {
                         if (reader.HasRows)
                         {

                             while (reader.Read())
                             {
                                 Account account = new Account();
                                 if (accountFilter.userType == "agent")
                                 {
                                     account = new Account()
                                     {
                                         userId = reader["agentId"].ToString(),
                                         name = reader["agentName"].ToString(),
                                         bankCode = reader["agentBankCode"].ToString(),
                                         AccNum = reader["agentAccNum"].ToString(),
                                         status = reader["agentStatus"].ToString(),
                                         dateCreated = reader["datecreated"].ToString(),
                                         dateUpdated =reader["dateUpdated"].ToString()

                                     };
                                 }else if (accountFilter.userType == "customer")
                                 {
                                     account = new Account()
                                     {
                                         userId = reader["customerId"].ToString(),
                                         name = reader["customerName"].ToString(),
                                         bankCode = reader["customerBankCode"].ToString(),
                                         AccNum = reader["customerAccNum"].ToString(),
                                         status = reader["customerStatus"].ToString(),
                                         dateCreated = reader["datecreated"].ToString(),
                                         dateUpdated = reader["dateUpdated"].ToString()
                                     };
                                 }

                                 accounts.Add(account);

                             }
                         }
                     }
                 }
             }

             return accounts;

         }*/



        public ReturnMessage GetAccountByBank(string userId, string bankCode, string accountType)
        {
            //string connectionString = databaseConnector.connectionString;
            string connectionString = config.configuration.GetConnectionString("akidiCnStr");
            ReturnMessage message = new ReturnMessage();
            List<Account> accounts = new List<Account>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = null;

                    if (accountType == "agent")
                        query = "SELECT * FROM agent_account WHERE agentId = '" + userId + "' AND BankCode ='" + bankCode + "';";
                    else if (accountType == "customer")
                        query = "SELECT * FROM customer_account WHERE agentId = '" + userId + "'AND BankCode ='" + bankCode + "';";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    Account account = new Account();
                                    if (accountType == "agent")
                                    {
                                        account = new Account()
                                        {
                                            userId = reader["agentId"].ToString(),
                                            customerName = reader["agentName"].ToString(),
                                            customerBankCode = reader["agentBankCode"].ToString(),
                                            accountNumber = reader["agentAccNum"].ToString(),
                                            status = reader["agentStatus"].ToString(),
                                            dateCreated = reader["datecreated"].ToString(),
                                            dateUpdated = reader["dateUpdated"].ToString()

                                        };
                                    }
                                    else if (accountType == "customer")
                                    {
                                        account = new Account()
                                        {
                                            userId = reader["customerId"].ToString(),
                                            customerName = reader["customerName"].ToString(),
                                            customerBankCode = reader["customerBankCode"].ToString(),
                                            accountNumber = reader["customerAccNum"].ToString(),
                                            status = reader["customerStatus"].ToString(),
                                            dateCreated = reader["datecreated"].ToString(),
                                            dateUpdated = reader["dateUpdated"].ToString()
                                        };
                                    }

                                    accounts.Add(account);

                                }
                            }
                        }
                    }
                }


                LogHandler.WriteLog("CALLING SERVICE --> GetAccountByBank \n RESPONSE--> GetAccountByBank \t" + JsonConvert.SerializeObject(accounts), "ACCOUNT_SERVICE", false, "ACCOUNT_SERVICE");
                message.returnObject = accounts;
                message.code = System.Net.HttpStatusCode.OK;


            }
            catch (Exception ex)
            {
                message.code = System.Net.HttpStatusCode.InternalServerError;
                message.message = ex.Message;
            }

            return message;


        }


        public ReturnMessage GetUserAccount(AccountFilter accountFilter)
        {
            string connectionString = config.configuration.GetConnectionString("akidiCnStr");
            ReturnMessage message = new ReturnMessage();
            List<Account> accounts = new List<Account>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = null;
                    LogHandler.WriteLog("CALLING SERVICE --> GetUserAccount \n " + JsonConvert.SerializeObject(accountFilter), "ACCOUNT_SERVICE", false, "ACCOUNT_SERVICE");
                    if (accountFilter.userType == "agent")
                        query = "SELECT * FROM agent_account where agentId = '" + accountFilter.userId + "';";
                    else if (accountFilter.userType == "customer")
                        query = "SELECT * FROM customer_account where customerId = '" + accountFilter.userId + "';";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {

                                while (reader.Read())
                                {
                                    Account account = new Account();
                                    if (accountFilter.userType == "agent")
                                    {
                                        account = new Account()
                                        {
                                            userId = reader["agentId"].ToString(),
                                            customerName = reader["agentName"].ToString(),
                                            customerBankCode = reader["agentBankCode"].ToString(),
                                            accountNumber = reader["agentAccNum"].ToString(),
                                            status = reader["agentStatus"].ToString(),
                                            dateCreated = reader["datecreated"].ToString(),
                                            dateUpdated = reader["dateUpdated"].ToString()

                                        };
                                    }
                                    else if (accountFilter.userType == "customer")
                                    {
                                        account = new Account()
                                        {
                                            userId = reader["customerId"].ToString(),
                                            customerName = reader["customerName"].ToString(),
                                            customerBankCode = reader["customerBankCode"].ToString(),
                                            accountNumber = reader["customerAccNum"].ToString(),
                                            status = reader["customerStatus"].ToString(),
                                            dateCreated = reader["datecreated"].ToString(),
                                            dateUpdated = reader["dateUpdated"].ToString()
                                        };
                                    }

                                    accounts.Add(account);

                                }
                            }
                        }
                    }
                }
                LogHandler.WriteLog("CALLING SERVICE --> GetUserAccount \n RESPONSE--> GetUserAccount \t" + JsonConvert.SerializeObject(accounts), "ACCOUNT_SERVICE", false, "ACCOUNT_SERVICE");
                message.returnObject = accounts;
                message.code = System.Net.HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                message.code = System.Net.HttpStatusCode.InternalServerError;
                message.message = ex.Message;
            }

            return message;


        }



        public ReturnMessage GetUserNameByAccount(string account)
        {
            string connectionString = config.configuration.GetConnectionString("akidiCnStr");
            ReturnMessage message = new ReturnMessage();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = null;
                    LogHandler.WriteLog("CALLING SERVICE --> GetUserNameByAccount \n " + JsonConvert.SerializeObject(account), "ACCOUNT_SERVICE", false, "ACCOUNT_SERVICE");
                    query = "SELECT * FROM customer_account where userId = '" + account + "' and accountType = 'WALLET';";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                message.message = reader["customerName"].ToString();
                            }
                        }
                    }
                }
            
                LogHandler.WriteLog("CALLING SERVICE --> GetUserNameByAccount \n RESPONSE--> \t" + JsonConvert.SerializeObject(account), "ACCOUNT_SERVICE", false, "ACCOUNT_SERVICE");
                
                message.code = System.Net.HttpStatusCode.OK;

            }
            catch (Exception ex)
            {
                message.code = System.Net.HttpStatusCode.InternalServerError;
                message.message = ex.Message;
            }

            return message;

        
        }



        private string GetStoredHashedPassword(string username)
        {
            return Utils.Utils.CalculateSHA256("test");
        }


        public ReturnMessage addNewAccountForCustomer(Account account)
        {
            ReturnMessage message = new ReturnMessage();
            string connectionString = config.configuration.GetConnectionString("akidiCnStr");
            try
            {
                LogHandler.WriteLog("CALLING SERVICE --> addNewAccountForCustomer \t" + JsonConvert.SerializeObject(account), "ACCOUNT_SERVICE", false, "ACCOUNT_SERVICE");
                message = isAccountAlreadyExists(account.accountNumber);
                int count = 0;
                if(message.code != System.Net.HttpStatusCode.OK)
                {
                    return message;
                }

                if(int.TryParse(message.returnObject.ToString(),out count))
                {
                    if(count > 0)
                    {
                        message.code = System.Net.HttpStatusCode.InternalServerError;
                        message.message = "This account number already exsits";
                        LogHandler.WriteLog("CALLING SERVICE --> addNewAccountForCustomer ERROR // \t" + message.message, "ACCOUNT_SERVICE", false, "ACCOUNT_SERVICE");
                        return message;
                    }

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            MySqlCommand myCommand = connection.CreateCommand();
                            myCommand.Connection = connection;
                            myCommand = getCommandForCreateUserAccount(account, myCommand);
                            myCommand.ExecuteScalar();

                            message.code = System.Net.HttpStatusCode.OK;
                            message.message = "Account added successfully";
                            LogHandler.WriteLog("CALLING SERVICE --> addNewAccountForCustomer ERROR // \t" + message.message, "ACCOUNT_SERVICE", false, "ACCOUNT_SERVICE");
                            return message;
                        }  
                        catch (Exception ex)
                        {
                            message.code = System.Net.HttpStatusCode.InternalServerError;
                            message.message = "Error got  " + ex.Message;
                        }

                        return message;

                    }


                }

                message.code = System.Net.HttpStatusCode.InternalServerError;
                message.message = "Issue got when adding user account";
                return message;

            }
            catch(Exception ex)
            {
                message.code = System.Net.HttpStatusCode.InternalServerError;
                message.message = "Error got  " + ex.Message;
                return message;

            }
           
        }


        public ReturnMessage isAccountAlreadyExists(string AccountNum)
        {
            ReturnMessage message = new ReturnMessage();
            string connectionString = config.configuration.GetConnectionString("akidiCnStr");
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT count(*) FROM customer_account where accountNumber = '" + AccountNum + "';";
                    MySqlCommand cmd = new MySqlCommand(query, connection);

                    try
                    {

                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        message.code = System.Net.HttpStatusCode.OK;
                        message.returnObject = count;
                        return message;

                    }
                    catch(Exception ex)
                    {
                        message.code = System.Net.HttpStatusCode.BadRequest;
                        message.message = ex.Message;
                    }
                   
                }
            }catch(Exception ex)
            {
                message.code = System.Net.HttpStatusCode.BadRequest;
                message.message = ex.Message;
            }

            return message;
        }


        public MySqlCommand getCommandForCreateUserAccount(Account account , MySqlCommand cmd)
        {
            cmd.CommandText =
              "INSERT INTO customer_account(userId, accountType, accountProvider, customerName, customerBankCode, AccountNumber, status, datecreated, " +
              "dateUpdated, bankName, userType"
             +
              " ) VALUES(" +
              "@userId, " +
              "@accountType, " +
              "@accountProvider, " +
              "@customerName, " +
              "@customerBankCode, " +
              "@AccountNumber, " +
              "@status, " +
              "@datecreated, " +
              "@dateUpdated, " +
              "@bankName, " +
              "@userType " +
              ")";

            cmd.Parameters.Clear();
            cmd.Parameters.Add("@userId", MySqlDbType.VarChar);
            cmd.Parameters["@userId"].Value =account.userId;

            cmd.Parameters.Add("@accountType", MySqlDbType.VarChar);
            cmd.Parameters["@accountType"].Value = account.accountType;

            cmd.Parameters.Add("@accountProvider", MySqlDbType.VarChar);
            cmd.Parameters["@accountProvider"].Value = account.accountProvider;

            cmd.Parameters.Add("@customerName", MySqlDbType.VarChar);
            cmd.Parameters["@customerName"].Value = account.customerName;

            cmd.Parameters.Add("@customerBankCode", MySqlDbType.VarChar);
            cmd.Parameters["@customerBankCode"].Value = account.customerBankCode;

            cmd.Parameters.Add("@accountNumber", MySqlDbType.VarChar);
            cmd.Parameters["@accountNumber"].Value = account.accountNumber;

            cmd.Parameters.Add("@status", MySqlDbType.VarChar);
            cmd.Parameters["@status"].Value = 1;

            cmd.Parameters.Add("@datecreated", MySqlDbType.DateTime);
            cmd.Parameters["@datecreated"].Value = DateTime.Now;

            cmd.Parameters.Add("@dateUpdated", MySqlDbType.DateTime);
            cmd.Parameters["@dateUpdated"].Value = DateTime.Now;

            cmd.Parameters.Add("@bankName", MySqlDbType.VarChar);
            cmd.Parameters["@bankName"].Value = account.bankName;

            cmd.Parameters.Add("@userType", MySqlDbType.VarChar);
            cmd.Parameters["@userType"].Value = account.userType;

            return cmd;
        }


        public int Save(dynamic person)
        {
            int rowsAffected = 0;
            string query = null;
            if (person.userType == "customer")
            {
                query = $"INSERT INTO customers (customerName,customerBankCode,status,tryCount,datecreated,lastLogin,userName,password,bankName) VALUES (@customerName,@customerBankCode,@status,@tryCount,@datecreated,@lastLogin,@userName,@password,@bankName)";
                if (databaseConnector.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                    cmd.Parameters.AddWithValue("@customerName", person.customerName);
                    cmd.Parameters.AddWithValue("@customerBankCode", person.customerBankCode);
                    cmd.Parameters.AddWithValue("@status", "1");
                    cmd.Parameters.AddWithValue("@tryCount", "0");
                    cmd.Parameters.AddWithValue("@datecreated", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));//YYYY - MM - DD HH: MM: SS
                    cmd.Parameters.AddWithValue("@lastLogin", null);
                    cmd.Parameters.AddWithValue("@userName", person.userName);
                    cmd.Parameters.AddWithValue("@password", person.password);
                    cmd.Parameters.AddWithValue("@bankName", person.bankName);


                    try
                    {
                        rowsAffected = cmd.ExecuteNonQuery();

                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Error occured during insertion : " + ex.Message);
                    }
                }
            }
            else if (person.userType == "merchant")
            {
                query = $"INSERT INTO merchant (customerName,customerBankCode,status,tryCount,datecreated,lastLogin,userName,password) VALUES (@customerName,@customerBankCode,@status,@tryCount,@datecreated,@lastLogin,@userName,@password)";
                if (databaseConnector.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                    cmd.Parameters.AddWithValue("@customerName", person.customerName);
                    cmd.Parameters.AddWithValue("@customerBankCode", person.customerBankCode);
                    cmd.Parameters.AddWithValue("@status", person.status);
                    cmd.Parameters.AddWithValue("@tryCount", person.tryCount);
                    cmd.Parameters.AddWithValue("@datecreated", person.datecreated);
                    cmd.Parameters.AddWithValue("@lastLogin", person.lastLogin);
                    cmd.Parameters.AddWithValue("@userName", person.userName);
                    cmd.Parameters.AddWithValue("@password", person.password);


                    try
                    {
                        rowsAffected = cmd.ExecuteNonQuery();

                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Error occured during insertion : " + ex.Message);
                    }
                }
            }
            else if (person.userType == "agent")
            {
                query = $"INSERT INTO agents (agentName,agentrBankCode,status,tryCount,datecreated,lastLogin,userName,password,bankName) VALUES (@agentName,@agentBankCode,@status,@tryCount,@datecreated,@lastLogin,@userName,@password,@bankName)";
                if (databaseConnector.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                    cmd.Parameters.AddWithValue("@agentName", person.agentrName);
                    cmd.Parameters.AddWithValue("@agentBankCode", person.agentBankCode);
                    cmd.Parameters.AddWithValue("@status", person.status);
                    cmd.Parameters.AddWithValue("@tryCount", person.tryCount);
                    cmd.Parameters.AddWithValue("@deviceId", person.deviceId);
                    cmd.Parameters.AddWithValue("@datecreated", person.datecreated);
                    cmd.Parameters.AddWithValue("@lastLogin", person.lastLogin);
                    cmd.Parameters.AddWithValue("@userName", person.userName);
                    cmd.Parameters.AddWithValue("@password", person.password);
                    cmd.Parameters.AddWithValue("@bankName", person.bankName);


                    try
                    {
                        rowsAffected = cmd.ExecuteNonQuery();

                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Error occured during insertion : " + ex.Message);
                    }
                }

            }



            databaseConnector.CloseConnection();

            return rowsAffected;


        }

        public int Save_(dynamic person)
        {
            int rowsAffected = 0;
            string query = null;
            if (person.userType == "customer")
            {
                query = $"INSERT INTO customers (customerName,customerBankCode,status,tryCount,datecreated,lastLogin,userName,password,bankName) VALUES (@customerName,@customerBankCode,@status,@tryCount,@datecreated,@lastLogin,@userName,@password,@bankName)";
                if (databaseConnector.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                    cmd.Parameters.AddWithValue("@customerName", person.customerName);
                    cmd.Parameters.AddWithValue("@customerBankCode", person.customerBankCode);
                    cmd.Parameters.AddWithValue("@status", "1");
                    cmd.Parameters.AddWithValue("@tryCount", "0");
                    cmd.Parameters.AddWithValue("@datecreated", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));//YYYY - MM - DD HH: MM: SS
                    cmd.Parameters.AddWithValue("@lastLogin", null);
                    cmd.Parameters.AddWithValue("@userName", person.userName);
                    cmd.Parameters.AddWithValue("@password", person.password);
                    cmd.Parameters.AddWithValue("@bankName", person.bankName);


                    try
                    {
                        rowsAffected = cmd.ExecuteNonQuery();

                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Error occured during insertion : " + ex.Message);
                    }
                }
            }
            else if (person.userType == "merchant")
            {
                query = $"INSERT INTO merchant (customerName,customerBankCode,status,tryCount,datecreated,lastLogin,userName,password) VALUES (@customerName,@customerBankCode,@status,@tryCount,@datecreated,@lastLogin,@userName,@password)";
                if (databaseConnector.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                    cmd.Parameters.AddWithValue("@customerName", person.customerName);
                    cmd.Parameters.AddWithValue("@customerBankCode", person.customerBankCode);
                    cmd.Parameters.AddWithValue("@status", person.status);
                    cmd.Parameters.AddWithValue("@tryCount", person.tryCount);
                    cmd.Parameters.AddWithValue("@datecreated", person.datecreated);
                    cmd.Parameters.AddWithValue("@lastLogin", person.lastLogin);
                    cmd.Parameters.AddWithValue("@userName", person.userName);
                    cmd.Parameters.AddWithValue("@password", person.password);


                    try
                    {
                        rowsAffected = cmd.ExecuteNonQuery();

                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Error occured during insertion : " + ex.Message);
                    }
                }
            }
            else if (person.userType == "agent")
            {
                query = $"INSERT INTO agents (agentName,agentrBankCode,status,tryCount,datecreated,lastLogin,userName,password,bankName) VALUES (@agentName,@agentBankCode,@status,@tryCount,@datecreated,@lastLogin,@userName,@password,@bankName)";
                if (databaseConnector.OpenConnection())
                {
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                    cmd.Parameters.AddWithValue("@agentName", person.agentrName);
                    cmd.Parameters.AddWithValue("@agentBankCode", person.agentBankCode);
                    cmd.Parameters.AddWithValue("@status", person.status);
                    cmd.Parameters.AddWithValue("@tryCount", person.tryCount);
                    cmd.Parameters.AddWithValue("@deviceId", person.deviceId);
                    cmd.Parameters.AddWithValue("@datecreated", person.datecreated);
                    cmd.Parameters.AddWithValue("@lastLogin", person.lastLogin);
                    cmd.Parameters.AddWithValue("@userName", person.userName);
                    cmd.Parameters.AddWithValue("@password", person.password);
                    cmd.Parameters.AddWithValue("@bankName", person.bankName);


                    try
                    {
                        rowsAffected = cmd.ExecuteNonQuery();

                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Error occured during insertion : " + ex.Message);
                    }
                }

            }



            databaseConnector.CloseConnection();

            return rowsAffected;


        }




        public ReturnMessage getUserAccount(AccountFilter accountFilter)
        {
            //string connectionString = databaseConnector.connectionString;
            ReturnMessage message = new ReturnMessage();
            List<UserAccount> accounts = new List<UserAccount>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(databaseConnector.connectionString))
                {
                    
                    if (databaseConnector.OpenConnection())
                    {
                        string query = null;
                        LogHandler.WriteLog("CALLING SERVICE --> getUserAccount \n " + JsonConvert.SerializeObject(accountFilter), "ACCOUNT_SERVICE", false, "ACCOUNT_SERVICE");
                        query = "SELECT * FROM customer_account where userId = '" + accountFilter.userId + "';";


                        using (MySqlCommand command = new MySqlCommand(query, databaseConnector.connection))
                        {
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {

                                    while (reader.Read())
                                    {
                                        UserAccount userAccount = new UserAccount();

                                        userAccount = new UserAccount()
                                        {
                                            userId = int.Parse(reader["userId"].ToString()),
                                            accountType = reader["accountType"].ToString(),
                                            accountProvider = reader["accountProvider"].ToString(),
                                            customerName = reader["customerName"].ToString(),
                                            customerBankCode = reader["customerBankCode"].ToString(),
                                            accountNumber = reader["accountNumber"].ToString(),
                                            bankName = reader["bankName"].ToString(),
                                            status = reader["status"].ToString(),
                                            dateCreated = reader["datecreated"].ToString(),
                                            dateUpdated = reader["dateUpdated"].ToString()

                                        };


                                        accounts.Add(userAccount);

                                    }
                                }
                            }
                        }
                        LogHandler.WriteLog("CALLING SERVICE --> GetUserAccount \n RESPONSE--> GetUserAccount \t" + JsonConvert.SerializeObject(accounts), "ACCOUNT_SERVICE", false, "ACCOUNT_SERVICE");
                        message.returnObject = accounts;
                        message.code = System.Net.HttpStatusCode.OK;
                    }
                    
                }
                

            }
            catch (Exception ex)
            {
                message.code = System.Net.HttpStatusCode.InternalServerError;
                message.message = ex.Message;
            }

            return message;


        }



        public int createWallet(UserAccount userAccount)
        {
            int rowsAffected = 0;
            string query = null;
            //query = $"insert into customer_account(userId,accountType,accountProvider,customerName,customerBankCode,accountNumber,STATUS,bankName) values(19, "MOBILE MONEY", "ORANGE MONEY", NULL, NULL, "2250709279464", "ACTIVATED", NULL)"
            query = $"insert into customer_account(userId,accountType,accountProvider,customerName,customerBankCode,accountNumber,STATUS,bankName,userType,current_balance) " +
                $"values(@userId,@accountType,@accountProvider,@customerName,@customerBankCode,@accountNumber,@STATUS,@bankName,@userType,@current_balance); SELECT LAST_INSERT_ID();";
            //query = $"INSERT INTO customers (customerName,customerPhone,customerEmail,customerBankCode,status,tryCount,deviceId,datecreated,lastLogin,userName,password) VALUES (@customerName,@customerPhone,@customerEmail,@customerBankCode,@status,@tryCount,@deviceId,@datecreated,@lastLogin,@userName,@password,@userType)";
            if (databaseConnector.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                cmd.Parameters.AddWithValue("@userId", userAccount.userId);
                cmd.Parameters.AddWithValue("@accountType", userAccount.accountType);
                cmd.Parameters.AddWithValue("@accountProvider", userAccount.accountProvider);
                cmd.Parameters.AddWithValue("@customerName", userAccount.customerName);
                cmd.Parameters.AddWithValue("@customerBankCode", userAccount.customerBankCode);
                cmd.Parameters.AddWithValue("@accountNumber", userAccount.accountNumber);
                cmd.Parameters.AddWithValue("@STATUS", userAccount.status);
                cmd.Parameters.AddWithValue("@userType", userAccount.userType);
                cmd.Parameters.AddWithValue("@bankName", userAccount.bankName);//YYYY - MM - DD HH: MM: SS
                cmd.Parameters.AddWithValue("@current_balance", userAccount.current_balance);//YYYY - MM - DD HH: MM: SS
                try
                {
                    rowsAffected = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error occured during insertion : " + ex.Message);
                }
            }
            databaseConnector.CloseConnection();
            return rowsAffected;


        }
    }
    
}