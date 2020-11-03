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
        public static async Task Main(string[] args)
        {

            var command = ConsoleParser.Parse("pomoc");
            await command.Execute();

            var exit = false;
            while (exit == false)
            {
                Console.WriteLine();
                Console.WriteLine("Wprowadź polecenie: ");

                command = ConsoleParser.Parse(Console.ReadLine());
                exit = await command.Execute();
            }

        }
    }
}
