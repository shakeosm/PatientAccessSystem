using Pas.Data.Models;
using Pas.Web.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pas.Service.Interface
{
    public interface IUserOrgRoleService
    {
        Task<UserOrganisationRole> Find(int Id);

        Task<IEnumerable<UserOrganisationRole>> FindRolesByUserId(int appUserId);
        Task<IEnumerable<UserRoleVM>> FindMappedRolesByUserId(int appUserId);

        Task<IEnumerable<UserOrganisationRole>> FindRolesInOrganisationByUserId(int orgIdm, int appUserId);

        Task<IEnumerable<UserOrganisationRole>> FindUsersByOrganisationId(int orgId);

        Task<bool> UserHasRoleInOrganisation(int userId, int organisationId, int appRoleId);
    }
}
