using System;
using Microsoft.AspNetCore.Identity;

namespace Pas.Common.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }

        [PersonalData]
        public string JobDescription { get; set; }

        [PersonalData]
        public DateTime? BirthDate { get; set; }

        [PersonalData]
        public int? RoleId { get; set; }
        
        [PersonalData]
        public string RoleName { get; set; }

        [PersonalData]
        public int? OrganisationId { get; set; }
        
        [PersonalData]
        public string OrganisationName { get; set; }

        [PersonalData]
        public string ImageUrl { get; set; }
    }
}
