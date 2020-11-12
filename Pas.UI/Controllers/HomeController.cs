using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pas.Service.Interface;
using Pas.UI.Models;
using Pas.Web.ViewModels;

namespace Pas.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IAppUserService _appUserService;
        private readonly IUserOrgRoleService _userOrgRoleService;

        public HomeController(ILogger<HomeController> logger,
                                UserManager<IdentityUser> userManager,
                                IAppUserService AppUserService,
                                IUserOrgRoleService UserOrgRoleService)
        {
            _logger = logger;
            _userManager = userManager;
            _appUserService = AppUserService;
            _userOrgRoleService = UserOrgRoleService;
        }

        /// <summary>
        /// This is the Main Point of Entry- in the PAS- WebApp
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var userEmail = _userManager.GetUserName(HttpContext.User);            

            //var u1 = HttpContext.User.Claims.ToList();
            //var userClaim = HttpContext.User.FindFirst(ClaimTypes.Email).Value;

            AppUserDetailsVM currentUser = new AppUserDetailsVM();

            if (userEmail != null) {
                currentUser = _appUserService.FindByEmail(userEmail);
            
            }
            
            //var claimList = ClaimsPrincipal.Current?.Identities.First().Claims.ToList();
            
            return View(currentUser);
            

            //## The User is found in the DB.. How many roles the User have. Is the User simply a Patient or a Doctor or Hospital-Director
            var roles = await _userOrgRoleService.FindRolesByUserId(currentUser.Id);
            if (roles is null)
            {
                //## This is a PatientOnly user.. redirect to Patient Landing/Dashboard page
            }
            else if (roles.Count() == 1)
            {
                //## Only One role- either a Doctor or a Technician or Simply a Director(Non-Doctor)- redirect to ther respective page
                string areaName = GetAreaName(roles.FirstOrDefault().RoleId);

                return RedirectToAction("Index", "Home", new { Area = areaName });
            }
            else {
                //## More than One roles-> Doctor+Director.
                return RedirectToAction("SwitchRole", "AppUser");
            }

            var currentClaims = ClaimsPrincipal.Current?.Identities.First().Claims.ToList();

            return View();
        }

        private string GetAreaName(int roleId)
        {
            throw new NotImplementedException();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SelectRole()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
