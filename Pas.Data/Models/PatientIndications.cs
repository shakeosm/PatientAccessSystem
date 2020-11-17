using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{

    public partial class PatientIndications : BaseEntityModel
    {
        [Required]
        public int PatientId { get; set; }
        
        [Required]
        public int PrescriptionId { get; set; }    
        
        [Required]
        public int IndicationTypeId { get; set; }       

        [Column(TypeName = "datetime2(3)")]
        public DateTime DateAddedd { get; set; }

        public virtual Prescription Prescription { get; set; }

        public virtual User Patient { get; set; }
    }
}
