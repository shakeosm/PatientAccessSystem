using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pas.Service.Interface;
using Pas.Web.ViewModels;

namespace Pas.UI.Areas.Patient.Controllers
{
    [Area("Patient")]
    public class HomeController : Controller
    {
        private IPatientService _patientService { get; }

        public HomeController(IPatientService PatientService)
        {
            _patientService = PatientService;
        }

        public IActionResult Index()
        {

            //## Somethig like Dashboard
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