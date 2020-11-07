using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class DiagnosticTest
    {
        public DiagnosticTest()
        {
            DiagnosticTestHistory = new HashSet<DiagnosticTestHistory>();
            PrescriptionDiagnosticTest = new HashSet<PrescriptionDiagnosticTest>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual ICollection<DiagnosticTestHistory> DiagnosticTestHistory { get; set; }
        public virtual ICollection<PrescriptionDiagnosticTest> PrescriptionDiagnosticTest { get; set; }
    }
}
