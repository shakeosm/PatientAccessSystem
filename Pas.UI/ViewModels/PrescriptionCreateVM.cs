using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pas.UI.ViewModels
{
    public class PrescriptionCreateVM
    {
        public HospitalDetailsVM HospitalDetails { get; set; }
        public DoctorDetailsVM DoctorDetails { get; set; }
        public PatientDetailsVM PatientDetails { get; set; }

        public int IndicatioId { get; set; }
        public IList<IndicationVM> indicationList { get; set; }
        
        public IList<DrugDetailsVM> DrugList { get; set; }

        public IList<DiagnosticTestDetailsVM> DiagnosticTestList { get; set; }

        //## Need while posting
        public int PreviousPrescriptionId { get; set; }

        public bool IsRepeatingVisit { get; set; }

        public string Notes { get; set; }
    }

    /// <summary>This is for POST method
    /// Drug Lsit will be saved via Ajax. Diagnostis Test recommendation will be saved via Ajax, too
    /// </summary>
    public class PrescriptionAddVM
    {
        public int Id { get; set; }
        public int IndicatioId { get; set; }

        //## Need while posting
        public int PreviousPrescriptionId { get; set; }

        public bool IsRepeatingVisit { get; set; }

        public string Notes { get; set; }
    }

    /// <summary>
    /// Once Doctor selects a Patient- will initiate a new Prescription.
    /// This will insert a Record in the Prescription table- with PatientId, DoctorId and Hospital Id.
    /// Remaining info will be taken in the UI
    /// </summary>
    public class PrescriptionCreateInitialVM
    {
        public int DoctorId { get; set; }
        public int HospitalId { get; set; }        
        public int PatientId { get; set; }
    }




}
