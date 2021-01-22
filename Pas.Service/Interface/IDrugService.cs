using Pas.Common.Enums;
using Pas.Data.Models;
using Pas.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pas.Service.Interface
{
    public interface IDrugService
    {
        Task<IEnumerable<DrugDetailsVM>> ListAll(bool forceDbRead = false);
        
        Task<Drugs> Find(int id);

        Task<Drugs> Add(Drugs drug);
        
        Task<Drugs> Update(Drugs drug);
        
        Task<bool> Delete(int id);

        Task<IList<IndicationTypes>> ListAllIndicationTypes();
        
        Task<IList<AdviseInstructions>> ListAllDrugTips(Tips tips);
        
        Task<IList<Manufacturer>> ListAllMenufacturer();
        
        Task<IList<ChiefComplaints>> ListAllChiefComplaints();
        Task<IList<DrugBrandsForDiagnosisVM>> ListAllBrandsForDiagnosis(int id = 0);

        /// <summary>This will load all the Dose Templates for a Brand Drug, ie: Nurofen with 3 possible Dose Templates</summary>
        /// <param name="drugBrandId">Brand Id, ie: Nurofen / Calpol </param>
        /// <returns>List of Dose Templates</returns>
        Task<IList<BrandDoseTemplateViewVM>> ListAllBrandsDoseTemplates(int drugBrandId);      
        Task<IList<DrugIntakePatternsForDiagnosisVM>> ListAllDrugPatternTemplates(int id);
        Task<IList<InvestigationForDiagnosisVM>> ListAllInvestigationsForDiagnosis(int id);               
        Task<IList<IntakePattern>> ListAllIntakePatterns ();
        Task<IList<ModeOfDelivery>> LisAllModeOfDeliveries();
        
        Task<int> Insert_DrugBrandForDiagnosis(DrugBrandsForDiagnosisVM vm, int userId);
        Task<string> Insert_BrandDoseTemplate(BrandDoseTemplateCreateVM vm, int userId);
        
    }
}