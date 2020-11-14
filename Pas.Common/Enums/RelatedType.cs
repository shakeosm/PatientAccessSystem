using System.ComponentModel.DataAnnotations;

namespace Pas.Common.Enums
{
    public enum Title
    {
        Unknwon = 0,
        Mr,
        Ms,
        Mrs,
        Dr,
        Esq,
        Hon,
        Prof,
        Rt,
        Rev,
        Moulvi,
        Mawlana,
        Other = 99
    }

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
        Technician = 4,
        SuperUser = 11,
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
