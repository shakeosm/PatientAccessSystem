using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class PrescriptionDiagnosticTest : BaseEntityModel
    {
        [Required]
        public int PrescriptionId { get; set; }

        [Required]
        public int DiagnosticTestId { get; set; }

        [Required]
        [Column(TypeName = "varchar(500)")]
        public string Instructions { get; set; }

        public virtual DiagnosticTest DiagnosticTest { get; set; }
        public virtual Prescription Prescription { get; set; }
    }
}
