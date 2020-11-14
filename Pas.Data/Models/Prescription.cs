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
        }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int HospitalId { get; set; }

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime DateCreated { get; set; }

        [Column(TypeName = "varchar(500)")]
        public string Notes { get; set; }
        public bool? IsRepeatingVisit { get; set; }
        public int? PreviousPrescriptionId { get; set; }

        public virtual ICollection<PrescriptionDiagnosticTest> PrescriptionDiagnosticTest { get; set; }
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }
        public virtual Organisation Hospital { get; set; }
        public virtual User Doctor { get; set; }
        public virtual User Patient { get; set; }

    }
}
