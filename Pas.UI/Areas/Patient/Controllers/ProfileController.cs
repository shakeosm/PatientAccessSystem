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
    public class ProfileController : BaseController
    {

        public ProfileController (IPrescriptionService PrescriptionService,
                        IPatientService PatientService,
                        UserManager<IdentityUser> UserManager,
                        IAppUserService AppUserService,
                        IAppAuthorisationService AppAuthorisationService,
                        IUserOrgRoleService UserOrgRoleService
    ) : base(UserManager, AppUserService, AppAuthorisationService, UserOrgRoleService)
        {

        }

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home", new { Area = "Patient" });
        }
    }
}