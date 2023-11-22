using BackEndServices.Models;
using BackEndServices.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Data;
using System.Configuration;
using System.Globalization;
using Newtonsoft.Json;

namespace BackEndServices.Services
{
    public class TransferFundService : BaseService
    {
        private const string TRANSFER_FUND = "TRANSFER-FUND";

        public static codeGenrateResponse generateCode(GenerateCodeRequest requestParam)
        {
          
            try
            {

                // check security Key 
                if (string.IsNullOrEmpty(requestParam.key))
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : SECURITY FOR TRANSACTION IS NULL OR EMPTY", "TRANSFER");
                    return new codeGenrateResponse
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = "SECURITY KEY FOR TRANSACTION IS NULL OR EMPTY"
                    };
                }

                /**
                * This Part Check the params 
                * return the params that are empty
                **/
                string[] ExcludedParams = new string[] { "sender_phone_number", "receiver_phone_number", "remarks" };
                string checkParamsResult = ValidateCodeRequest(requestParam, ExcludedParams);
                if (!string.IsNullOrEmpty(checkParamsResult))
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : " + checkParamsResult + " CANNOT BE EMPTY OR NULL", "TRANSFER");
                    return new codeGenrateResponse
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = checkParamsResult + " CANNOT BE EMPTY OR NULL"
                    };
                }

                // If the hash key provided by merchant is different from the one               
                //hash value here
                string requestDataHashed = string.Empty;
              //  string requestDataHashed = PaymentServices.GetHashOf(requestDataToBeHashed);  // Dosso to check
                requestDataHashed = "test";  // Dosso to check

                if (!requestDataHashed.ToLower().Equals(requestParam.key.ToLower()))
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : UNABLE TO VALIDATE THE TRANSACTION KEY", "TRANSFER");
                    return new codeGenrateResponse()
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = "UNABLE TO VALIDATE THE TRANSACTION KEY"
                    };
                }

                // MERCHANT BANK ACCOUNT
                string accountToDebit = requestParam.senderAccount;

                string merchantReference = requestParam.clientTransReference.Trim();
                int amount = requestParam.sendingAmount;


                // CHECK IF THE TRANSACTION EXIST
                /*int transactionReferenceStatus = CheckTransactionReference(requestParam.clientTransReference);
                if (!transactionReferenceStatus.Equals(0))
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : REFERNECE HAS BEEN USED BEFORE", "BANKDEPOSIT");
                    return new codeGenrateResponse
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = "REFERENCE HAS BEEN USED BEFORE"
                    };
                }*/


                // CHECK TRANSACTION AMOUNT
                if (amount < 1)
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : INVALID TRANSACTION AMOUNT", "BANKDEPOSIT");
                    return new codeGenrateResponse
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = "INVALID TRANSACTION AMOUNT"
                    };
                }

                // A valid IBAN Account number should have 15 digits
                /*if (!accountToDebit.Length.Equals(15))
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : WRONG SOURCE BANK ACCOUNT FORMAT, 24 CHARACTERS REQUIRED PLEASE CONTACT SUPPORT", "TRANSFER");
                    return new codeGenrateResponse
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = "WRONG SOURCE BANK ACCOUNT FORMAT, 15 CHARACTERS REQUIRED PLEASE CONTACT"
                    };
                }*/




                /***MAKE TRANSFER ***/

                // Get The explanation code setup for bank deposit for the merchant
                //  string merchantExplCode = serviceAccessConfig.bankDeposit.explanationCode;

                string transferResult = string.Empty;
                string transCode = string.Empty;
                string transferResultCode = string.Empty;
                string myPayTransactionReference = string.Empty;
                // string MerchantTransactionRemarks = string.Empty;

                myPayTransactionReference = GenerateTransactionReference(TRANSFER_FUND+"-"+requestParam.transactionType);
                transCode = GetCode();

                
                string TransactionStatus = string.Empty;
                string GTBTransactionDate = string.Empty;
                DateTime utcDate = DateTime.UtcNow;
                var culture = new CultureInfo("fr-FR");
                TransactionStatus = "PENDING";
                DateTime tDate = DateTime.Now;
                var expirationHours = Convert.ToInt32(ConfigurationManager.AppSettings["expiryTime"].ToString());
                DateTime dt2 = tDate.AddMinutes(expirationHours);
                // log to file 
                LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccountName + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> RESPONSE : " + transferResult, "TRANSFER");

                // LOG tRANSACTION to keep record 
                int transactionLogged = LogCodeTransfer(requestParam, requestParam.senderAccountName, requestParam.senderAccount, "PENDING", requestParam.transactionType, myPayTransactionReference, "PENDING", transCode);


                GTBTransactionDate = utcDate.ToString(culture);

                if (transactionLogged < 1)
                {
                    return new codeGenrateResponse
                    {
                        code = 718,
                        message = "ERROR_TRANSACTION_FAILED",
                        description = "TRANSACTION FAILED FOR THIS TRANSFER"
                    };
                }


                return new codeGenrateResponse
                {
                    code = 201,
                    message = "SUCCESSFUL",
                    data = new codeGenerationResponseData()
                    {
                        merchant_reference = requestParam.clientTransReference,
                        transCode = transCode,
                        reference = myPayTransactionReference,
                        sender_name = requestParam.senderAccountName,
                        sender_phone_number = requestParam.senderMobileNumber,
                        sending_amount = requestParam.sendingAmount,
                        sending_currency = "XOF",
                        receiverAccount = requestParam.receiverAccount,
                        receiverMobileNumber = requestParam.receiverMobileNumber,
                        receiverEmail = requestParam.receiverEmail,
                        receiverAccountName = requestParam.receiverAccountName,
                        sending_reason = requestParam.sendingReason,
                        transaction_status = TransactionStatus,
                        transaction_date = GTBTransactionDate,
                        expirationTime = dt2.ToString()

                    },
                    description = null
                };
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccountName + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : " + ex.Message, "BANKDEPOSIT");
                return new codeGenrateResponse
                {
                    code = 503,
                    message = "SERVICE_NOT_UNAVAILABLE",
                    description = "THE SERVICE IS CURRENTLY OR TEMPORARILY NOT AVAILABLE, PLEASE CONTACT QIPS Pay"
                };
            }

        }


        private static string GetCode()
        {
            //const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            const string chars = "0123456789";
            Random rnd = new Random();
            //string uniqueCode = DateTime.UtcNow.ToString("ffff") + new string(Enumerable.Repeat(chars, 6)
            //                              .Select(s => s[rnd.Next(s.Length)])
            //                              .ToArray());
            string uniqueCode =  new string(Enumerable.Repeat(chars, 6)
                                          .Select(s => s[rnd.Next(s.Length)])
                                          .ToArray());
            return uniqueCode;
        }

        /**
         * Cancel Code Request 
         */
        public static codeGenrateResponse cancelRequestCode(payCancelCodeRequest requestParam)
        {
           

            try
            {

                // check security Key 
                if (string.IsNullOrEmpty(requestParam.key))
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : SECURITY FOR TRANSACTION IS NULL OR EMPTY", "TRANSFER");
                    return new codeGenrateResponse
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = "SECURITY KEY FOR TRANSACTION IS NULL OR EMPTY"
                    };
                }





                // string requestDataHashed = GetHashOf(requestDataToBeHashed);  // we calculate here
                string requestDataHashed = string.Empty;
                if (!requestDataHashed.ToLower().Equals(requestParam.key.ToLower()))
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : UNABLE TO VALIDATE THE TRANSACTION KEY", "TRANSFER");
                    return new codeGenrateResponse()
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = "UNABLE TO VALIDATE THE TRANSACTION KEY"
                    };
                }

                // MERCHANT BANK ACCOUNT
                string accountToDebit = requestParam.senderAccount;

                string merchantReference = requestParam.clientTransReference.Trim();



                // CHECK IF THE TRANSACTION EXIST, IF NOT EXIST RETURN FAILED
                int transactionReferenceStatus = CheckTransactionReference(requestParam.clientTransReference);
                if (transactionReferenceStatus.Equals(0))
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : REFERNECE HAS BEEN USED BEFORE", "BANKDEPOSIT");
                    return new codeGenrateResponse
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = "REFERENCE HAS BEEN USED BEFORE"
                    };
                }


                // A valid IBAN Account number should have 24 digits
                if (!accountToDebit.Length.Equals(24))
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : WRONG SOURCE BANK ACCOUNT FORMAT, 24 CHARACTERS REQUIRED PLEASE CONTACT SUPPORT", "TRANSFER");
                    return new codeGenrateResponse
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = "WRONG SOURCE BANK ACCOUNT FORMAT, 24 CHARACTERS REQUIRED PLEASE CONTACT"
                    };
                }

                /***Update TRAnSAcTION to cancel ***/

                // Get The explanation code setup for bank deposit for the merchant
                //  string merchantExplCode = serviceAccessConfig.bankDeposit.explanationCode;

                string transferResult = string.Empty;
                string transCode = string.Empty;
                string transferResultCode = string.Empty;
                string myPayTransactionReference = string.Empty;
                // string MerchantTransactionRemarks = string.Empty;


                // log to file 
                LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> RESPONSE : " + transferResult, "TRANSFER");

                // update the table to cancel  
                bool transactionLogged = UpdateTranStatus(requestParam); //add status here 

                if (!transactionLogged)
                {
                    return new codeGenrateResponse
                    {
                        code = 718,
                        message = "ERROR_TRANSACTION_FAILED",
                        description = "TRANSACTION FAILED FOR THIS TRANSFER"
                    };
                }


                return new codeGenrateResponse
                {
                    code = 201,
                    message = "SUCCESSFUL",
                    data = new codeGenerationResponseData()
                    {
                        merchant_reference = requestParam.clientTransReference,
                        reference = myPayTransactionReference,
                    },
                    description = "SUCCESSFULLY CANCELLED THE TRANSACTION"
                };
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : " + ex.Message, "BANKDEPOSIT");
                return new codeGenrateResponse
                {
                    code = 503,
                    message = "SERVICE_NOT_UNAVAILABLE",
                    description = "THE SERVICE IS CURRENTLY OR TEMPORARILY NOT AVAILABLE, PLEASE CONTACT QIPS Pay"
                };
            }


        }


        /***
         * Pay code Request
         */

        public static codeGenrateResponse payRequestCode(payCodeRequest requestParam)
        {
           

            try
            {

                // check security Key 
                //if (string.IsNullOrEmpty(requestParam.key))
                //{
                //    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : SECURITY FOR TRANSACTION IS NULL OR EMPTY", "TRANSFER");
                //    return new codeGenrateResponse
                //    {
                //        code = 717,
                //        message = "ERROR_TRANSACTION_DATA_INVALID",
                //        description = "SECURITY KEY FOR TRANSACTION IS NULL OR EMPTY"
                //    };
                //}


                // If the hash key provided by merchant is different from the one               
                string requestDataHashed = string.Empty;

                // string requestDataHashed = PaymentServices.GetHashOf(requestDataToBeHashed);  // we calculate here

                //if (!requestDataHashed.ToLower().Equals(requestParam.key.ToLower()))
                //{
                //    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> MERCHANT NAME : " + requestParam.senderAccount + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : UNABLE TO VALIDATE THE TRANSACTION KEY", "TRANSFER");
                //    return new codeGenrateResponse()
                //    {
                //        code = 717,
                //        message = "ERROR_TRANSACTION_DATA_INVALID",
                //        description = "UNABLE TO VALIDATE THE TRANSACTION KEY"
                //    };
                //}





                // CHECK IF THE TRANSACTION EXIST, IF NOT EXIST RETURN FAILED
                GenerateCodeRequest transaction = getTransaction(requestParam.transCode,requestParam.secretAnswer);
                // MERCHANT BANK ACCOUNT
                string accountToDebit = transaction.senderAccount;

                string merchantReference = transaction.clientTransReference.Trim(); //check if same as code 
                if (transaction.Equals(0))
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> TRANSACTION CODE : " + requestParam.transCode + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : REFERNECE HAS BEEN USED BEFORE", "BANKDEPOSIT");
                    return new codeGenrateResponse
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = "REFERENCE HAS BEEN USED BEFORE"
                    };
                }


                // A valid IBAN Account number should have 24 digits
                if (!accountToDebit.Length.Equals(24))
                {
                    LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> TRANSACTION CODE : " + requestParam.transCode + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : WRONG SOURCE BANK ACCOUNT FORMAT, 24 CHARACTERS REQUIRED PLEASE CONTACT SUPPORT", "TRANSFER");
                    return new codeGenrateResponse
                    {
                        code = 717,
                        message = "ERROR_TRANSACTION_DATA_INVALID",
                        description = "WRONG SOURCE BANK ACCOUNT FORMAT, 24 CHARACTERS REQUIRED PLEASE CONTACT"
                    };
                }

                /***Update TRAnSAcTION to cancel ***/

                // Get The explanation code setup for bank deposit for the merchant
                //  string merchantExplCode = serviceAccessConfig.bankDeposit.explanationCode;

                string transferResult = string.Empty;
                string transCode = string.Empty;
                string transferResultCode = string.Empty;
                string myPayTransactionReference = string.Empty;
                // string MerchantTransactionRemarks = string.Empty;


                // log to file 
                LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==>  TRANSACTION CODE : " + requestParam.transCode + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> RESPONSE : " + transferResult, "TRANSFER");

                // update the table to cancel  
                bool transactionLogged = UpdateTransUsingCodeStatus(requestParam); //add status here 

                if (!transactionLogged)
                {
                    return new codeGenrateResponse
                    {
                        code = 718,
                        message = "ERROR_TRANSACTION_FAILED",
                        description = "TRANSACTION FAILED FOR THIS TRANSFER"
                    };
                }


                return new codeGenrateResponse
                {
                    code = 201,
                    message = "SUCCESSFUL",
                    data = new codeGenerationResponseData()
                    {
                        merchant_reference = transaction.clientTransReference,
                        reference = myPayTransactionReference,
                    },
                    description = "SUCCESSFULLY CANCELLED THE TRANSACTION"
                };
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("\t|==> CALLING TRANSFER \n\t|==> TRANSACTION CODE : " + requestParam.transCode + " \n\t|==> DATA PASSED : " + JsonConvert.SerializeObject(requestParam) + " \n\t|==> ERROR : " + ex.Message, "BANKDEPOSIT");
                return new codeGenrateResponse
                {
                    code = 503,
                    message = "SERVICE_NOT_UNAVAILABLE",
                    description = "THE SERVICE IS CURRENTLY OR TEMPORARILY NOT AVAILABLE, PLEASE CONTACT QIPS Pay"
                };
            }


        }


        private static GenerateCodeRequest getTransaction(string transCode, string secretAnswer)
        {
            //try
            //{
            //    MySqlConnection conn = new MySqlConnection(ConfigurationManager.AppSettings["ConString"].ToString());
            //    MySqlCommand commandSql = new MySqlCommand("CheckIfTransactionExist", conn);
            //    commandSql.CommandType = CommandType.StoredProcedure;
            //    if (conn.State != ConnectionState.Open)
            //    {
            //        conn.Open();
            //    }

            //    commandSql.Parameters.AddWithValue("@UniqueRef", transactionReference);
            //    commandSql.CommandType = CommandType.StoredProcedure;

            //    MySqlDataReader reader = commandSql.ExecuteReader();
            //    if (!reader.HasRows)
            //    {
            //        return -1;
            //    }

            //    reader.Read();
            //    string referenceCount = reader["referenceCount"].ToString();

            //    conn.Close();
            //    return Convert.ToInt32(referenceCount);
            //}
            //catch (Exception ex)
            //{
            //    LogHandler.WriteLog(ex.Message);
            //    return -1;
            //}
            return null;
        }

        private static int CheckTransactionReference(string transactionReference)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.AppSettings["ConString"].ToString());
                MySqlCommand commandSql = new MySqlCommand("CheckIfTransactionExist", conn);
                commandSql.CommandType = CommandType.StoredProcedure;  
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                commandSql.Parameters.AddWithValue("@UniqueRef", transactionReference);
                commandSql.CommandType = CommandType.StoredProcedure;

                MySqlDataReader reader = commandSql.ExecuteReader();
                if (!reader.HasRows)
                {
                    return -1;
                }

                reader.Read();
                string referenceCount = reader["referenceCount"].ToString();

                conn.Close();
                return Convert.ToInt32(referenceCount);
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog(ex.Message);
                return -1;
            }
        }


        private static int CheckTransactionIsExisting(string transactionCode, string secretAnswer)
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.AppSettings["ConString"].ToString());
                MySqlCommand commandSql = new MySqlCommand("CheckIfTransactionExist", conn);
                commandSql.CommandType = CommandType.StoredProcedure;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                commandSql.Parameters.AddWithValue("@transactionCode", transactionCode);
                commandSql.Parameters.AddWithValue("@secretAnswer", secretAnswer);
                commandSql.CommandType = CommandType.StoredProcedure;

                MySqlDataReader reader = commandSql.ExecuteReader();
                if (!reader.HasRows)
                {
                    return -1;
                }

                reader.Read();
                string referenceCount = reader["referenceCount"].ToString();

                conn.Close();
                return Convert.ToInt32(referenceCount);
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog(ex.Message);
                return -1;
            }
        }

        private static int LogCodeTransfer(GenerateCodeRequest transferParams, string merchantName, string merchantID, string status, string TransactionType, string myPayTransactionReference, string transactionStatusMessage, string transCode)
        {

            try
            {

                //int merchant_id = Convert.ToInt32(merchantID);
                string merchant_id = merchantID;
                MySqlConnection conn = new MySqlConnection(ConfigurationManager.AppSettings["ConString"].ToString());
                MySqlCommand commandSql = new MySqlCommand("SaveBDTransaction", conn); ;
                commandSql.CommandType = CommandType.StoredProcedure;
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                commandSql.Parameters.AddWithValue("@transactionType", TransactionType);
                commandSql.Parameters.AddWithValue("@merchantId", merchant_id);
                commandSql.Parameters.AddWithValue("@merchant", merchantName);
                commandSql.Parameters.AddWithValue("@merchant_reference", transferParams.clientTransReference);
                commandSql.Parameters.AddWithValue("@transactionReference", myPayTransactionReference);
                commandSql.Parameters.AddWithValue("@sender_name", transferParams.senderAccountName);
                commandSql.Parameters.AddWithValue("@customerAccountNumber", transferParams.senderAccount);
                commandSql.Parameters.AddWithValue("@sending_amount", Convert.ToString(transferParams.sendingAmount));
                commandSql.Parameters.AddWithValue("@sending_reason", transferParams.sendingReason);
                commandSql.Parameters.AddWithValue("@transCode", transCode);
                commandSql.Parameters.AddWithValue("@receiver_name", transferParams.receiverAccountName);
                commandSql.Parameters.AddWithValue("@receiverAccountNumber", transferParams.receiverAccount);
                commandSql.Parameters.AddWithValue("@receiver_mobileMoney", transferParams.receiverMobileNumber);
                commandSql.Parameters.AddWithValue("@transactionDate", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));//YYYY - MM - DD HH: MM: SS
                commandSql.Parameters.AddWithValue("@transStatus", status);
                commandSql.Parameters.AddWithValue("@secretAnswer", transferParams.secretAnswer);


                commandSql.ExecuteNonQuery();

                conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("\t|==> Saving Bank Transaction to DB \t|==> Transaction Ref: " + myPayTransactionReference + " \t|==> ERROR: " + ex.Message);
                return 0;
            }
        }


        private static string ValidateCodeRequest(GenerateCodeRequest requestData, string[] ExcludedField = null)
        {
            try
            {
                List<string> emptyParamsList = new List<string>();  // Holds the list of Params that are empty
                PropertyInfo[] properties = requestData.GetType().GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    string propertyName = property.Name.ToString();     // Getting the Object property name
                    string propertyType = property.PropertyType.Name;   // Gettign the type "string; Int32, Float ..." of the property
                    if (ExcludedField != null)
                    {                        // if an excluded list of parameters is passed
                        if (!ExcludedField.Contains(propertyName))
                        {
                            if (propertyType == "String")
                            {
                                string propertyValue = (string)property.GetValue(requestData);
                                if (string.IsNullOrEmpty(propertyValue))
                                {
                                    emptyParamsList.Add(propertyName.Replace("_", " ").ToUpper());
                                }
                            }

                            if (propertyType == "Int32")
                            {
                                int propertyValue = (int)property.GetValue(requestData);
                                if (propertyValue <= 0)
                                {
                                    emptyParamsList.Add(propertyName.Replace("_", " ").ToUpper());
                                }
                            }
                        }
                    }
                    else
                    {
                        if (propertyType == "String")
                        {
                            string propertyValue = (string)property.GetValue(requestData);
                            if (string.IsNullOrEmpty(propertyValue))
                            {
                                emptyParamsList.Add(propertyName.Replace("_", " ").ToUpper());
                            }
                        }

                        if (propertyType == "Int32")
                        {
                            int propertyValue = (int)property.GetValue(requestData);
                            if (propertyValue == 0)
                            {
                                emptyParamsList.Add(propertyName.Replace("_", " ").ToUpper());
                            }
                        }
                    }
                }

                // If List empty means there is no error
                if (emptyParamsList.Count == 0)
                {
                    return null;
                }

                // else there errors
                // return the request params involved
                return string.Join(", ", emptyParamsList.ToArray());
            }
            catch (Exception ex)
            {
                LogHandler.WriteLog("TRANSFER_FUND Trying validating request data with error : " + ex.Message);
                return null;
            }
        }

        private static string GenerateTransactionReference(string typeOfTransfer)
        {


            string currentDate = DateTime.UtcNow.ToString("yyMMddffffff");
            return (typeOfTransfer.ToUpper().Equals(TRANSFER_FUND)) ? "TRF." + currentDate + "." + DateTime.UtcNow.ToString("fffff") : "TRF." + currentDate + "." + DateTime.UtcNow.ToString("fffff");

        }

        private static bool UpdateTransUsingCodeStatus(payCodeRequest transferParams)
        {
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.AppSettings["ConString"].ToString()))
            {
                MySqlCommand comm = new MySqlCommand("UpateSI_Log", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@transactionCode", transferParams.transCode);
                comm.Parameters.AddWithValue("@secretAnswer", transferParams.secretAnswer);

                try
                {
                    //open connetion
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    int val = new int();
                    val = comm.ExecuteNonQuery();
                    if (val < 1)
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    //ErrHandler.WriteError(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
            return true;
        }


        private static bool UpdateTranStatus(payCancelCodeRequest transferParams)
        {
            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.AppSettings["ConString"].ToString()))
            {
                MySqlCommand comm = new MySqlCommand("UpateSI_Log", conn);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@BraCode", transferParams.transCode);
                comm.Parameters.AddWithValue("@Customer_No", transferParams.clientTransReference);

                try
                {
                    //open connetion
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    int val = new int();
                    val = comm.ExecuteNonQuery();
                    if (val < 1)
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    //ErrHandler.WriteError(ex.Message);
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
            return true;
        }


        public static string transferFundService(TransferPayload transferPayload)
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            string connectionString = ConfigurationManager.AppSettings["ConString"].ToString(); 
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "CALL `TransferFunds_Wallet`(" + 1 + ", " + transferPayload.senderID + ", " + transferPayload.receiverID + ", " + transferPayload.amount+")";
                using (MySqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;
                        string x = command.ExecuteScalar().ToString();
                        return x;
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                }
            }
        }

        public static transferResponse transferFund(TransferPayload transferPayload)
        {
            DatabaseConnector databaseConnector = new DatabaseConnector();
            string connectionString = ConfigurationManager.AppSettings["ConString"].ToString();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "CALL `TransferFunds_Wallet`(" + 1 + ", " + transferPayload.senderID + ", " + transferPayload.receiverID + ", " + transferPayload.amount + ")";
                using (MySqlCommand command = connection.CreateCommand())
                {
                    try
                    {
                        command.CommandText = query;
                        command.CommandType = CommandType.Text;

                        string x = command.ExecuteScalar().ToString();

                        if (x.Equals("Transfer Successful"))
                        {
                            return new codeGenrateResponse
                            {
                                code = 201,
                                message = "TRANSACTION_SUCCESSFULLY",
                                description = "SUCCESS"
                            };
                        }
                        else
                        {
                            return new codeGenrateResponse
                            {
                                code = 717,
                                message = x,// "ERROR_TRANSACTION_FAILED",
                                description = "TRANSACTION FAILED"
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new codeGenrateResponse
                        {
                            code = 717,
                            message = "ERROR_DATA_MISSING",
                            description = ex.Message.ToUpper()
                        };
                    }
                }
            }
        }


        public static transferResponse transferFund_old(TransferPayload transferPayload)
        {

            DatabaseConnector databaseConnector = new DatabaseConnector();
            string connectionString = ConfigurationManager.AppSettings["ConString"].ToString();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "TransferFunds_Wallet";
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        if (transferPayload != null)
                        {
                            command.Parameters.AddWithValue("@userType", transferPayload.customerType != null ? transferPayload.customerType : "");
                            command.Parameters.AddWithValue("@senderWalletId", transferPayload.senderID);
                            command.Parameters.AddWithValue("@recipientWalletId", transferPayload.receiverID);
                            command.Parameters.AddWithValue("@amount", transferPayload.amount);

                            // Assuming your MySQL function returns a string
                            // If it returns a different data type, adjust accordingly
                            command.Parameters.AddWithValue("@result", null);
                            command.Parameters["@result"].Direction = ParameterDirection.ReturnValue;

                            int i = command.ExecuteNonQuery();


                            string returnValue = command.Parameters["result"].Value.ToString();


                            

                            if (!returnValue.ToString().Equals("Transfer Successful"))
                            {
                                return new codeGenrateResponse
                                {
                                    code = 201,
                                    message = "TRANSACTION_SUCCESSFULLY",
                                    data = new codeGenerationResponseData()
                                    {

                                    },
                                    description = "SUCCESS"
                                };
                            }
                            else
                            {
                                return new codeGenrateResponse
                                {
                                    code = 717,
                                    message = "ERROR_TRANSACTION_FAILED",
                                    description = "TRANSACTION FAILED"
                                };
                            }

                        }
                        else
                        {
                            return new codeGenrateResponse
                            {
                                code = 717,
                                message = "ERROR_DATA_MISSING",
                                description = "DATA MISSING"
                            };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new codeGenrateResponse
                        {
                            code = 717,
                            message = "ERROR_DATA_MISSING",
                            description = ex.Message.ToUpper()
                        };
                    }



                }









            }
        }
    }
}