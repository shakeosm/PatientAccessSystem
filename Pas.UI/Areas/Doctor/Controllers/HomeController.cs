using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Common.Enums;
using Pas.Common.Extensions;
using Pas.Service.Interface;
using Pas.Web.ViewModels;
using System.Threading.Tasks;

namespace Pas.UI.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    public class HomeController : Controller
    {
        private readonly IAppUserService _appUserService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppAuthorisationService _appAuthorisationService;

        private IPrescriptionService _prescriptionService { get; }
        private IPatientService _patientService { get; }

        public HomeController(IPrescriptionService PrescriptionService,
                                IPatientService PatientService,
                                IAppUserService AppUserService,
                                UserManager<IdentityUser> UserManager,
                                IAppAuthorisationService AppAuthorisationService)
        {
            _prescriptionService = PrescriptionService;
            _patientService = PatientService;
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

            //## This is the Dashboard Page of a Doctor. Can view- Chart, Profile, Search Patients and Create a new Prescription
            PrescriptionCreateInitialVM vm = new PrescriptionCreateInitialVM()
            { 
                DoctorId = 3,
                HospitalId = 4,
                PatientId = 1,

                HospitalDetails = null,
                DoctorDetails = null
            };

            //## Re-factor UserDetails- 'Doctor' type values     
            SetDoctorsProfileValues(currentUser);            

            return View(vm);
        }

        private void SetDoctorsProfileValues(AppUserDetailsVM currentUser)
        {
            currentUser.Name = "Dr. " + currentUser.Name;
            currentUser.AddressAreaLocality = currentUser.CurrentRole.OrganisationName;     //## Current Selected Chamber
            currentUser.ImageUrl = "user-3.png";
            ViewBag.UserDetails = currentUser;
        }

        private async Task<AppUserDetailsVM> GetCurrentUser()
        {
            var userEmail = _userManager.GetUserName(HttpContext.User);

            var currentUser = await _appAuthorisationService.GetActiveUserFromCache(userEmail);
            
            return currentUser;
        }

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
            
            //## Re-factor UserDetails- 'Doctor' type values     
            SetDoctorsProfileValues(currentUser);

            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> SearchPatient(PatientSearchVM search)
        {
            //## search the patient using the Search VM
            search.FirstName = "car";
            var matchingPatients = await _patientService.SearchPatient(search);

            //## Doctor will Search Patients and Create a new Prescription
            PrescriptionCreateInitialVM vm = new PrescriptionCreateInitialVM()
            {
                DoctorId = 3,
                HospitalId = 4,
                PatientId = 1,

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
            AppUserDetailsVM currentUser = await GetCurrentUser();

            if (currentUser.Not_A_Doctor())
            {
                return RedirectToAction("AccessDenied", "Account", new { Area = "Identity" });
            }

            //## Doctor will View a Patient Profile- study and then New Prescription
            PrescriptionCreateInitialVM vm = new PrescriptionCreateInitialVM()
            {
                DoctorId = 3,
                HospitalId = 4,
                PatientId = 1,

                HospitalDetails = null,
                DoctorDetails = null
            };

            //## Re-factor UserDetails- 'Doctor' type values     
            SetDoctorsProfileValues(currentUser);

            return View();
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
                HospitalId = 4 ,
                DoctorId = 3,
                PatientId = 1
            };

            //## Create a new Record in the Prescription Table- with Patient, Doctor and Hospital Id- 
            // then capture the remaining info from the UI

            int prescriptionId = await _prescriptionService.CreateInitialDefault(vm);

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
    }
}