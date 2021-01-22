using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class ExaminationItem : BaseEntityModel
    {
        public ExaminationItem()
        {
            ExaminationItemOptions = new HashSet<ExaminationItemOption>();
            PrescriptionExaminations = new HashSet<PrescriptionExamination>();
        }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string Description { get; set; }

        public int? ParentId { get; set; }

        public bool Show { get; set; }

        public virtual ICollection<ExaminationItemOption> ExaminationItemOptions { get; set; }
        public virtual ICollection<PrescriptionExamination> PrescriptionExaminations { get; set; }

    }
}
