using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class User
    {
        public User()
        {
            Organisation = new HashSet<Organisation>();
            UserOrganisationRole = new HashSet<UserOrganisationRole>();
            UserRelated = new HashSet<UserRelated>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BanglaName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public string Mobile { get; set; }
        public string ShortId { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool? IsDeceased { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<Organisation> Organisation { get; set; }
        public virtual ICollection<UserOrganisationRole> UserOrganisationRole { get; set; }
        public virtual ICollection<UserRelated> UserRelated { get; set; }
    }
}
