﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Service.Interface;
using Pas.UI.Controllers;
using Pas.Web.ViewModels;

namespace Pas.UI.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Authorize]
    public class HomeController : BaseController
    {
        private IPatientService _patientService { get; }        

        public HomeController(IPrescriptionService PrescriptionService,
                                IPatientService PatientService,
                                UserManager<IdentityUser> UserManager,
                                IAppUserService AppUserService,
                                IAppAuthorisationService AppAuthorisationService,
                                IUserOrgRoleService UserOrgRoleService
            ) : base(UserManager, AppUserService, AppAuthorisationService, UserOrgRoleService)
        {
            _patientService = PatientService;
        }

        public async Task<IActionResult> Index()
        {
            //## Somethig like Patient Dashboard
            //var _userEmail = _userManager.GetUserName(HttpContext.User);

            AppUserDetailsVM currentUser = await GetCurrentUser();
            ClinicalHistoryVM clinicalInfo = await _patientService.GetClinicalDetails(currentUser.Id);
            

            PatientProfileWrapperVM vm = new PatientProfileWrapperVM() {
                Patient = currentUser,  //## This is Patient Profile... Patient/Home/Index Page
                ClinicalInfo = clinicalInfo,
                LabResults =null,   //## Not required in Profile Page
                PrescriptionList = null,   //## Not required in Profile Page 
            };

            //## Re-factor UserDetails- 'Patient' type values     
            SetPatientProfileValues(currentUser);

            return View(vm);
        }

        public async Task<IActionResult> Profile()
        {
            var currentUser = await GetCurrentUser();

            //## Re-factor UserDetails- 'Patient' type values     
            SetPatientProfileValues(currentUser);

            return View(currentUser);
        }

        public async Task<IActionResult> EditProfile()
        {
            var currentUser = await GetCurrentUser();

            //## Re-factor UserDetails- 'Patient' type values     
            SetPatientProfileValues(currentUser);

            return View(currentUser);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult UpdateProfile()
        {
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePersonalHistory(PatientPersonalHistoryVM vm)
        {
            if (vm.PatientId < 1) return Json(false);

            var result = await _patientService.UpdatePersonalHistory(vm);

            return Json(result ? "success" : "fail");
        }

    }
}