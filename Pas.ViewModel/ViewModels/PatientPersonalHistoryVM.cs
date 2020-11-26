using Pas.Common.Enums;
using System;

namespace Pas.Web.ViewModels
{
    public class PatientPersonalHistoryVM
    {
        public int PatientId { get; set; }
        public BloodGroup BloodGroup { get; set; }

        public short Age { get; set; }
        
        public DateTime DateOfBirth { get; set; }

        public short SmokePerDay { get; set; }
        
        public DrinkHabit Drinker { get; set; }

        public bool Excercise { get; set; }

        public Sports Sports { get; set; }
        
        public string Height { get; set; }        
    }
}
