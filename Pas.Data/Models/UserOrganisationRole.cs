using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class UserOrganisationRole
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrganisationId { get; set; }
        public int RoleId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public int? ApprovedById { get; set; }

        public bool? IsDeleted { get; set; }

        public virtual Organisation Organisation { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
