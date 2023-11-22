
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using akidi_core.Models.VFD;
using akidi_core.Services.VFD;
using BackEndServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace akidi_core.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class VFDApiController  : ControllerBase
    {
        [HttpPost]
        [Route("/onboard")]

        public IActionResult OnBoardCustomer(CustomerOnboardInformation customer)
        {
            try
            {
                ReturnMessage message = new VFDOnboardingService().SetupAccount(customer);

                if (message.code ==  (HttpStatusCode) ReturnedCode.UNAUTHORIZED)
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


        [HttpGet]
        [Route("/generate-token")]
        public IActionResult GetToken()
        {
            try
            {

                ReturnMessage message = new VFDAuthentification().GetToken();

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


        [HttpGet]
        [Route("/get-biller-by-category/{category}")]
        public IActionResult GetBillerByCategory(string category)
        {
            try
            {

                ReturnMessage message = new VFDBillerCategory().GetBillerByCategory(category).Result;

                if (message.code ==  (HttpStatusCode) ReturnedCode.UNAUTHORIZED)
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

    }
}
