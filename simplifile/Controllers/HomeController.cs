using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using simplifile.Models;

namespace simplifile.Controllers
{
    
    public class HomeController : Controller
    {
       
        public HomeController()
        {
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Mantenimiento()
        {
            return View();
        }

        public IActionResult Navegador()
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
