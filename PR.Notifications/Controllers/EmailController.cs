using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PR.Notifications.Model;

namespace PR.Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {

        [HttpGet]
        public void DummyGet()
        {
            Ok();
        }

        [HttpPost]
        public void Send(EmailMessageRequest message)
        {

            //string messageFrom = "s9953hk@gmail.com";

            //var smtpClient = new SmtpClient("smtp.gmail.com")
            //{
            //    Port = 587,
            //    Credentials = new NetworkCredential(messageFrom, "@nt@lis12"),
            //    EnableSsl = true,
            //};


            //smtpClient.Send(messageFrom,message.Recipents,message.Subject,message.Body);

            EmailSender emailSender = new EmailSender();

            emailSender.SendNewUserEmail(message);


        }
    }
}