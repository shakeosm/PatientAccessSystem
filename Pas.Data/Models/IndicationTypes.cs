using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class IndicationTypes : BaseEntityModel
    {
        public IndicationTypes()
        {
            BrandForIndications = new HashSet<BrandForIndications>();
        }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } 
 
        [Required]
        public int CreatedById { get; set; }

        public bool? Show { get; set; }

        public virtual ICollection<DrugIndicationTypes> DrugIndicationTypes { get; set; }
        public virtual ICollection<BrandForIndications> BrandForIndications { get; set; }
    }
}
