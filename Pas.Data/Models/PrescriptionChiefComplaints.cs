﻿using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{

    public partial class PrescriptionChiefComplaints : BaseEntityModel
    {
        [Required]
        public int PrescriptionId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int SymptomId { get; set; }
       

        public virtual Symptoms Symptom { get; set; }

        public virtual User Patient { get; set; }
    }
}
