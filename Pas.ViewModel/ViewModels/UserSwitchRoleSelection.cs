using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Web.ViewModels
{
    public class UserSwitchRoleViewVM
    {
        public int UserId { get; set; }
        public IEnumerable<UserRole>  UserRoleList { get; set; }
    }

    public class UserRole
    {
        public int UserOrganisationRoleId { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public string OrganisationName { get; set; }
        public int OrganisationId { get; set; }

    }

    public class UserSwitchRoleUpdate
    {
        public int UserId { get; set; }
        public int UserOrganisationRoleId { get; set; }
    }

}
