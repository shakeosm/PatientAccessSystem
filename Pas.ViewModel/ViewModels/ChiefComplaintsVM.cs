using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Web.ViewModels
{
    public class ChiefComplaintsVM
    {
        public int Id { get; set; }

        public int PrescriptionId { get; set; }
        
        public int PatientId { get; set; }

        public string Name { get; set; }
    }
}
