using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pas.Data.Models
{
    public partial class DrugModeOfDelivery : BaseEntityModel
    {
        
        [Required]
        public int DrugId { get; set; }

        [Required]
        public int ModeOfDeliveryId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Drugs Drug { get; set; }
        public virtual ModeOfDelivery ModeOfDelivery { get; set; }
    }
}
