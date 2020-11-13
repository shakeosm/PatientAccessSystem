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
        private readonly IAppUserService _appUserService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppAuthorisationService _appAuthorisationService;

        public BaseController(IAppUserService AppUserService,
                                UserManager<IdentityUser> UserManager,
                                IAppAuthorisationService AppAuthorisationService)
        {
            _appUserService = AppUserService;
            _userManager = UserManager;
            _appAuthorisationService = AppAuthorisationService;
        }

        private async Task<AppUserDetailsVM> GetCurrentUser()
        {
            var userEmail = _userManager.GetUserName(HttpContext.User);

            var currentUser = await _appAuthorisationService.GetActiveUserFromCache(userEmail);

            return currentUser;
        }

    }
}