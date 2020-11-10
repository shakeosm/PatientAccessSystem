using Pas.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Common.Extensions
{
    public static class UserExtensions    
    {

        public static bool IsA_PatientOnly(this User user)
        {
            //var result = _commissioningOrganisationTypes.Contains(organisation.OrganisationType);

            return true;
        }


        public static bool IsProvider(this User user)
        {        
            return true; 
        }
    }
}
