namespace Pas.Web.ViewModels
{
    public class InvestigationVM
    {
        public int Id { get; set; }        

        public string Description { get; set; }

        public int? ParentId { get; set; }
    }
}
