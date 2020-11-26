using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class MedicalDegree : BaseEntityModel
    {
        public MedicalDegree()
        {
            DoctorMedicalDegrees = new HashSet<DoctorMedicalDegrees>();
        }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string BanglaName { get; set; }

        public virtual ICollection<DoctorMedicalDegrees> DoctorMedicalDegrees { get; set; }
    }
}
