using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PR.Patients.Model;
using PR.Patients.Services;

using System.Text.Json;
using System.Text;

namespace PR.Patients.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PatientsDataContext _context;
        private readonly ServiceBusSender _sender;

        private const string testNotificationSubject = "Ważna wiadomość z Narodowego Centrum Epidemiologicznego";
        private const string testNotificationBody = "Z przykrością informujemy że jest Pan nosicielem wirusa Covid. Prosimy o kontakt z najbliższą jednostką SANEPID. Wrsja przez Service Bus";

        public PatientsController(PatientsDataContext context, ServiceBusSender sender)
        {
            _context = context;
            _sender = sender;
        }

        [HttpGet]
        public IActionResult GetAllData()
        {
            return Ok(_context.Patients.ToList());
        }                         
        
        [HttpPost]
        public async Task<IActionResult> Add(Patient patient)
        {
            _context.Patients.Add(patient);
            _context.SaveChanges();

            await QueueMessage(new MessagePayLoad()
            {
                EventName = "NotificationEmail",
                Recipents = (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") ?
                                            "hubert.kalinowski@gmail.com" : patient.Email,
                Subject = testNotificationSubject,
                Body = testNotificationBody
            }
            );

            return Created("/api/users/" + patient.Id,patient);

        }

        private Task QueueMessage(MessagePayLoad message)
        {

           return _sender.SendMessage(message);

        }

        private Task SendMessage(MessagePayLoad message)
        {


            HttpClient client = new HttpClient();
            string messageJson = JsonSerializer.Serialize(message);

            return client.PostAsync("https://localhost:5002/api/email",
                    new StringContent( messageJson, Encoding.UTF8, "application/json"));

        }
    }
}