using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class Organisation
    {
        public Organisation()
        {
            DiagnosticTestHistoryDiagnosticHospital = new HashSet<DiagnosticTestHistory>();
            DiagnosticTestHistoryDoctorHospital = new HashSet<DiagnosticTestHistory>();
            UserOrganisationRole = new HashSet<UserOrganisationRole>();
            Prescriptions = new HashSet<Prescription>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string HeaderEnglish { get; set; }
        public string HeaderBangla { get; set; }
        public int ContactPersonId { get; set; }
        public string LogoImageFile { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual User ContactPerson { get; set; }
        public virtual ICollection<DiagnosticTestHistory> DiagnosticTestHistoryDiagnosticHospital { get; set; }
        public virtual ICollection<DiagnosticTestHistory> DiagnosticTestHistoryDoctorHospital { get; set; }
        public virtual ICollection<UserOrganisationRole> UserOrganisationRole { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
