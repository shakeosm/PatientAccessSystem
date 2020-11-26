using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{

    public partial class ClinicalHistory : BaseEntityModel
    {
        [Required]
        public int UserId { get; set; }

        [Column(TypeName = "tinyint")]
        public int? BloodGroupId { get; set; }

        [Column(TypeName = "tinyint")]
        public short? Smoker { get; set; }

        [Column(TypeName = "tinyint")]
        public short? Drinker { get; set; }

        public bool? Excercise  { get; set; }

        [Column(TypeName = "tinyint")]
        public short Sports { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime PersonalHistoryLastUpdated { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string Height { get; set; }

        [Column(TypeName = "tinyint")]
        public short? Weight { get; set; }

        [Column(TypeName = "tinyint")]
        public short? Pulse { get; set; }

        [Column(TypeName = "tinyint")]
        public short? PressureSystolic { get; set; }

        [Column(TypeName = "tinyint")]
        public short? PressureDiastolic { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal? Cholesterol { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal? Diabetes { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime ClinicalInfoLastUpdated { get; set; }

        [Column(TypeName = "varchar(200)")]
        public string AllergyInfo { get; set; }
       

        public virtual User User { get; set; }
    }
}
