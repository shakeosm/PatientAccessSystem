using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class AilmentTypes : BaseEntityModel
    {
        public AilmentTypes()
        {
            PatientAilmentTypes = new HashSet<PatientAilmentType>();
        }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Name { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime DateAdded { get; set; }               
        
        public bool? IsDeleted { get; set; }

        public virtual ICollection<PatientAilmentType> PatientAilmentTypes{ get; set; }
    }
}
