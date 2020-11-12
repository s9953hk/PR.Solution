using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Net.Http;

using Newtonsoft.Json;
using Microsoft.Identity.Client;

namespace PR.Client
{
    public class PatientRepository : IRepository
    {

        public async Task PrintAll()
        {

            Console.WriteLine("Pobieram pacjentów ....");

            List <Patient> patients = (List < Patient >) await this.GetAll();

            if (!(patients is null))
            {
                foreach (Patient patient in patients)
                {
                    Console.WriteLine(patient.ToString());
                }

                Console.WriteLine("Koniec listy pacjentów");
            }
        }


        public async Task<IEnumerable<IItem>> GetAll()
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //#3 >>>
            string token = await Auth.GetToken();

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token);
            //#3 <<<

            HttpResponseMessage responseMessage = null;

            try
            {
                //responseMessage = await client.GetAsync("https://localhost:44317/api/patients");
                responseMessage = await client.GetAsync("https://localhost:5005/api/patients");

            }
            catch (HttpRequestException e)
            {

                Console.WriteLine("Błąd połączenia z serwisem. Nie można pobrać listy pacjentów.");

            }

            if (!(responseMessage is null))
            { 
                var responseContent = await responseMessage.Content.ReadAsStringAsync();

                List<Patient> patients = JsonConvert.DeserializeObject<List<Patient>>(responseContent);

                return patients;
            }
            else
            {
                return null;
            }
        }
        public async Task<IItem> GetByID(int itemId)
        {
            return null;
        }
        public async Task Insert(IItem item)
        {
            HttpClient client = new HttpClient();

            //#3 >>>
            string token = await Auth.GetToken();

            client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse("Bearer " + token);
            //#3 <<<

            string userJson = JsonConvert.SerializeObject(item);

            Console.WriteLine("Dodaję pacjenta do bazy......");

            bool exit = true;

            do
            {

                try
                {
                    await client.PostAsync("https://localhost:5005/api/patients",
                    new StringContent(userJson, Encoding.UTF8, "application/json"));

                    Console.WriteLine("Pacjent dodany");
                }

                catch (HttpRequestException e)
                {

                    Console.WriteLine("Błąd połączenia z serwisem. Nie można dodać pacjenta. Czy chcesz spróbować jeszcze raz ? (t/n)");

                    ConsoleKeyInfo inputResult = Console.ReadKey();
                    Console.WriteLine(Environment.NewLine);

                    while (!(inputResult.Key == ConsoleKey.T || inputResult.Key == ConsoleKey.N))
                    {
                        Console.WriteLine("Nieprawidłowy wybór, wciśnij klawisz t lub n.");
                        inputResult = Console.ReadKey();
                        Console.WriteLine(Environment.NewLine);
                    }

                    exit = (inputResult.Key == ConsoleKey.N);

                }
            }
            while (!exit);

            
        }

        public async Task Delete(int itemId)
        {
            return;
        }
        public async Task Update(IItem item)
        {
            return;
        }
        public async Task Save()
        {
            return;
        }
    }

    public interface IRepository
    {
        Task<IEnumerable<IItem>> GetAll();
        Task<IItem> GetByID(int itemId);
        Task Insert(IItem item);
        Task PrintAll();
        Task Delete(int itemId);
        Task Update(IItem item);
        Task Save();
    }
}
