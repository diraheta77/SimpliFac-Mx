using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simplifile.Models;

namespace simplifile.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        public IActionResult Nomina()
        {
            return View();
        }

        [Produces("application/json")]
        [Route("api/dashboard/nomina")]
        public IActionResult getDataNomina()
        {
            List<Dashboard.PieData> pieData = new List<Dashboard.PieData>();
            Dashboard.PieData pie = new Dashboard.PieData();

            pie.color = "#dd4b39";
            pie.highlight = "#dd4b39";
            pie.label = "Completamente validas";
            pie.value = 9;
            pieData.Add(pie);

            pie = new Dashboard.PieData();
            pie.color = "#00a65a";
            pie.highlight = "#00a65a";
            pie.label = "No existe informacion";
            pie.value = 2;
            pieData.Add(pie);

            return Ok(pieData);
        }


        public IActionResult Index()
        {
            return View();
        }

    }
}
