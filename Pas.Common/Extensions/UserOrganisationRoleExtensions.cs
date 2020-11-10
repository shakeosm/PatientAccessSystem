using Pas.Common.Enums;
using Pas.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Common.Extensions
{
    public static class UserOrganisationRoleExtensions
    {

        public static bool Is_A_DoctorOnly(this UserOrganisationRole organisationRole)
        {
            return organisationRole.RoleId == (int) ApplicationRole.Doctor;
        }

        public static bool Is_A_Director(this UserOrganisationRole organisationRole)
        {
            return organisationRole.RoleId == (int)ApplicationRole.Director;
        }

        
    }
}
