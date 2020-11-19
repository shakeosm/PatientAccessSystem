using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class DrugIndicationTypes : BaseEntityModel
    {         
        [Required]
        public int DrugId { get; set; } 
        
        [Required]
        public int IndicationTypesId { get; set; } 
 
        public int CreatedById { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual Drugs Drug { get; set; }
        public virtual IndicationTypes IndicationTypes { get; set; }
    }
}
