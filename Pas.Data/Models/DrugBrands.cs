using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{

    public partial class DrugBrands : BaseEntityModel
    {
        public DrugBrands()
        {
            PrescriptionDrugs = new HashSet<PrescriptionDrugs>();
        }
        [Required]
        public int DrugId { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string BrandName { get; set; }

        [Required]
        public int ManufacturerId { get; set; }


        public DateTime DateAdded { get; set; }

        public bool IsDeleted { get; set; }


        public virtual Drugs Drug { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
        
        public virtual ICollection<PrescriptionDrugs> PrescriptionDrugs { get; set; }
       
        //public virtual Manu User { get; set; }
    }
}
