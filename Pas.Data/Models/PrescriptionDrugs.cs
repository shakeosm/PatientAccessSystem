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

        [Required]
        public int DrugId { get; set; }

        [Required]
        public int DrugBrandId { get; set; }

        [Required]
        public int DosageId { get; set; }

        [Required]
        public int ModeOfDeliveryId { get; set; }

        [Required]
        public short Quantity { get; set; }

        [Required]
        public short DurationDays { get; set; }
        
        [Column(TypeName = "nvarchar(100)")]
        public string Notes { get; set; }

        public virtual DosageTypes Dosage { get; set; }
        public virtual Drugs Drug { get; set; }
        public virtual DrugBrands DrugBrands { get; set; }
        public virtual ModeOfDelivery ModeOfDelivery { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
