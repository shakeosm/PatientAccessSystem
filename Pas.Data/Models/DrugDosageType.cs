using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class DrugDosageType
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public int DosageTypeId { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual DosageTypes DosageType { get; set; }
        public virtual Drugs Drug { get; set; }
    }
}
