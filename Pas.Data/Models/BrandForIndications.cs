using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class BrandForIndications : BaseEntityModel
    {         
        [Required]
        public int DrugBrandId { get; set; } 
        
        [Required]
        public int IndicationTypeId { get; set; } 
 
        public int AddedBy { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual DrugBrands DrugBrands { get; set; }
        public virtual IndicationTypes IndicationType { get; set; }
    }
}
