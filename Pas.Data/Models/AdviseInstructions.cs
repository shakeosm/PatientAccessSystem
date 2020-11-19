using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{
    public partial class AdviseInstructions : BaseEntityModel
    {        
        [Required]
        [Column(TypeName = "varchar(200)")]
        public string Description { get; set; }

        [Column(TypeName = "nvarchar(200)")]
        public string DescriptionBangla { get; set; }

        [Column(TypeName = "tinyint")]
        public short TypeId { get; set; }

        public bool IsDeleted { get; set; }
    }
}
