using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{
    public partial class PatientAilment : BaseEntityModel
    {
        public PatientAilment()
        {

        }

        [Required]
        public int AilmentTypeId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime DateAddedd { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime DateRemoved { get; set; }

        public virtual User Patient { get; set; }

        public virtual AilmentTypes AilmentTypes { get; set; }


    }
}
