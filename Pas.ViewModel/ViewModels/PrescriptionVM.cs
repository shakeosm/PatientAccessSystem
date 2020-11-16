using Pas.Data.Models;
using System;
using System.Collections.Generic;

namespace Pas.Web.ViewModels
{
    public class PrescriptionCreateVM
    {
        public int Id { get; set; }
        public AppUserDetailsVM Doctor { get; set; }
        public AppUserDetailsVM PatientDetails { get; set; }

        public int IndicationId { get; set; }
        public IList<IndicationTypes> indicationList { get; set; }
        
        public IEnumerable<DrugDetailsVM> DrugList { get; set; }

        public IList<DiagnosticTestDetailsVM> DiagnosticTestList { get; set; }

        public IList<string> PreviousNotes { get; set; }
        
        public IEnumerable<PatientAilmentType> Ailments { get; set; }
        
        /// <summary>List of known Allergies- from [Patient].[ClinicalHistory]</summary>
        public string AllergyList { get; set; }
        
        public IEnumerable<DrugDetailsVM> RecentMedication { get; set; }
    }

    /// <summary>This is for POST method
    /// Drug List will be saved via Ajax. Diagnostis Test recommendation will be saved via Ajax, too
    /// Just last few details will be saved - once all data is entered
    /// </summary>
    public class PrescriptionConfirmSaveVM
    {
        public int Id { get; set; }
        public int IndicatioId { get; set; }

        public bool IsFollowUpVisit { get; set; }

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
        public IEnumerable<AppUserDetailsVM> PatientsList { get; set; }
        
        public string PatientsListTitle { get; set; }

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

    /// <summary>To show the Prescription- to a Doctor or a Patient- to view a Prescription Details</summary>
    public class PrescriptionViewVM
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int HospitalId { get; set; }
        public string HospitalName { get; set; }
        public DateTime DateCreated{ get; set; }

        public IList<DrugItemVM> DrugItems { get; set; }

    }

    public class DrugItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string Dosage { get; set; }
        public int Quantity { get; set; }
        public int Duration { get; set; }
        public string ModeOfDelivery { get; set; }
    }



}
