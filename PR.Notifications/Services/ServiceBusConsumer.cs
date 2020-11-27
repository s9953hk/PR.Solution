using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using System.Text;
using PR.Notifications.Model;
using System.Text.RegularExpressions;
using Serilog;

namespace PR.Notifications.Services
{
    public class ServiceBusConsumer
    {
        private readonly QueueClient _queueClient;
        
        //#4
        private readonly ILogger _logger;

        private const string QueueName = "prmessages";

        public ServiceBusConsumer(IConfiguration configuration, ILogger logger /*#4*/)
        {
            _queueClient = new QueueClient(configuration.GetConnectionString("ServiceBusConnectionString"), QueueName);
            _logger = logger;
        }


        public void Register()
        {
            var option = new MessageHandlerOptions(e => Task.CompletedTask) { AutoComplete = false };


            _queueClient.RegisterMessageHandler(ProcessMessage, option);
        }

        private async Task ProcessMessage(Message message, CancellationToken token)
        {

            try
            {
                var payLoad = JsonConvert.DeserializeObject<MessagePayLoad>(Encoding.UTF8.GetString(message.Body));


                if (payLoad.EventName == "NotificationEmail")
                {


                    //#4 >>
                    string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                        + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                        + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                    Regex emailValidator = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

                    bool validEmail = emailValidator.IsMatch(payLoad.Recipents);
                    //#4 <<

                    //#4 >>
                    if (!validEmail)
                    {
                        await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
                        throw new InvalidOperationException("Notification email sender - błędny email:" + payLoad.Recipents);
                    }
                    //#4 <<


                    EmailSender emailSender = new EmailSender();


                    ////ACHTUNG MINEN - zamienić klasę EmailMessageRequest na MessagePayload
                    //MessagePayLoad
                    //MessagePayLoad emailMessage = new EmailMessageRequest()
                    //{
                    //    Body = payLoad.Body,
                    //    Recipents = payLoad.Recipents,
                    //    Subject = payLoad.Subject
                    //};

                    emailSender.SendNewUserEmail(payLoad);

                }

                await _queueClient.CompleteAsync(message.SystemProperties.LockToken);

            }

            catch (Exception e)
            {
                //#4 >>
                _logger.Error(e,e.Message);
                throw;
                //#4 <<
            }
        }

    }

}
