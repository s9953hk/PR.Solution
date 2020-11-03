using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Net.Http;


namespace PR.Client
{
    public class Patient : IItem
    {

        public int Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int Age { get; set; }
        public DateTime Test_Date { get; set; }
        public string Email { get; set; }

        public override string ToString()
        {
            string patientDescriptor = $"Id: {Id}, Imię i nazwisko: {First_Name + " " + Last_Name}, "
                                        + $" Wiek: {Age}, Data testu: {Test_Date}, Email: {Email}";

            return patientDescriptor;
        }
    }

    public interface IItem
    {

        string ToString();
    }
}
