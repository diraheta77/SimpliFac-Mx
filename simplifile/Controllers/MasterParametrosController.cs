using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;
using simplifile.Data;
using simplifile.Helpers;
using simplifile.Models;
using simplifile.Services;
using simplifile.ViewModels.MasterParm;
using simplifile.ViewModels.Parm;

namespace simplifile.Controllers
{
    public class MasterParametrosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurity _security;
        private readonly IParamaters _parameters;

        public MasterParametrosController(ApplicationDbContext context, ISecurity security, IParamaters parameters)
        {
            _context = context;
            _security = security;
            _parameters = parameters;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Produces("application/json")]
        [Route("api/master-parametros")]
        public IActionResult Search(int page = 0 ,int length = 10, string fparmid  = "", string fparmdesc = "",string fparmval = "")
        {
            //Search
            var search = _context.MasterParms.Filter(User).ToList();
            if (!string.IsNullOrEmpty(fparmid))
            {
                search = search.Where(f => f.MParmId.ToLower().Contains(fparmid.ToLower())).ToList();
            }
           
            if (!string.IsNullOrEmpty(fparmdesc))
            {
                search = search.Where(f => f.MParmDesc.ToLower().Contains(fparmdesc.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(fparmval))
            {
                search = search.Where(f => f.MParmValTxt.ToLower().Contains(fparmval.ToLower()) || f.MParmValNum.ToString().ToLower().Contains(fparmval.ToLower())).ToList();
            }
            //End search
            List<MasterParm> Items = search.ToPagedList(page,length).ToList();
            int Count = Items.Count();
            return Ok(new {
                recordsTotal = _context.MasterParms.Count(),
                recordsFiltered = search.Count(),
                data = Items
            });
        }

        public IActionResult Insert()
        {
            var model = new MasterParmInsertarViewModel 
            {
                Tipo = 0
            };
            return PartialView("_InsertPartial", model);
        }

        [HttpPost]
        public IActionResult Insert(MasterParmInsertarViewModel model)
        {
            if (ModelState.IsValid && this.valid(model))
            {
                var parm = new MasterParm()
                {
                    MParmId = model.MParmId,
                    MParmDesc = model.MParmDesc
                };
                this.cargarValor(parm, model.Tipo, model.MParmValTxt, model.MParmValNum, model.MParmBool);

                _context.MasterParms.Add(parm);
                _context.SaveChanges();
                
                this.ShowSuccess("El parámetro master se ha agregado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }
            
            return PartialView("_InsertPartial", model);
        }

        public IActionResult Update(string id)
        {
            var parm = _context.MasterParms.FirstOrDefault(e => e.MParmId == id);
            if(parm == null)
            {
                this.ShowError("El parámetro que intenta modificar ya no existe");
                return this.RedirectAjax(Url.Action("Index"));
            }
            var model = new MasterParmModificarViewModel()
            {
                Tipo = parm.Tipo,
                MParmId = parm.MParmId,
                MParmDesc = parm.MParmDesc,
                MParmValTxt = _parameters.MustEncript(parm.MParmId) ? _security.DesEncriptar(parm.MParmValTxt) : parm.MParmValTxt,
                MParmValNum = parm.MParmValNum
            };
            return PartialView("_UpdatePartial", model);
        }


        [HttpPost]
        public IActionResult Update(MasterParmModificarViewModel model)
        {
            if (ModelState.IsValid && this.valid(model))
            {
                var parm = _context.MasterParms.FirstOrDefault(e => e.MParmId == model.MParmId);
                parm.MParmDesc = model.MParmDesc;
                this.cargarValor(parm, model.Tipo, model.MParmValTxt, model.MParmValNum, model.MParmBool);
                _context.MasterParms.Update(parm);
                _context.SaveChanges();

                this.ShowSuccess("El parámetro master se ha modificado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }
            ViewBag.Empresas = new SelectList(_context.Empresas, "EmpId", "EmpRazonSocial");
            return PartialView("_UpdatePartial", model);
        }

        public IActionResult Delete(string id)
        {
            var parm = _context.MasterParms.FirstOrDefault(e => e.MParmId == id);
            if (parm == null)
            {
                this.ShowSuccess("El parámetro que intenta eliminar ya no existe");
                return this.RedirectAjax(Url.Action("Index"));
            }
            var model = new MasterParmEliminarViewModel()
            {
                MParmId = parm.MParmId
            };

            return PartialView("_DeletePartial", model);
        }


        [HttpPost]
        public IActionResult Delete(MasterParmEliminarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var parm = _context.MasterParms.FirstOrDefault(e => e.MParmId == model.MParmId);
                _context.MasterParms.Remove(parm);
                _context.SaveChanges();

                this.ShowSuccess("El parámetro master se ha eliminado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }

            return PartialView("_DeletePartial", model);
        }

        #region "Validaciones"


        private void cargarValor(MasterParm parm, int tipo, string txtValor, decimal numValor, string boolValor)
        {
            switch (tipo)
            {
                case 0:
                    parm.MParmValNum = 0;
                    if (_parameters.MustEncript(parm.MParmId))
                    {
                        parm.MParmValTxt = _security.Encriptar(txtValor);
                    }
                    else
                    {
                        parm.MParmValTxt = txtValor;
                    }
                    break;
                case 1:
                    parm.MParmValNum = numValor;
                    parm.MParmValTxt = "";
                    break;
                case 2:
                    parm.MParmValNum = 0;
                    parm.MParmValTxt = boolValor;
                    break;
            }
        }

        private bool valid(MasterParmInsertarViewModel model)
        {
            var result = true;
            if (_context.MasterParms.Count(e => e.MParmId == model.MParmId ) > 0)
            {
                ModelState.AddModelError("MParmId", $"Ya existe otro parámetro con el Id especificado.");
                result = false;
            }

            if (model.Tipo == 0 && string.IsNullOrEmpty(model.MParmValTxt))
            {
                ModelState.AddModelError("MParmValTxt", "Debe especificar un valor para el parámetro");
                result = false;
            }
            if (model.Tipo == 1 && model.MParmValNum == 0)
            {
                ModelState.AddModelError("MParmValNum", "Debe especificar un valor para el parámetro");
                result = false;
            }
            if (model.Tipo == 2 && string.IsNullOrEmpty(model.MParmBool))
            {
                ModelState.AddModelError("MParmBool", "Debe especificar un valor para el parámetro");
                result = false;
            }

            return result;
        }

        private bool valid(MasterParmModificarViewModel model)
        {
            var result = true;

            if (model.Tipo == 0 && string.IsNullOrEmpty(model.MParmValTxt))
            {
                ModelState.AddModelError("MParmValTxt", "Debe especificar un valor para el parámetro");
                result = false;
            }
            if (model.Tipo == 1 && model.MParmValNum == 0)
            {
                ModelState.AddModelError("MParmValNum", "Debe especificar un valor para el parámetro");
                result = false;
            }
            if (model.Tipo == 2 && string.IsNullOrEmpty(model.MParmBool))
            {
                ModelState.AddModelError("MParmBool", "Debe especificar un valor para el parámetro");
                result = false;
            }
            return result;
        }

        #endregion
    }
}