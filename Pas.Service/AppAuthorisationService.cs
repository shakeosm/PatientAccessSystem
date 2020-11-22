using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Pas.Common.Enums;
using Pas.Data;
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
        private readonly IHttpContextAccessor _httpContext;
        private readonly PasContext _context;
        private readonly IAppUserService _appUserService;
        //private readonly IActiveUserService ActiveUserService;
        private readonly IUserOrgRoleService _userOrgRoleService;
        //private readonly IOrganisationService _organisationService;
        private readonly ICacheService _cacheService;
        //private readonly IHttpContextAccessor httpContextAccessor;

        public UserManager<IdentityUser> _userManager { get; }

        public AppAuthorisationService(IHttpContextAccessor HttpContext,
                                        PasContext Context,
                                        IAppUserService AppUserService,
                                        //IActiveUserService activeUserService, 
                                        //IOrganisationService organisationService, 
                                        ICacheService cacheService,
                                        //UserManager<IdentityUser> UserManager,
                                        IUserOrgRoleService UserOrgRoleService
                                        //IHttpContextAccessor HttpContextAccessor
                                        )
        {
            _httpContext = HttpContext;
            _context = Context;
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
                bool isUpdated = UpdateActiveUserRoleIn_DB(user);

                if(isUpdated)
                    _cacheService.SetCacheValue(user.Email, user);

                return isUpdated;            
            }

            return false;
         
        }

        private bool UpdateActiveUserRoleIn_DB(AppUserDetailsVM user)
        {
            ActiveRole activeRole = _context.ActiveRoles.FirstOrDefault(ar=> ar.UserId == user.Id);

            if (activeRole is null)
            {
                //## User First time Switched Role (as a Doctor)
                activeRole = new ActiveRole()
                {
                    OrganisationId = (user.ApplicationRole == ApplicationRole.Patient ? 0 : user.CurrentRole.OrganisationId),
                    UserId = user.Id,
                    RoleId = (int) user.ApplicationRole
                };

                _context.ActiveRoles.Add(activeRole);

            }
            else {
                activeRole.OrganisationId = user.CurrentRole.OrganisationId;
                activeRole.RoleId = user.CurrentRole.RoleId;

                _context.ActiveRoles.Update(activeRole);
            }

            _context.SaveChangesAsync();

            return true;

        }

        /// <summary>This will return a AppUserDetailsVM Object- which doesn't have any child entity</summary>
        /// <returns>AppUser Object</returns>
        public async Task<AppUserDetailsVM> GetActiveUserFromCache(string userEmail)
        {
            //var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var userEmail = Context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            //if (userEmail is null) return null;     //## Unauthenticated User

            AppUserDetailsVM currentUser = _cacheService.GetCacheValue<AppUserDetailsVM>(userEmail);
            IEnumerable<UserRoleVM> userRoles = null;

            if (currentUser is null) {
                currentUser = _appUserService.FindByEmail(userEmail);   //## No CurrentUser info in RedisCache- Read from Table

                //## If this is a Doctor/Director- with additional Roles- then find out what was their Last selected Role from the DB
                if (currentUser.HasMultipleRoles)
                {

                    //## Cache is Empty- Does the user have any 'ActiveRole' Table?
                    var activeRole = _context.ActiveRoles.FirstOrDefault(ar => ar.UserId == currentUser.Id);

                    //## Check- for any previous 'Active Role' in the DB Table?
                    if (activeRole is null)
                    {
                        //## ActiveRole Table is empty. Means- this Doctor never SwitchedRole before- so we can assume this User as a Patient- for the first time. 
                        //## Let the User- to go to SwitchRole page and from their Insert a Record in the ActiveRole table- then we can use that info

                        currentUser.ApplicationRole = ApplicationRole.Patient;

                    }
                    else
                    { //## ActiveRole present in the DB- but not in the Cache.. so- Update the values from DB int to RedisCache
                        currentUser.CurrentRole = new UserRoleVM() {                             
                            RoleId = activeRole.RoleId,
                            OrganisationId = activeRole.OrganisationId
                        };

                        currentUser.ApplicationRole = (ApplicationRole)activeRole.RoleId;
                        currentUser.OrganisationId = activeRole.OrganisationId;

                    }

                    //## And now set them in the Cache- so we will have everything we need
                    SetUserRolesInCache(userRoles, currentUser.Id);

                }
                else {
                    //## This is a Patient Only
                    currentUser.ApplicationRole = ApplicationRole.Patient;
                }

                SetActiveUserInCache(currentUser);
            }

            //if (cachedUser is null) return null;     //## First Time Logged in- no cache value

            return currentUser;
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
            _cacheService.SetCacheValue(appUserId.ToString(), roles);
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