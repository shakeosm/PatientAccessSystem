using Pas.Common.Enums;
using Pas.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Web.ViewModels
{
    public class ClinicalHistoryVM
    {
        public int UserId { get; set; }

        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }

        public BloodGroup? BloodGroupType { get; set; }

        public short? Smoker { get; set; }

        public string Drinker { get; set; }

        public bool Excercise { get; set; }
        public string ExcerciseDisplay() => Excercise ? "Yes" : "No";

        public Sports Sports { get; set; }

        public DateTime PersonalHistoryLastUpdated { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string Pulse { get; set; }

        public string BloodPressure { get; set; }

        public string Cholesterol { get; set; }

        public string Diabetes { get; set; }

        public DateTime ClinicalInfoLastUpdated { get; set; }

        /// <summary>List of known Allergies- from [Patient].[ClinicalHistory]</summary>
        public IList<string> AllergyList { get; set; }
        public IEnumerable<DrugDetailsVM> RecentMedication { get; set; }
        public IEnumerable<IndicationVM> RecentDiagnosis { get; set; }
        //public IEnumerable<ChiefComplaintsVM> ChiefComplaints{ get; set; }        
    }
}
