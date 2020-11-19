using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class Symptoms : BaseEntityModel
    {        
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Description { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
