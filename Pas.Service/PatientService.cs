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
        private PasContext _pasContext { get; }
        public PatientService(PasContext PasContext)
        {
            _pasContext = PasContext;
        }

        

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<AppUserDetailsVM> Find(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUserDetailsVM> FindByEmail(string email)
        {
            User user = await _pasContext.User.FirstOrDefaultAsync(u => u.Email == email);
            
            return MapToPatientDetails(user);
            
        }

        AppUserDetailsVM MapToPatientDetails(User user)
        {
            return new AppUserDetailsVM()
            {
                Address = "",
                Age = user.Age,
                BanglaName = user.BanglaName,
                DateOfBirth  = user.DateOfBirth.Value.ToDD_MM_YYYY(),
                Gender = (Gender) user.Gender,
                Id = user.Id,
                Mobile = user.Mobile,
                Email = user.Email,
                Name = $"{user.FirstName} {user.LastName}",
                ShortId = user.ShortId                
            };
        }

        public Task<AppUserDetailsVM> FindByMobile(string mobileNumber)
        {
            //var result = _pasContext.User.Where(u => u.FirstName.StartsWith("Car"))
            //                .Select(p=> new AppUserDetailsVM() { 
            //                });
            throw new NotImplementedException();
        }

        public Task<AppUserDetailsVM> FindByShortId(string ShortId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(AppUserDetailsVM AppUserDetailsVM)
        {
            throw new NotImplementedException();
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

            var result = await Search(userSearch);

            return result;

            }

        public async Task<IEnumerable<AppUserDetailsVM>> GetRegularPatientList(int doctorId)
        {            
            Expression<Func<User, bool>> userSearch = u => (u.FirstName.Contains("Ma"));

            var result = await Search(userSearch);

            return result;
        }

        /// <summary>Search Patients as per your Search Expression</summary>
        /// <param name="userSearch">Search expression</param>
        /// <returns>Get PatientList in AppUserDetailsVM</returns>
        private async Task<IEnumerable<AppUserDetailsVM>> Search(Expression<Func<User, bool>> userSearch)
        {
            try
            {
                List<User> result = await _pasContext.User.Where(userSearch).ToListAsync();

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
                    Address = ""
                });

                return resultVM;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<UserRole>> GetRolesByUser(int id)
        {
            var userRoles = await _pasContext.UserOrganisationRole
                                .Include(uor => uor.Organisation)
                                .Include(uor => uor.Role)
                                .Where(uor => uor.UserId == id).ToListAsync();

            var userSwitchRoleViewVM = userRoles.Select(ur=> new UserRole { 
                OrganisationId = ur.OrganisationId,
                OrganisationName = ur.Organisation.Name,
                RoleId = ur.RoleId,
                //RoleName - ur.Role.Name,
                UserOrganisationRoleId = ur.Id
            });

            return userSwitchRoleViewVM;
        }
    }
}
