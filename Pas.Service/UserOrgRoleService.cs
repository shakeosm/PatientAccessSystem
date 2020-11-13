using Microsoft.EntityFrameworkCore;
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
    public class UserOrgRoleService : IUserOrgRoleService
    {
        private readonly PasContext _dataContext;

        public UserOrgRoleService(PasContext DataContext)
        {
            _dataContext = DataContext;
        }

        public async Task<UserOrganisationRole> Find(int Id)
        {
            var result = await _dataContext.UserOrganisationRole
                                                .AsNoTracking()
                                                .Include(u => u.Organisation)
                                                .Include(u=> u.Role)
                                                .FirstAsync(u => u.Id == Id);


            return result;
        }

        public async Task<IEnumerable<UserOrganisationRole>> FindRolesByUserId(int appUserId)
        {
            var result = await _dataContext.UserOrganisationRole
                                    //.Include(uor => uor.User)
                                    .AsNoTracking()
                                    //.Include(uor => uor.Organisation)
                                    .Include(uor => uor.Role)
                                    .Where(uor => uor.UserId == appUserId && uor.IsDeleted != true)
                                    .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<UserRoleVM>> FindMappedRolesByUserId(int appUserId)
        {
            var result = await _dataContext.UserOrganisationRole
                                    //.Include(uor => uor.User)
                                    .AsNoTracking()
                                    .Include(uor => uor.Organisation)
                                    .Include(uor => uor.Role)
                                    .Where(uor => uor.UserId == appUserId && uor.IsDeleted != true)
                                    .ToListAsync();


            var mappedVM = result.Select(u => new UserRoleVM() {
                OrganisationId = u.OrganisationId,
                OrganisationName = u.Organisation.Name,
                RoleId = u.RoleId,
                RoleName = u.Role.Name,
                UserOrganisationRoleId = u.Id,
                DateAdded = u.CreatedOn
            });
            return mappedVM;
        }
        

        public async Task<IEnumerable<UserOrganisationRole>> FindRolesInOrganisationByUserId(int orgId, int appUserId)
        {
            var result = await _dataContext.UserOrganisationRole
                                    .AsNoTracking()
                                    .Include(uor => uor.User)
                                    .Include(uor => uor.Organisation)
                                    .Where(uor => uor.UserId == appUserId && uor.OrganisationId == orgId)
                                    .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<UserOrganisationRole>> FindUsersByOrganisationId(int orgId)
        {
            var result = await _dataContext.UserOrganisationRole
                                    .AsNoTracking()
                                    .Include(uor => uor.User)
                                    .Where(uor => uor.OrganisationId == orgId)
                                    .ToListAsync();

            return result;
        }

        public async Task<bool> UserHasRoleInOrganisation(int userId, int organisationId, int appRoleId)
        {
            var roleFound = await _dataContext.UserOrganisationRole.AnyAsync(o=> o.UserId == userId
                                                                        && o.OrganisationId == organisationId
                                                                        && o.RoleId == appRoleId);


            return roleFound;
        }
    }
}
