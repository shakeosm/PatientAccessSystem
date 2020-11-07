using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class DrugModeOfDelivery
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public int ModeOfDeliveryId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual Drugs Drug { get; set; }
        public virtual ModeOfDelivery ModeOfDelivery { get; set; }
    }
}
