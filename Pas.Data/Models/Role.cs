using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class Role : BaseEntityModel
    {
        public Role()
        {
            UserOrganisationRole = new HashSet<UserOrganisationRole>();
        }

        [Required]
        [Column(TypeName = "varchar(30)")]
        public string Name { get; set; }

        public virtual ICollection<UserOrganisationRole> UserOrganisationRole { get; set; }
    }
}
