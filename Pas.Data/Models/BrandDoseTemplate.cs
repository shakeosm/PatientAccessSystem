using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{

    public partial class BrandDoseTemplate : BaseEntityModel
    {
        [Required]
        public int DrugBrandId { get; set; }

        [Required]
        public int ModeOfDeliveryId { get; set; }

        public int? StrengthTypeId { get; set; }

        [Column(TypeName = "tinyint")]
        public short? Dose { get; set; }

        [Required]
        [Column(TypeName = "tinyint")]
        public short Frequency { get; set; }

        [Required]
        [Column(TypeName = "smallint")]
        public short Duration { get; set; }

        [Required]
        public int IntakePatternId { get; set; }                

        [Required]
        public int CreatedBy { get; set; }
        
        public DateTime DateCreated { get; set; }

        public bool? IsDeleted { get; set; }


        public virtual DrugBrands DrugBrands { get; set; }
        public virtual ModeOfDelivery ModeOfDelivery { get; set; }        
        public virtual StrengthType StrengthType { get; set; }
        public virtual IntakePattern IntakePattern { get; set; }               
    }
}
