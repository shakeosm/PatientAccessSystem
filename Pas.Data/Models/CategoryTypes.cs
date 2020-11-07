using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class CategoryTypes
    {
        public CategoryTypes()
        {
            Drugs = new HashSet<Drugs>();
        }

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Drugs> Drugs { get; set; }
    }
}
