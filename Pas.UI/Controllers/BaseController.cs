using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Service.Interface;
using Pas.Web.ViewModels;

namespace Pas.UI.Controllers
{
    public abstract partial class BaseController : Controller
    {
        protected readonly IAppUserService _appUserService;
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly IAppAuthorisationService _appAuthorisationService;
        protected readonly IUserOrgRoleService _userOrgRoleService;

        public BaseController(UserManager<IdentityUser> UserManager,
                                IAppUserService AppUserService,
                                IAppAuthorisationService AppAuthorisationService,
                                IUserOrgRoleService UserOrgRoleService)
        {
            _appUserService = AppUserService;
            _userManager = UserManager;
            _appAuthorisationService = AppAuthorisationService;
            _userOrgRoleService = UserOrgRoleService;
        }

        protected async Task<AppUserDetailsVM> GetCurrentUser()
        {
            var userEmail = _userManager.GetUserName(HttpContext.User);

            var currentUser = await _appAuthorisationService.GetActiveUserFromCache(userEmail);

            return currentUser;
        }

        protected string GetLoggedInEmail()
        { 
            return _userManager.GetUserName(HttpContext.User);
        }

    }
}