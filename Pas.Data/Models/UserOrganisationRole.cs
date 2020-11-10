using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class UserOrganisationRole : BaseEntityModel
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int OrganisationId { get; set; }

        [Required]
        public int RoleId { get; set; }

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public int? ApprovedById { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual Organisation Organisation { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
