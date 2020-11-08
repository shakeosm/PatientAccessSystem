using Microsoft.AspNetCore.Mvc;
using Pas.Service;
using Pas.Service.Interface;
using Pas.Web.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Pas.UI.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    public class HomeController : Controller
    {
        private IPrescriptionService _prescriptionService { get; }
        private IPatientService _patientService { get; }

        public HomeController(IPrescriptionService PrescriptionService,
            IPatientService PatientService)
        {
            _prescriptionService = PrescriptionService;
            this._patientService = PatientService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //## This is the Dashboard Page of a Doctor. Can view- Chart, Profile, Search Patients and Create a new Prescription
            PrescriptionCreateInitialVM vm = new PrescriptionCreateInitialVM()
            { 
                DoctorId = 3,
                HospitalId = 4,
                PatientId = 1,

                HospitalDetails = null,
                DoctorDetails = null
            };

            return View(vm);
        }


        [HttpGet]
        public async Task<IActionResult> SearchPatient()
        {
            //## Get Current Doctor's - Regular Patients
            var regularPatients = await _patientService.GetRegularPatientList(1);
            
            //## Doctor will Search Patients and Create a new Prescription
            PrescriptionCreateInitialVM vm = new PrescriptionCreateInitialVM()
            {
                DoctorId = 3,
                HospitalId = 4,
                PatientId = 1,

                PatientsList = regularPatients,
                PatientsListTitle = $"Recent visitors",
                HospitalDetails = null,
                DoctorDetails = null
            };

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
        public IActionResult ViewPatient(int id)
        {
            //## Doctor will View a Patient Profile- study and then New Prescription
            PrescriptionCreateInitialVM vm = new PrescriptionCreateInitialVM()
            {
                DoctorId = 3,
                HospitalId = 4,
                PatientId = 1,

                HospitalDetails = null,
                DoctorDetails = null
            };

            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> StartNewPrescription(int Id)
        {
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