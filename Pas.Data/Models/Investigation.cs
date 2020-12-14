using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class Investigation : BaseEntityModel
    {
        public Investigation()
        {
            PrescriptionInvestigations = new HashSet<PrescriptionInvestigation>();
        }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public virtual ICollection<PrescriptionInvestigation> PrescriptionInvestigations { get; set; }

    }
}
