using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class Drugs : BaseEntityModel
    {
        public Drugs()
        {
            DrugDosageType = new HashSet<DrugStrengthType>();
            DrugModeOfDelivery = new HashSet<DrugModeOfDelivery>();
            PrescriptionDrugs = new HashSet<PrescriptionDrugs>();
            DrugBrands = new HashSet<DrugBrands>();
        }

        [Column(TypeName = "datetime2(3)")]
        public DateTime DateCreated { get; set; }
        
        [Required]
        public int CreatedById { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
        
        [Required]
        public int DrugCategoryTypeId { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual CategoryTypes DrugCategoryType { get; set; }
        public virtual ICollection<DrugStrengthType> DrugDosageType { get; set; }
        public virtual ICollection<DrugModeOfDelivery> DrugModeOfDelivery { get; set; }
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }
        public virtual ICollection<DrugBrands> DrugBrands { get; set; }
    }

}
