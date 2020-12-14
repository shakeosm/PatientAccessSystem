using Microsoft.EntityFrameworkCore;
using Pas.Common.Constants;
using Pas.Common.Enums;
using Pas.Data;
using Pas.Data.Models;
using Pas.Service.Interface;
using Pas.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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
                indicationTypes = await _context.IndicationTypes.Where(i=> i.Show==true).ToListAsync();

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
                symptomList = await _context.Symptoms
                                            .AsNoTracking()
                                            .OrderBy(s=> s.Description)
                                            .ToListAsync();

                _cacheService.SetCacheValue(cacheKey, symptomList);

            }

            return symptomList;
        }

        public async Task<IList<DrugBrandsForDiagnosisVM>> ListAllBrandsForDiagnosis(int diagnosisId = 0)
        {
            string cacheKey = $"{CacheKey.BrandsForDiagnosis}_{diagnosisId}";

            var brandList = _cacheService.GetCacheValue<List<DrugBrandsForDiagnosisVM>>(cacheKey);
            bool applyFilter = diagnosisId > 0;

            if (brandList is null)
            {
                try
                {
                    //Expression<Func<BrandForIndications, bool>> searchPredicate = p => (applyFilter && p.IndicationTypeId == diagnosisId);
                    Expression<Func<BrandForIndications, bool>> searchPredicate = o => (true);
                    if (diagnosisId > 0) {
                        searchPredicate = p => (p.IndicationTypeId == diagnosisId);
                    }

                    var result = await _context.BrandForIndications
                                                .Where(searchPredicate)
                                                .AsNoTracking()
                                                .Include(d=> d.DrugBrands)
                                                .ToListAsync();
                
                    brandList = result.Select(r => new DrugBrandsForDiagnosisVM()
                    {
                        Id = r.DrugBrandId,
                        Name = r.DrugBrands.BrandName
                    }).ToList();

                    _cacheService.SetCacheValue(cacheKey, brandList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }


            }

            return brandList;
        }

        public Task<IList<DrugIntakePatternsForDiagnosisVM>> ListAllDrugPatternTemplates(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IList<InvestigationForDiagnosisVM>> ListAllInvestigationsForDiagnosis(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<IntakePattern>> ListAllIntakePatterns()
        {
            string redisKey = CacheKey.IntakePatterns;

            var intakePatterns = _cacheService.GetCacheValue<List<IntakePattern>>(redisKey);

            if (intakePatterns is null)
            {
                intakePatterns = await _context.IntakePatterns.ToListAsync();

                _cacheService.SetCacheValue(redisKey, intakePatterns);

            }

            return intakePatterns;
        }

        public async Task<int> Insert_DrugBrandForDiagnosis(DrugBrandsForDiagnosisVM vm, int userId)
        {
            int brandId = 0;
            //## If no ID provided- then Create a New Brand and Insert the Brand<->Diagnosis links
            if (vm.Id < 1)
            {
                DrugBrands drugBrands = new DrugBrands()
                {
                    DrugId = vm.DrugId,
                    BrandName = vm.Name,
                    ManufacturerId = vm.ManufacturerId,
                    DateAdded = DateTime.Now,

                    BrandForIndications = new List<BrandForIndications>() {
                        new BrandForIndications() {
                            IndicationTypeId = vm.IndicationsTypeId,
                            AddedBy = userId
                        }
                    }
                };

                await _context.DrugBrands.AddAsync(drugBrands);
                await _context.SaveChangesAsync();

                brandId = drugBrands.Id;

            }
            else {
                //## DrugBrand Exist-- Just create a new Brand to Doagnosis Assignment
                BrandForIndications brandForIndications = new BrandForIndications()
                {
                    DrugBrandId = vm.Id,
                    IndicationTypeId = vm.IndicationsTypeId,
                    AddedBy = userId
                };

                await _context.BrandForIndications.AddAsync(brandForIndications);
                await _context.SaveChangesAsync();

                brandId = vm.Id;
            }


            //## Now Update the Cache- insert the new Value in the Redis cache
            var cachedList = await ListAllBrandsForDiagnosis(vm.IndicationsTypeId);
            cachedList.Add(vm);
            string cacheKey = $"ListAllBrandsForDiagnosis_{vm.IndicationsTypeId}";
            _cacheService.SetCacheValue(cacheKey, cachedList);

            return brandId;  //## Return BrandID- so we can Add this new brand in the SelectList - with Value/Id
        }

        public async Task<string> Insert_BrandDoseTemplate(BrandDoseTemplateCreateVM vm, int userId)
        {
            try
            {
                //## First get the Strength id- read the list from Cache- don't go to DB.
                var strengthTypeList = _cacheService.GetCacheValue<List<StrengthType>>(CacheKey.StrengthTypes);

                var strengthType = strengthTypeList.FirstOrDefault(s => s.Name.Equals(vm.StrengthTypeText));        //## Use the Text to find the id, ie: '10 mg' = Id:2
                if (strengthType is null)
                {
                    //## Something peculiar strength may be, ie: "199 mg"--> insert it in the DB
                    strengthType = new StrengthType() { Name = vm.StrengthTypeText };

                    await _context.StrengthTypes.AddAsync(strengthType);
                    await _context.SaveChangesAsync();
                }

                string teamplateName = CreateTemplateName(vm);

                BrandDoseTemplate doseTemplate = new BrandDoseTemplate()
                {
                    DrugBrandId = vm.DrugBrandId,
                    ModeOfDeliveryId = vm.ModeOfDeliveryId,
                    StrengthTypeId = strengthType.Id,
                    Dose = vm.Dose,
                    Frequency = vm.Frequency,
                    IntakePatternId = vm.IntakePatternId,
                    Duration = vm.Duration,
                    CreatedBy = userId,
                    DateCreated = DateTime.Now,
                    TemplateText = teamplateName
                };

                await _context.BrandDoseTemplates.AddAsync(doseTemplate);
                await _context.SaveChangesAsync();
                
                //## Add this new BrandDoseTemplate to the Cache- so others can see it immediately
                UpdateBrandDoseTemplateIn_Cache(doseTemplate);                

                string result = $"{doseTemplate.Id};{teamplateName}";   //## Combine the Id of newly inserted Template and the TemplateTExt- to add to the Dropdown- on Success

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return "error";
                //throw;
            }
            

        }

        /// <summary>This will create DrugDose Template, ie: '10 mg 1x3T-7D'</summary>
        /// <param name="vm">BrandDoseTemplateCreateVM view model</param>
        /// <returns></returns>
        private string CreateTemplateName(BrandDoseTemplateCreateVM vm)
        {
            //## Get ModeOfDelivery List- and find the 'Delivery' Text
            var listOfDeliveryMode = _cacheService.GetCacheValue<IList<ModeOfDelivery>>(CacheKey.ModeOfDeliveries);
            var deliveryMode = listOfDeliveryMode.First(d => d.Id == vm.ModeOfDeliveryId).Name; //## we will know- Tablet/Capsule/Syrup!
            string drugDose = vm.Dose == (short)DoseSolid.Half ? "half" : vm.Dose.ToString();

            string durationText = vm.Duration == 0 ? "TBC" : vm.Duration.ToString() + " Days";
            string teamplateName = $"{vm.StrengthTypeText}-{deliveryMode}-{vm.Dose}*{vm.Frequency}Times-{durationText}"; //## ie: "10mg-Tablet-1*3Times-7 Days"
            return teamplateName;
        }

        private void UpdateBrandDoseTemplateIn_Cache(BrandDoseTemplate doseTemplate)
        {
            //## Add this new BrandDoseTemplate to the Cache- so others can see it immediately
            var existingBrandDoseTemplate = _cacheService.GetCacheValue<IList<BrandDoseTemplateCreateVM>>(CacheKey.BrandDoseTemplate); //## Get from Cache
            if (existingBrandDoseTemplate is null)
                existingBrandDoseTemplate = new List<BrandDoseTemplateCreateVM>();

            BrandDoseTemplateCreateVM newBrandDoseTeamplate = MapTo_BrandDoseTemplateVM(doseTemplate);
            existingBrandDoseTemplate.Add(newBrandDoseTeamplate);                                        //## Add this new BrandTemplate
            _cacheService.SetCacheValue(CacheKey.BrandDoseTemplate, existingBrandDoseTemplate);        //## Push that new list to Cache
        }

        /// <summary>This will Map the 'BrandDoseTemplate' to 'BrandDoseTemplateCreateVM' and make it easier to store in the Redis Cache</summary>
        /// <param name="dose">BrandDoseTemplate object</param>
        /// <returns>BrandDoseTemplateCreateVM View Model</returns>
        private BrandDoseTemplateCreateVM MapTo_BrandDoseTemplateVM(BrandDoseTemplate dose)
        {
            BrandDoseTemplateCreateVM result = new BrandDoseTemplateCreateVM() { 
                DrugBrandId = dose.DrugBrandId,
                IntakePatternId = dose.IntakePatternId,
                Duration = dose.Duration,
                ModeOfDeliveryId = dose.ModeOfDeliveryId,
                StrengthTypeId = dose.StrengthTypeId.Value,
                Dose = dose.Dose.Value,
                Frequency = dose.Frequency
            };

            return result;
        }

        public async Task<IList<ModeOfDelivery>> LisAllModeOfDeliveries()
        {

            var modeOfDeliveryList = _cacheService.GetCacheValue<List<ModeOfDelivery>>(CacheKey.ModeOfDeliveries);

            if (modeOfDeliveryList is null)
            {
                modeOfDeliveryList = await _context.ModeOfDelivery
                                                        .OrderBy(m=> m.RowOrder)
                                                        .ToListAsync();

                _cacheService.SetCacheValue(CacheKey.ModeOfDeliveries, modeOfDeliveryList);

                //## An extra peice of work- while reading Mode Of deliveries- also Load the StrengthList and save in the cache for future usage
                SetAllStrengthTypesInCache();

            }


            return modeOfDeliveryList;
        }

        private void SetAllStrengthTypesInCache()
        {
            var allStrengthTypes = _cacheService.GetCacheValue<List<StrengthType>>(CacheKey.StrengthTypes);

            if (allStrengthTypes is null)
            {
                allStrengthTypes = _context.StrengthTypes.ToList();

                _cacheService.SetCacheValue(CacheKey.StrengthTypes, allStrengthTypes);

            }
        }

        /// <summary>This will load all the Dose Templates for a Brand Drug, ie: Nurofen with 3 possible Dose Templates</summary>
        /// <param name="drugBrandId">Brand Id, ie: Nurofen / Calpol </param>
        /// <returns>List of Dose Templates</returns>
        public async Task<IList<BrandDoseTemplateViewVM>> ListAllBrandsDoseTemplates(int drugBrandId)
        {
            string doseCacheKey = $"{CacheKey.BrandDoseTemplate}_{drugBrandId}"; 
            var templateList = _cacheService.GetCacheValue<List<BrandDoseTemplateViewVM>>(doseCacheKey);

            if (templateList is null || templateList.Count < 1)
            {
                try
                {
                    var result = await _context.BrandDoseTemplates
                                                        .AsNoTracking()
                                                        .Where(bd=> bd.DrugBrandId == drugBrandId)
                                                        .ToListAsync();

                    templateList = result.Select(r => new BrandDoseTemplateViewVM()
                    {
                        TemplateId = r.Id,
                        DrugBrandId = r.DrugBrandId,
                        TemplateName = r.TemplateText
                    }).ToList();

                    _cacheService.SetCacheValue(doseCacheKey, templateList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    throw;
                }


            }

            return templateList;
        }
    }
}
