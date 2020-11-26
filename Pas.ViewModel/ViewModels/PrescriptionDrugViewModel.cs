using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Web.ViewModels
{
    public class PrescriptionDrugCreateVM
    {
        public int PrescriptionId { get; set; }
        public int DrugId { get; set; }
        public int DrugBrandId { get; set; }

        public int BrandDoseTemplateId { get; set; }

        public int AdviseInstructionId { get; set; }
        //TODO: Add Notes field to the Table
        public string Notes { get; set; }
    }

    /// <summary>To View a Prescription Item in the Prescription</summary>
    public class PrescriptionDrugViewVM
    {
        public int PrescriptionItemId { get; set; }
        public string Name { get; set; }

        public string ModeOfDelivery { get; set; }
        public string Strength { get; set; }
        public string Dose { get; set; }
        public string IntakeTemplate { get; set; }
        public string Instruction { get; set; }
        public string Duration { get; set; }
    }
}
