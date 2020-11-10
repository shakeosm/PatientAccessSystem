using Microsoft.AspNetCore.Http;
using Pas.Common.Enums;
using Pas.Data.Models;
using Pas.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pas.Service
{
    public class AppAuthorisationService : IAppAuthorisationService
    {
        private readonly IHttpContextAccessor Context;
        private readonly IPatientService AppUserService;
        //private readonly IActiveUserService ActiveUserService;
        private readonly IUserOrgRoleService UserOrgRoleService;
        //private readonly IOrganisationService _organisationService;
        private readonly ICacheService _cacheService;

        public AppAuthorisationService(IHttpContextAccessor context, 
                                        IPatientService appUserService, 
                                        //IActiveUserService activeUserService, 
                                        //IOrganisationService organisationService, 
                                        ICacheService cacheService,
                                        IUserOrgRoleService userOrgRoleService)
        {
            Context = context;
            AppUserService = appUserService;
            //ActiveUserService = activeUserService;
            UserOrgRoleService = userOrgRoleService;
            //_organisationService = organisationService;
            _cacheService = cacheService;            
    }

      
        public User GetActiveUser(string userName)
        {
            var cachedUser = _cacheService.GetCacheValue<User>(userName);
            return cachedUser;

            //if (cachedUser is null)
            //{
            //    //stored user has an entry in ActiveUser table
            //    var storedUser = ActiveUserService.GetByUserId(AppUserService.GetByUserName(userName, false).Id);

            //    if (storedUser is null)
            //    {
            //        cachedUser =  SetActiveUser(userName);
            //    }
            //    else
            //    {
            //        _cacheService.SetCacheValue(userName, storedUser);                    
            //        cachedUser = storedUser;
            //    }
            //}

            //return null if the cached user doesn't match a current UserOrgRole
            //if (UserOrgRoleService.CheckCachedDetails(cachedUser))
            //{
            //    return cachedUser;
            //}
            //else
            //{
            //    return null;
            //}
        }

        public User SetActiveUser(string userName)
        {
            return new User();
            ////TODO: can this be null when authenticating with NHS SSO?
            //var appUser = AppUserService.GetByUserName(userName, true);

            ////create default Activer User
            //User activeUser = new User()
            //    {
            //        UserId = appUser.Id,
            //        OrganisationId = appUser.UserOrganisationRoles.OrderBy(x => x.EffectiveFrom).First().OrganisationId,
            //        RoleId = (int)appUser.UserOrganisationRoles.OrderBy(x => x.EffectiveFrom).First().AppRoleId
            //    };

            //if (ActiveUserService.AddActiveUser(activeUser))
            //{
            //    _cacheService.SetCacheValue(userName, activeUser);  
            //    return activeUser;
            //}
            //else
            //{
            //    //TODO: amend to throw the correct (or custom) exception
            //    throw new Exception();
            //}

        }


        /// <summary>This will return a AppUser Object- with child entities, ie: Organisation, Roles, OrgRoles</summary>
        /// <returns>AppUser with Include Predicate</returns>
        public User GetCurrentAppUserWithIncludes()
        {
            return GetUser(true);
        }

        /// <summary>This will return a AppUser Object- which doesn't have any child entity</summary>
        /// <returns>AppUser Object</returns>
        public User GetCurrentAppUser()
        {
            return GetUser(false);
        }

        private User GetUser(bool includePredicate)
        {
            //var userNameClaim = Context.HttpContext.User.FindFirst(AppClaims.Name).Value;
            //var userEmail = HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            //var appUser = AppUserService.GetByUserName(userNameClaim, includePredicate);

            //TODO: work out why this is hit 3 times on startup
            return new User();
        }

        public ApplicationRole GetCurrentRole()
        {
            //TODO - Spec what happens with multiple claims, for now we only want one, and if there isn't one, null exception will be fine.
            return (ApplicationRole)GetCurrentAppUserWithIncludes().UserOrganisationRoles.First().RoleId;
        }


        public Organisation GetCurrentOrganisation()
        {
            //TODO - Spec what happens with multiple claims, for now we only want one, and if there isn't one, null exception will be fine.
            return GetCurrentAppUserWithIncludes().UserOrganisationRoles.Select(o => o.Organisation).First();
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


        public bool SetCurrentOrganisationRole(int userId, int organisationId, int roleId) 
        {             
            //TODO: Write to Redis Cache and update ActiveUser
            //var activeUser = ActiveUserService.GetByUserId(userId);

            //activeUser.RoleId = roleId;
            //activeUser.OrganisationId = organisationId;


            //    //TODO: more robust check on username
            //    _cacheService.SetCacheValue(Context.HttpContext.User.Identity.Name, activeUser);
                return true;

        }


    }
}