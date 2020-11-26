using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class DoctorSpeciality : BaseEntityModel
    {

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        public int SpecialityId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string BanglaName { get; set; }

        public virtual DoctorProfile Doctor { get; set; }
        public virtual Speciality Speciality { get; set; }
    }
}
