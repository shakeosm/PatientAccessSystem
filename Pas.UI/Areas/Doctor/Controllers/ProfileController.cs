using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Common.Enums;
using Pas.Service.Interface;
using Pas.Web.ViewModels;

namespace Pas.UI.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize]
    public class ProfileController : Controller
    {

        private readonly IAppUserService _appUserService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppAuthorisationService _appAuthorisationService;
        
        public ProfileController(IPrescriptionService PrescriptionService,
                                IPatientService PatientService,
                                IAppUserService AppUserService,
                                UserManager<IdentityUser> UserManager,
                                IAppAuthorisationService AppAuthorisationService)
        {            
            _appUserService = AppUserService;
            _userManager = UserManager;
            _appAuthorisationService = AppAuthorisationService;
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

        private void SetDoctorsProfileValues(AppUserDetailsVM currentUser)
        {
            currentUser.Name = "Dr. " + currentUser.Name;
            currentUser.ImageUrl = "user-3.png";
            ViewBag.UserDetails = currentUser;
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

        private async Task<AppUserDetailsVM> GetCurrentUser()
        {
            var userEmail = _userManager.GetUserName(HttpContext.User);

            var currentUser = await _appAuthorisationService.GetActiveUserFromCache(userEmail);

            return currentUser;
        }
    }
}