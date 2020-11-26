using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{

    public partial class Manufacturer : BaseEntityModel
    {

        public Manufacturer()
        {
            DrugBrands = new HashSet<DrugBrands>();
        }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string Name { get; set; }
        
        [Column(TypeName = "varchar(100)")]
        public string Address { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string Website { get; set; }        

        public virtual ICollection<DrugBrands> DrugBrands { get; set; }

        //public virtual Manu User { get; set; }
    }
}
