using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class Speciality : BaseEntityModel
    {
        public Speciality()
        {
            DoctorSpecialities = new HashSet<DoctorSpeciality>();
        }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string BanglaName { get; set; }

        public virtual ICollection<DoctorSpeciality> DoctorSpecialities{ get; set; }        
    }
}
