using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{

    public partial class AddressBook : BaseEntityModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string AddressLine1 { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string LocalArea { get; set; }

        [Required]
        public int CityId { get; set; }

        [Column(TypeName = "datetime2(3)")]
        public DateTime DateCreated { get; set; }

        public virtual City  City{ get; set; }

        public virtual User User { get; set; }
    }
}
