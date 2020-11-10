using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class IndicationTypes : BaseEntityModel
    {
        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime DateCreated { get; set; }

        [Required]
        public int CreatedById { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; }
    }
}
