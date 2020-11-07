using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class PrescriptionDrugs
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int DrugId { get; set; }
        public int DosageId { get; set; }
        public int ModeOfDeliveryId { get; set; }
        public short Quantity { get; set; }
        public short DurationDays { get; set; }
        
        [Column(TypeName = "nvarchar(100)")]
        public string Notes { get; set; }

        public virtual DosageTypes Dosage { get; set; }
        public virtual Drugs Drug { get; set; }
        public virtual ModeOfDelivery ModeOfDelivery { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
