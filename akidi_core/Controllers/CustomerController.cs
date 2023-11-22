using BackEndServices.Models;
using BackEndServices.Services;
using BackEndServices.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;
using FromBodyAttribute = System.Web.Http.FromBodyAttribute;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;

namespace BackEndServices.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        AuthenticationService authenticationService = new AuthenticationService();
        CustomerService customerService = new CustomerService();
        PinHashingService PinHashingService = new PinHashingService();


        [HttpPost]
        [Route("api/register-customer")]
        public ReturnMessage Register([FromBody] Customer person)
        {

            if (!ModelState.IsValid)
            {
                var errorList = (from item in ModelState.Values
                                 from error in item.Errors
                                 select error.ErrorMessage).ToArray();
                return new ReturnMessage() { code = System.Net.HttpStatusCode.BadRequest, returnObject = null, message = "Invalid Data" };
            }

            person.password = Utils.Utils.CalculateSHA256(person.password);
            //Please encrypt pin before passing 
            String responPin = PinHashingService.SavePinCode(person.pinCode);

            Customer customerResponse = new Customer();

            string[] resApires = responPin.Split('|');
            person.saltedPin = resApires[0].ToString();
            person.hashedPin = resApires[1].ToString();

            Utils.Utils.SaveLog("CustomerController", "Customer/api/register-customer", JsonConvert.SerializeObject(person));
            ReturnMessage resp = authenticationService.Register(person);

            return resp;
        }


        [HttpGet]
        [Route("api/get-banks")]

        public ReturnMessage GetBanks()
        {
            Utils.Utils.SaveLog("CustomerController", "Customer/api/get-banks", null);
            ReturnMessage message = new CommonService().getAllBanks();

            return message;
        }





    }
}
