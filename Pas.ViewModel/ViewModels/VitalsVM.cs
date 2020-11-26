using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Web.ViewModels
{
    public class VitalsVM
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int PrescriptionId { get; set; }

        public short BloodPulse { get; set; }

        public short Diastolic { get; set; }

        public short Systolic { get; set; }

        public decimal Weight { get; set; }

        public decimal Temperature { get; set; }

        public DateTime DateAdded { get; set; }
    }
}
