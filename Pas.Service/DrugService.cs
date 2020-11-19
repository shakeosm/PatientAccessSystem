using Microsoft.EntityFrameworkCore;
using Pas.Common.Enums;
using Pas.Data;
using Pas.Data.Models;
using Pas.Service.Interface;
using Pas.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pas.Service
{
    public class DrugService : IDrugService
    {
        private readonly PasContext _context;
        private readonly ICacheService _cacheService;

        public DrugService(PasContext Context, ICacheService CacheService)
        {
            _context = Context;
            _cacheService = CacheService;
        }

        public Task<Drugs> Add(Drugs drug)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Drugs> Find(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DrugDetailsVM>> ListAll(bool forceDbRead = false)
        {
            string cacheKey = "DrugListAll";
            var cachedResult = _cacheService.GetCacheValue<IEnumerable<DrugDetailsVM>>(cacheKey);

            if (cachedResult is null || forceDbRead) {

                var dbSearch = await _context.Drugs
                                                    .Include(d => d.DrugCategoryType)
                                                    .AsNoTracking()
                                                    .ToListAsync();

                cachedResult = MapToDrugListViewModel(dbSearch);

                //## Keep this search result for next 4 hours
                _cacheService.SetCacheValue(cacheKey, cachedResult);
            }

            return cachedResult;

        }

        /// <summary>This will Map the Drug list to DrugListViewModel- for Configure/Drug view page</summary>
        /// <param name="dbSearch"></param>
        /// <returns></returns>
        private IEnumerable<DrugDetailsVM> MapToDrugListViewModel(List<Drugs> dbSearch)
        {
            var result = dbSearch.Select(d=> new DrugDetailsVM()
            { 
                Id = d.Id,
                Name = d.Name,
                Category = d.DrugCategoryType.Name,
            });

            return result;
        }


        public Task<Drugs> Update(Drugs drug)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<IndicationTypes>> ListAllIndicationTypes()
        {
            string cacheKey = "ListAllIndicationTypes";
            var indicationTypes = _cacheService.GetCacheValue<List<IndicationTypes>>(cacheKey);

            if (indicationTypes is null)
            {
                indicationTypes = await _context.IndicationTypes.ToListAsync();

                _cacheService.SetCacheValue(cacheKey, indicationTypes);
                
            }

            return indicationTypes;
        }

        public async Task<IList<Manufacturer>> ListAllMenufacturer()
        {
            string cacheKey = "ListAllMenufacturer";
            var manufacturerList = _cacheService.GetCacheValue<List<Manufacturer>>(cacheKey);

            if (manufacturerList is null)
            {
                manufacturerList = await _context.Manufacturers
                                                    .OrderBy(m=> m.Name)
                                                    .ToListAsync();

                _cacheService.SetCacheValue(cacheKey, manufacturerList);

            }

            return manufacturerList;
        }

        public async Task<IList<AdviseInstructions>> ListAllDrugTips(Tips tips)
        {
            string cacheKey = $"ListAllDrugTips_{tips}";

            var drugTips = _cacheService.GetCacheValue<List<AdviseInstructions>>(cacheKey);

            if (drugTips is null)
            {
                drugTips = await _context.AdviseInstructions
                                                .Where(m => m.TypeId == (int)tips)
                                                .ToListAsync();

                _cacheService.SetCacheValue(cacheKey, drugTips);

            }

            return drugTips;
        }

        public async Task<IList<Symptoms>> ListAllSymptoms()
        {
            string cacheKey = $"ListAllSymptoms";

            var symptomList = _cacheService.GetCacheValue<List<Symptoms>>(cacheKey);

            if (symptomList is null)
            {
                symptomList = await _context.Symptoms.ToListAsync();

                _cacheService.SetCacheValue(cacheKey, symptomList);

            }

            return symptomList;
        }
    }
}
