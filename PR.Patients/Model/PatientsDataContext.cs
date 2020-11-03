using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PR.Patients.Model
{
    public class PatientsDataContext :DbContext
    {
        public PatientsDataContext(DbContextOptions options): base(options)
        {

        }
        public DbSet<Patient> Patients { get; set; }
    }
}
