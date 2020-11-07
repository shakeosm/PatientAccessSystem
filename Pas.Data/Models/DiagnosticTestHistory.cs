using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class DiagnosticTestHistory
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DiagnosticTestId { get; set; }
        public DateTime TestDate { get; set; }
        public int DoctorId { get; set; }
        public int DoctorHospitalId { get; set; }
        public int DiagnosticHospitalId { get; set; }
        public string Result { get; set; }
        public string TestReportImageUrl { get; set; }

        public virtual Organisation DiagnosticHospital { get; set; }
        public virtual DiagnosticTest DiagnosticTest { get; set; }
        public virtual Doctors Doctor { get; set; }
        public virtual Organisation DoctorHospital { get; set; }
    }
}
