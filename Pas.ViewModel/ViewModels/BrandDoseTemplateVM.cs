namespace Pas.Web.ViewModels
{
    /// <summary>
    /// This will be used for "New Prescription Create" page- to list all the available DrugBrands for a particular Diagnosis
    /// </summary>
    public class BrandDoseTemplateCreateVM
    {
        public int DrugBrandId { get; set; }
        
        public int ModeOfDeliveryId { get; set; }

        public int StrengthTypeId { get; set; }

        public short Dose { get; set; }

        public short Frequency { get; set; }

        public short Duration { get; set; }

        public int IntakePatternId { get; set; }
    }
}
