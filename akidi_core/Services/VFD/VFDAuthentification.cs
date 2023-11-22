using akidi_core.Utils.Configuration;
using BackEndServices.Models;
using BackEndServices.Services;
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
    public class VFDAuthentification
    {
        public VFDAuthentification()
        {
        }


        public ReturnMessage GetToken()
        {
            return GetTokenAsync().Result;
        }

        private async Task<ReturnMessage> GetTokenAsync()
        {

            Configuration config = new Configuration();
            string url = /*VFDConfiguration.INSTANCE().baseUrl + */ config.get("VFDAuth");
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
                    //url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    var request = await client.PostAsync(url, null);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();

                        message.returnObject = JsonSerializer.Deserialize<dynamic>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  GetTokenAsync \n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  GetTokenAsync \n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

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
