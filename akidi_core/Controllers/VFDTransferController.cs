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
    public class VFDTransferController : ControllerBase
    {
        [HttpPost]
        [Route("/transfer")]

        public IActionResult Transfer(VFDTransfer transfer)
        {
            try
            {

                ReturnMessage message = new VFDTransferService().Transfer(transfer).Result;

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
        [Route("/generate-qr")]

        public IActionResult GenerateQrCode(VFDQrCode qrCode)
        {
            try
            {

                ReturnMessage message = new VFDTransferService().GenerateQrCode(qrCode).Result;

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
        [Route("/pay-qr")]

        public IActionResult PayWithQr(VFDPayQr qrCode)
        {
            try
            {

                ReturnMessage message = new VFDTransferService().PayByQrCode(qrCode).Result;

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
        [Route("/retrieve-qr")]

        public IActionResult RetrieveQrCode(string qrCode)
        {
            try
            {

                ReturnMessage message = new VFDTransferService().RetrieveQrCode(qrCode).Result;

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
        [Route("/credit")]

        public IActionResult Credit(VFDCredit credit)
        {
            try
            {

                ReturnMessage message = new VFDTransferService().Credit(credit).Result;

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
        [Route("/beneficiary-enquiry/{accountNumb}/{bank}")]

        public IActionResult BeneficiaryEnquiry(string accountNumb, string bank)
        {
            try
            {

                ReturnMessage message = new VFDTransferService().BeneficiaryEnquiry(accountNumb, bank).Result;

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
        [Route("/account-enquiry/{accountNumber}")]

        public IActionResult AccountEnquiry(string accountNumber)
        {
            try
            {

                ReturnMessage message = new VFDTransferService().AccountEnquiry(accountNumber).Result;

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
        [Route("/banks")]

        public IActionResult GetBanks()
        {
            try
            {

                ReturnMessage message = new VFDTransferService().GetBanks().Result;

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
        [Route("/transaction/{reference}")]

        public IActionResult GetTransactions(string reference)
        {
            try
            {

                ReturnMessage message = new VFDTransferService().GetTransactions(reference).Result;

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
        [Route("/tsq/{reference}")]

        public IActionResult GetReversalTsq(string reference)
        {
            try
            {

                ReturnMessage message = new VFDTransferService().GetReversalTsq(reference).Result;

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

    }
}
