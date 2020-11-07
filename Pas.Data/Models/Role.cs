using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class Role
    {
        public Role()
        {
            UserOrganisationRole = new HashSet<UserOrganisationRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<UserOrganisationRole> UserOrganisationRole { get; set; }
    }
}
