using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PR.Client
{
    public static class ObjectFactory
    {
        public static IItem CreateObject(string objectName)
        {
            switch (objectName)
            {

                case "pacjent": return (IItem)CreatePatient();
                default: return null;
            }

        }

        public static void ListObjects(string objectName)
        {
            switch (objectName)
            {
                case "pacjentów": 
                    ListPatients();
                    return;
                default: 
                    return;
     
            }

        }
        private static Patient CreatePatient()
        {
            Patient patient = new Patient();

            patient.Age = ConsoleDataInput.GetValue("wiek",patient.Age);
            patient.Email = ConsoleDataInput.GetValue("email", patient.Email);
            patient.First_Name = ConsoleDataInput.GetValue("imię", patient.First_Name);
            patient.Last_Name = ConsoleDataInput.GetValue("nazwisko", patient.Last_Name);
            patient.Test_Date = ConsoleDataInput.GetValue("datę testu", patient.Test_Date);


            PatientRepository patientRepository = new PatientRepository();

            Task task=  patientRepository.Insert(patient);

            task.Wait();

            return patient;

        }

        private static void ListPatients()
        {
            PatientRepository patient = new PatientRepository();

            Task task = patient.PrintAll();

            task.Wait();

        }
    }



}
