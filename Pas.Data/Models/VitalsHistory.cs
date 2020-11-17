using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{

    public partial class VitalsHistory : BaseEntityModel
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int PrescriptionId { get; set; }

        [Column(TypeName = "tinyint")]
        public short BloodPulse { get; set; }

        [Column(TypeName = "tinyint")]
        public short Diastolic { get; set; }

        [Column(TypeName = "tinyint")]
        public short Systolic { get; set; }

        [Column(TypeName = "tinyint")]
        public short Weight { get; set; }

        [Column(TypeName = "decimal(4, 2)")]
        public short Temperature { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime DateAdded { get; set; }

        public virtual Prescription Prescription { get; set; }

        public virtual User Patient { get; set; }
    }
}
