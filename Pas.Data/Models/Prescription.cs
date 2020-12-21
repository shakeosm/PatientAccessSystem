using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class Prescription : BaseEntityModel
    {
        public Prescription()
        {
            PrescriptionDiagnosticTest = new HashSet<PrescriptionDiagnosticTest>();
            PrescriptionDrugs = new HashSet<PrescriptionDrugs>();
            PatientIndications = new HashSet<PatientIndications>();
            PrescriptionExaminations = new HashSet<PrescriptionExamination>();
            PrescriptionInvestigations = new HashSet<PrescriptionInvestigation>();
        }

        //[ForeignKey("Patient")]
        [Required]
        public int PatientId { get; set; }

        //[ForeignKey("Doctor")]
        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int HospitalId { get; set; }

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public string Notes { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Plans { get; set; }

        [Column(TypeName = "nvarchar(500)")]
        public string Advise { get; set; }
        
        public bool? IsRepeatingVisit { get; set; }
        
        public int Status { get; set; }

        public virtual ICollection<PrescriptionDiagnosticTest> PrescriptionDiagnosticTest { get; set; }
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }
        public virtual ICollection<PatientIndications> PatientIndications { get; set; }
        public virtual ICollection<PrescriptionExamination> PrescriptionExaminations { get; set; }
        public virtual ICollection<PrescriptionInvestigation> PrescriptionInvestigations { get; set; }
        public virtual Organisation Hospital { get; set; }
        public virtual User Doctor { get; set; }
        public virtual User Patient { get; set; }        
        public virtual VitalsHistory VitalsHistory { get; set; }


    }
}
