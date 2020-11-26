using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Web.ViewModels
{
    public class UserSwitchRoleViewVM
    {
        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public IEnumerable<UserRoleVM>  UserRoleList { get; set; }
    }

    public class UserRoleVM
    {
        public int UserOrganisationRoleId { get; set; }
        public string RoleName { get; set; }
        public int RoleId { get; set; }
        public string OrganisationName { get; set; }
        public string OrganisationAddress { get; set; }
        public int OrganisationId { get; set; }
        public DateTime DateAdded { get; set; }

    }

    public class UserSwitchRoleUpdate
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }
        public int UserOrganisationRoleId { get; set; }
    }

}
