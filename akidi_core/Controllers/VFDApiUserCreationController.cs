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
    public class VFDApiUserCreationController : ControllerBase
    {

        [HttpPost]
        [Route("/corporate-account")]

        public IActionResult CreateCorporateUser(VFDCorporateAccount corporateAccount)
        {
            try
            {

                ReturnMessage message = new VFDAccountCreation().createCorporateAccount(corporateAccount).Result;

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



        [HttpPost]
        [Route("/individual-account")]

        public IActionResult CreateIndividualUser(VFDIndividualAccount individualAccount)
        {
            try
            {

                ReturnMessage message = new VFDAccountCreation().createIndividualAccount(individualAccount).Result;

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



        [HttpPost]
        [Route("/virtual-account")]

        public IActionResult CreateVirtualUser(VFDVirtualAccount virtualAccount)
        {
            try
            {

                ReturnMessage message = new VFDAccountCreation().createVirtualAccount(virtualAccount).Result;

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


        [HttpPost]
        [Route("/release-account")]

        public IActionResult CreateReleaseAccount(VFDReleaseAccount releaseAccount)
        {
            try
            {

                ReturnMessage message = new VFDAccountCreation().releaseAccount(releaseAccount).Result;

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
    }
}
