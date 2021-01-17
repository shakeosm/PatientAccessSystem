using System.ComponentModel.DataAnnotations;

namespace Pas.Web.ViewModels
{
    public class PrescriptionPrintPreviewVM
    {
        public int Id { get; set; }

        public string HospitalHeader { get; set; }
        public string DoctorHeader { get; set; }
        public string PatientDetails { get; set; }
        public string DateTime { get; set; }
        public string CC_Details { get; set; }
        public string Examination { get; set; }
        public string Notes { get; set; }
        public string Plans { get; set; }
        public string Advise { get; set; }
        public string Investigations { get; set; }
        public string DrugDetails { get; set; }

    }
}
