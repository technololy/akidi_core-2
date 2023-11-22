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
    public class VFDKYCEnquiry
    {
        Configuration config = new Configuration();

        public VFDKYCEnquiry()
        {
        }

        public async Task<ReturnMessage> getClientUsingBvn(string bvn)
        {
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDClientBvn");
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
                    url = url.Replace("{bvn}", bvn);
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<BillerCategory>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  getClientUsingBvn \n\t|==> DATA PASSED Bvn :  " + bvn + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  getClientUsingBvn \n\t|==> DATA PASSED  Bvn : " + bvn + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

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


        public async Task<ReturnMessage> BvnConsent(string bvn)
        {
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDBvnConsent");
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
                    url = url.Replace("{bvn}", bvn);
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<BillerCategory>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  BvnConsent \n\t|==> DATA PASSED Bvn :  " + bvn + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  BvnConsent \n\t|==> DATA PASSED Bvn :  " + bvn + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

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

        public async Task<ReturnMessage> phoneVerification(PhoneVerification phoneVerification)
        {
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDPhoneVerification");
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
                    StringContent stringContent = new StringContent(JsonSerializer.Serialize(phoneVerification), Encoding.UTF8, "application/json");
                    var request = await client.PostAsync(url, stringContent);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<PhoneVerificationPayload>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  phoneVerification \n\t|==> DATA PASSED   " + JsonSerializer.Serialize(phoneVerification) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  phoneVerification \n\t|==> DATA PASSED Bvn :  " + JsonSerializer.Serialize(phoneVerification) + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

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
    }
}
