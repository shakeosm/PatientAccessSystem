using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pas.Service.Interface;

namespace Pas.UI.Controllers
{
    [Authorize]
    public class ConfigureController : Controller
    {
        private readonly IDrugService _drugService;

        public ConfigureController(IDrugService DrugService)
        {
            _drugService = DrugService;
        }
        public IActionResult Index()
        {
            //## Show list of All Drugs            
            return View();
        }

        /// <summary>Show the Details fo a Specific Drug item</summary>
        /// <param name="id">Drug Id</param>
        /// <returns></returns>
        public IActionResult Drug(int id)
        {
            //## Show list of All Drugs            
            return View();
        }


    }
}