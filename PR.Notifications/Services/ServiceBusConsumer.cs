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

namespace PR.Notifications.Services
{
    public class ServiceBusConsumer
    {
        private readonly QueueClient _queueClient;

        private const string QueueName = "prmessages";

        public ServiceBusConsumer(IConfiguration configuration)
        {
            _queueClient = new QueueClient(configuration.GetConnectionString("ServiceBusConnectionString"),
                QueueName);
        }


        public void Register()
        {
            var option = new MessageHandlerOptions(e => Task.CompletedTask) { AutoComplete = false };


            _queueClient.RegisterMessageHandler(ProcessMessage, option);
        }

        private async Task ProcessMessage(Message message, CancellationToken token)
        {
            var payLoad = JsonConvert.DeserializeObject<MessagePayLoad>(Encoding.UTF8.GetString(message.Body));


            if(payLoad.EventName=="NotificationEmail")
            {

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

    }

}
