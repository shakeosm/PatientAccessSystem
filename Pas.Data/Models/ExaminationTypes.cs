using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class ExaminationTypes : BaseEntityModel
    {
        public ExaminationTypes()
        {
            Examinations = new HashSet<PrescriptionExamination>();
        }

        [Required]
        public int Category { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual ICollection<PrescriptionExamination> Examinations { get; set; }
    }
}
