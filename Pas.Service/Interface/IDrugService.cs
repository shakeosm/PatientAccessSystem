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

    }
}