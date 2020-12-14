namespace Pas.Web.ViewModels
{
    public class PrescriptionInvestigationVM
    {
        public int Id { get; set; }
        public int PrescriptionId { get; set; }
        public int InvestigationId { get; set; }
        public string Notes { get; set; }
    }
}
