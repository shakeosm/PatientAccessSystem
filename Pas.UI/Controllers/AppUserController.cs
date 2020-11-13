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

namespace Pas.UI.Controllers
{
    public class AppUserController : Controller
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

        ///private readonly ApplicationUserClaimsPrincipalFactory _applicationUserClaimsPrincipalFactory;
        private readonly IPatientService _patientService;
        private readonly IUserOrgRoleService _userOrgRoleService;
        private readonly ICacheService _cacheService;
        private readonly IAppAuthorisationService _appAuthorisationService;
        public readonly UserManager<IdentityUser> _userManager;
        //private readonly ApplicationUser _userManager;

        //public AppUserController(IUserClaimsPrincipalFactory<ApplicationUser> UserClaimsPrincipalFactory

        public AppUserController(IPatientService PatientService
                                , IUserOrgRoleService UserOrgRoleService
                                , ICacheService cacheService
                                , IAppAuthorisationService AppAuthorisationService
                                , UserManager<IdentityUser> UserManager
            //,UserManager<ApplicationUser> userManager
            )
        {
            //_userClaimsPrincipalFactory = UserClaimsPrincipalFactory;
            //_applicationUserClaimsPrincipalFactory = UserClaimsPrincipalFactory;
            _patientService = PatientService;
            _userOrgRoleService = UserOrgRoleService;
            _cacheService = cacheService;
            _appAuthorisationService = AppAuthorisationService;
            _userManager = UserManager;
        }

        

        [HttpGet]
        public async Task<IActionResult> SwitchRole()
        {
            //## The reason for coming here is-> You have multiple ROles, ie: Doctor+Director. 
            //##    Or You wanted to Switch between roles.

            var userEmail = _userManager.GetUserName(HttpContext.User);

            //var userEmail = "shakeosm@gmail.com"; // HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            if (userEmail is null) {
                //## User Not Found
                return RedirectToAction("Login", "Account", new { Area = "Identity" });
            }

            var currentAppUser = await GetCurrentUser(userEmail);

            //## is it a Patient- who is trying to access this SwitchROle page?
            if (currentAppUser.HasAdditionalRoles == false) {
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
                    UserEmail = userEmail,
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
            AppUserDetailsVM cachedUser = await GetCurrentUser(vm.UserEmail);

            //## This is a Patient- update only ApplicationRole
            cachedUser.ApplicationRole = ApplicationRole.Patient;
            cachedUser.HasAdditionalRoles = false;  
            _appAuthorisationService.SetActiveUserInCache(cachedUser);

            return RedirectToAction("Index", "Home", new { Area = "Patient" });
        }


        [HttpPost]
        public async Task<IActionResult> SwitchRole(UserSwitchRoleUpdate vm)
        {
            //## Get the existing UserDetails from Redis Cache- 
            AppUserDetailsVM cachedUser = await GetCurrentUser(vm.UserEmail);

            //## Check this is not a hacker trying to allocate Roles that doesn't exist
            var selectedOrgRole = await _userOrgRoleService.Find(vm.UserOrganisationRoleId);

            //TODO: Also check- does current user has a Role in that Organisation? Is it a Genuine User selection or BOT Attack?
            bool validRoleSelected = await _userOrgRoleService.UserHasRoleInOrganisation(
                                               selectedOrgRole.UserId, selectedOrgRole.OrganisationId, selectedOrgRole.RoleId);

            if (selectedOrgRole is null || validRoleSelected == false) {
                return RedirectToAction("Login", "Account", new { Area = "Identity" });
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

        private async Task<AppUserDetailsVM> GetCurrentUser(string userEmail)
        {
            return await _appAuthorisationService.GetActiveUserFromCache(userEmail);
        }
    }
}