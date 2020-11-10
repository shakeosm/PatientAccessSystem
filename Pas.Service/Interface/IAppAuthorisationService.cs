using Pas.Common.Enums;
using Pas.Data.Models;
using System.Collections.Generic;

namespace Pas.Service.Interface
{
    public interface IAppAuthorisationService
    {
        /// <summary>This will return a AppUser Object- with child entities, ie: Organisation, Roles, OrgRoles</summary>
        /// <returns>AppUser with Include Predicate</returns>
        User GetCurrentAppUserWithIncludes();

        /// <summary>This will return a AppUser Object- which doesn't have any child entity</summary>
        /// <returns>AppUser Object</returns>
        User GetCurrentAppUser();

        Organisation GetCurrentOrganisation();

        //bool HasPermission(params ApplicationPermission[] requiredPermissions);

        ApplicationRole GetCurrentRole();

        bool UserHasRoleAtOrganisation(int organisationId, int roleId);


        User GetActiveUser(string userName);

        User SetActiveUser(string userName);
        
        bool SetCurrentOrganisationRole(int userId, int organisationId, int roleId);
    }
}