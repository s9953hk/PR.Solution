using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace PR.Client
{
    public static class ConsoleDataInput
    {

        public static dynamic GetValue(string caption, object data)
        {

            if (data is int)
            {
                Console.WriteLine($"Wprowadź {caption} (liczba całkowita)");
                return GetInt();
            }
            else if (data is DateTime)
            {
                Console.WriteLine($"Wprowadź {caption} (data w formacie {CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern} : ");
                return GetDate();
            }
            else
            {
                Console.WriteLine($"Wprowadź {caption}");
                return GetString();
            }
        }

        private static int GetInt()
        {
            var isCorrect = false;
            while (isCorrect == false)
            {
                string inputText = Console.ReadLine();
                int inputValue;

                isCorrect = int.TryParse(inputText, out inputValue);

                if (isCorrect)
                {
                    return inputValue;
                }    
                else
                {
                    Console.WriteLine("Niepoprawna wartość, wprowadź ponownie");
                }

            }
            return 0;
     
        }

        private static DateTime GetDate()
        {
            var isCorrect = false;
            while (isCorrect == false)
            {
                string inputText = Console.ReadLine();
                DateTime inputValue;

                isCorrect = DateTime.TryParse(inputText, out inputValue);

                if (isCorrect)
                {
                    return inputValue;
                }
                else
                {
                    Console.WriteLine("Niepoprawna wartość, wprowadź ponownie");
                }

            }
            return DateTime.MinValue;

        }

        private static string GetString()
        {
            return  Console.ReadLine();
        }
    }
}
