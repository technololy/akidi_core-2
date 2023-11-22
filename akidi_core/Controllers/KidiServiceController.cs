using akidi_core.Utils;
using BackEndServices.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace akidi_core.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KidiServiceController : ControllerBase
    {
        EmailHandler emailHandler = new EmailHandler();
         
        [HttpPost]
        [Route("api/send-mail")]
        public void sendEmail()
        {
            emailHandler.sendGmailEmailInternal();

        }

    }
}
