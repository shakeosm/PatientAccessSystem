using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class StrengthType : BaseEntityModel
    {
        public StrengthType()
        {
            DrugDosageType = new HashSet<DrugStrengthType>();
            PrescriptionDrugs = new HashSet<PrescriptionDrugs>();
        }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string Name { get; set; }

        public virtual ICollection<DrugStrengthType> DrugDosageType { get; set; }
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }
    }
}
