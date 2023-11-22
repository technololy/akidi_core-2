using BackEndServices.Models;
using BackEndServices.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndServices.Services
{
    public class CommonService
    {
        DatabaseConnector databaseConnector = new DatabaseConnector();
        public ReturnMessage getAllBanks()
        {
            //string connectionString = databaseConnector.connectionString;
            ReturnMessage message = new ReturnMessage();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(databaseConnector.connectionString) )
                {
                    databaseConnector.OpenConnection();
                    string query     = "SELECT merchantName name , merchantCode code FROM merchant";
                    MySqlCommand cmd = new MySqlCommand(query, databaseConnector.connection);
                    List <MerchantSimple> merchants = new List<MerchantSimple>();

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {

                            MerchantSimple merchant;

                            while (reader.Read())
                            {
                                merchant = new MerchantSimple()
                                {
                                    code = reader["code"].ToString(),
                                    name = reader["name"].ToString()
                                };

                                merchants.Add(merchant);
                            }

                            message.code = System.Net.HttpStatusCode.OK;
                            message.returnObject = merchants;
                            databaseConnector.CloseConnection();
                            return message;
                        }
                        else
                        {
                            message.code = System.Net.HttpStatusCode.OK;
                            message.returnObject = null;
                            connection.Close();
                            return message;
                        }


                    }


                }

            }catch(Exception ex)
            {
                message.message = ex.Message;
                message.code = System.Net.HttpStatusCode.InternalServerError;
            }

            return message;
        }
    }


}