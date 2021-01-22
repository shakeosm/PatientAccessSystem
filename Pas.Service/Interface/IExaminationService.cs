using Pas.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pas.Service.Interface
{
    public interface IExaminationService
    {

        Task<IEnumerable<ExaminationItemVM>> GetCategories(bool includeGynaeOption = false);

        IEnumerable<ExaminationItemVM> GetItems(int categoryId);

        Task<IEnumerable<ExaminationSubItemOptionsVM>> GetSubItems(int examItemId);

    }
}
