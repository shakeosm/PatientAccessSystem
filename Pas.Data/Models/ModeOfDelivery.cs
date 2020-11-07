using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class ModeOfDelivery
    {
        public ModeOfDelivery()
        {
            DrugModeOfDelivery = new HashSet<DrugModeOfDelivery>();
            PrescriptionDrugs = new HashSet<PrescriptionDrugs>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<DrugModeOfDelivery> DrugModeOfDelivery { get; set; }
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }
    }
}
