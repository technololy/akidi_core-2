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
using System.Text.Json;
using System.Threading.Tasks;

namespace akidi_core.Services.VFD
{
    public class VFDBillerCategory
    {

        Configuration config = new Configuration();

        public VFDBillerCategory()
        {
        }



        public async Task<ReturnMessage> GetBillerByCategory(string category)
        {
            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDBillerByCategory");
            ReturnMessage message = new ReturnMessage();
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {VFDConfiguration.INSTANCE().token}");
                    //client.DefaultRequestHeaders.Add("token", VFDConfiguration.INSTANCE().token);
                    string parametre = $"&?categoryName={category}";
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", VFDConfiguration.INSTANCE().token);
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    url = url + parametre;
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<BillerCategory>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  ProfilAccount \n\t|==> DATA PASSED  GetBillerByCategory : " + category + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  ProfilAccount \n\t|==> DATA PASSED  GetBillerByCategory : " + category + "\n\t|==> RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

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


        public async Task<ReturnMessage> GetBillerCategory()
        {

            string url = VFDConfiguration.INSTANCE().baseUrl + config.get("VFDBillerCategory");
            ReturnMessage message = new ReturnMessage();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("token", VFDConfiguration.INSTANCE().token);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", VFDConfiguration.INSTANCE().token);
                    url = url.Replace(" ", "%20");
                    url = url.Replace("{credential}", VFDConfiguration.INSTANCE().credential);
                    var request = await client.GetAsync(url);
                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        string json = await request.Content.ReadAsStringAsync();
                        message.returnObject = JsonSerializer.Deserialize<BillerCategory>(json);
                        message.code = (HttpStatusCode)request.StatusCode;
                        LogHandler.WriteLog("\t|==> CALLING  GetBillerCategory \n\t|==>  RESPONSE : " + JsonSerializer.Serialize(message.returnObject), "VFD");

                    }
                    else
                    {
                        message.code = (HttpStatusCode)request.StatusCode;
                        message.message = request.Content.ReadAsStringAsync().Result;
                        LogHandler.WriteLog("\t|==> CALLING  GetBillerCategory \n\t|==>  RESPONSE : " + JsonSerializer.Serialize(message.message), "VFD");

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
    }
}
