using Microsoft.EntityFrameworkCore;
using Pas.Common.Constants;
using Pas.Common.Enums;
using Pas.Common.Extensions;
using Pas.Data;
using Pas.Data.Models;
using Pas.Service.Interface;
using Pas.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pas.Service
{
    public class PatientService : IPatientService
    {
        private readonly ICacheService _cacheService;

        private PasContext _pasContext { get; }
        public PatientService(PasContext PasContext,
                        ICacheService CacheService)
        {
            _pasContext = PasContext;
            _cacheService = CacheService;
        }

        

        public Task<bool> Update(PrescriptionConfirmSaveVM prescriptionUpdateVM)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUserDetailsVM>> SearchPatient(PatientSearchVM searchVM)
        {
            bool hasMobileNumber = ( !string.IsNullOrEmpty(searchVM.Mobile) && searchVM.Mobile.Length >= 10);
            bool hasShortId = (!string.IsNullOrEmpty(searchVM.ShortId) && searchVM.ShortId.Length >= 5);
            bool hasFirstName = (!string.IsNullOrEmpty(searchVM.FirstName) && searchVM.FirstName.Length >= 3);
            bool hasLastName = (!string.IsNullOrEmpty(searchVM.LastName) && searchVM.LastName.Length >= 3);

            Expression<Func<User, bool>> userSearch = u => ((hasMobileNumber && u.Mobile.Equals(searchVM.Mobile))
                                                            || (hasShortId && u.ShortId.Equals(searchVM.ShortId))
                                                            || (
                                                                    (hasFirstName && u.FirstName.Contains(searchVM.FirstName))
                                                                    ||
                                                                    (hasLastName && u.LastName.Contains(searchVM.FirstName))
                                                                )
                                                            );

            var patientList = await Search(userSearch);

            var mappedResult = MapSeachResultToAppUserViewModel(patientList);

            return mappedResult;

            }

        public async Task<IEnumerable<AppUserDetailsVM>> GetRegularPatientList(int doctorId)
        {

            //IEnumerable<AppUserDetailsVM> result = new List<AppUserDetailsVM>();
            string cacheKey = $"{doctorId}_RegularPatient";

            IEnumerable<AppUserDetailsVM> cachedResult = _cacheService.GetCacheValue<IEnumerable<AppUserDetailsVM>>(cacheKey);    //## Recent-Patients are saved for Doctor Home Page

            if (cachedResult is null) {
                Expression<Func<User, bool>> userSearch = u => (u.FirstName.Contains("Ma"));    //TODO: Read the Patients - seen by this Doctor

                var searchResult = await Search(userSearch);

                cachedResult = MapSeachResultToAppUserViewModel(searchResult);

                _cacheService.SetCacheValue(cacheKey, cachedResult);
            }


            return cachedResult;
        }

        /// <summary>Search Patients as per your Search Expression</summary>
        /// <param name="userSearch">Search expression</param>
        /// <returns>Get PatientList in AppUserDetailsVM</returns>
        private async Task<List<User>> Search(Expression<Func<User, bool>> userSearch)
        {
            try
            {
                List<User> result = await _pasContext.User
                                            .Include(u=> u.AddressBooks)
                                            .AsNoTracking()
                                            .Where(userSearch).ToListAsync();


                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }


        /// <summary>This will Map a list of Users to AppUserDetail view Model
        /// Used in - Doctor/Home/SearchPatient- to list all the Patients in a Table</summary>
        /// <param name="result">List of Users/Aptients</param>
        /// <returns>AppUserDetailsVM</returns>
        private IEnumerable<AppUserDetailsVM> MapSeachResultToAppUserViewModel(IEnumerable<User> result)
        {

            IEnumerable<AppUserDetailsVM> resultVM = result.Select(u => new AppUserDetailsVM()
            {
                Id = u.Id,
                Name = $"{u.FirstName} {u.LastName}",
                BanglaName = u.BanglaName,
                Mobile = u.Mobile,
                ShortId = u.ShortId,
                Age = u.Age,
                DateOfBirth = u.DateOfBirth.ToDDMMMYYYY(),
                Gender = (Gender)u.Gender,
                AddressAreaLocality = $"{u.AddressBooks.FirstOrDefault().LocalArea}, {(Common.Enums.City)u.AddressBooks.FirstOrDefault().CityId}"
                //AddressBook = new AddressBookVM()
                //{
                //    AddressLine1 = u.AddressBooks.FirstOrDefault().AddressLine1,
                //    LocalArea = u.AddressBooks.FirstOrDefault().LocalArea,
                //    City = ((Common.Enums.City)u.AddressBooks.FirstOrDefault().CityId).ToString(),
                //}
            }); ;


            return resultVM;

        }


        public async Task<ClinicalHistoryVM> GetClinicalDetails(int id)
        {
            string redisKey = $"{CacheKey.ClinicalDetails}_{id}";

            var cachedResult = _cacheService.GetCacheValue<ClinicalHistoryVM>(redisKey);
            if (cachedResult is null)
            {
                var search = await _pasContext.ClinicalHistory
                        .AsNoTracking()
                        .Include(c => c.User)
                        .FirstOrDefaultAsync(c => c.UserId == id);

                cachedResult = MapToClinicalHistoryVM(search);

                cachedResult.RecentMedication = await GetRecentMedication(id);
                cachedResult.RecentDiagnosis = await GetRecentDiagnosis(id);

                _cacheService.SetCacheValue(redisKey, cachedResult);
            }
            
            return cachedResult;
        }

        /// <summary>
        /// This will get the list of Recent Medication- prescribed to the Patient. So- Doctor can assess the situation
        /// </summary>
        /// <param name="id">Patient Id</param>
        /// <returns></returns>
        private async Task<IEnumerable<IndicationVM>> GetRecentDiagnosis(int id)
        {
            string redisKey = $"{CacheKey.RecentDiagnosis}_{id}";

            var medicationList = _cacheService.GetCacheValue<IEnumerable<IndicationVM>>(redisKey);
            if (medicationList is null)
            {
                var patientMedications = await _pasContext.PatientIndications
                                                    .Include(pi => pi.IndicationType)
                                                    .AsNoTracking()
                                                    .Where(pi => pi.PatientId == id)
                                                    .Select(pi=> new { 
                                                        pi.IndicationType.Id,
                                                        pi.IndicationType.Name
                                                    })
                                                    .ToListAsync();

                medicationList = patientMedications.Select(p=> new IndicationVM() { 
                    Id = p.Id,
                    Name = p.Name
                });

                _cacheService.SetCacheValue(redisKey, medicationList);

            }

            return medicationList;
        }


        private ClinicalHistoryVM MapToClinicalHistoryVM(ClinicalHistory ch)
        {
            try
            {
                ClinicalHistoryVM clinicalHistoryVM = new ClinicalHistoryVM()
                {
                    UserId = ch.UserId,
                    Age = ch.User.Age,
                    BloodGroupType = ch.BloodGroupId is null ? BloodGroup.Unknown : (BloodGroup)ch.BloodGroupId,
                    Smoker = ch.Smoker,
                    Drinker = ch.Drinker.HasValue ? ((DrinkHabit) ch.Drinker).ToString() : "",
                    Excercise = ch.Excercise.Value,
                    Sports = (Sports) ch.Sports,

                    BloodPressure = (ch.PressureSystolic is null ? "-" : $"{ch.PressureSystolic} / {ch.PressureDiastolic}"),
                    Pulse = ch.Pulse.ToString(),
                    Cholesterol = ch.Cholesterol?.ToString(),
                    Diabetes = ch.Diabetes?.ToString(),
                    Height = ch.Height,
                    Weight = ch.Weight?.ToString(),

                    AllergyList = ch.AllergyInfo.Split(",", StringSplitOptions.RemoveEmptyEntries),

                    ClinicalInfoLastUpdated = ch.ClinicalInfoLastUpdated,
                    PersonalHistoryLastUpdated = ch.PersonalHistoryLastUpdated
                };                                

                return clinicalHistoryVM;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ClinicalHistoryVM();
            }

        }

        public async Task<IEnumerable<ChiefComplaintsVM>> GetPatientChiefComplaints(int id)
        {
            string redisKey = $"{CacheKey.PatientChiefComplaints}_{id}";

            var complaints = _cacheService.GetCacheValue<IEnumerable<ChiefComplaintsVM>>(redisKey);

            if (complaints is null)
            {
                var prescriptionChiefComplaints = await _pasContext.PrescriptionChiefComplaints
                                                                    .Include(pc=> pc.Symptom)
                                                                    .AsNoTracking()
                                                                    .Where(pa => pa.PatientId == id)
                                                                    .ToListAsync();
                complaints = prescriptionChiefComplaints.Select(pc => new ChiefComplaintsVM() { 
                    Id = pc.Id,
                    Name = pc.Symptom.Description
                });

                _cacheService.SetCacheValue(redisKey, complaints);
            }

            return null;
        }

        public async Task<IEnumerable<DrugDetailsVM>> GetRecentMedication(int id)
        {
            string redisKey = $"{CacheKey.RecentMedications}_{id}";

            var medicationList = _cacheService.GetCacheValue<IEnumerable<DrugDetailsVM>> (redisKey);
            if (medicationList is null)
            {
                var patientMedications = await _pasContext.PrescriptionDrugs
                                                    .Include(pd=> pd.BrandDoseTemplate)
                                                    .ThenInclude(p=> p.StrengthType)
                                                    .Include(pd=> pd.DrugBrands)
                                                    .AsNoTracking()
                                                    .Where(pd=> pd.Prescription.PatientId == id)
                                                    .OrderByDescending(pd=> pd.Id)
                                                    .Select(p=> new {
                                                        p.DrugBrandId, 
                                                        p.DrugBrands.BrandName, 
                                                        p.BrandDoseTemplate.StrengthType.Name 
                                                    })
                                                    .ToListAsync();

                //medicationList = MapToMedicationListVM(patientMedications);
                medicationList = patientMedications.Select(pm => new DrugDetailsVM()
                {
                    Name = pm.BrandName,
                    Dosage = pm.Name,
                    Id = pm.DrugBrandId
                });

                _cacheService.SetCacheValue(redisKey, medicationList);

            }

            return medicationList;
        }

        public async Task<bool> UpdatePersonalHistory(PatientPersonalHistoryVM vm)
        {
            if (vm.PatientId < 1) return false;

            ClinicalHistory ch = await _pasContext.ClinicalHistory
                                            .Include(ch=> ch.User)
                                            .FirstAsync(ch => ch.UserId == vm.PatientId);

            ch.User.Age = DateTime.Today.Year - vm.DateOfBirth.Year;
            ch.User.DateOfBirth = vm.DateOfBirth;
            ch.BloodGroupId = (int)vm.BloodGroup;
            ch.Smoker = vm.SmokePerDay;
            ch.Drinker = (short)vm.Drinker;
            ch.Excercise = vm.Excercise;
            ch.Sports = (short)vm.Sports;

            ch.PersonalHistoryLastUpdated = DateTime.Now;

            _pasContext.ClinicalHistory.Update(ch);
            await _pasContext.SaveChangesAsync();

            //## Now Update the Cache- if the page is reloaded- show the latest info
            string redisKey = $"{CacheKey.ClinicalDetails}_{vm.PatientId}";

            var cachedResult = _cacheService.GetCacheValue<ClinicalHistoryVM>(redisKey);
            
            cachedResult.Age = ch.User.Age;
            cachedResult.DateOfBirth = vm.DateOfBirth;
            cachedResult.BloodGroupType = vm.BloodGroup;
            cachedResult.Smoker = vm.SmokePerDay;
            cachedResult.Drinker = vm.Drinker.ToString();
            cachedResult.Excercise = vm.Excercise;
            cachedResult.Sports = vm.Sports;

            ch.PersonalHistoryLastUpdated = DateTime.Now;
            _cacheService.SetCacheValue(redisKey, cachedResult);

            //TODO: Log this Event- Patient updated Personal History

            return true;
        }

        /// <summary>This will Map list of Medications to MedicationListVM- 
        /// which will be used in Patient's Profile- as a Doctor/Patient or while creating a Prescription</summary>
        /// <param name="patientMedications">List of Prescription Drugs</param>
        /// <returns></returns>
        //private IEnumerable<DrugDetailsVM> MapToMedicationListVM(IEnumerable<PrescriptionDrugs> patientMedications)
        //{
        //    var mappedResult = patientMedications.Select(pm=> new DrugDetailsVM()
        //    { 
        //        Name = pm.DrugBrands.BrandName,
        //        Dosage = pm.BrandDoseTemplate.StrengthType.Name,
        //        Id = pm.DrugBrandId
        //    });

        //    return mappedResult;
        //}
    }
}
