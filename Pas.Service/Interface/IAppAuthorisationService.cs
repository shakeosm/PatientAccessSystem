using Pas.Common.Enums;
using Pas.Data.Models;
using Pas.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pas.Service.Interface
{
    public interface IAppAuthorisationService
    {

        bool SetActiveUserInCache(AppUserDetailsVM user);

        Task<AppUserDetailsVM> GetActiveUserFromCache(string userEmail);

        bool SetUserRolesInCache(IEnumerable<UserRoleVM> roles, int appUserId);

        Task<IEnumerable<UserRoleVM>> GetUserRolesFromCache(int appUserId);

        /// <summary>This will return a AppUser Object- with child entities, ie: Organisation, Roles, OrgRoles</summary>
        /// <returns>AppUser with Include Predicate</returns>
        //User GetCurrentAppUserWithIncludes();

        /// <summary>This will return a AppUser Object- which doesn't have any child entity</summary>
        /// <returns>AppUser Object</returns>
        //User GetCurrentAppUser();

        bool UserHasRoleAtOrganisation(int organisationId, int roleId);
        
        //bool SetCurrentOrganisationRole(int userId, int organisationId, int roleId);
    }
}