using Pas.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pas.Service.Interface
{
    public interface IPatientService
    {                
        /// <summary>
        /// Drug List will be saved via Ajax. Diagnostis Test recommendation will be saved via Ajax, too
        /// Just last few details will be saved - once all data is entered
        /// </summary>
        /// <param name="prescriptionUpdateVM">PrescriptionUpdateVM View Model</param>
        /// <returns>True/False</returns>
        Task<bool> Update(PrescriptionConfirmSaveVM prescriptionUpdateVM);
        Task<IEnumerable<AppUserDetailsVM>> SearchPatient(PatientSearchVM searchVM);

        Task<IEnumerable<AppUserDetailsVM>> GetRegularPatientList(int doctorId);
        Task<ClinicalHistoryVM> GetClinicalDetails(int id);

        Task<IEnumerable<ChiefComplaintsVM>> GetPatientChiefComplaints(int id);

        Task<IEnumerable<DrugDetailsVM>> GetRecentMedication(int id);
        Task<bool> UpdatePersonalHistory(PatientPersonalHistoryVM vm);
    }
}
