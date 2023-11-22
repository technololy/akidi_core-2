using akidi_core.Models.VFD;
using akidi_core.Utils.Configuration;
using BackEndServices.Models;
using BackEndServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace akidi_core.Services.VFD
{
    public class VFDTransferService
    {
        Configuration config = new Configuration();

        public VFDTransferService()
        {
        }



        public async Task<ReturnMessage> Transfer(VFDTransfer transfer)
        {

            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDTransfer");
            ReturnMessage message = new ReturnMessage();
            try
            {

                using (var client = new HttpClient())
                {
                    //Define Headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    StringContent stringContent = new StringContent(JsonSerializer.Serialize(transfer), Encoding.UTF8, "application/json");
                    var request = await client.PostAsync(url, stringContent);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<dynamic>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  Transfer \n\t|==> DATA PASSED " + JsonSerializer.Serialize(transfer) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  Transfer \n\t|==> DATA PASSED " + JsonSerializer.Serialize(transfer) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

                    }
                    return message;
                }

            }
            catch (Exception ex)
            {
                message.code = (HttpStatusCode)ReturnedCode.NOT_VALIDATED;
                message.message = ex.Message;
            }
            return message;


        }


        public async Task<ReturnMessage> PayByQrCode(VFDPayQr payQr)
        {
            Configuration config = new Configuration();
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDQrCodePay");
            ReturnMessage message = new ReturnMessage();
            try
            {

                using (var client = new HttpClient())
                {
                    //Define Headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    StringContent stringContent = new StringContent(JsonSerializer.Serialize(payQr), Encoding.UTF8, "application/json");
                    var request = await client.PostAsync(url, stringContent);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<dynamic>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  PayByQrCode \n\t|==> DATA PASSED " + JsonSerializer.Serialize(payQr) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  PayByQrCode \n\t|==> DATA PASSED " + JsonSerializer.Serialize(payQr) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

                    }
                    return message;
                }

            }
            catch (Exception ex)
            {
                message.code = (HttpStatusCode) ReturnedCode.NOT_VALIDATED;
                message.message = ex.Message;
            }
            return message;


        }

        public async Task<ReturnMessage> Credit(VFDCredit credit)
        {
            Configuration config = new Configuration();
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDCredit");
            ReturnMessage message = new ReturnMessage();
            try
            {

                using (var client = new HttpClient())
                {
                    //Define Headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    StringContent stringContent = new StringContent(JsonSerializer.Serialize(credit), Encoding.UTF8, "application/json");
                    var request = await client.PostAsync(url, stringContent);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<dynamic>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  Credit \n\t|==> DATA PASSED " + JsonSerializer.Serialize(credit) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  Credit \n\t|==> DATA PASSED " + JsonSerializer.Serialize(credit) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

                    }
                    return message;
                }

            }
            catch (Exception ex)
            {
                message.code = (HttpStatusCode)ReturnedCode.NOT_VALIDATED;
                message.message = ex.Message;
            }
            return message;


        }



        public async Task<ReturnMessage> RetrieveQrCode(string qrCode)
        {

            Configuration config = new Configuration();
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDQrCodeRetrieve");
            ReturnMessage message = new ReturnMessage();
            try
            {

                using (var client = new HttpClient())
                {
                    //Define Headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    StringContent stringContent = new StringContent(JsonSerializer.Serialize(qrCode), Encoding.UTF8, "application/json");
                    var request = await client.PostAsync(url, stringContent);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<dynamic>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  RetrieveQrCode \n\t|==> DATA PASSED " + JsonSerializer.Serialize(qrCode) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  RetrieveQrCode \n\t|==> DATA PASSED " + JsonSerializer.Serialize(qrCode) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

                    }
                    return message;
                }

            }
            catch (Exception ex)
            {
                message.code =(HttpStatusCode) ReturnedCode.NOT_VALIDATED;
                message.message = ex.Message;
            }
            return message;

        }

        public async Task<ReturnMessage> GenerateQrCode(VFDQrCode vFDQrCode)
        {

            Configuration config = new Configuration();
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDQrCodeGeneration");
            ReturnMessage message = new ReturnMessage();
            try
            {

                using (var client = new HttpClient())
                {
                    //Define Headers
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    StringContent stringContent = new StringContent(JsonSerializer.Serialize(vFDQrCode), Encoding.UTF8, "application/json");
                    var request = await client.PostAsync(url, stringContent);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<dynamic>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  GenerateQrCode \n\t|==> DATA PASSED " + JsonSerializer.Serialize(vFDQrCode) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  GenerateQrCode \n\t|==> DATA PASSED " + JsonSerializer.Serialize(vFDQrCode) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

                    }
                    return message;
                }

            }
            catch (Exception ex)
            {
                message.code = (HttpStatusCode) ReturnedCode.NOT_VALIDATED;
                message.message = ex.Message;
            }
            return message;

        }





        public async Task<ReturnMessage> BeneficiaryEnquiry(string accountNub, string bank)
        {
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDBenefEnquiry");
            ReturnMessage message = new ReturnMessage();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    url = url.Replace("{accountNumb}", accountNub);
                    url = url.Replace("{bank}", bank);
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<BillerCategory>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  BeneficiaryEnquiry \n\t|==> DATA PASSED " + url + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  BeneficiaryEnquiry \n\t|==> DATA PASSED " + url + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

                    }
                    return message;
                }
            }
            catch (Exception ex)
            {
                message.code =(HttpStatusCode) ReturnedCode.NOT_VALIDATED;
                message.message = ex.Message;
            }
            return message;
        }

        public async Task<ReturnMessage> AccountEnquiry(string accountNumber)
        {
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDAcctEnquiry");
            ReturnMessage message = new ReturnMessage();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    url = url.Replace("{accountNumb}", accountNumber);
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<BillerCategory>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  AccountEnquiry \n\t|==> DATA PASSED " + accountNumber + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  AccountEnquiry \n\t|==> DATA PASSED " + accountNumber + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

                    }
                    return message;
                }
            }
            catch (Exception ex)
            {
                message.code = (HttpStatusCode) ReturnedCode.NOT_VALIDATED;
                message.message = ex.Message;
            }
            return message;
        }

        public async Task<ReturnMessage> GetBanks()
        {
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDBanks");
            ReturnMessage message = new ReturnMessage();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<dynamic>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  GetBanks \n\t|==> DATA PASSED " + null + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  GetBanks \n\t|==> DATA PASSED " + null + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

                    }
                    return message;
                }
            }
            catch (Exception ex)
            {
                message.code = (HttpStatusCode) ReturnedCode.NOT_VALIDATED;
                message.message = ex.Message;
            }
            return message;
        }

        public async Task<ReturnMessage> GetTransactions(string reference)
        {
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDTransaction");
            ReturnMessage message = new ReturnMessage();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    url = url.Replace("{reference}", reference);
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<dynamic>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  GetTransactions \n\t|==> DATA PASSED  reference  :" + reference + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  GetTransactions \n\t|==> DATA PASSED reference :" + reference + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

                    }
                    return message;
                }
            }
            catch (Exception ex)
            {
                message.code =  (HttpStatusCode) ReturnedCode.NOT_VALIDATED;
                message.message = ex.Message;
            }
            return message;
        }

        public async Task<ReturnMessage> GetReversalTsq(string reference)
        {
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDTsq");
            ReturnMessage message = new ReturnMessage();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    url = url.Replace("{reference}", reference);
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<BillerCategory>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  GetReversalTsq \n\t|==> DATA PASSED  reference  :" + reference + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  GetReversalTsq \n\t|==> DATA PASSED  reference  :" + reference + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

                    }
                    return message;
                }
            }
            catch (Exception ex)
            {
                message.code = (HttpStatusCode)ReturnedCode.NOT_VALIDATED;
                message.message = ex.Message;
            }
            return message;
        }


        public async Task<ReturnMessage> GetTransactionStatus(string reference)
        {
            ReturnMessage message = new ReturnMessage();
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDTransactonStatus");
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    url = url.Replace("{reference}", reference);
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<TransactionStatusResponse>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  GetTransactionStatus \n\t|==> DATA PASSED  reference  :" + reference + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  GetTransactionStatus \n\t|==> DATA PASSED  reference  :" + reference + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

                    }
                    return message;
                }

            }
            catch (Exception ex)
            {
                message.code = (HttpStatusCode)ReturnedCode.NOT_VALIDATED;
                message.message = ex.Message;
            }

            return message;

        }
    }
}
