using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class IntakePattern : BaseEntityModel
    {
        public IntakePattern()
        {
            BrandDoseTemplates = new HashSet<BrandDoseTemplate>();
        }

        [Required]
        [Column(TypeName = "nvarchar(100)")]
        public string Pattern { get; set; }

        public virtual ICollection<BrandDoseTemplate> BrandDoseTemplates { get; set; }
    }
}
