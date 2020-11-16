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

        public BloodGroup? BloodGroupType { get; set; }

        public string Smoker { get; set; }

        public string Drinker { get; set; }

        public string Excercise { get; set; }

        public string Sports { get; set; }

        public DateTime PersonalHistoryLastUpdated { get; set; }

        public string Height { get; set; }

        public string Weight { get; set; }

        public string Pulse { get; set; }

        public string BloodPressure { get; set; }

        public string Cholesterol { get; set; }

        public string Diabetes { get; set; }

        public DateTime ClinicalInfoLastUpdated { get; set; }

        public string AllergyInfo { get; set; }
        public IEnumerable<PatientAilmentType> AilmentList { get; set; }

        public IEnumerable<DrugDetailsVM> DrugList { get; set; }
    }
}
