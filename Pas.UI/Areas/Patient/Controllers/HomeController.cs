using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Service.Interface;
using Pas.Web.ViewModels;

namespace Pas.UI.Areas.Patient.Controllers
{
    [Area("Patient")]
    public class HomeController : Controller
    {
        private IPatientService _patientService { get; }

        private readonly IAppAuthorisationService _appAuthorisationService;
        public readonly UserManager<IdentityUser> _userManager;
        
        public HomeController(IPatientService PatientService,
                            IAppAuthorisationService AppAuthorisationService,
                            UserManager<IdentityUser> UserManager)
        {
            _patientService = PatientService;
            _appAuthorisationService = AppAuthorisationService;
            _userManager = UserManager;
            
        }

        public async Task<IActionResult> Index()
        {
            //## Somethig like Patient Dashboard
            var _userEmail = _userManager.GetUserName(HttpContext.User);

            var currentUser = await _appAuthorisationService.GetActiveUserFromCache(_userEmail);
            currentUser.AddressAreaLocality = "Panchlaish, Chattogram";
            
            ViewBag.UserDetails = currentUser;

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> FindPatientByEmail(string email)
        {
            var patient = await _patientService.FindByEmail(email);

            return Json(patient);
        }

        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> FindPatientByMobile(string mobile)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string mobile, string shortId, string firstName, string lastName)
        {
            PatientSearchVM searchVM = new PatientSearchVM() { FirstName = "Car", LastName = "", ShortId = "",  Mobile = "" };

            var searchResult = await _patientService.SearchPatient(searchVM);

            return PartialView("Views/Shared/_patientSearchResult.cshtml", searchResult);
            //return View("Views/Shared/_patientSearchResult.cshtml", searchResult);
        }


        public IActionResult Profile()
        {
            return View();
        }

        public IActionResult EditProfile()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult UpdateProfile()
        {
            return View();
        }

    }
}