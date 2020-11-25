using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class DoctorProfile
    {
        public DoctorProfile()
        {
            DiagnosticTestHistory = new HashSet<DiagnosticTestHistory>();
            Specialities = new HashSet<DoctorSpeciality>();
            DoctorMedicalDegrees = new HashSet<DoctorMedicalDegrees>();
        }

        [Required]
        public int Id { get; set; }

        [Required]
        public int SpecialityId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Acheivements { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public string RegistrationNumber { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string AdditionalInfo { get; set; }

        [Column(TypeName = "varchar(300)")]
        public string HeaderEnglish { get; set; }

        [Column(TypeName = "nvarchar(300)")]
        public string HeaderBangla { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime DateCreated { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public virtual ICollection<DoctorSpeciality> Specialities { get; set; }
        public virtual ICollection<DoctorMedicalDegrees> DoctorMedicalDegrees { get; set; }
        public virtual ICollection<DiagnosticTestHistory> DiagnosticTestHistory { get; set; }
    }
}
