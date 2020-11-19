namespace Pas.Web.ViewModels
{
    /// <summary>
    /// This will be used for "New Prescription Create" page- to list all the available DrugBrands for a particular Diagnosis
    /// </summary>
    public class DrugBrandsForDiagnosisVM
    {
        public int Id { get; set; }
        public int DrugId { get; set; }
        public string Name { get; set; }
        public int ManufacturerId { get; set; }
        public int IndicationsTypeId { get; set; }
    }
}
