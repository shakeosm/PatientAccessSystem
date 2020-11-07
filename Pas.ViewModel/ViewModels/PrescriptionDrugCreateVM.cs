using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Web.ViewModels
{
    public class PrescriptionDrugCreateVM
    {
        public int PrescriptionId { get; set; }
        public int DrugId { get; set; }
        public int DosageId { get; set; }
        public int ModeOfDeliveryId { get; set; }
        public int Quantity { get; set; }
        public int DurationDays { get; set; }
        //TODO: Add Notes field to the Table
        public string Notes { get; set; }
    }
}
