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
using System.Threading.Tasks;

namespace Pas.Service
{
    public class AppUserService : IAppUserService
    {
        private readonly PasContext _context;
        private readonly ICacheService _cacheService;
        private readonly IUniquePatientCodeGenerator _uniquePatientCodeGenerator;

        public AppUserService( PasContext Context,
                                ICacheService CacheService,
                                IUniquePatientCodeGenerator UniquePatientCodeGenerator)
        {
            _context = Context;
            _cacheService = CacheService;
            _uniquePatientCodeGenerator = UniquePatientCodeGenerator;
        }

        public async Task<AppUserDetailsVM> Find(int id, bool includeAddressBook = false, bool trackingEnabled = false)
        {
            
            var cacheKey = $"Find_AppUserDetailsVM_{id}";    //## Current Doctor-looking-for-Patient-and-Name for that Key

            var cachedResult = _cacheService.GetCacheValue<AppUserDetailsVM>(cacheKey); //## First always check in the Cache- have we read it previosly?
            if (cachedResult?.AddressBook is null && includeAddressBook)
            {
                cachedResult = null;
            }

            if (cachedResult is null) {

                //## Search in the Database

                User appUser = new User();
                if (includeAddressBook)
                {
                    appUser = await _context.User
                                            .AsNoTracking()
                                            .Include(u=> u.AddressBooks)
                                            .FirstAsync(u=> u.Id == id);    
                }
                else {
                    appUser = await _context.User.FindAsync(id);    
                
                }

                cachedResult = MapToViewModel(appUser);                 //## Map to ViewPage readable format
                _cacheService.SetCacheValue(cacheKey, cachedResult);    //## Now Cache it for Later use
            }

            return cachedResult;
        }

        public AppUserDetailsVM FindByEmail(string email)
        {
            var appUser = _context.User
                            .Include(p=> p.UserOrganisationRoles)
                            //.ThenInclude(p=> p.Organisation)
                            .Include(p=> p.AddressBooks)
                            .AsNoTracking()
                            .First(p=> p.Email== email);

            AppUserDetailsVM result = MapToViewModel(appUser);
            return result;
        }

        public async Task<IList<AppUserDetailsVM>> FindByFirstName(string firstName)
        {
            var userList = await _context.User.Where(p => p.FirstName.Contains(firstName)).ToListAsync();

            var result = MapToViewModel(userList);
            return result;
        }

        public async Task<IList<AppUserDetailsVM>> FindByLastName(string lastName)
        {
            var userList = await _context.User.Where(p => p.LastName.Contains(lastName)).ToListAsync();

            var result = MapToViewModel(userList);
            return result;
        }

        public async Task<AppUserDetailsVM> FindByMobile(string mobile)
        {
            var appUser = await _context.User.FirstOrDefaultAsync(p => p.Mobile == mobile);
            AppUserDetailsVM result = MapToViewModel(appUser);
            return result;
        }

        public AppUserDetailsVM FindByShortId(string shortId)
        {
            var appUser = _context.User.First(p => p.ShortId.Equals(shortId));

            AppUserDetailsVM result = MapToViewModel(appUser);
            return result;
        }

        public async Task<bool> LockUser(User appUser)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RegisterA_User(User appUser)
        {
            //## Blank ClinicalHistory Record
            appUser.ClinicalHistories = new List<ClinicalHistory>() 
            {
                new ClinicalHistory()
                {
                    UserId = 0,
                    //ClinicalInfoLastUpdated = new DateTime(2000, 1, 1),
                    //PersonalHistoryLastUpdated = new DateTime(2000, 1, 1),
                }
            };

            //## Generate a Unique PAS Code for this Patient
            string pasCode = _uniquePatientCodeGenerator.Get(appUser.Mobile, 10);

            //## Finally save the User record.            
            appUser.ShortId = pasCode;
            await _context.User.AddAsync(appUser);
            await _context.SaveChangesAsync();

            return appUser.Id > 0;
        }



        public async Task<bool> UnlockUser(User appUser)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateUser(User appUser)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This will Mapp all basic User Details- except any Role Details.
        /// Wait fo the User to go the page to select a role
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private AppUserDetailsVM MapToViewModel(User appUser)
        {
            AppUserDetailsVM mappedVM = new AppUserDetailsVM()
            {
                Id = appUser.Id,
                Title = ((Title)appUser.Title).ToString(),
                Name = $"{appUser.FirstName} {appUser.LastName}",
                BanglaName = appUser.BanglaName ?? "",

                Age = appUser.Age,
                DateOfBirth = appUser.DateOfBirth?.ToShortDateString(),                
                Gender = (Gender)appUser.Gender,                
                
                Mobile = appUser.Mobile,
                Email = appUser.Email,

                ShortId = appUser.ShortId ?? "",  
                AddressBook = MapToAddressBookVM(appUser.AddressBooks),

                HasMultipleRoles = appUser.UserOrganisationRoles.Any()
            };

            //## Keep the Mobile and Email in the Address book
            if (appUser.AddressBooks.Count >= 1) {
                mappedVM.AddressBook.Mobile = appUser.Mobile;
                mappedVM.AddressBook.Email = appUser.Email;
            }

            return mappedVM;
        }

        private AddressBookVM MapToAddressBookVM(ICollection<AddressBook> addressBooks)
        {
            if (addressBooks is null)
            {
                return new AddressBookVM();
            }
            else {
                var firstAddress = addressBooks.Select(ab=> new AddressBookVM()
                {  
                    AddressLine1 = ab.AddressLine1,
                    City = ((Common.Enums.City) ab.CityId).ToString(),
                    LocalArea  =ab.LocalArea,
                    Id = ab.Id
                });

                return firstAddress.FirstOrDefault();
            }
        }


        private IList<AppUserDetailsVM> MapToViewModel(IList<User> appUserList)
        {
            var mappedVM = appUserList.Select(u=> new AppUserDetailsVM()
            {
                //Address = (u.AddressBooks.Count() >= 1 ? u.AddressBooks.FirstOrDefault().AddressLine1 : ""),
                //AddressAreaLocality = (u.AddressBooks.Count() >= 1 ? u.AddressBooks.FirstOrDefault().LocalArea : ""),
                AddressBook = MapToAddressBookVM(u.AddressBooks),
                Age = u.Age,
                BanglaName = u.BanglaName,
                DateOfBirth = u.DateOfBirth.Value.ToDDMMMYYYY(),
                Email = u.Email,
                Gender = (Gender)u.Gender,
                HasMultipleRoles = u.UserOrganisationRoles.Any(),
                Id = u.Id,
                Mobile = u.Mobile,
                Name = $"{u.FirstName} {u.LastName}",
                ShortId = u.ShortId
            }).ToList();



            return mappedVM;
        }

        public async Task<IEnumerable<UserRoleVM>> GetRolesByUser(int id)
        {
            var userRoles = await _context.UserOrganisationRole
                                .Include(uor => uor.Organisation)
                                .Include(uor => uor.Role)
                                .Where(uor => uor.UserId == id).ToListAsync();

            var userSwitchRoleViewVM = userRoles.Select(ur => new UserRoleVM
            {
                OrganisationId = ur.OrganisationId,
                OrganisationName = ur.Organisation.Name,
                RoleId = ur.RoleId,
                //RoleName - ur.Role.Name,
                UserOrganisationRoleId = ur.Id
            });

            return userSwitchRoleViewVM;
        }

        public async Task<IList<SpecialityVM>> Get_DoctorSpeciality(int doctorId)
        {
            var redisKey = $"{CacheKey.DoctorSpeciality}_{doctorId}";
            var specialityList = _cacheService.GetCacheValue<IList<SpecialityVM>>(redisKey);

            if (specialityList is null || specialityList.Count < 1) { 
                specialityList = await _context.DoctorSpeciality
                                                        .Include(d => d.Speciality)
                                                        .AsNoTracking()
                                                        .Where(d => d.DoctorId == doctorId)
                                                        .Select(d => new SpecialityVM()
                                                        {
                                                            Name = d.Speciality.Name,
                                                            BanglaName = d.Speciality.BanglaName
                                                        })
                                                        .ToListAsync();

                _cacheService.SetCacheValue(redisKey, specialityList);
            }


            return specialityList;
        }


        public async Task<IList<DoctorDegreesVM>> Get_DoctorDegrees(int doctorId)
        {
            var redisKey = $"{CacheKey.DoctorDegrees}_{doctorId}";
            var specialityList = _cacheService.GetCacheValue<IList<DoctorDegreesVM>>(redisKey);

            if (specialityList is null || specialityList.Count < 1) { 
                specialityList = await _context.DoctorMedicalDegrees
                                                        .Include(d => d.MedicalDegree)
                                                        .AsNoTracking()
                                                        .Where(d => d.DoctorId == doctorId)
                                                        .Select(d => new DoctorDegreesVM()
                                                        {
                                                            Name = d.MedicalDegree.Name,
                                                            BanglaName = d.MedicalDegree.BanglaName
                                                        })
                                                        .ToListAsync();

                _cacheService.SetCacheValue(redisKey, specialityList);
            }

            return specialityList;
        }

        public async Task<HospitalDetailsVM> Get_DoctorChamber(string email)
        {
            AppUserDetailsVM currentUser = _cacheService.GetCacheValue<AppUserDetailsVM>(email);

            string redisKey = $"{CacheKey.DoctorChamber}_{currentUser.OrganisationId}";

            var chamber = _cacheService.GetCacheValue<HospitalDetailsVM>(redisKey);
            if (chamber is null)
            {
                var organisation = await _context.Organisation
                                                    .FindAsync(currentUser.CurrentRole.OrganisationId);

                chamber = new HospitalDetailsVM()
                {
                    Id = organisation.Id,
                    Name = organisation.Name,
                    HeaderBangla = organisation.HeaderBangla,
                    HeaderEnglish = organisation.HeaderEnglish,
                    Address = organisation.Address,
                    LogoImageUrl = organisation.LogoImageFile
                };

                //## Now save it in the Redis Cache- for later use
                _cacheService.SetCacheValue(redisKey, chamber);
            }

            return chamber;

        }


    }
}
