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
    public class AppUserService : IAppUserService
    {
        private readonly PasContext _context;

        public AppUserService( PasContext Context)
        {
            _context = Context;
        }

        public async Task<AppUserDetailsVM> Find(int id)
        {
            var appUser = await _context.User.FindAsync(id);

            AppUserDetailsVM result = MapToViewModel(appUser);
            return result;
        }

        public AppUserDetailsVM FindByEmail(string email)
        {
            var appUser = _context.User
                            .Include(p=> p.UserOrganisationRoles)
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

        private AppUserDetailsVM MapToViewModel(User appUser)
        {
            AppUserDetailsVM mappedVM = new AppUserDetailsVM()
            {
                Address = "",
                Age = appUser.Age,
                BanglaName = appUser.BanglaName ?? "",
                DateOfBirth = appUser.DateOfBirth?.ToString("dd/MM/yyyy"),
                Email = appUser.Email,
                Gender = (Gender)appUser.Gender,
                HasMultipleRoles = appUser.UserOrganisationRoles.Any(),
                Id = appUser.Id,
                Mobile = appUser.Mobile,
                Name = $"{appUser.FirstName} {appUser.LastName}",
                ShortId = appUser.ShortId ?? "",                
            };

            return mappedVM;
        }

        private IList<AppUserDetailsVM> MapToViewModel(IList<User> appUserList)
        {
            var mappedVM = appUserList.Select(u=> new AppUserDetailsVM()
            {
                Address = "",
                Age = u.Age,
                BanglaName = u.BanglaName,
                DateOfBirth = u.DateOfBirth.Value.ToString("dd/MM/yyyy"),
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
    }
}
