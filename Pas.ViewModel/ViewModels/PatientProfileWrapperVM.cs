using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Web.ViewModels
{
    /// <summary>This ViewModel will be passed on to PatientProfileView page- from DoctorHomeViewPatient</summary>
    public class PatientProfileWrapperVM
    {
        public AppUserDetailsVM Patient { get; set; }

        public ClinicalHistoryVM ClinicalInfo { get; set; }

        public IEnumerable<PrescriptionViewVM> PrescriptionList { get; set; }
                
        public IEnumerable<DiagnosticTestDetailsVM> LabResults { get; set; }
    }
}
