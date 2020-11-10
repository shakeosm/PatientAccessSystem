using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class DiagnosticTest : BaseEntityModel
    {
        public DiagnosticTest()
        {
            DiagnosticTestHistory = new HashSet<DiagnosticTestHistory>();
            PrescriptionDiagnosticTest = new HashSet<PrescriptionDiagnosticTest>();
        }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Details { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime DateCreated { get; set; }

        public virtual ICollection<DiagnosticTestHistory> DiagnosticTestHistory { get; set; }
        public virtual ICollection<PrescriptionDiagnosticTest> PrescriptionDiagnosticTest { get; set; }
    }
}
