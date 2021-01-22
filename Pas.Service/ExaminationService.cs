using Microsoft.EntityFrameworkCore;
using Pas.Data;
using Pas.Service.Interface;
using Pas.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pas.Service
{
    public class ExaminationService : IExaminationService
    {
        private readonly PasContext _dataContext;
        private readonly ICacheService _cacheService;

        public ExaminationService(PasContext DataContext,
            ICacheService CacheService)
        {
            _dataContext = DataContext;
            _cacheService = CacheService;
        }

        public async Task<IEnumerable<ExaminationItemVM>> GetCategories(bool includeGynaeOption = false)
        {
            string cacheKey = "ExaminationItemsVM";

            var cachedResult = _cacheService.GetCacheValue<IEnumerable<ExaminationItemVM>>(cacheKey);

            if (cachedResult is null)
            {
                //## Read all the Examination Items- Including the Main categories
                var result = await _dataContext.ExaminationItems
                                            .AsNoTracking()
                                            .Select(e => new ExaminationItemVM()
                                            {
                                                Id = e.Id,
                                                Name = e.Description,
                                                ParentId = e.ParentId ?? 0
                                            })
                                            .ToListAsync();

                //## Store all Items
                _cacheService.SetCacheValue(cacheKey, cachedResult);

            }

            //## Return the main Categories-> which don't have ParentId
            var categories = cachedResult.Where(e => e.ParentId == 0).ToList();
            if (includeGynaeOption == false)
            {
                categories.RemoveAll(c => c.Name == "P/A" || c.Name == "P/V");
            }

            return categories;
        }

        public IEnumerable<ExaminationItemVM> GetItems(int categoryId)
        {
            string cacheKey = "ExaminationItemsVM";

            var cachedResult = _cacheService.GetCacheValue<IEnumerable<ExaminationItemVM>>(cacheKey);
            //## NO need to to Check - if cachedResult is NULL! 

            //## Return the Child Items of a Main category
            var examItems = cachedResult.Where(e => e.ParentId == categoryId);

            return examItems;
        }

        public async Task<IEnumerable<ExaminationSubItemOptionsVM>> GetSubItems(int examItemId)
        {
            string cacheKey = "ExaminationSubItemOptionsVM";

            var cachedResult = _cacheService.GetCacheValue<IEnumerable<ExaminationSubItemOptionsVM>>(cacheKey);
            
            if (cachedResult is null)
            {
                cachedResult = await _dataContext.ExaminationItemOptions
                                            .AsNoTracking()
                                            .Select(e => new ExaminationSubItemOptionsVM()
                                            {
                                                Id = e.Id,
                                                Name = e.Description,
                                                ParentId = e.ExaminationId
                                            })
                                            .ToListAsync();                    

                //## Store all Items
                _cacheService.SetCacheValue(cacheKey, cachedResult);
            }                  

            //## Return the SubItems of an Examination Type
            var examinationSubItems = cachedResult.Where(e => e.ParentId == examItemId);

            return examinationSubItems;
        }

    }
}
