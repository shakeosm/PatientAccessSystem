using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
/*
 * 
 * https://www.thereformedprogrammer.net/a-better-way-to-handle-authorization-in-asp-net-core/
 *  
 */
namespace Pas.Common.Enums
{
    public enum Permissions
    {
        //Here is an example of very detailed control over something
        [Display(Name = "Patient", Description = "Can view Patient Profile")]
        Patient = 0x10,
        [Display(Name = "Read", Description = "Can manage patient")]
        Doctor = 0x11,
        [Display(Name = "Read", Description = "Can manage hospital and users")]
        Director = 0x12,
        [Display(Name = "Read", Description = "Can manage data")]
        Technician = 0x13,

        [Display(Name = "Read", Description = "Can do anything")]
        SuperUser = 0x99,

    }
}
