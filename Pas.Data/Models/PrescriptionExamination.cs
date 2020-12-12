using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{

    public partial class PrescriptionExamination : BaseEntityModel
    {
        [Required]
        public int PrescriptionId { get; set; }
        
        [Required]
        public int ExaminationCategoryId { get; set; }        
        
        [Required]
        public int ExaminationTypeId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Result { get; set; }

        public virtual Prescription Prescription { get; set; }
        public virtual ExaminationTypes ExaminationType { get; set; }
    }
}
