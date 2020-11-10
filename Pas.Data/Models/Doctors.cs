using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class Doctors : BaseEntityModel
    {
        public Doctors()
        {
            DiagnosticTestHistory = new HashSet<DiagnosticTestHistory>();
        }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string FullName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string BanglaName { get; set; }

        [Required]
        public int SpecialityId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Acheivements { get; set; }

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime DateCreated { get; set; }

        [Required]
        public int CreatedBy { get; set; }

        public virtual DoctorSpeciality Speciality { get; set; }
        public virtual ICollection<DiagnosticTestHistory> DiagnosticTestHistory { get; set; }
    }
}
