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
        public int ExaminationItemId { get; set; }

        /// <summary>Exminaition Can have fixed options, ie: Gynae-> P/A: Membrane can have 'intact' / 'ruptured' </summary>
        public int? ExaminationItemOptionId { get; set; }

        [Column(TypeName = "varchar(100)")]
        public string Result { get; set; }

        public virtual Prescription Prescription { get; set; }

        public virtual ExaminationItemOption ExaminationItemOption { get; set; }
        public virtual ExaminationItem ExaminationItem { get; set; }
    }
}
