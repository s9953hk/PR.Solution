using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PR.Patients.Model
{
    public class Patient
    {

        public int Id{ get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int Age { get; set; }
        public DateTime Test_Date { get; set; }
        public string Email { get; set; }

    }
}
