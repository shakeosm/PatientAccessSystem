using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class PrescriptionDrugs : BaseEntityModel
    {
        [Required]
        public int PrescriptionId { get; set; }

        public int? DrugId { get; set; }

        [Required]
        public int DrugBrandId { get; set; }

        [Required]
        public int BrandDoseTemplateId { get; set; }

        public int? AdviseInstructionId { get; set; }

        
        [Column(TypeName = "nvarchar(100)")]
        public string Notes { get; set; }

        public virtual Drugs Drug { get; set; }
        public virtual DrugBrands DrugBrands { get; set; }
        public virtual Prescription Prescription { get; set; }
        public virtual BrandDoseTemplate BrandDoseTemplate{ get; set; }
        public virtual AdviseInstructions AdviseInstruction { get; set; }
    }
}
