using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class DosageTypes
    {
        public DosageTypes()
        {
            DrugDosageType = new HashSet<DrugDosageType>();
            PrescriptionDrugs = new HashSet<PrescriptionDrugs>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DrugDosageType> DrugDosageType { get; set; }
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }
    }
}
