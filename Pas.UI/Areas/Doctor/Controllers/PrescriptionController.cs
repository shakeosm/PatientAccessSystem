using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Data.Models;
using Pas.Service.Interface;
using Pas.UI.Controllers;
using Pas.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
//using System.Web.Mvc;

namespace Pas.UI.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize]
    public class PrescriptionController : BaseController
    {
        private readonly IDrugService _drugService;
        private readonly IPatientService _patientService;

        private IPrescriptionService _prescriptionService { get; }

        public PrescriptionController(IPrescriptionService PrescriptionService,
                                IDrugService DrugService,
                                IPatientService PatientService,
                                UserManager<IdentityUser> UserManager,
                                IAppUserService AppUserService,
                                IAppAuthorisationService AppAuthorisationService,
                                IUserOrgRoleService UserOrgRoleService
            ) : base(UserManager, AppUserService, AppAuthorisationService, UserOrgRoleService)
        {
            _prescriptionService = PrescriptionService;
            _drugService = DrugService;
            _patientService = PatientService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }


            if (id < 1) 
                return RedirectToAction("SearchPatient", "Home", new { Area = "Doctor" });

            //## we get the PrescriptionId via Post call. use that to get Prescription info- Doctor, Hospital, Patient
            //## Doctor has already initiated a "New Prescription" from "Doctor/Home/StartNewPrescription()"- which is their Home page
            var prescription = await _prescriptionService.Find(id);
            
            if (prescription is null) 
                return RedirectToAction("SearchPatient", "Home", new { Area = "Doctor" });


            var newPrescriptionId = prescription.Id;
            
            AppUserDetailsVM patientDetails = await _appUserService.Find(prescription.PatientId, includeAddressBook: true);

            IList<IndicationTypes> indicationList = await _drugService.ListAllIndicationTypes();
            IEnumerable<DrugDetailsVM> drugList = await _drugService.ListAll();

            IList<DiagnosticTestDetailsVM> diagnosticTestList = null;   //TODO
            var clinicialInfo = await _patientService.GetClinicalDetails(patientDetails.Id);
            var recentMedication = await _patientService.GetRecentMedication(patientDetails.Id);
            var ailmentList = await _patientService.GetPatientAilments(patientDetails.Id);

            //## PrescriptionCreateVM- will have all necessary info to make a Prescription- 
            //## When the Doc needs to see preview of Prescription before Print/Save

            var vm = new PrescriptionCreateVM()
            {
                Id = newPrescriptionId,
                Doctor = currentUser,
                PatientDetails = patientDetails,
                DrugList = drugList,
                indicationList = indicationList,
                DiagnosticTestList = diagnosticTestList,
                AllergyList = clinicialInfo.AllergyInfo,
                Ailments = ailmentList,
                RecentMedication = recentMedication
            };

            //## Re-factor UserDetails- 'Doctor' type values     
            SetDoctorsProfileValues(currentUser);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PrescriptionConfirmSaveVM vm)
        {
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            return View();
        }

        private void SetDoctorsProfileValues(AppUserDetailsVM currentUser)
        {
            currentUser.Name = "Dr. " + currentUser.Name;
            //currentUser.AddressBook.LocalArea = currentUser.CurrentRole.OrganisationName;     //## Current Selected Chamber
            currentUser.ImageUrl = "user-3.png";
            ViewBag.UserDetails = currentUser;
        }

        [HttpGet]
        public async Task<ActionResult> ListAllBrandsForDiagnosis(int id)
        {
            if (id < 1)
                return Json("ListAllBrandsForDiagnosis- id = NULL");

            var result = await _drugService.ListAllBrandsForDiagnosis(id);

            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert_DrugBrandForDiagnosis(DrugBrandsForDiagnosisVM vm)
        {
            if (vm is null)
                return Json("error");

            AppUserDetailsVM currentUser = await GetCurrentUser();

            var result = await _drugService.Insert_DrugBrandForDiagnosis(vm, currentUser.Id);

            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert_BrandDoseTemplate(BrandDoseTemplateCreateVM vm)
        {
            return Json("success 101 " + vm.Duration);

            //## This will create a Template for a Specific Drug Brand. ie: 'Ibuprofen 200mg Tablet 4 Times a Day for 7 days'
            if (vm is null)
                return Json("error");

            AppUserDetailsVM currentUser = await GetCurrentUser();

            var result = await _drugService.Insert_BrandDoseTemplate(vm, currentUser.Id);

            return Json(result);
        }


        [HttpGet]
        public async Task<ActionResult> ListAllInvestigationsForDiagnosis(int id)
        {
            return Json("Not implemented: ListAllInvestigationsForDiagnosis"); //TODO

            if (id < 1)
                return Json(null);

            var result = await _drugService.ListAllInvestigationsForDiagnosis(id);
            return Json(result);
        }

        /// <summary>
        /// This will load the templates for these selected DrugBrand.
        /// Templates- will offera combination of medicine intake patterns, ie: (1+0+1), (1+1+1)
        /// </summary>
        /// <param name="id">Drug Brand id</param>
        /// <returns>List of Templates</returns>
        [HttpGet]
        public async Task<ActionResult> ListAllDrugPatternTemplates(int id)
        {

            return Json("Not implemented: ListAllDrugPatternTemplates"); //TODO

            if (id < 1)
                return Json(null);

            var result = await _drugService.ListAllDrugPatternTemplates(id);
            return Json(result);
        }
    }
}