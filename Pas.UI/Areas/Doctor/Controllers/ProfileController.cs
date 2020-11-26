using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Common.Enums;
using Pas.Service;
using Pas.Service.Interface;
using Pas.UI.Controllers;
using Pas.Web.ViewModels;

namespace Pas.UI.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize]
    public class ProfileController : BaseController
    {                
        public ProfileController(
                                IAppUserService AppUserService,
                                UserManager<IdentityUser> UserManager,
                                IAppAuthorisationService AppAuthorisationService,
                                IUserOrgRoleService UserOrgRoleService
            ) : base(UserManager, AppUserService, AppAuthorisationService, UserOrgRoleService)
        {            

        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            var availableRoles = await _appAuthorisationService.GetUserRolesFromCache(currentUser.Id);
            currentUser.RolesList = availableRoles;

            //## Re-factor UserDetails- 'Doctor' type values     
            SetDoctorsProfileValues(currentUser);

            return View(currentUser);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Update(int id)
        {
            return View();
        }
        
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePrescriptionHeader(DoctorDetailsUpdateVM vm)
        {
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }
            
            vm.Id = currentUser.Id;

            var result = await _appUserService.UpdatePrescriptionHeader(vm, currentUser);

            return Json(result ? "success" : "fail");
        }


        
    }
}