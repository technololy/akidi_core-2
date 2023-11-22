using BackEndServices.Models;
using BackEndServices.Services;
using BackEndServices.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ActionNameAttribute = Microsoft.AspNetCore.Mvc.ActionNameAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using FromBodyAttribute = System.Web.Http.FromBodyAttribute;
using HttpPutAttribute = Microsoft.AspNetCore.Mvc.HttpPutAttribute;

namespace BackEndServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        AuthenticationService authenticationService = new AuthenticationService();

        [HttpPost]
        [Route("api/authenticate")]
        //public HttpResponseMessage Authenticate([FromBody] UserCredentialsEnc credentialsEnc)
        public AuthenticateResponse Authenticate([FromBody] UserCredentials userCredentials)
        {

            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new AuthenticateResponse() { code = 1010, customerInfos = null, message = "Invalid Data" };
            }

            //try catch
            AuthenticateResponse authenticateResponse = new AuthenticateResponse();
            try
            {
                //byte[] plainBytes = Encoding.UTF8.GetBytes(credentialsEnc.payload);
                //byte[] plainBytes = Encoding.UTF8.GetBytes("zkGQPcBRAJxGmL5zwBBq+XwKA7C3ePz5F7wziOkUm8URbImN92XjKUm+EHq3Imym");
                //UserCredentials userCredentials = EncryptionService.DecryptJsonData<UserCredentials>(credentialsEnc.payload);
                //dynamic userCredentialss = EncryptionService.Decrypt<Object>(plainBytes);
                //UserCredentials userCredentials = EncryptionService.DecryptJsonDatas<UserCredentials>(plainBytes);
                Utils.Utils.SaveLog("AuthenticationController", "Authentication/api/authenticate", JsonConvert.SerializeObject(userCredentials));
                authenticateResponse = authenticationService.IsValidCredentials(userCredentials);
            }
            catch (Exception ex)
            {
                authenticateResponse = new AuthenticateResponse() { code = 1010, message = ex.Message, customerInfos = null };
                return authenticateResponse;

                throw;
            }
            
            //return new HttpResponseMessage(HttpStatusCode.OK) { RequestMessage = Request, ReasonPhrase = JsonConvert.SerializeObject(authenticateResponse) };


            //if (authenticateResponse != null)
            //{
            //    authenticateResponse = new AuthenticateResponse() { code = authenticateResponse.code, message = authenticateResponse.message, customerInfos = authenticateResponse.customerInfos };
            //    return new HttpResponseMessage(HttpStatusCode.OK) { RequestMessage = Request, ReasonPhrase = JsonConvert.SerializeObject(authenticateResponse) };
            //}


            authenticateResponse = new AuthenticateResponse() { code = authenticateResponse.code, message = authenticateResponse.message, customerInfos = authenticateResponse.customerInfos };
            return authenticateResponse;
        }



        [HttpPut]
        [Route("api/reset-password")]
        public AuthenticateResponse resetUserPassword([FromBody] UserPasswordEditPayload userPasswordEditPayload)
        {

            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new AuthenticateResponse() { code = 1010, customerInfos = null, message = "Invalid Data" };
            }
            //try catch
            AuthenticateResponse authenticateResponse = new AuthenticateResponse();
            try
            {
                Utils.Utils.SaveLog("AuthenticationController", "Authentication/api/reset-password", JsonConvert.SerializeObject(userPasswordEditPayload));
                authenticateResponse = authenticationService.resetUserPassword(userPasswordEditPayload);
            }
            catch (Exception ex)
            {
                authenticateResponse = new AuthenticateResponse() { code = 1010, message = ex.Message, customerInfos = null };
                return authenticateResponse;

                throw;
            }

            authenticateResponse = new AuthenticateResponse() { code = authenticateResponse.code, message = authenticateResponse.message, customerInfos = authenticateResponse.customerInfos };
            return authenticateResponse;
        }










    }
}
