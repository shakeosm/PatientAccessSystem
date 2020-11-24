using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace Pas.Common.Enums
{
    
    public enum PrescriptionStatus
    {
        Draft = 1,
        Complete = 2,
        Abandoned = 9
    } 
    public enum DoseSolid
    {
        Half = 50,
        One = 1,
        Two = 2,
        Three = 3
    }

    public enum DoseLiquid
    {
        [Display(Name = "One Spoon")]
        One_Spoon = 5,
        [Display(Name = "Two Spoons")]
        Two_Spoon = 6,
        [Display(Name = "Three Spoons")]
        Three_Spoon = 7        
    }

    public enum MealsCondition
    {
        Not_Related =0,
        Before = 1,
        After = 2

    }

    public enum Tips
    {
        Advise = 1,
        Instructions = 2
    }

    public enum DrugUnitVolume
    {
        cc,
        gram,
        IU,
        kg,
        litre,
        mcg = 1,
        mg,
        mm,
        ml,
        pound,

    }

}
