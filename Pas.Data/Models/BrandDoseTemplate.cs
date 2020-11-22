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
        public BrandDoseTemplate()
        {
            PrescriptionDrugs = new HashSet<PrescriptionDrugs>();
        }
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

        [Column(TypeName = "varchar(50)")]
        public string TemplateText { get; set; }


        public virtual DrugBrands DrugBrand { get; set; }
        public virtual ModeOfDelivery ModeOfDelivery { get; set; }        
        public virtual StrengthType StrengthType { get; set; }
        public virtual IntakePattern IntakePattern { get; set; }               
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }               
    }
}
