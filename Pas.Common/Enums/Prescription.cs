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

    public enum Tips
    {
        Advise = 1,
        Instructions = 2
    }

    public enum DrugUnitVolume
    {
        mcg,
        mg,
        gram,
        kg,
        pound,        
        ml,
        cc,
        litre,
        IU,
    }

    public enum ExminationCategory
    {
        [Display(Name = "Abdomen")]
        Abdominal = 1,
        [Display(Name = "CVS")]
        Cardiovascular = 2,
        [Display(Name = "R/S")]
        Respiratory,
        [Display(Name = "M/S")]
        Musculostkeletal,
        [Display(Name = "N/S")]
        Nervous, //## will use: public enum ExminationNervousSystemItems
        [Display(Name = "U/G")]
        Urogenital
    }

    public enum ExminationGenericItems
    { 
        Auscultation = 1,
        Inspection = 2,
        Palpation = 3,
        Percussion = 4  
    }

    public enum ExminationNervousSystemItems
    {
        Gait = 10,
        MotorSystem = 11,
        Sensory = 12
    }

    public enum InvestigationCategory
    {
        [Display(Name = "Biochemistry")]
        Biochemistry = 1,
        [Display(Name = "Blood Typing & Rh Factor")]
        Blood_Typing_And_Rh_Factor,
        [Display(Name = "Cross Matching")] 
        Cross_Matching,
        [Display(Name = "Cytology")]
        Cytology,
        [Display(Name = "Fluid")]
        Fluid,
        [Display(Name = "Haematology")]
        Haematology,
        [Display(Name = "Cross Matching")]
        Histopathology,
        [Display(Name = "Immunology")]
        Immunology,
        [Display(Name = "Microbiology")]
        Microbiology,
        [Display(Name = "Semen Analysis")]
        Semen_Analysis,
        [Display(Name = "Serology")]
        Serology,
        [Display(Name = "Stool")]
        Stool,
        [Display(Name = "Urine")]
        Urine


    }

}
