using Pas.Common.Enums;
using System;

namespace Pas.Web.ViewModels
{
    public class AppUserDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BanglaName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string ShortId { get; set; }
        public bool HasMultipleRoles { get; set; }

        public UserRole  CurrentRole { get; set; }
        public string Organisation { get; set; }
        public int OrganisationId { get; set; }
    }
}
