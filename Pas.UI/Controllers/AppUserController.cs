using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Common.Enums;
using Pas.Service.Interface;
using Pas.UI.Infrastructure.ApplicationUserClaims;
using Pas.UI.Models.Identity;
using Pas.Web.ViewModels;

namespace Pas.UI.Controllers
{
    public class AppUserController : Controller
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

        ///private readonly ApplicationUserClaimsPrincipalFactory _applicationUserClaimsPrincipalFactory;
        private readonly IPatientService _patientService;
        private readonly IUserOrgRoleService _userOrgRoleService;

        //private readonly ApplicationUser _userManager;

        //public AppUserController(IUserClaimsPrincipalFactory<ApplicationUser> UserClaimsPrincipalFactory

        public AppUserController(IPatientService PatientService
                                , IUserOrgRoleService UserOrgRoleService
            //,UserManager<ApplicationUser> userManager
            )
        {
            //_userClaimsPrincipalFactory = UserClaimsPrincipalFactory;
            //_applicationUserClaimsPrincipalFactory = UserClaimsPrincipalFactory;
            _patientService = PatientService;
            _userOrgRoleService = UserOrgRoleService;
        }


        [HttpGet]
        public async Task<IActionResult> SwitchRole()
        {
            //var currentUserClaims = await _userClaimsPrincipalFactory.
            var currentClaims = ClaimsPrincipal.Current?.Identities.First().Claims.ToList();
            if (currentClaims is null) {
                return RedirectToAction("Login", "Account", new { Area = "Identity" });
            }

            var x = currentClaims.FirstOrDefault();
            var y = currentClaims?.FirstOrDefault(x => x.Type.Equals("UserName", StringComparison.OrdinalIgnoreCase)); //?.Value;

            

            var currentAppUser = await _patientService.FindByEmail("shakeosm@gmail.com");

            //## Go to DB.. See if this User has multiple roles or only one.
            var currentRole = await _userOrgRoleService.FindRolesByUserId(currentAppUser.Id);
            
            if (currentRole is null) {
                //## No Role in UserOrgTable - means this is a Patient- go to Patient Home Page
                
                ApplicationUser currentUser = new ApplicationUser()
                {
                    Email = currentAppUser.Email,
                    FullName = currentAppUser.Name,
                    OrganisationId = -1,
                    OrganisationName = "Patient",
                    RoleId = (int) AppRole.Patient,
                    RoleName = AppRole.Patient.ToString()
                };

               // await _applicationUserClaimsPrincipalFactory.CreateAsync(currentUser);

                return RedirectToAction("Index", "Home", new { id = 123, Area = "Patient" });
            }
            
            //## We found there are roles found for this User- so- we need to ask the user to Select a Role
            //## Passing the ViewModel with some values
            var vm = new UserSwitchRoleSelection()
            {
                OrganisationId = 4,
                RoleId = 2,
                UserId = currentAppUser.Id  //## So- on PostBack- we know who is the User
            };

            //## Roles found-> As the user which role they want to select
            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> SwitchRole(UserSwitchRoleSelection vm)
        {
            AppRole selectedRole = (AppRole) vm.RoleId;
            string areaName = selectedRole.ToString();

            var selectedOrgRole = await _userOrgRoleService.FindRolesInOrganisationByUserId(vm.OrganisationId, vm.UserId);
            var firstRole = selectedOrgRole.First();


            ApplicationUser currentUser = new ApplicationUser()
            {               
                Email = firstRole.User.Email,
                FullName = $"{firstRole.User.FirstName} {firstRole.User.LastName}",
                OrganisationId = firstRole.OrganisationId,
                OrganisationName = firstRole.Organisation.Name,
                RoleId = firstRole.RoleId,
                RoleName = firstRole.Role.Name
            };

            //await _applicationUserClaimsPrincipalFactory.CreateAsync(currentUser);

            return RedirectToAction("Index", "Home", new { id = firstRole.UserId, Area = areaName });

        }
    }
}