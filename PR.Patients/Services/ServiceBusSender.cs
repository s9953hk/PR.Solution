using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR.Patients.Services
{
    public class ServiceBusSender
    {
        private readonly QueueClient _queueClient;
        private const string QueueName = "prmessages";

        public ServiceBusSender(IConfiguration configuration)
        {
            _queueClient = new QueueClient(configuration.GetConnectionString("ServiceBusConnectionString"),
                QueueName);
        }

        public async Task SendMessage(MessagePayLoad payLoad)
        {
            string data = JsonConvert.SerializeObject(payLoad);
            Message message = new Message(Encoding.UTF8.GetBytes(data));

            await _queueClient.SendAsync(message);
        }

    }

    public class MessagePayLoad
    {
        public string EventName { get; set; }
        public string Recipents { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
