using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class CategoryTypes : BaseEntityModel
    {
        public CategoryTypes()
        {
            Drugs = new HashSet<Drugs>();
        }

        [Column(TypeName = "datetime2(3)")]
        public DateTime DateCreated { get; set; }
        
        [Required]
        public int CreatedById { get; set; }
        public int? UpdatedById { get; set; }
        
        [Required]
        public string Name { get; set; }
        public bool? IsDeleted { get; set; }

        public virtual ICollection<Drugs> Drugs { get; set; }
    }
}
