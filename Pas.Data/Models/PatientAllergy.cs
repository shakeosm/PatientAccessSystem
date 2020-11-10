using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{
    public partial class PatientAllergy : BaseEntityModel
    {
        public PatientAllergy()
        {
            
        }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int AllergyTypeId { get; set; }               

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime DateAddedd { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime DateRemoved { get; set; }

        public virtual User Patient { get; set; }

        public virtual AllergyTypes AllergyType { get; set; }


    }
}
