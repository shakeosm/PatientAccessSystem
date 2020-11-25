using Pas.Data.Models;
using Pas.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pas.Service.Interface
{
    public interface IAppUserService
    {
        Task<bool> RegisterA_User(User appUser);

        Task<bool> UpdateUser(User appUser);

        Task<bool> LockUser(User appUser);

        Task<bool> UnlockUser(User appUser);
        
        Task<AppUserDetailsVM> Find(int id, bool includeAddressBook = false, bool trackingEnabled = false);
        AppUserDetailsVM FindByEmail(string email);
        Task<AppUserDetailsVM> FindByMobile(string mobile);
        AppUserDetailsVM FindByShortId(string shortId);
        Task<IList<AppUserDetailsVM>> FindByFirstName(string firstName);
        Task<IList<AppUserDetailsVM>> FindByLastName(string lastName);

        Task<IEnumerable<UserRoleVM>> GetRolesByUser(int id);
        
        Task<IList<SpecialityVM>> Get_DoctorSpeciality(int doctorId);
        Task<IList<DoctorDegreesVM>> Get_DoctorDegrees(int doctorId);

        /// <summary>Will get the Current selected Hospital/Chamber Details</summary>
        /// <param name="email">Doctor Email- to get the Cache value</param>
        /// <returns></returns>
        Task<HospitalDetailsVM> Get_DoctorChamber(string email);
    }
}
