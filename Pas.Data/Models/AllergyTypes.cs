using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{
    public partial class AllergyTypes : BaseEntityModel
    {
        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string BanglaName { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
