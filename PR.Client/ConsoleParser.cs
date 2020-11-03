using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR.Client
{

    public static class ConsoleParser
    {
        public static ICommand Parse(string commandString)
        {

            var commandParts = commandString.Split(' ').ToList();
            var commandName = commandParts[0];
            var arg = commandParts.Count>1?commandParts[1]:"";

            switch (commandName)
            {

                case "koniec": return new ExitCommand();
                case "pomoc": return new HelpCommand();
                case "nowy": return new NewPatientCommand(arg);
                case "lista": return new ListCommand(arg);

                default: return new UnrecognizedCommand();

            }
        }

    }

    public interface ICommand
    {
        Task<bool> Execute();
    }

    public class ExitCommand : ICommand
    {
        public Task<bool> Execute()
        {
            return Task.FromResult(true);
        }
    }

    public class UnrecognizedCommand : ICommand
    {
        public Task<bool> Execute()
        {
            Console.WriteLine("Niepoprawne polecenie");
            return Task.FromResult(false);
        }
    }

    public class HelpCommand : ICommand
    {
        public Task<bool> Execute()
        {
            Console.WriteLine("pomoc  - wyświetla dostępne polecenia");
            Console.WriteLine("nowy pacjent   - wprowadzanie danych nowego pacjenta");
            Console.WriteLine("lista pacjentów  - wyświetla listę pacjentów");
            Console.WriteLine("koniec - zakończenie pracy z programem");
            return Task.FromResult(false);
        }
    }

    public class NewPatientCommand : ICommand
    {
        string objectName;
        public NewPatientCommand(string objectName)
        {
            this.objectName = objectName;
        }
        public async Task<bool> Execute()
        {
            if (!string.IsNullOrEmpty(objectName))
            {

                IItem newObject =  ObjectFactory.CreateObject(objectName);             
            }
            else
            {
                Console.WriteLine("Niepoprawne polecenie");
            }
            return false;
        }
    }


    public class ListCommand : ICommand
    {
        string objectName;
        public ListCommand(string objectName)
        {
            this.objectName = objectName;
        }
        public async Task<bool> Execute()
        {
            if (!string.IsNullOrEmpty(objectName))
            {

                ObjectFactory.ListObjects(objectName);
            }
            else
            {
                Console.WriteLine("Niepoprawne polecenie");
            }
            return false;
        }
    }

}
