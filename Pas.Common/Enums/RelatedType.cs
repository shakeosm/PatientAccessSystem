using System.ComponentModel.DataAnnotations;

namespace Pas.Common.Enums
{
    public enum RelatedType
    {
        Unknown = 0,
        Kids = 1,
        Spouse,
        Parents,
        Siblings,
        Uncle,
        Aunt,
        Nephew,
        Neice,
        Friend
    }

    public enum Gender
    {
        NotDisclosed = 0,
        Male = 1,
        Female = 2
    }

    public enum ApplicationRole
    {
        Patient = 1,
        Doctor = 2,
        Director = 3,        
        Hospital_SuperUser = 11,
        Hospital_Technician = 12,
        App_SuperUser = 99

            
    }

    public enum BloodGroup
    {
        Unknown = 0,

        [Display(Name = "A+", Description = "A RhD positive (A+)")]
        A_Positive = 1,

        [Display(Name = "A-", Description = "A RhD negative (A-)")]
        A_Negative,

        [Display(Name = "B+", Description = "B RhD positive (B+)")]
        B_Positive = 1,

        [Display(Name = "B-", Description = "B RhD negative (B-)")]
        B_Negative,

        [Display(Name = "O+", Description = "O RhD positive (O+)")]
        O_Positive,

        [Display(Name = "O-", Description = "O RhD negative (O-)e")]
        O_Negative,

        [Display(Name = "AB+", Description = "AB RhD positive (AB+)")]
        AB_Positive,
        
        [Display(Name = "AB-", Description = "AB RhD negative (AB-)")]
        AB_Negative,

    }

}
