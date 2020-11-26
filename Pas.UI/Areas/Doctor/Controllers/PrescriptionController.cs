using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Common.Enums;
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

        [HttpPost]
        public async Task<IActionResult> Create(int id)
        {
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            PrescriptionCreateInitialVM vm = new PrescriptionCreateInitialVM()
            {
                HospitalId = currentUser.CurrentRole.OrganisationId,
                DoctorId = currentUser.Id,
                PatientId = id
            };

            int prescriptionId = await _prescriptionService.CreateInitialDefault(vm);


            //if (id < 1) 
            //    return RedirectToAction("SearchPatient", "Home", new { Area = "Doctor" });

            //## we get the PrescriptionId via Post call. use that to get Prescription info- Doctor, Hospital, Patient
            //## Doctor has already initiated a "New Prescription" from "Doctor/Home/StartNewPrescription()"- which is their Home page
            //var prescription = await _prescriptionService.Find(id);
            
            //if (prescription is null || prescription.Status != (int) PrescriptionStatus.Draft) 
            //    return RedirectToAction("SearchPatient", "Home", new { Area = "Doctor" });


            //var newPrescriptionId = prescription.Id;

            var chamber = await _appUserService.Get_DoctorChamber(currentUser.Email);
            AppUserDetailsVM patientDetails = await _appUserService.Find(vm.PatientId, includeAddressBook: true);

            ClinicalHistoryVM clinicialInfo = await _patientService.GetClinicalDetails(patientDetails.Id);            
            //var chiefComplaints = await _patientService.GetPatientChiefComplaints(patientDetails.Id);

            //## PrescriptionCreateVM- will have all necessary info to make a Prescription- 
            //## When the Doc needs to see preview of Prescription before Print/Save

            var prescriptionVM = new PrescriptionCreateVM()
            {
                Id = prescriptionId,
                Doctor = currentUser, //## Doctor details is at- AppUserDetailsVM.DoctorDetailsVM()
                ChamberDetails = chamber,
                PatientDetails = patientDetails,
                //AllergyList = clinicialInfo.AllergyList,
                ClinicialInfo = clinicialInfo, //## AllergyList is withi ClinicalInfo
            };

            //## Re-factor UserDetails- 'Doctor' type values     
            SetDoctorsProfileValues(currentUser);

            return View(prescriptionVM);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> FinishAndPreview(PrescriptionConfirmSaveVM vm)
        {
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            return PartialView("Areas/Doctor/Views/Prescription/_FinishAndPreview.cshtml");

        }

       
        public async Task<IActionResult> TestPreview(PrescriptionConfirmSaveVM vm)
        {
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            return View("Areas/Doctor/Views/Prescription/_FinishAndPreview.cshtml");
            //return PartialView("/Doctor/Prescription/Views/FinishAndPreview");

        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create_Prescription_HTML(PrescriptionConfirmSaveAndFinishVM vm)
        {
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            var result = await _prescriptionService.Prescription_FinishAndCreate_HTML(vm);

            return Json("success");
            //return Json(result ? "success" : "fail");

        }

        public async Task<IActionResult> GetPrescription_HTML(int id)
        {
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            var result = await _prescriptionService.GetPrescription_HTML(id);

            return Json(result);

        }

        [HttpGet]
        public async Task<ActionResult> ListAllBrandsForDiagnosis(int id)
        {
            if (id < 1)
                return Json("error");

            var result = await _drugService.ListAllBrandsForDiagnosis(id);

            return Json(result);
        }


        [HttpGet]
        public async Task<ActionResult> ListAllBrandsDoseTemplates(int id)
        {
            if (id < 1)
                return Json("error");

            IList<BrandDoseTemplateViewVM> result = await _drugService.ListAllBrandsDoseTemplates(id);

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
            //## This will create a Template for a Specific Drug Brand. ie: 'Ibuprofen 200mg Tablet 4 Times a Day for 7 days'
            if (vm is null || vm.StrengthTypeText is null || vm.ModeOfDeliveryId == 0 || vm.DrugBrandId == 0 || vm.Duration == 0)
                return Json("error");

            AppUserDetailsVM currentUser = await GetCurrentUser();

            var result = await _drugService.Insert_BrandDoseTemplate(vm, currentUser.Id);
                                    
            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert_PescriptionItem(PrescriptionDrugCreateVM vm)
        {            
            //## This will create a Template for a Specific Drug Brand. ie: 'Ibuprofen 200mg Tablet 4 Times a Day for 7 days'
            if (vm is null || vm.DrugBrandId < 1 || vm.BrandDoseTemplateId < 1)
                return Json("error");

            var result = await _prescriptionService.Insert_PescriptionItem(vm);
                                    
            return Json(result);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete_PescriptionItem(int prescriptionItemId)
        {            
            //## This will create a Template for a Specific Drug Brand. ie: 'Ibuprofen 200mg Tablet 4 Times a Day for 7 days'
            if (prescriptionItemId < 1)
                return Json("error");
            
            var result = await _prescriptionService.Delete_PescriptionItem(prescriptionItemId);
                                  
            return Json(result ? "success" : "fail");
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Update_Vitals(VitalsVM vm)
        {            
            //## This will create a Template for a Specific Drug Brand. ie: 'Ibuprofen 200mg Tablet 4 Times a Day for 7 days'
            if (vm.PatientId < 1)
                return Json("error");
            
            int recordId = await _prescriptionService.Update_Vitals(vm);
                                  
            return Json(recordId);
        }


        [HttpGet]
        public async Task<ActionResult> ListAllInvestigationsForDiagnosis(int id)
        {
            return Json("Not implemented: ListAllInvestigationsForDiagnosis"); //TODO: ListAllInvestigationsForDiagnosis()

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

            return Json("Not implemented: ListAllDrugPatternTemplates"); //TODO: ListAllDrugPatternTemplates()

            if (id < 1)
                return Json(null);

            var result = await _drugService.ListAllDrugPatternTemplates(id);
            return Json(result);
        }
    }
}