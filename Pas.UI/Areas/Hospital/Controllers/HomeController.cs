using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Pas.UI.Areas.Hospital.Controllers
{
    [Area("Hospital")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}