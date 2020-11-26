using Pas.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Common.Extensions
{
    public static class ApplicationRoleExtionsion
    {
        public static bool Not_A_Doctor(this ApplicationRole applicationRole)
        {            
            return applicationRole != ApplicationRole.Doctor;
        }
    }
}
