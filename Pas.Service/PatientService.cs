using Microsoft.EntityFrameworkCore;
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
            string cacheKey = $"{id}_GetClinicalDetails";

            var cachedResult = _cacheService.GetCacheValue<ClinicalHistoryVM>(cacheKey);
            if (cachedResult is null)
            {
                var search = await _pasContext.ClinicalHistory
                        .AsNoTracking()
                        .Include(c => c.User)
                        .FirstOrDefaultAsync(c => c.UserId == id);

                cachedResult = await MapToClinicalHistoryVM(search);

                _cacheService.SetCacheValue(cacheKey, cachedResult);
            }
            
            return cachedResult;
        }


        private async Task<ClinicalHistoryVM> MapToClinicalHistoryVM(ClinicalHistory ch)
        {
            try
            {
                ClinicalHistoryVM clinicalHistoryVM = new ClinicalHistoryVM()
                {
                    UserId = ch.UserId,
                    Age = ch.User.Age,
                    BloodGroupType = ch.BloodGroupId is null ? BloodGroup.Unknown : (BloodGroup)ch.BloodGroupId,
                    Smoker = ch.Smoker?.ToString(),
                    Drinker = ch.Drinker?.ToString(),
                    Excercise = ch.Excercise?.ToString(),
                    Sports = ch.Sports,

                    BloodPressure = (ch.PressureSystolic is null ? "-" : $"{ch.PressureSystolic} / {ch.PressureDiastolic}"),
                    Pulse = ch.Pulse.ToString(),
                    Cholesterol = ch.Cholesterol?.ToString(),
                    Diabetes = ch.Diabetes?.ToString(),
                    Height = ch.Height,
                    Weight = ch.Weight?.ToString(),

                    AllergyInfo = ch.AllergyInfo,                    

                    ClinicalInfoLastUpdated = ch.ClinicalInfoLastUpdated,
                    PersonalHistoryLastUpdated = ch.PersonalHistoryLastUpdated
                };

                clinicalHistoryVM.AilmentList = await GetPatientAilments(ch.UserId);    // "Ailment section is not filled up yet",

                return clinicalHistoryVM;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new ClinicalHistoryVM();
            }

        }

        public async Task<IEnumerable<PatientAilmentType>> GetPatientAilments(int id)
        {
            string cacheKey = $"GetPatientAilments_{id}";

            var ailments = _cacheService.GetCacheValue<IEnumerable<PatientAilmentType>>(cacheKey);
            
            if (ailments is null) {
                ailments = await _pasContext.PatientAilmentTypes
                                            .AsNoTracking()
                                            .Where(pa => pa.PatientId == id)
                                            .ToListAsync();
                
                _cacheService.SetCacheValue(cacheKey, ailments);                
            }

            return ailments;
        }

        public async Task<IEnumerable<DrugDetailsVM>> GetRecentMedication(int id)
        {
            string cacheKey = $"GetRecentMedication{id}";

            var medicationList = _cacheService.GetCacheValue<IEnumerable<DrugDetailsVM>> (cacheKey);
            if (medicationList is null)
            {
                var patientMedications = await _pasContext.PrescriptionDrugs
                                                    .Include(pd=> pd.BrandDoseTemplate)
                                                    .ThenInclude(p=> p.StrengthType)
                                                    .AsNoTracking()
                                                    .Where(pd=> pd.Prescription.PatientId == id)
                                                    .ToListAsync();

                medicationList = MapToMedicationListVM(patientMedications);
                _cacheService.SetCacheValue(cacheKey, medicationList);

            }

            return medicationList;
        }

        /// <summary>This will Map list of Medications to MedicationListVM- 
        /// which will be used in Patient's Profile- as a Doctor/Patient or while creating a Prescription</summary>
        /// <param name="patientMedications">List of Prescription Drugs</param>
        /// <returns></returns>
        private IEnumerable<DrugDetailsVM> MapToMedicationListVM(IEnumerable<PrescriptionDrugs> patientMedications)
        {
            var mappedResult = patientMedications.Select(pm=> new DrugDetailsVM()
            { 
                Name = pm.DrugBrands.BrandName,
                Dosage = pm.BrandDoseTemplate.StrengthType.Name,
                Id = pm.DrugBrandId
            });

            return mappedResult;
        }
    }
}
