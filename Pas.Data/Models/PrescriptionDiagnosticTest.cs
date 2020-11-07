using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class PrescriptionDiagnosticTest
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int DiagnosticTestId { get; set; }
        public string Instructions { get; set; }

        public virtual DiagnosticTest DiagnosticTest { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
