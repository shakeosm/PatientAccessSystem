using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class Prescription
    {
        public Prescription()
        {
            PrescriptionDiagnosticTest = new HashSet<PrescriptionDiagnosticTest>();
            PrescriptionDrugs = new HashSet<PrescriptionDrugs>();
        }

        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int OrganisationId { get; set; }
        public DateTime DateCreated { get; set; }
        public string Notes { get; set; }
        public bool? IsRepeatingVisit { get; set; }
        public int? PreviousPrescriptionId { get; set; }

        public virtual ICollection<PrescriptionDiagnosticTest> PrescriptionDiagnosticTest { get; set; }
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }
        public virtual Organisation Organisation { get; set; }
        public virtual User Doctor { get; set; }
        public virtual User Patient { get; set; }

    }
}
