using akidi_core.Models.VFD;
using akidi_core.Services.VFD;
using BackEndServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace akidi_core.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class VFDKycEnquiryController : ControllerBase
    {

        [HttpPost]
        [Route("/verify-phone")]

        public IActionResult PhoneVerification(PhoneVerification phone)
        {
            try
            {

                ReturnMessage message = new VFDKYCEnquiry().phoneVerification(phone).Result;

                if (message.code == (HttpStatusCode)ReturnedCode.UNAUTHORIZED)
                {
                    return Unauthorized(new ReturnError() { error = 401, title = message.message });
                }

                if (message.code == (HttpStatusCode)ReturnedCode.OK)
                {
                    return Ok(message);
                }
                return StatusCode((int)message.code, new ReturnError() { error = (int)message.code, title = message.message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ReturnError() { error = 500, title = ex.Message });
            }
        }


        [HttpGet]
        [Route("/get-client/{bvn}")]

        public IActionResult GetClientUsingBvn(string bvn)
        {
            try
            {

                ReturnMessage message = new VFDKYCEnquiry().getClientUsingBvn(bvn).Result;

                if (message.code == (HttpStatusCode)ReturnedCode.UNAUTHORIZED)
                {
                    return Unauthorized(new ReturnError() { error = 401, title = message.message });
                }

                if (message.code == (HttpStatusCode)ReturnedCode.OK)
                {
                    return Ok(message);
                }
                return StatusCode((int)message.code, new ReturnError() { error = (int)message.code, title = message.message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ReturnError() { error = 500, title = ex.Message });
            }
        }


        [HttpGet]
        [Route("/get-client-consent/{bvn}")]

        public IActionResult GetClientByBvnConsent(string bvn)
        {
            try
            {

                ReturnMessage message = new VFDKYCEnquiry().BvnConsent(bvn).Result;

                if (message.code == (HttpStatusCode)ReturnedCode.UNAUTHORIZED)
                {
                    return Unauthorized(new ReturnError() { error = 401, title = message.message });
                }

                if (message.code == (HttpStatusCode) ReturnedCode.OK)
                {
                    return Ok(message);
                }
                return StatusCode((int)message.code, new ReturnError() { error = (int)message.code, title = message.message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ReturnError() { error = 500, title = ex.Message });
            }
        }



        [HttpPost]
        [Route("/get-client-subaccount")]

        public IActionResult GetClientBySubAccount(VFDSubAccount client)
        {
            try
            {

                ReturnMessage message = new VFDAccountEnquiry().getClientSubAccount(client).Result;

                if (message.code == (HttpStatusCode) ReturnedCode.UNAUTHORIZED)
                {
                    return Unauthorized(new ReturnError() { error = 401, title = message.message });
                }

                if (message.code == (HttpStatusCode)ReturnedCode.OK)
                {
                    return Ok(message);
                }
                return StatusCode((int)message.code, new ReturnError() { error = (int)message.code, title = message.message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ReturnError() { error = 500, title = ex.Message });
            }
        }




        [HttpPost]
        [Route("/get-client-inwar")]

        public IActionResult GetInwarInquiry(VFDSubAccount client)
        {
            try
            {

                ReturnMessage message = new VFDAccountEnquiry().getInwarInquiry(client).Result;

                if (message.code == (HttpStatusCode) ReturnedCode.UNAUTHORIZED)
                {
                    return Unauthorized(new ReturnError() { error = 401, title = message.message });
                }

                if (message.code == (HttpStatusCode)ReturnedCode.OK)
                {
                    return Ok(message);
                }
                return StatusCode((int)message.code, new ReturnError() { error = (int)message.code, title = message.message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ReturnError() { error = 500, title = ex.Message });
            }
        }
    }
}
