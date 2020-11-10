using Pas.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pas.Service.Interface
{
    public interface IUserOrgRoleService
    {
        Task<UserOrganisationRole> Find(int Id);

        Task<IEnumerable<UserOrganisationRole>> FindRolesByUserId(int appUserId);

        Task<IEnumerable<UserOrganisationRole>> FindRolesInOrganisationByUserId(int orgIdm, int appUserId);

        Task<IEnumerable<UserOrganisationRole>> FindUsersByOrganisationId(int orgId);

    }
}
