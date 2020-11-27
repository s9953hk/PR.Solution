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
using Microsoft.AspNetCore.Authorization;
using IdentityModel.Client;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace PR.Patients.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] //#3
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
        //[Authorize] //#3
        //[AllowAnonymous] //#3
        public IActionResult GetAllData()
        {
            return Ok(_context.Patients.ToList());
        }

        //#4
        [AllowAnonymous]
        [HttpPut]
        public IActionResult InvalidAction()
        {
            throw new InvalidOperationException("Symulowany problem z aplikacją");
        }
        
        [HttpPost]
        //[Authorize] //#3
        public async Task<IActionResult> Add(Patient patient)
        {

            //#4 >>
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            Regex emailValidator = new Regex(validEmailPattern, RegexOptions.IgnoreCase);

            bool validEmail = emailValidator.IsMatch(patient.Email);
            //#4 <<

            _context.Patients.Add(patient);
            _context.SaveChanges();


            //#3 >> ACHTUNG MINEN - tymczasowo wylączona kolejka
            //#4 Kolejka ponownie włączona
            await QueueMessage(new MessagePayLoad()
                {
                    EventName = "NotificationEmail",

                    Recipents = patient.Email,

                    Subject = testNotificationSubject,
                    Body = testNotificationBody
                }
            );



            //await SendMessage(new MessagePayLoad()
            //{
            //    EventName = "NotificationEmail",

            //    Recipents = patient.Email,

            //    Subject = testNotificationSubject,
            //    Body = testNotificationBody
            //}
            //);

            //#4 >>
            if (!validEmail)
            {
                throw new InvalidOperationException("Wystąpił błąd podczas dodawania pacjenta - błędny email: " + patient.Email);
            }
            //#4 <<

            return Created("/api/users/" + patient.Id,patient);

        }

        private Task QueueMessage(MessagePayLoad message)
        {

           return _sender.SendMessage(message);

        }

        private async Task SendMessage(MessagePayLoad message)
        {


            HttpClient client = new HttpClient();
            string messageJson = JsonSerializer.Serialize(message);

            string token = await GetToken();

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);

            client.PostAsync("https://localhost:5002/api/email",
                    new StringContent( messageJson, Encoding.UTF8, "application/json"));

        }

        private static async Task<string> GetToken()
        {
            using var client = new HttpClient();

            DiscoveryDocumentResponse disco = await client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest()
            {
                Address = "https://login.microsoftonline.com/146ab906-a33d-47df-ae47-fb16c039ef96/v2.0/",
                Policy =
                {
                ValidateEndpoints = false
                }
            } );


            if (disco.IsError)
                throw new InvalidOperationException($"No discovery document. Details: { disco.Error}");

            var tokenRequest = new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "67dd9cfb-4344-4cc8-a2ca-573f6bb4422f",
                ClientSecret = "tlVkd6QAX-kcl.XP_4Yslh00-2kPS6G_9_",
                Scope = "api://67dd9cfb-4344-4cc8-a2ca-573f6bb4422f/.default"
            } ;

            var token = await client.RequestClientCredentialsTokenAsync(tokenRequest);

            if (token.IsError)
                throw new InvalidOperationException($"Couldn't gather token. Details: { token.Error}");

            return $"{ token.TokenType} { token.AccessToken}";
        }
    }
}