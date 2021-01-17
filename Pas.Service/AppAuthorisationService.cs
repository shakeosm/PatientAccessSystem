using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pas.Common.Constants;
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
                activeRole.RoleId = (int) user.ApplicationRole;

                _context.ActiveRoles.Update(activeRole);
            }

            _context.SaveChangesAsync();

            return true;

        }

        /// <summary>This will return a AppUserDetailsVM Object- which doesn't have any child entity</summary>
        /// <returns>AppUser Object</returns>
        public async Task<AppUserDetailsVM> GetActiveUserFromCache()
        {
            var userEmail = _httpContext.HttpContext.User.Identity?.Name;

            //var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //var userEmail = Context.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            //if (userEmail is null) return null;     //## Unauthenticated User

            AppUserDetailsVM currentUser = _cacheService.GetCacheValue<AppUserDetailsVM>(userEmail);            

            if (currentUser is null) {
                currentUser = _appUserService.FindByEmail(userEmail);   //## No CurrentUser info in RedisCache- Read from Table

                //## If this is a Doctor/Director- with additional Roles- then find out what was their Last selected Role from the DB
                if (currentUser.HasMultipleRoles)
                {

                    //## Cache is Empty- Does the user have any 'ActiveRole' Table?
                    var activeRole = _context.ActiveRoles
                                        .AsNoTracking()
                                        .Include(ar=> ar.Organisation)
                                        .FirstOrDefault(ar => ar.UserId == currentUser.Id);

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
                            OrganisationId = activeRole.OrganisationId,                            
                            OrganisationName = activeRole.Organisation.Name,
                            OrganisationAddress = activeRole.Organisation.Address //+ ", " + activeRole.Organisation.Address
                        };

                        currentUser.ApplicationRole = (ApplicationRole)activeRole.RoleId;
                        currentUser.OrganisationId = activeRole.OrganisationId;

                    }

                    //## Set the Roles in the Cache
                    //SetUserRolesInCache(userRoles, currentUser.Id);

                }
                else {
                    //## This is a Patient Only
                    currentUser.ApplicationRole = ApplicationRole.Patient;
                }
                
                //## Now set the entire CurrentUserVM in the Cache- so we will have everything we need
                SetActiveUserInCache(currentUser);
            }

            //## Current User seems to have multiple roles- is this User a Doctor, as well? Then read the Degrees and Specialities of this Doctor
            if (currentUser.Is_A_Doctor() && currentUser.DoctorDetails is null)
            {
                var specialities = await _appUserService.Get_DoctorSpeciality(currentUser.Id);
                var doctorDegreeList = await _appUserService.Get_DoctorDegrees(currentUser.Id);

                var doctorProfile = await _context.DoctorProfile.FindAsync(currentUser.Id);

                DoctorDetailsVM doctor = new DoctorDetailsVM()
                {
                    Id = currentUser.Id,
                    Name = currentUser.Name,
                    BanglaName = currentUser.BanglaName,
                    SpecialityList = specialities,
                    DoctorDegreeList = doctorDegreeList,

                    RegistrationNumber = doctorProfile.RegistrationNumber,
                    HeaderEnglish = doctorProfile.HeaderEnglish,
                    HeaderBangla = doctorProfile.HeaderBangla
                };

                currentUser.DoctorDetails = doctor;
                
                SetActiveUserInCache(currentUser);  //## Update in the Cache- with this Doctor's Details
            }


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

      
        public bool SetUserRolesInCache(IEnumerable<UserRoleVM> roles, int appUserId)
        {
            string redisKey = $"{CacheKey.UserRoles}_{appUserId}";
            _cacheService.SetCacheValue(redisKey, roles);
            return true;
        }

        public async Task<IEnumerable<UserRoleVM>> GetUserRolesFromCache(int appUserId)
        {
            string redisKey = $"{CacheKey.UserRoles}_{appUserId}";

            IEnumerable<UserRoleVM> result = _cacheService.GetCacheValue<IList<UserRoleVM>>(redisKey);
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