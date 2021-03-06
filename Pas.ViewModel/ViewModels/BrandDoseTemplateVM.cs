﻿namespace Pas.Web.ViewModels
{
    /// <summary>
    /// This will be used for "New Prescription Create" page- to list all the available DrugBrands for a particular Diagnosis
    /// </summary>
    public class BrandDoseTemplateCreateVM
    {
        public int DrugBrandId { get; set; }
        
        public int ModeOfDeliveryId { get; set; }

        public int StrengthTypeId { get; set; }

        /// <summary>This will take the StrngthType in Text format (From UI) and read the Id from Table</summary>
        public string StrengthTypeText { get; set; }

        public short Dose { get; set; }

        public short Frequency { get; set; }

        public short Duration { get; set; }

        public int IntakePatternId { get; set; }
    }

    public class BrandDoseTemplateViewVM
    {
        public int DrugBrandId { get; set; }
        
        public int TemplateId { get; set; }

        public string TemplateName { get; set; }
    }
}
