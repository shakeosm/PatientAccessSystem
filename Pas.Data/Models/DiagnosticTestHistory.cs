using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class DiagnosticTestHistory : BaseEntityModel
    {
        
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DiagnosticTestId { get; set; }

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime TestDate { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public int DoctorHospitalId { get; set; }

        [Required]
        public int DiagnosticHospitalId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(1000)")]
        public string Result { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string TestReportImageUrl { get; set; }

        public virtual Organisation DiagnosticHospital { get; set; }
        public virtual DiagnosticTest DiagnosticTest { get; set; }
        public virtual DoctorProfile Doctor { get; set; }
        public virtual Organisation DoctorHospital { get; set; }
    }
}
