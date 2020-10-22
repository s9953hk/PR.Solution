using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;

namespace PR.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {

            HttpClient client = new HttpClient();

            Patients u = new Patients()

            {
                FirstName = "Test",
                LastName = "User1"
            };

            string userJson = JsonSerializer.Serialize(u);

            await client.PostAsync("https://localhost:44316/api/patients",
                new StringContent(userJson, Encoding.UTF8, "application/json"));
        }
    }

    public class Patients
    {
        //[Column("Id")] // ???
        public int Id { get; set; }
        //[Column("FirstName")]
        public String FirstName{ get; set; }
        //[Column("LastName")]
        public String LastName { get; set; }
        //[Column("Age")]
        public int Age { get; set; }
        //[Column("PositiveTestResultDate")]
        public String PositiveTestResultDate { get; set; }
    }
}
