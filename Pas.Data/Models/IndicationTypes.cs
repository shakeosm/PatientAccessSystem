using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class IndicationTypes : BaseEntityModel
    {         
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } 
 
        [Required]
        public int CreatedById { get; set; }

        public virtual ICollection<DrugIndicationTypes> DrugIndicationTypes { get; set; }
    }
}
