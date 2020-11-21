using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class ModeOfDelivery : BaseEntityModel
    {
        public ModeOfDelivery()
        {
            DrugModeOfDelivery = new HashSet<DrugModeOfDelivery>();
            PrescriptionDrugs = new HashSet<PrescriptionDrugs>();
            BrandDoseTemplates = new HashSet<BrandDoseTemplate>();
        }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
        
        public bool? IsDeleted { get; set; }

        [Column(TypeName = "tinyint")]
        public int? RowOrder { get; set; }

        public virtual ICollection<DrugModeOfDelivery> DrugModeOfDelivery { get; set; }
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }
        public virtual ICollection<BrandDoseTemplate> BrandDoseTemplates { get; set; }

    }
}
