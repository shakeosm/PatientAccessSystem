using Pas.Common.Enums;
using System;
using System.Collections.Generic;

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
        public string AddressAreaLocality { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string ShortId { get; set; }
        public bool HasMultipleRoles { get; set; }

        public UserRoleVM  CurrentRole { get; set; }
        public IEnumerable<UserRoleVM> RolesList { get; set; }
        public string Organisation { get; set; }
        public int OrganisationId { get; set; }
        public ApplicationRole ApplicationRole { get; set; }
        public bool HasAdditionalRoles { get; set; }
        public string ImageUrl { get; set; }

        //## Helper Methods
        public bool Is_A_Doctor() => ApplicationRole == ApplicationRole.Doctor;
        public bool Not_A_Doctor() => ApplicationRole != ApplicationRole.Doctor;
        

        public bool Is_A_Patient() => ApplicationRole == ApplicationRole.Patient;

        public string ProfilePath()
        {
            if (ApplicationRole == 0)
            {
                return "Patient/Profile";      //### Default to Patient
            }

            return $"{ApplicationRole.ToString()}/Profile";      //### Default to Patient    
            
        }
    }
}
