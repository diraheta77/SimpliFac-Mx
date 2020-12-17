using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PagedList;
using simplifile.Data;
using simplifile.Helpers;
using simplifile.Models;
using simplifile.Services;
using simplifile.ViewModels.MasterParm;
using simplifile.ViewModels.Parm;
using simplifile.ViewModels.RequisicionSat;

namespace simplifile.Controllers
{
    [Authorize]
    public class RequisicionesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUpload _upload;
        private readonly IMemoryCache _memoryCache;

        public RequisicionesController(ApplicationDbContext context, IUpload upload, IMemoryCache memoryCache)
        {
            _context = context;
            _upload = upload;
            _memoryCache = memoryCache;
        }

        public ActionResult Dashboards()
        {
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return View();
        }

        

        public ActionResult Conciliacion()
        {
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return View();
        }

        public ActionResult Index()
        {
            var empresas = _context.Empresas.ToList();
            var unidades = _context.UnidadadesNegocios.ToList();
            //var roles = User.FindFirst("RoleDes").Value;
            var empresasFilter = empresas.Filter(User).ToList();
            var unidadesFilter = unidades.Filter(User).ToList();
            ViewBag.Empresas = new SelectList(empresasFilter, "EmpId", "EmpRazonSocial");
            ViewBag.Unidades = new SelectList(unidadesFilter, "UnId", "UnRazonSocial");
            //ViewBag.Empresas = new SelectList(_context.Empresas.ToList(), "EmpId", "EmpRazonSocial");
            //ViewBag.Unidades = new SelectList(_context.UnidadadesNegocios.ToList(), "UnId", "UnRazonSocial");
            return View();
        }
        
        [Produces("application/json")]
        [Route("api/requisiciones")]
        public IActionResult Search(string fempid  = "", string funid = "",string festatus = "", string ffechai = "",string ffechaf = "",string forigen = "")
        {
            //Search
            //var search = _context.Requisiciones.Include(f=>f.Empresa).Include(f=>f.UnidadNegocio).Filter(User).ToList();
            var search = _context.Requisiciones.FromSqlRaw("select r.* from RequisicionesSAT r join UnidadNegocio u on r.UnId = u.UnId " +
                "join Empresas e on e.EmpId = u.EmpId and e.EmpId = r.EmpId").Filter(User).ToList();
            if (!string.IsNullOrEmpty(fempid))
            {
                search = search.Where(f => f.EmpId.ToLower().Contains(fempid.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(funid))
            {
                search = search.Where(f => f.UnId.ToLower().Contains(funid.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(festatus))
            {
                search = search.Where(f => f.RsEstatusRequisicion == festatus || (festatus == "0" && f.RsEstatusRequisicion == null )).ToList();
            }

            if (!string.IsNullOrEmpty(ffechai))
            {
                search = search.Where(f => f.RsFechaInfoInicio >= DateTime.ParseExact(ffechai, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
            }
            if (!string.IsNullOrEmpty(ffechaf))
            {
                search = search.Where(f => f.RsFechaInfoFinal <= DateTime.ParseExact(ffechaf, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
            }
            if (!string.IsNullOrEmpty(forigen))
            {
                search = search.Where(f => f.RsOrigenCodigo == forigen).ToList();
            }
            //if (!string.IsNullOrEmpty(ffechac))
            //{
            //    search = search.Where(f => f.RsFechaRequisicion == DateTime.ParseExact(ffechac, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
            //}
            //End search
            search = search.OrderByDescending(f => f.RsFechaRequisicion).ToList();
            return Ok(search);
            //List<RequisicionSat> Items = search.ToPagedList(page,length).ToList();
            //int Count = Items.Count();
            //return Ok(new {
            //    recordsTotal = _context.Requisiciones.Count(),
            //    recordsFiltered = search.Count(),
            //    data = Items
            //});
        }

        [Produces("application/json")]
        [Route("api/solicitudes/unidades")]
        public IActionResult Units(string empresa="")
        {
            return Ok(new
            {
                unidades = _context.UnidadadesNegocios.Where(f => f.EmpId == empresa && f.UnEstatus == 1).Filter(User).ToList()
            });
        }

        public IActionResult Insert()
        {
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            ViewBag.Unidades = new SelectList(_context.UnidadadesNegocios.Filter(User), "UnId", "UnRazonSocial");
            var model = new RequisicionSatInsertarViewModel { };
            return PartialView("_InsertPartial", model);
        }

        [HttpPost]
        public IActionResult Insert(RequisicionSatInsertarViewModel model)
        {
            if (ModelState.IsValid && this.valid(model))
            {
                List<string> tipos = new List<string>();
                string RsTipoSolicitante = model.FiguraUnidadNegocio;
                if (model.Ingreso)
                {
                    tipos.Add("I");
                }
                if (model.Egreso)
                {
                    tipos.Add("E");
                }
                if (model.ComplementoPago)
                {
                    tipos.Add("P");
                }
                if (model.ReciboNomina)
                {
                    tipos.Add("N");
                }
                string[] valoresTipos = tipos.ToArray();
                var valor = string.Join(",", tipos);
                var unidad = _context.UnidadadesNegocios.FirstOrDefault(f => f.EmpId == model.EmpId && f.UnId == model.UnId);
                _context.Requisiciones.Add(new RequisicionSat()
                {
                    RsOrigenCodigo = "M",
                    EmpId = model.EmpId,
                    UnId = model.UnId,
                    RsRFCRequisicion = (unidad != null) ? unidad.UnRFC : "",
                    RsIdRequisicionSAT = "",
                    RsFechaInfoInicio = model.RsFechaInfoInicioDate,
                    RsEstatusDescarga = "H",
                    RsFechaInfoFinal = model.RsFechaInfoFinalDate,
                    RsFechaRequisicion = DateTime.Now,
                    RsEstatusRequisicion = "0",
                    RsFechaModificacion = DateTime.Now,
                    RsTipoRequisicion = "CFDI",
                    //nuevs campos
                    RsTipoComprobante = valor,
                    RsTipoSolicitante = RsTipoSolicitante
                });
                _context.SaveChanges();
                
                this.ShowSuccess("La solicitud se ha registrao correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            ViewBag.Unidades = new SelectList(_context.UnidadadesNegocios.Where(f=>f.EmpId == model.EmpId).Filter(User).ToList(), "UnId", "UnRazonSocial");
            return PartialView("_InsertPartial", model);
        }

        public FileResult Descargar(string id)
        {
            var req = _context.Requisiciones.FirstOrDefault(f => f.RsId.ToString() == id);
            req.RsEstatusDescarga = "U";
            if(req == null)
            {
                this.ShowError("No se puede realzar la descarga");
                return null;
            }
            
            byte[] fileBytes = this._upload.GetFileZip(req.EmpId,req.RsRFCRequisicion,req.RsIdRequisicionSAT);
            string fileName = $"{req.RsIdRequisicionSAT}.zip";

            _context.SaveChanges();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        [HttpPost]
        public IActionResult SetCacheReorderColumns(int[] order, eTablesType etype)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromDays(30));

            ConfigUserTable configUserTable = new ConfigUserTable();
            configUserTable.User = User.Identity.Name;
            configUserTable.OrderColumTable = order;
            configUserTable.TablesType = etype;
            string optionTableName = Enum.GetName(typeof(eTablesType), etype);
            optionTableName = User.Identity.Name + "--" + optionTableName;
            _memoryCache.Set(optionTableName, configUserTable, cacheEntryOptions);
            return Ok(new { success = true, response = true });
        }

        public IActionResult GetCacheReorderColumns()
        {
            ConfigUserTable configUserTable = new ConfigUserTable();
            string optionTableName = Enum.GetName(typeof(eTablesType), 0);
            optionTableName = User.Identity.Name + "--" + optionTableName;
            var x = _memoryCache.Get(optionTableName);
            if (_memoryCache.TryGetValue(optionTableName, out configUserTable))
            {
                return Ok(new { success = true, response = configUserTable });
            }
            return Ok(new { success = false, response = "" });
        }


        #region "Validaciones"
        private bool valid(RequisicionSatInsertarViewModel model)
        {
            var result = true;

            if (model.RsFechaInfoFinalDate > DateTime.Today)
            {
                ModelState.AddModelError("RsFechaInfoFinal", $"La fecha final no debe ser mayor a la fecha de hoy.");
                result = false;
            }

            if (model.RsFechaInfoFinalDate < model.RsFechaInfoInicioDate)
            {
                ModelState.AddModelError("RsFechaInfoFinal", $"La fecha final debe ser menor a la fecha inicial.");
                result = false;
            }

            return result;
        }

        #endregion
    }
}