using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class ChiefComplaints : BaseEntityModel
    {

        public ChiefComplaints()
        {
            PrescriptionChiefComplaints = new HashSet<PrescriptionChiefComplaints>();
        }
        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Description { get; set; }

        public bool? IsDeleted { get; set; }

       
        public virtual ICollection<PrescriptionChiefComplaints> PrescriptionChiefComplaints { get; set; }
    }
}
