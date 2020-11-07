using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class Doctors
    {
        public Doctors()
        {
            DiagnosticTestHistory = new HashSet<DiagnosticTestHistory>();
        }

        public int Id { get; set; }
        public string FullName { get; set; }
        public string BanglaName { get; set; }
        public int SpecialityId { get; set; }
        public string Acheivements { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }

        public virtual DoctorSpeciality Speciality { get; set; }
        public virtual ICollection<DiagnosticTestHistory> DiagnosticTestHistory { get; set; }
    }
}
