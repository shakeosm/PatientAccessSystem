using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pas.UI.Controllers
{
    public class ConfigureController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}