using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Web.ViewModels
{
    public class DoctorViewPatientWrapperVM
    {
        public AppUserDetailsVM Doctor { get; set; }

        /// <summary>This ViewModel will be passed on to PatientProfileView page- from DoctorHomeViewPatient</summary>
        public PatientProfileWrapperVM PatientProfileWrapper { get; set; }
    }

}
