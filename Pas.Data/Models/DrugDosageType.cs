using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pas.Data.Models
{
    public partial class DrugDosageType : BaseEntityModel
    {
        
        [Required]
        public int DrugId { get; set; }

        [Required]
        public int DosageTypeId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual DosageTypes DosageType { get; set; }
        public virtual Drugs Drug { get; set; }
    }
}
