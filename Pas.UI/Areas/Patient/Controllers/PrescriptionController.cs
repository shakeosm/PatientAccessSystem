using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PrescriptionController : BaseController
    {
        private readonly IPrescriptionService _prescriptionService;

        private IPatientService _patientService { get; }

        public PrescriptionController (IPrescriptionService PrescriptionService,
                                IPatientService PatientService,
                                UserManager<IdentityUser> UserManager,
                                IAppUserService AppUserService,
                                IAppAuthorisationService AppAuthorisationService,
                                IUserOrgRoleService UserOrgRoleService
            ) : base(UserManager, AppUserService, AppAuthorisationService, UserOrgRoleService)
        {
            _prescriptionService = PrescriptionService;
            _patientService = PatientService;
        }

        public async Task<IActionResult> Index()
        {            

            var currentUser = await GetCurrentUser();

            var prescriptionList = _prescriptionService.ListByPatient(currentUser.Id);

            //## Re-factor UserDetails- 'Patient' type values     
            SetUserProfileValues(currentUser);

            return View(prescriptionList);
        }

        private void SetUserProfileValues(AppUserDetailsVM currentUser)
        {
            currentUser.AddressAreaLocality = "Badurtola, Chottagram";  //TODO: Should read from Database
            currentUser.ImageUrl = "user-3.png";
            ViewBag.UserDetails = currentUser;
        }
    }
}