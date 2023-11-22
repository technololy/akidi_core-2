using BackEndServices.Models;
using BackEndServices.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndServices.Services
{
    public class CodeTransactionService : BaseService
    {
        DatabaseConnector databaseConnector = new DatabaseConnector();

        public int Save(dynamic codeTransaction)
        {
            int rowsAffected = 0;
            string query = null;
            
            query = $"INSERT INTO code_transactions (merchantId ,merchantName ,merchantCode ,bankCode ,customerAccountNumber ,customerName ,customerEmail ,customerPhoneNumber ,transType ,transAmount ,transactionCode ,transactionPin ,transactionReference ,transStatus ,transReason ,merchantTransReference ,agentCustomerFlag ,settlementAccount ,settlementCommissionAccount ,transactionDate ,treatedDate ) VALUES (@merchantId ,@merchantName ,@merchantCode ,@bankCode ,@customerAccountNumber ,@customerName ,@customerEmail ,@customerPhoneNumber ,@transType ,@transAmount ,@transactionCode ,@transactionPin ,@transactionReference ,@transStatus ,@transReason ,@merchantTransReference ,@agentCustomerFlag ,@settlementAccount ,@settlementCommissionAccount ,@transactionDate ,@treatedDate )";
            if (databaseConnector.OpenConnection())
            {
                MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                
                cmd.Parameters.AddWithValue("@merchantId", codeTransaction.merchantId);
                cmd.Parameters.AddWithValue("@merchantName", codeTransaction.merchantName);
                cmd.Parameters.AddWithValue("@merchantCode", codeTransaction.merchantCode);
                cmd.Parameters.AddWithValue("@bankCode", codeTransaction.bankCode);
                cmd.Parameters.AddWithValue("@customerAccountNumber", codeTransaction.customerAccountNumber);
                cmd.Parameters.AddWithValue("@customerName", codeTransaction.customerName);
                cmd.Parameters.AddWithValue("@customerEmail", codeTransaction.customerEmail);
                cmd.Parameters.AddWithValue("@customerPhoneNumber", codeTransaction.customerPhoneNumber);
                cmd.Parameters.AddWithValue("@transType", codeTransaction.transType);
                cmd.Parameters.AddWithValue("@transAmount", codeTransaction.transAmount);
                cmd.Parameters.AddWithValue("@transactionCode", codeTransaction.transactionCode);
                cmd.Parameters.AddWithValue("@transactionPin", codeTransaction.transactionPin);
                cmd.Parameters.AddWithValue("@transactionReference", codeTransaction.transactionReference);
                cmd.Parameters.AddWithValue("@transStatus", codeTransaction.transStatus);
                cmd.Parameters.AddWithValue("@transReason", codeTransaction.transReason);
                cmd.Parameters.AddWithValue("@merchantTransReference", codeTransaction.merchantTransReference);
                cmd.Parameters.AddWithValue("@agentCustomerFlag", codeTransaction.agentCustomerFlag);
                cmd.Parameters.AddWithValue("@settlementAccount", codeTransaction.settlementAccount);
                cmd.Parameters.AddWithValue("@settlementCommissionAccount", codeTransaction.settlementCommissionAccount);
                cmd.Parameters.AddWithValue("@transactionDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));//YYYY - MM - DD HH: MM: SS);
                cmd.Parameters.AddWithValue("@treatedDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));//YYYY - MM - DD HH: MM: SS


                try
                {
                    rowsAffected = cmd.ExecuteNonQuery();

                }
                catch (MySqlException ex)
                {
                    Console.WriteLine("Error occured during insertion : " + ex.Message);
                }
            }
            
            



            databaseConnector.CloseConnection();

            return rowsAffected;


        }


        //public int Save(dynamic codeTransaction)
        //{
        //    int rowsAffected = 0;
        //    string query = null;
        //    if (person.userType == "customer")
        //    {
        //        query = $"INSERT INTO customers (customerName,customerPhone,customerEmail,customerBankCode,status,tryCount,deviceId,datecreated,lastLogin,userName,password) VALUES (@customerName,@customerPhone,@customerEmail,@customerBankCode,@status,@tryCount,@deviceId,@datecreated,@lastLogin,@userName,@password)";
        //        if (databaseConnector.OpenConnection())
        //        {
        //            MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
        //            cmd.Parameters.AddWithValue("@customerName", person.customerName);
        //            cmd.Parameters.AddWithValue("@customerPhone", person.customerPhone);
        //            cmd.Parameters.AddWithValue("@customerEmail", person.customerEmail);
        //            cmd.Parameters.AddWithValue("@customerBankCode", person.customerBankCode);
        //            cmd.Parameters.AddWithValue("@status", "1");
        //            cmd.Parameters.AddWithValue("@tryCount", "0");
        //            cmd.Parameters.AddWithValue("@deviceId", person.deviceId);
        //            cmd.Parameters.AddWithValue("@datecreated", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));//YYYY - MM - DD HH: MM: SS
        //            cmd.Parameters.AddWithValue("@lastLogin", null);
        //            cmd.Parameters.AddWithValue("@userName", person.userName);
        //            cmd.Parameters.AddWithValue("@password", person.password);


        //            try
        //            {
        //                rowsAffected = cmd.ExecuteNonQuery();

        //            }
        //            catch (MySqlException ex)
        //            {
        //                Console.WriteLine("Error occured during insertion : " + ex.Message);
        //            }
        //        }
        //    }
        //    else if (person.userType == "merchant")
        //    {
        //        query = $"INSERT INTO merchant (customerName,customerPhone,customerEmail,customerBankCode,status,tryCount,deviceId,datecreated,lastLogin,userName,password) VALUES (@customerName,@customerPhone,@customerEmail,@customerBankCode,@status,@tryCount,@deviceId,@datecreated,@lastLogin,@userName,@password)";
        //        if (databaseConnector.OpenConnection())
        //        {
        //            MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
        //            cmd.Parameters.AddWithValue("@customerName", person.customerName);
        //            cmd.Parameters.AddWithValue("@customerPhone", person.customerPhone);
        //            cmd.Parameters.AddWithValue("@customerEmail", person.customerEmail);
        //            cmd.Parameters.AddWithValue("@customerBankCode", person.customerBankCode);
        //            cmd.Parameters.AddWithValue("@status", person.status);
        //            cmd.Parameters.AddWithValue("@tryCount", person.tryCount);
        //            cmd.Parameters.AddWithValue("@deviceId", person.deviceId);
        //            cmd.Parameters.AddWithValue("@datecreated", person.datecreated);
        //            cmd.Parameters.AddWithValue("@lastLogin", person.lastLogin);
        //            cmd.Parameters.AddWithValue("@userName", person.userName);
        //            cmd.Parameters.AddWithValue("@password", person.password);


        //            try
        //            {
        //                rowsAffected = cmd.ExecuteNonQuery();

        //            }
        //            catch (MySqlException ex)
        //            {
        //                Console.WriteLine("Error occured during insertion : " + ex.Message);
        //            }
        //        }
        //    }
        //    else if (person.userType == "agent")
        //    {
        //        query = $"INSERT INTO agents (agentName,agentrPhone,agentEmail,agentrBankCode,status,tryCount,deviceId,datecreated,lastLogin,userName,password) VALUES (@agentName,@agentPhone,@agentEmail,@agentBankCode,@status,@tryCount,@deviceId,@datecreated,@lastLogin,@userName,@password)";
        //        if (databaseConnector.OpenConnection())
        //        {
        //            MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
        //            cmd.Parameters.AddWithValue("@agentName", person.agentrName);
        //            cmd.Parameters.AddWithValue("@agentPhone", person.agentPhone);
        //            cmd.Parameters.AddWithValue("@agentEmail", person.agentEmail);
        //            cmd.Parameters.AddWithValue("@agentBankCode", person.agentBankCode);
        //            cmd.Parameters.AddWithValue("@status", person.status);
        //            cmd.Parameters.AddWithValue("@tryCount", person.tryCount);
        //            cmd.Parameters.AddWithValue("@deviceId", person.deviceId);
        //            cmd.Parameters.AddWithValue("@datecreated", person.datecreated);
        //            cmd.Parameters.AddWithValue("@lastLogin", person.lastLogin);
        //            cmd.Parameters.AddWithValue("@userName", person.userName);
        //            cmd.Parameters.AddWithValue("@password", person.password);


        //            try
        //            {
        //                rowsAffected = cmd.ExecuteNonQuery();

        //            }
        //            catch (MySqlException ex)
        //            {
        //                Console.WriteLine("Error occured during insertion : " + ex.Message);
        //            }
        //        }

        //    }



        //    databaseConnector.CloseConnection();

        //    return rowsAffected;


        //}
    }
    
}