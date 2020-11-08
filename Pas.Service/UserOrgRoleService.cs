using Microsoft.EntityFrameworkCore;
using Pas.Data;
using Pas.Data.Models;
using Pas.Service.Interface;
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
        public async Task<IEnumerable<UserOrganisationRole>> FindRolesByUserId(int appUserId)
        {
            var result = await _dataContext.UserOrganisationRole
                                    .Include(uor => uor.User)
                                    .Include(uor => uor.Organisation)
                                    .Where(uor => uor.UserId == appUserId && uor.IsDeleted != true)
                                    .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<UserOrganisationRole>> FindRolesInOrganisationByUserId(int orgId, int appUserId)
        {
            var result = await _dataContext.UserOrganisationRole
                                    .Include(uor => uor.User)
                                    .Include(uor => uor.Organisation)
                                    .Where(uor => uor.UserId == appUserId && uor.OrganisationId == orgId)
                                    .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<UserOrganisationRole>> FindUsersByOrganisationId(int orgId)
        {
            var result = await _dataContext.UserOrganisationRole
                                    .Include(uor => uor.User)
                                    .Where(uor => uor.OrganisationId == orgId)
                                    .ToListAsync();

            return result;
        }
    }
}
