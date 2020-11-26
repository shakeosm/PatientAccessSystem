using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Common.Enums;
using Pas.Common.Models.Identity;
using Pas.Service.Interface;
using Pas.UI.Infrastructure.ApplicationUserClaims;
using Pas.Web.ViewModels;
using Pas.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace Pas.UI.Controllers
{
    [Authorize]
    public class AppUserController : BaseController
    {
        //private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

        ///private readonly ApplicationUserClaimsPrincipalFactory _applicationUserClaimsPrincipalFactory;
        private readonly IPatientService _patientService;
        //private readonly ICacheService _cacheService;
        //private readonly IUserOrgRoleService _userOrgRoleService;
        //private readonly IAppAuthorisationService _appAuthorisationService;
        //public readonly UserManager<IdentityUser> _userManager;
        //private readonly ApplicationUser _userManager;

        //public AppUserController(IUserClaimsPrincipalFactory<ApplicationUser> UserClaimsPrincipalFactory

        public AppUserController(IPatientService PatientService,
                                UserManager<IdentityUser> UserManager,
                                IAppUserService AppUserService,
                                IAppAuthorisationService AppAuthorisationService,
                                IUserOrgRoleService UserOrgRoleService
            ) : base(UserManager, AppUserService, AppAuthorisationService, UserOrgRoleService)
        {
            //_userClaimsPrincipalFactory = UserClaimsPrincipalFactory;
            //_applicationUserClaimsPrincipalFactory = UserClaimsPrincipalFactory;
            _patientService = PatientService;
            //_userOrgRoleService = UserOrgRoleService;
            //_cacheService = cacheService;
            //_appAuthorisationService = AppAuthorisationService;
            //_userManager = UserManager;
        }

        

        [HttpGet]
        public async Task<IActionResult> SwitchRole()
        {
            //## The reason for coming here is-> You have multiple ROles, ie: Doctor+Director. 
            //##    Or You wanted to Switch between roles.

            //var userEmail = GetLoggedInEmail();

            ////var userEmail = "shakeosm@gmail.com"; // HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            //if (userEmail is null) {
            //    //## User Not Found
            //    return RedirectToAction("Login", "Account", new { Area = "Identity" });
            //}

            var currentAppUser = await GetCurrentUser();

            //## is it a Patient- who is trying to access this SwitchRole page?
            if (currentAppUser.HasMultipleRoles == false) {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            //## Read Roles From Cache, too
            var  userRoles = await _appAuthorisationService.GetUserRolesFromCache(currentAppUser.Id);
            
            //## How many UserRoles we have?
            //if (userRoles.Count() < 1)
            //{
            //    //## Looks like the Patient was trying to be Clever by accessing this Page--!
            //    currentAppUser.CurrentRole = new UserRoleVM();
            //    //## No Role in UserOrgTable - means this is a Patient- go to Patient Home Page
            //    currentAppUser.CurrentRole.RoleId = (int)ApplicationRole.Patient;
            //    currentAppUser.CurrentRole.RoleName = ApplicationRole.Patient.ToString();
            //    currentAppUser.CurrentRole.OrganisationName = "Patient";

            //    //## Add this User in the RedisCache
            //    _appAuthorisationService.SetActiveUserInCache(currentAppUser);                               

            //    return RedirectToAction("Index", "Home", new { id = currentAppUser.Id, Area = "Patient" });
            //}
            //else {
                //## We found there are roles for this User- so- we need to ask the user to Select a Role
                //## Passing the ViewModel with some values

                //var  userRoleList = await _patientService.GetRolesByUser(currentAppUser.Id);
                //IEnumerable<UserRoleVM> userRoleList = MapToUserRoleVM(userRoles);

                UserSwitchRoleViewVM vm = new UserSwitchRoleViewVM() {
                    UserEmail = currentAppUser.Email,
                    UserId = currentAppUser.Id,
                    UserRoleList = userRoles
                };

                //## Roles found-> As the user which role they want to select
                return View(vm);
            //}
            
        }

        private IEnumerable<UserRoleVM> MapToUserRoleVM(IEnumerable<UserOrganisationRole> userRoles)
        {
            var result = userRoles.Select(u => new UserRoleVM {
                UserOrganisationRoleId = u.Id,
                OrganisationId = u.OrganisationId,
                OrganisationName = u.Organisation.Name,
                RoleId = u.RoleId,
                RoleName = ((ApplicationRole) u.RoleId).ToString()
            });

            return result;
        }


        [HttpPost]
        public async Task<IActionResult> SwitchRoleToPatient(UserSwitchRoleUpdate vm)
        {
            //## Get the existing UserDetails from Redis Cache- 
            AppUserDetailsVM cachedUser = await GetCurrentUser();

            //## This is a Patient- update only ApplicationRole
            cachedUser.ApplicationRole = ApplicationRole.Patient;
            _appAuthorisationService.SetActiveUserInCache(cachedUser);

            return RedirectToAction("Index", "Home", new { Area = "Patient" });
        }


        [HttpPost]
        public async Task<IActionResult> SwitchRole(UserSwitchRoleUpdate vm)
        {
            //## Get the existing UserDetails from Redis Cache- 
            AppUserDetailsVM cachedUser = await GetCurrentUser();

            //## Check this is not a hacker trying to allocate Roles that doesn't exist
            var selectedOrgRole = await _userOrgRoleService.Find(cachedUser.Id, vm.UserOrganisationRoleId);

            if (selectedOrgRole is null) {
                //## Someone tempered the data- hence no Role found for this User in the UserOrgTable
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }
                        
            //## So- now we know what the User has selected to be                
            cachedUser.ApplicationRole = (ApplicationRole)selectedOrgRole.RoleId;
            //cachedUser.HasAdditionalRoles = true;

            //## Save it in the Redis Cache- with the new UserOrgRole value
            cachedUser.CurrentRole = new UserRoleVM() { 
                OrganisationId = selectedOrgRole.OrganisationId,
                OrganisationName = selectedOrgRole.Organisation.Name,
                RoleId = selectedOrgRole.RoleId,
                RoleName = selectedOrgRole.Role.Name
            };

            //## Save it back in redis
            _appAuthorisationService.SetActiveUserInCache(cachedUser);

            //await _applicationUserClaimsPrincipalFactory.CreateAsync(currentUser);

            var areaName = ((ApplicationRole)selectedOrgRole.RoleId).ToString();

            return RedirectToAction("Index", "Home", new { Area = areaName });

        }

    }
}