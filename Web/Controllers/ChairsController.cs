using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Vue.Controllers
{
    public class ChairsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}