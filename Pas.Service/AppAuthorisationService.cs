using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Pas.Common.Enums;
using Pas.Data.Models;
using Pas.Service.Interface;
using Pas.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pas.Service
{
    public class AppAuthorisationService : IAppAuthorisationService
    {
        private readonly IHttpContextAccessor Context;
        private readonly IAppUserService _appUserService;
        //private readonly IActiveUserService ActiveUserService;
        private readonly IUserOrgRoleService _userOrgRoleService;
        //private readonly IOrganisationService _organisationService;
        private readonly ICacheService _cacheService;
        //private readonly IHttpContextAccessor httpContextAccessor;

        public UserManager<IdentityUser> _userManager { get; }

        public AppAuthorisationService(IHttpContextAccessor context,
                                        IAppUserService AppUserService,
                                        //IActiveUserService activeUserService, 
                                        //IOrganisationService organisationService, 
                                        ICacheService cacheService,
                                        //UserManager<IdentityUser> UserManager,
                                        IUserOrgRoleService UserOrgRoleService
                                        //IHttpContextAccessor HttpContextAccessor
                                        )
        {
            Context = context;
            _appUserService = AppUserService;
            //ActiveUserService = activeUserService;
            _userOrgRoleService = UserOrgRoleService;
            //_organisationService = organisationService;
            _cacheService = cacheService;
            //httpContextAccessor = HttpContextAccessor;
            //_userManager = UserManager;
        }


        public bool SetActiveUserInCache(AppUserDetailsVM user)
        {         
            if (user != null) { 
                _cacheService.SetCacheValue(user.Email, user);
                return true;            
            }

            return false;
         
        }
        
        /// <summary>This will return a AppUserDetailsVM Object- which doesn't have any child entity</summary>
        /// <returns>AppUser Object</returns>
        public async Task<AppUserDetailsVM> GetActiveUserFromCache(string userEmail)
        {
            //var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var userEmail = Context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            //if (userEmail is null) return null;     //## Unauthenticated User

            var cachedUser = _cacheService.GetCacheValue<AppUserDetailsVM>(userEmail);

            if (cachedUser is null) {
                cachedUser = _appUserService.FindByEmail(userEmail);
                
                //## this User Details were never read from DB.. So- now read the UserOrganisationRoles as well- and put them in the Cache
                var userRoles = await _userOrgRoleService.FindMappedRolesByUserId(cachedUser.Id);

                //## Is this a PatientOnly Account? Who has no other Roles anywhere?
                //cachedUser.HasAdditionalRoles = userRoles.Count() >= 1;

                if (userRoles.Count() < 1) {
                    cachedUser.ApplicationRole = ApplicationRole.Patient;
                }

                SetActiveUserInCache(cachedUser);

                //## And now set them in the Cache- so we will have everything we need
                SetUserRolesInCache(userRoles, cachedUser.Id);
            }

            //if (cachedUser is null) return null;     //## First Time Logged in- no cache value

            return cachedUser;
        }

                
                
        /// <summary>
        /// See if a user has a given role at a given organisation
        /// </summary>
        /// <param name="organisationId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool UserHasRoleAtOrganisation(int organisationId, int roleId)
        {
            //return GetCurrentAppUserWithIncludes().UserOrganisationRoles.Any(x =>
            //    x.OrganisationId == organisationId
            //    && x.AppRoleId == roleId);

            return true;
        }

        /// <summary>
        /// When User is switchig Roles- update all values at once. User Details+Roles- all info
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="organisationId"></param>
        /// <param name="organisationName"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public bool SetCurrentOrganisationRole(AppUserDetailsVM user) 
        {
             _cacheService.SetCacheValue<AppUserDetailsVM>(user.Email, user);
            return true;

            //## get the Cached Value
            //var currentUser = _cacheService.GetCacheValue<AppUserDetailsVM>(user.Email);

            //currentUser.OrganisationId = user.OrganisationId;
            //currentUser.CurrentRole.RoleId = user.CurrentRole.RoleId;
            //currentUser.CurrentRole.OrganisationId = user.CurrentRole.OrganisationId;
            //currentUser.CurrentRole.OrganisationName = user.CurrentRole.OrganisationName;


        }

        public bool SetUserRolesInCache(IEnumerable<UserRoleVM> roles, int appUserId)
        {
            _cacheService.SetCacheValue<IEnumerable<UserRoleVM>>(appUserId.ToString(), roles);
            return true;
        }

        public async Task<IEnumerable<UserRoleVM>> GetUserRolesFromCache(int appUserId)
        {
            IEnumerable<UserRoleVM> result = _cacheService.GetCacheValue<IList<UserRoleVM>>(appUserId.ToString());
            if (result is null) {
                //## If User Roles list isnt found in the cache- read from DB
                result = await _userOrgRoleService.FindMappedRolesByUserId(appUserId);

                //## Save it in the cache
                _cacheService.SetCacheValue(appUserId.ToString(), result);
            }
            return result;
            
        }
    }
}