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
    }
}