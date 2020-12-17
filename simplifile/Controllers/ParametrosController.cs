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
using simplifile.ViewModels.Parm;

namespace simplifile.Controllers
{
    public class ParametrosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISecurity _security;
        private readonly IParamaters _parameters;

        public ParametrosController(ApplicationDbContext context, ISecurity security, IParamaters parameters)
        {
            _context = context;
            _security = security;
            _parameters = parameters;
        }

        public ActionResult Index()
        {
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            ViewBag.Unidades = new SelectList(_context.UnidadadesNegocios.Filter(User), "UnId", "UnRazonSocial");
            return View();
        }

        [Produces("application/json")]
        [Route("api/parametros")]
        public IActionResult Search(int page = 0 ,int length = 10, string fparmid  = "", string fparmempid = "", string fparmuniid="", string fparmdesc = "",string fparmval = "")
        {
            //Search
            var search = _context.Parms.Include(p=>p.Empresa).Filter(User).ToList();
            if (!string.IsNullOrEmpty(fparmid))
            {
                search = search.Where(f => f.ParmId.ToLower().Contains(fparmid.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(fparmempid))
            {
                search = search.Where(f => f.EmpId == fparmempid).ToList();
            }
            if (!string.IsNullOrEmpty(fparmuniid))
            {
                search = search.Where(f => f.EmpId == fparmuniid).ToList();
            }
            if (!string.IsNullOrEmpty(fparmdesc))
            {
                search = search.Where(f => f.ParmDesc.ToLower().Contains(fparmdesc.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(fparmval))
            {
                search = search.Where(f => f.ParmValTxt.ToLower().Contains(fparmval.ToLower()) || f.ParmValNum.ToString().ToLower().Contains(fparmval.ToLower())).ToList();
            }
            //End search
            List<Parm> Items = search.ToPagedList(page,length).ToList();
            int Count = Items.Count();
            return Ok(new {
                recordsTotal = _context.Parms.Count(),
                recordsFiltered = search.Count(),
                data = Items
            });
        }

        public IActionResult Insert()
        {
            var model = new ParmInsertarViewModel 
            {
                Tipo = 0
            };
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return PartialView("_InsertPartial", model);
        }

        [HttpPost]
        public IActionResult Insert(ParmInsertarViewModel model)
        {
            if (ModelState.IsValid && this.valid(model))
            {
                var parm = new Parm()
                {
                    ParmId = model.ParmId,
                    EmpId = model.ParmEmpId,
                    ParmDesc = model.ParmDesc,
                };
                this.cargarValor(parm, model.Tipo, model.ParmValTxt, model.ParmValNum, model.ParmBool);

                _context.Parms.Add(parm);
                _context.SaveChanges();
                
                this.ShowSuccess("El parámetro se ha agregado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return PartialView("_InsertPartial", model);
        }

        public IActionResult Update(string id)
        {
            var parm = _context.Parms.Include(e=>e.Empresa).FirstOrDefault(e => e.ParmId == id);
            if(parm == null)
            {
                this.ShowSuccess("El parámetro que intenta modificar ya no existe");
                return this.RedirectAjax(Url.Action("Index"));
            }
            var model = new ParmModificarViewModel()
            {
                ParmId = parm.ParmId,
                ParmEmpId = parm.EmpId,
                ParmUniId=parm.UnId,
                ParmUniNombre=parm.UnId,
                ParmDesc = parm.ParmDesc,
                ParmValTxt =  _parameters.MustEncript(parm.ParmId) ? _security.DesEncriptar(parm.ParmValTxt) : parm.ParmValTxt ,
                ParmValNum = parm.ParmValNum,
                Tipo = parm.Tipo,
                ParmBool = parm.ParmValTxt,
                ParmEmpNombre = (parm.Empresa != null ) ? parm.Empresa.EmpRazonSocial : ""
            };
            
            return PartialView("_UpdatePartial", model);
        }


        [HttpPost]
        public IActionResult Update(ParmModificarViewModel model)
        {
            if (ModelState.IsValid && this.valid(model))
            {
                var parm = _context.Parms.FirstOrDefault(e => e.ParmId == model.ParmId && e.EmpId == model.ParmEmpId);
                parm.ParmDesc = model.ParmDesc;
                parm.EmpId = model.ParmEmpId;

                this.cargarValor(parm, model.Tipo, model.ParmValTxt, model.ParmValNum, model.ParmBool);
                

                
                _context.Parms.Update(parm);
                _context.SaveChanges();

                this.ShowSuccess("El parámetro se ha modificado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return PartialView("_UpdatePartial", model);
        }

        public IActionResult Delete(string id)
        {
            var parm = _context.Parms.FirstOrDefault(e => e.ParmId == id);
            if (parm == null)
            {
                this.ShowSuccess("El parámetro que intenta eliminar ya no existe");
                return this.RedirectAjax(Url.Action("Index"));
            }
            var model = new ParmEliminarViewModel()
            {
                ParmId = parm.ParmId,
                ParmEmpId = parm.EmpId
            };

            return PartialView("_DeletePartial", model);
        }


        [HttpPost]
        public IActionResult Delete(ParmEliminarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var parm = _context.Parms.FirstOrDefault(e => e.ParmId == model.ParmId && e.EmpId == model.ParmEmpId);
                _context.Parms.Remove(parm);
                _context.SaveChanges();

                this.ShowSuccess("El parámetro se ha eliminado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }

            return PartialView("_DeletePartial", model);
        }

        #region "Validaciones"

        private void cargarValor(Parm parm,int tipo,string txtValor,decimal numValor,string boolValor)
        {
            switch (tipo)
            {
                case 0:
                    parm.ParmValNum = 0;
                    if (_parameters.MustEncript(parm.ParmId))
                    {
                        parm.ParmValTxt = _security.Encriptar(txtValor);
                    }
                    else
                    {
                        parm.ParmValTxt = txtValor;
                    }
                    break;
                case 1:
                    parm.ParmValNum = numValor;
                    parm.ParmValTxt = "";
                    break;
                case 2:
                    parm.ParmValNum = 0;
                    parm.ParmValTxt = boolValor;
                    break;
            }
        }

        private bool valid(ParmInsertarViewModel model)
        {
            var result = true;
            if (_context.Parms.Count(e => e.ParmId == model.ParmId && e.EmpId == model.ParmEmpId ) > 0)
            {
                ModelState.AddModelError("ParmId", $"Ya existe otro parámetro con el Id especificado para la empresa {model.ParmEmpId}");
                result = false;
            }

            if (model.Tipo == 0 && string.IsNullOrEmpty(model.ParmValTxt))
            {
                ModelState.AddModelError("ParmValTxt", "Debe especificar un valor para el parámetro");
                result = false;
            }
            if (model.Tipo == 1 && model.ParmValNum == 0)
            {
                ModelState.AddModelError("ParmValNum", "Debe especificar un valor para el parámetro");
                result = false;
            }
            if (model.Tipo == 2 && string.IsNullOrEmpty(model.ParmBool))
            {
                ModelState.AddModelError("ParmBool", "Debe especificar un valor para el parámetro");
                result = false;
            }


            return result;
        }

        private bool valid(ParmModificarViewModel model)
        {
            var result = true;
            if (_context.Parms.Count(e => e.ParmId != model.ParmId && e.EmpId == model.ParmEmpId) > 0)
            {
                ModelState.AddModelError("ParmId", $"Ya existe otro parámetro con el Id especificado para la empresa {model.ParmEmpId}");
            }
            if (model.Tipo == 0 && string.IsNullOrEmpty(model.ParmValTxt))
            {
                ModelState.AddModelError("ParmValTxt", "Debe especificar un valor para el parámetro");
                result = false;
            }
            if (model.Tipo == 1 && model.ParmValNum == 0)
            {
                ModelState.AddModelError("ParmValNum", "Debe especificar un valor para el parámetro");
                result = false;
            }
            if (model.Tipo == 2 && string.IsNullOrEmpty(model.ParmBool))
            {
                ModelState.AddModelError("ParmBool", "Debe especificar un valor para el parámetro");
                result = false;
            }
            return result;
        }

        #endregion
    }
}