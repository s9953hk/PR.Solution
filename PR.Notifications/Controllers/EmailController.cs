using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PR.Notifications.Model;

namespace PR.Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //#3
    public class EmailController : ControllerBase
    {

        [HttpGet]
        public void DummyGet()
        {
            Ok();
        }

        [HttpPost]
        public void Send(MessagePayLoad message)
        {

            EmailSender emailSender = new EmailSender();

            emailSender.SendNewUserEmail(message);

        }
    }
}