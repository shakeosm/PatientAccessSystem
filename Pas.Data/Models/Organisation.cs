using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class Organisation : BaseEntityModel
    {
        public Organisation()
        {
            DiagnosticTestHistoryDiagnosticHospital = new HashSet<DiagnosticTestHistory>();
            DiagnosticTestHistoryDoctorHospital = new HashSet<DiagnosticTestHistory>();
            UserOrganisationRole = new HashSet<UserOrganisationRole>();
            Prescriptions = new HashSet<Prescription>();
        }

        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "varchar(500)")]
        public string Address { get; set; }

        [Required]
        [Column(TypeName = "varchar(1000)")]
        public string HeaderEnglish { get; set; }
        
        [Column(TypeName = "varchar(1000)")]
        public string HeaderBangla { get; set; }
        
        [Required]
        public int ContactPersonId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string LogoImageFile { get; set; }

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedOn { get; set; }

        public virtual User ContactPerson { get; set; }
        public virtual ICollection<DiagnosticTestHistory> DiagnosticTestHistoryDiagnosticHospital { get; set; }
        public virtual ICollection<DiagnosticTestHistory> DiagnosticTestHistoryDoctorHospital { get; set; }
        public virtual ICollection<UserOrganisationRole> UserOrganisationRole { get; set; }
        public virtual ICollection<Prescription> Prescriptions { get; set; }
    }
}
