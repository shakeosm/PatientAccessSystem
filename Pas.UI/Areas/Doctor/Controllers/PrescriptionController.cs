using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pas.Service.Interface;
using Pas.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pas.UI.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize]
    public class PrescriptionController : Controller
    {

        private IPrescriptionService _prescriptionService { get; }

        public PrescriptionController(IPrescriptionService PrescriptionService)
        {
            _prescriptionService = PrescriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            id = 1;
            //## we get the PrescriptionId via Post call. use that to get Prescription info- Doctor, Hospital, Patient
            //## Doctor has already initiated a "New Prescription" from "Doctor/Home/StartNewPrescription()"- which is their Home page
            var prescription = await _prescriptionService.Find(id);

            
                        
            var newPrescriptionId = prescription.Id;

            HospitalDetailsVM hospitalDetails = null;
            DoctorDetailsVM doctorDetails = null;
            AppUserDetailsVM patientDetails = null;

            IList<IndicationVM> indicationList = null;
            IList<DrugDetailsVM> drugList = null;
            IList<DiagnosticTestDetailsVM> diagnosticTestList = null;

            //## PrescriptionCreateVM- will have all necessary info to make a Prescription- 
            //## When the Doc needs to see preview of Prescription before Print/Save

            var vm = new PrescriptionCreateVM()
            {
                Id = newPrescriptionId,
                DiagnosticTestList = diagnosticTestList,
                DoctorDetails = doctorDetails,
                DrugList = drugList,
                HospitalDetails = hospitalDetails,
                indicationList = indicationList,
                PatientDetails = patientDetails
            };

            return View("CreatePrescription", vm);
        }

        [HttpPost]
        public IActionResult Create(PrescriptionConfirmSaveVM vm)
        {
            return View();
        }
    }
}