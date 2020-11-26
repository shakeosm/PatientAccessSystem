using Pas.Data.Models;
using Pas.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pas.Service.Interface
{
    public interface IPrescriptionService
    {

        Task<Prescription> Find(int id);

        /// <summary>
        /// Once Doctor selects a Patient- will initiate a new Prescription.
        /// This will insert a Record in the Prescription table- with PatientId, DoctorId and Hospital Id.
        /// </summary>
        /// <param name="vm">PrescriptionCreateInitialVM ViewModel</param>
        /// <returns>Newly created Prescription Id</returns>
        Task<int> CreateInitialDefault(PrescriptionCreateInitialVM vm);

        /// <summary>
        /// This will add a Drug to the Prescription
        /// </summary>
        /// <param name="prescriptionDrug">Drug Details View Model</param>
        /// <returns>Id of the new Record</returns>
        Task<int> AddDrugToPrescription(PrescriptionDrugCreateVM prescriptionDrug);


        /// <summary>
        /// This will add a Recommended Test to the Prescription
        /// </summary>
        /// <param name="prescriptionDiagnosticTest">PrescriptionDiagnosticTestCreateVM View Model</param>
        /// <returns>Id of the new Record</returns>
        Task<int> AddTestToPrescription(PrescriptionDiagnosticTestCreateVM prescriptionDiagnosticTest);
        
        /// <summary>
        /// Drug List will be saved via Ajax. Diagnostis Test recommendation will be saved via Ajax, too
        /// Just last few details will be saved - once all data is entered
        /// </summary>
        /// <param name="prescriptionUpdateVM">PrescriptionUpdateVM View Model</param>
        /// <returns>True/False</returns>
        Task<bool> Update(PrescriptionConfirmSaveVM prescriptionUpdateVM);

        Task<IEnumerable<Prescription>> ListByPatient(int id);

        /// <summary>This will Insert a New drug Item in the Prescription</summary>
        /// <param name="vm">PrescriptionDrugCreateVM View Model</param>
        /// <returns>A String- semi-colon separated value</returns>
        Task<string> Insert_PescriptionItem(PrescriptionDrugCreateVM vm);


        Task<bool> Delete_PescriptionItem(int prescriptionItemId);

        Task<bool> Prescription_FinishAndCreate_HTML(PrescriptionConfirmSaveAndFinishVM vm);

        Task<string> GetPrescription_HTML(int id);
        Task<int> Update_Vitals(VitalsVM vm);
    }
}
