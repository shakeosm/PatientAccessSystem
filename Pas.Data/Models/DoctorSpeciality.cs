using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class DoctorSpeciality : BaseEntityModel
    {
        public DoctorSpeciality()
        {
            Doctors = new HashSet<Doctors>();
        }


        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string BanglaName { get; set; }

        public virtual ICollection<Doctors> Doctors { get; set; }
    }
}
