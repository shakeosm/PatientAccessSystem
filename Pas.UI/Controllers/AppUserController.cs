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

namespace Pas.UI.Controllers
{
    public class AppUserController : Controller
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

        ///private readonly ApplicationUserClaimsPrincipalFactory _applicationUserClaimsPrincipalFactory;
        private readonly IPatientService _patientService;
        private readonly IUserOrgRoleService _userOrgRoleService;
        private readonly ICacheService _cacheService;

        //private readonly ApplicationUser _userManager;

        //public AppUserController(IUserClaimsPrincipalFactory<ApplicationUser> UserClaimsPrincipalFactory

        public AppUserController(IPatientService PatientService
                                , IUserOrgRoleService UserOrgRoleService
                                , ICacheService cacheService
            //,UserManager<ApplicationUser> userManager
            )
        {
            //_userClaimsPrincipalFactory = UserClaimsPrincipalFactory;
            //_applicationUserClaimsPrincipalFactory = UserClaimsPrincipalFactory;
            _patientService = PatientService;
            _userOrgRoleService = UserOrgRoleService;
            _cacheService = cacheService;
        }


        [HttpGet]
        public async Task<IActionResult> SwitchRole()
        {
            //## The reason for coming here is-> You have multiple ROles, ie: Doctor+Director. 
            //##    Or You wanted to Switch between roles.


            ////var currentUserClaims = await _userClaimsPrincipalFactory.
            //var currentClaims = ClaimsPrincipal.Current?.Identities.First().Claims.ToList();
            //if (currentClaims is null) {
            //    return RedirectToAction("Login", "Account", new { Area = "Identity" });
            //}

            //var x = currentClaims.FirstOrDefault();
            //var y = currentClaims?.FirstOrDefault(x => x.Type.Equals("UserName", StringComparison.OrdinalIgnoreCase)); //?.Value;

            var userEmail = "shakeosm@gmail.com"; // HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            if (userEmail is null) { 
                //## User Not Found
                return RedirectToAction("Index", "Home");
            }

            var currentAppUser = await _patientService.FindByEmail(userEmail);

            //## Go to DB.. See if this User has multiple roles or only one.
            var userRoles = await _userOrgRoleService.FindRolesByUserId(currentAppUser.Id);

            if (userRoles is null)
            {
                //## No Role in UserOrgTable - means this is a Patient- go to Patient Home Page

                ApplicationUser currentUser = new ApplicationUser()
                {
                    Email = currentAppUser.Email,
                    FullName = currentAppUser.Name,
                    OrganisationId = -1,
                    OrganisationName = "Patient",
                    RoleId = (int)ApplicationRole.Patient,
                    RoleName = ApplicationRole.Patient.ToString()
                };
                //## Add this User in the RedisCache

                // await _applicationUserClaimsPrincipalFactory.CreateAsync(currentUser);

                return RedirectToAction("Index", "Home", new { id = currentAppUser.Id, Area = "Patient" });
            }
            else {
                //## We found there are roles for this User- so- we need to ask the user to Select a Role
                //## Passing the ViewModel with some values

                var  userRoleList = await _patientService.GetRolesByUser(currentAppUser.Id);

                UserSwitchRoleViewVM vm = new UserSwitchRoleViewVM() { 
                    UserId = currentAppUser.Id,
                    UserRoleList = userRoleList
                };

                //## Roles found-> As the user which role they want to select
                return View(vm);
            }
            
        }


        [HttpPost]
        public async Task<IActionResult> SwitchRole(UserSwitchRoleUpdate vm)
        {
            //## Check this is not a hacker trying to allocate Roles that doesn't exist

            var selectedOrgRole = await _userOrgRoleService.Find(vm.UserOrganisationRoleId);
            if (selectedOrgRole is null) {
                return RedirectToAction("Index", "Login", new { Area = "Identity" });
            }

            //TODO: Also check- does current user has a Role in that Organisation? Is it a Genuine User selection or BOT Attack?

            ApplicationRole selectedRole = (ApplicationRole)selectedOrgRole.RoleId;
            string areaName = selectedRole.ToString();

            ApplicationUser currentUser = new ApplicationUser()
            {               
                Email = selectedOrgRole.User.Email,
                FullName = $"{selectedOrgRole.User.FirstName} {selectedOrgRole.User.LastName}",
                OrganisationId = selectedOrgRole.OrganisationId,
                OrganisationName = selectedOrgRole.Organisation.Name,
                RoleId = selectedOrgRole.RoleId,
                RoleName = selectedOrgRole.Role.Name,
                ImageUrl = ""
            };

            //await _applicationUserClaimsPrincipalFactory.CreateAsync(currentUser);

            return RedirectToAction("Index", "Home", new { id = selectedOrgRole.UserId, Area = areaName });

        }
    }
}