using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class DoctorMedicalDegrees : BaseEntityModel
    {        
        [Required]
        public int DoctorId { get; set; }
        
        [Required]
        public int MedicalDegreeId { get; set; }
        
        public virtual DoctorProfile Doctor { get; set; }
        public virtual MedicalDegree MedicalDegree { get; set; }
    }
}
