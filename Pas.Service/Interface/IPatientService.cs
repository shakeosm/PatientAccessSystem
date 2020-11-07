using Pas.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pas.Service.Interface
{
    public interface IPatientService
    {
        
        
        /// <summary>
        /// This will add a Recommended Test to the Prescription
        /// </summary>
        /// <param name="prescriptionDiagnosticTest">PrescriptionDiagnosticTestCreateVM View Model</param>
        /// <returns>Id of the new Record</returns>
        Task<PatientDetailsVM> Find(int id);
        
        Task<PatientDetailsVM> FindByEmail(string email);

        Task<PatientDetailsVM> FindByMobile(string mobileNumber);
        
        Task<PatientDetailsVM> FindByShortId(string ShortId);

        Task<bool> Update(PatientDetailsVM patientDetailsVM);

        /// <summary>
        /// If a Patient haven't seen any Doctor within 3 months of creating an account- delete it.
        /// </summary>
        /// <param name="id">Patient Id</param>
        /// <returns></returns>
        Task<bool> Delete(int id);

        /// <summary>
        /// Drug List will be saved via Ajax. Diagnostis Test recommendation will be saved via Ajax, too
        /// Just last few details will be saved - once all data is entered
        /// </summary>
        /// <param name="prescriptionUpdateVM">PrescriptionUpdateVM View Model</param>
        /// <returns>True/False</returns>
        Task<bool> Update(PrescriptionConfirmSaveVM prescriptionUpdateVM);
        Task<IEnumerable<PatientDetailsVM>> SearchPatient(PatientSearchVM searchVM);

        Task<IEnumerable<PatientDetailsVM>> GetRegularPatientList(int doctorId);
    }
}
