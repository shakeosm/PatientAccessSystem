using System.Collections.Generic;

namespace Pas.Web.ViewModels
{
    public class PrescriptionCreateVM
    {
        public int Id { get; set; }
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
    /// Drug List will be saved via Ajax. Diagnostis Test recommendation will be saved via Ajax, too
    /// Just last few details will be saved - once all data is entered
    /// </summary>
    public class PrescriptionConfirmSaveVM
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

        public HospitalDetailsVM HospitalDetails { get; set; }
        public DoctorDetailsVM DoctorDetails { get; set; }
        
        /// <summary>Doctor may chose to select one from the regular patient and create a new Prescription</summary>
        public IEnumerable<PatientDetailsVM> RegularPatients { get; set; }
        public PatientSearchVM SearchVM { get; set; } = new PatientSearchVM();
    }

    //## Get Method View Model
    public class PrescriptionAddVM
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int HospitalId { get; set; }
        public int PatientId { get; set; }
    }






}
