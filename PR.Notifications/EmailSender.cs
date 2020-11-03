using PR.Notifications.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PR.Notifications
{
    public class EmailSender
    {

        public void SendNewUserEmail(EmailMessageRequest message)
        {
            string messageFrom = "s9953hk@gmail.com";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(messageFrom, "@nt@lis12"),
                EnableSsl = true,
            };


            smtpClient.Send(messageFrom, message.Recipents, message.Subject, message.Body);

        }
    }
}
