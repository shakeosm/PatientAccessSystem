using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{

    public partial class PrescriptionInvestigation : BaseEntityModel
    {
        [Required]
        public int InvestigationId { get; set; }

        [Required]
        public int PrescriptionId { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Notes { get; set; }

                
        public virtual Prescription Prescription { get; set; }
        public virtual Investigation Investigation { get; set; }
    }
}
