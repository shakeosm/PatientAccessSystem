using System.Linq;
using System.Security.Claims;

namespace Pas.UI.Infrastructure.ApplicationUserClaims
{
    public static class ApplicationUserClaimsPrincipalExtensions
    {
        public static string GetFullNameOrEmail(this ClaimsPrincipal principal)
        {
            var fullName = principal.Claims.FirstOrDefault(c => c.Type == "FullName");
            return fullName?.Value ?? principal.Identity?.Name;
        }

        public static string GetFullName(this ClaimsPrincipal principal)
        {
            var fullName = principal.Claims.FirstOrDefault(c => c.Type == "FullName");
            return fullName?.Value;
        }

        /// <summary>Doctor, Director or Technician in a Hospital</summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentRole(this ClaimsPrincipal principal)
        {
            var userRole = principal.Claims.FirstOrDefault(c => c.Type == "Role");
            return userRole?.Value;
        }

        /// <summary>Current Chamber of the Doctor</summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string GetCurrentOrganisation(this ClaimsPrincipal principal)
        {
            var userOrganisation = principal.Claims.FirstOrDefault(c => c.Type == "Organisation");
            return userOrganisation?.Value;
        }


        // You can add other extension methods here to access user properties exposed
        // via the ApplicationUserClaimsPrincipalFactory class
    }
}
