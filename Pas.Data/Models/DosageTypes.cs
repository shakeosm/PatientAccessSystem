using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class DosageTypes : BaseEntityModel
    {
        public DosageTypes()
        {
            DrugDosageType = new HashSet<DrugDosageType>();
            PrescriptionDrugs = new HashSet<PrescriptionDrugs>();
        }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string Name { get; set; }

        public virtual ICollection<DrugDosageType> DrugDosageType { get; set; }
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }
    }
}
