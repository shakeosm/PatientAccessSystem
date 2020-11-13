using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pas.Service.Interface;
using Pas.UI.Controllers;

namespace Pas.UI.Areas.Director.Controllers
{
    [Area("Director")]
    [Authorize]
    public class HomeController : BaseController
    {

        public HomeController(
                                UserManager<IdentityUser> UserManager,
                                IAppUserService AppUserService,
                                IAppAuthorisationService AppAuthorisationService,
                                IUserOrgRoleService UserOrgRoleService
            ) : base(UserManager, AppUserService, AppAuthorisationService, UserOrgRoleService)
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}