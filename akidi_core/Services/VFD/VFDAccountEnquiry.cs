using akidi_core.Models.VFD;
using akidi_core.Utils.Configuration;
using BackEndServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace akidi_core.Services.VFD
{
    public class VFDAccountEnquiry
    {
        Configuration config = new Configuration();

        public VFDAccountEnquiry()
        {
        }

        public async Task<ReturnMessage> getClientSubAccount(VFDSubAccount vFDSubAccount)
        {
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDSubAccount");
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
                    url = url.Replace("{accountNumb}", vFDSubAccount.accountNo);
                    url = url.Replace("{bank}", vFDSubAccount.bank);
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        try
                        {
                            message.returnObject = JsonSerializer.Deserialize<VFDEnquiryResponse>(json);
                        }
                        catch (Exception)
                        {
                            message.returnObject = JsonSerializer.Deserialize<dynamic>(json);
                        }

                        message.code = (HttpStatusCode)request.StatusCode;
                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
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



        public async Task<ReturnMessage> getInwarInquiry(VFDSubAccount vFDSubAccount)
        {
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDInwarInquiry");
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
                    url = url.Replace("{accountNumb}", vFDSubAccount.accountNo);
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<dynamic>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
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
