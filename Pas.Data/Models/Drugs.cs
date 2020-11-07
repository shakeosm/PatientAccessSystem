using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class Drugs
    {
        public Drugs()
        {
            DrugDosageType = new HashSet<DrugDosageType>();
            DrugModeOfDelivery = new HashSet<DrugModeOfDelivery>();
            PrescriptionDrugs = new HashSet<PrescriptionDrugs>();
        }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedById { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public int DrugCategoryTypeId { get; set; }

        public virtual CategoryTypes DrugCategoryType { get; set; }
        public virtual ICollection<DrugDosageType> DrugDosageType { get; set; }
        public virtual ICollection<DrugModeOfDelivery> DrugModeOfDelivery { get; set; }
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }
    }
}
