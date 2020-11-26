using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Common.Enums;
using Pas.Common.Extensions;
using Pas.Data.Models;
using Pas.Service.Interface;
using Pas.UI.Controllers;
using Pas.Web.ViewModels;
using System.Threading.Tasks;

namespace Pas.UI.Areas.Doctor.Controllers
{
    //[Authorize(Roles = "Doctor")]
    [Area("Doctor")]
    [Authorize]
    public class HomeController : BaseController
    {
        //private readonly IAppUserService _appUserService;
        //private readonly UserManager<IdentityUser> _userManager;
        //private readonly IAppAuthorisationService _appAuthorisationService;

        private IPrescriptionService _prescriptionService { get; }
        private IPatientService _patientService { get; }

        public HomeController(IPrescriptionService PrescriptionService,
                                IPatientService PatientService,
                                UserManager<IdentityUser> UserManager,
                                IAppUserService AppUserService,
                                IAppAuthorisationService AppAuthorisationService,
                                IUserOrgRoleService UserOrgRoleService
            ) : base(UserManager, AppUserService, AppAuthorisationService, UserOrgRoleService)
        {
            _prescriptionService = PrescriptionService;
            _patientService = PatientService;
            //_appUserService = AppUserService;
            //_userManager = UserManager;
            //_appAuthorisationService = AppAuthorisationService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            //## This is the Dashboard Page of a Doctor. Can view- Chart, Profile, Search Patients and Create a new Prescription
            PrescriptionCreateInitialVM vm = new PrescriptionCreateInitialVM()
            { 
                DoctorId = currentUser.Id,
                HospitalId = currentUser.CurrentRole.OrganisationId,
                PatientId = 1,

                //HospitalDetails = null,
                //DoctorDetails = null
            };

            SetDoctorsProfileValues(currentUser);            
            
            return View(vm);
        }

        //private async Task<AppUserDetailsVM> GetCurrentUser()
        //{
        //    var userEmail = _userManager.GetUserName(HttpContext.User);

        //    var currentUser = await _appAuthorisationService.GetActiveUserFromCache(userEmail);
            
        //    return currentUser;
        //}

        [HttpGet]
        public async Task<IActionResult> SearchPatient()
        {
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            //## Get Current Doctor's - Regular Patients
            var regularPatients = await _patientService.GetRegularPatientList(currentUser.Id);
            
            //## Doctor will Search Patients and Create a new Prescription
            PrescriptionCreateInitialVM vm = new PrescriptionCreateInitialVM()
            {
                DoctorId = currentUser.Id,
                HospitalId = currentUser.CurrentRole.OrganisationId,
                PatientId = 1,

                PatientsList = regularPatients,
                PatientsListTitle = $"Recent visitors",
                HospitalDetails = null,
                DoctorDetails = null
            };
            
            SetDoctorsProfileValues(currentUser);

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchPatient(PatientSearchVM search)
        {
            AppUserDetailsVM currentUser = await GetCurrentUser();

            //## search the patient using the Search VM
            search.FirstName = "car";
            var matchingPatients = await _patientService.SearchPatient(search);

            //## Doctor will Search Patients and Create a new Prescription
            PrescriptionCreateInitialVM vm = new PrescriptionCreateInitialVM()
            {
                DoctorId = currentUser.Id,
                HospitalId = currentUser.CurrentRole.OrganisationId,
                //PatientId = 1,

                SearchVM = search,

                PatientsList = matchingPatients,
                PatientsListTitle = $"Matching records",
                HospitalDetails = null,
                DoctorDetails = null
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> ViewPatient(int id)
        {
            if (id < 1) {
                return RedirectToAction("Index");
            }
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            AppUserDetailsVM patientDetails = await _appUserService.Find(id, true);
            ClinicalHistoryVM patientClinicalInfo = await _patientService.GetClinicalDetails(id);

            DoctorViewPatientWrapperVM vm = new DoctorViewPatientWrapperVM() { 
                Doctor = currentUser,
                PatientProfileWrapper = new PatientProfileWrapperVM() { 
                    Patient = patientDetails,
                    ClinicalInfo = patientClinicalInfo,
                }
            };

            SetDoctorsProfileValues(currentUser);

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> StartNewPrescription(int Id)
        {
            var currentUser = await GetCurrentUser();
            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            PrescriptionCreateInitialVM vm = new PrescriptionCreateInitialVM() { 
                HospitalId = currentUser.CurrentRole.OrganisationId,
                DoctorId = currentUser.Id,
                PatientId = Id
            };

            //## Create a new Record in the Prescription Table- with Patient, Doctor and Hospital Id- 
            // then capture the remaining info from the UI

            int prescriptionId = 1003;  // await _prescriptionService.CreateInitialDefault(vm);

            return RedirectToAction("Create", "Prescription", new { id = prescriptionId });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> FindPatient(PatientSearchVM searchVM)
        {
            //## Create a new Record in the Prescription Table- with Patient, Doctor and Hospital Id- 
            // then capture the remaining info from the UI

            var searchResult = await _patientService.SearchPatient(searchVM);

            return View(searchResult);
            
        }

        [HttpGet]
        public async Task<IActionResult> Drugs()
        {            
            return View();
        }
    }
}