using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PagedList;
using simplifile.Data;
using simplifile.Helpers;
using simplifile.Models;
using simplifile.ViewModels.Empresa;

namespace simplifile.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpresasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Produces("application/json")]
        [Route("api/empresas")]
        public IActionResult Search(int page = 0 ,int length = 10, string fempid  = "",string frazonsocial = "",string frfc = "")
        {
            //Search
            var search = _context.Empresas.Filter(User).ToList();
            if (!string.IsNullOrEmpty(fempid))
            {
                search = search.Where(f => f.EmpId.ToLower().Contains(fempid.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(frazonsocial))
            {
                search = search.Where(f => f.EmpRazonSocial.ToLower().Contains(frazonsocial.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(frfc))
            {
                search = search.Where(f => f.EmpRFC.ToLower().Contains(frfc.ToLower())).ToList();
            }
            //End search
            List<Empresa> Items = search.ToPagedList(page,length).ToList();
            int Count = Items.Count();
            return Ok(new {
                recordsTotal = _context.Empresas.Filter(User).Count(),
                recordsFiltered = search.Count(),
                data = Items
            });
        }

        public IActionResult Insert()
        {
            var model = new EmpresaInsertarViewModel { };

            return PartialView("_InsertPartial", model);
        }

        [HttpPost]
        public IActionResult Insert(EmpresaInsertarViewModel model)
        {
            this.valid(model);
            if (ModelState.IsValid)
            {
                _context.Empresas.Add(new Models.Empresa()
                {
                    EmpId = model.EmpId,
                    EmpRazonSocial = model.EmpRazonSocial,
                    EmpRFC = model.EmpRFC,
                    EmpEstatus = 1
                });
                //actualizare los parametros cuando cree una nueva unidad de negocios.
                //this.ActualizaParametros(model.EmpId);
                _context.SaveChanges();
                
                this.ShowSuccess("La empresa se ha agregado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }

            return PartialView("_InsertPartial", model);
        }

        public IActionResult Update(string id)
        {
            var company = _context.Empresas.FirstOrDefault(e => e.EmpId == id);
            if(company == null)
            {
                this.ShowSuccess("La empresa que intenta modificar ya no existe");
                return this.RedirectAjax(Url.Action("Index"));
            }
            var model = new EmpresaModificarViewModel()
            { 
                EmpId = company.EmpId,
                EmpRazonSocial = company.EmpRazonSocial,
                EmpRFC = company.EmpRFC,
                EmpEstatus = company.EmpEstatus
            };

            return PartialView("_UpdatePartial", model);
        }


        [HttpPost]
        public IActionResult Update(EmpresaModificarViewModel model)
        {
            this.valid(model);
            if (ModelState.IsValid)
            {
                var company = _context.Empresas.FirstOrDefault(e => e.EmpId == model.EmpId);
                company.EmpRFC = model.EmpRFC;
                company.EmpRazonSocial = model.EmpRazonSocial;
                company.EmpEstatus = model.EmpEstatus;
                _context.Empresas.Update(company);
                //this.ActualizaParametros(model.EmpId);
                _context.SaveChanges();

                this.ShowSuccess("La empresa se ha modificado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }

            return PartialView("_UpdatePartial", model);
        }

        public IActionResult Delete(string id)
        {
            var company = _context.Empresas.FirstOrDefault(e => e.EmpId == id);
            if (company == null)
            {
                this.ShowSuccess("La empresa que intenta eliminar ya no existe");
                return this.RedirectAjax(Url.Action("Index"));
            }
            var model = new EmpresaEliminarViewModel()
            {
                EmpId = company.EmpId
            };

            return PartialView("_DeletePartial", model);
        }


        [HttpPost]
        public IActionResult Delete(EmpresaEliminarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var company = _context.Empresas.FirstOrDefault(e => e.EmpId == model.EmpId);
                _context.Empresas.Remove(company);
                _context.SaveChanges();

                this.ShowSuccess("La empresa se ha eliminado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }

            return PartialView("_DeletePartial", model);
        }

        #region "Parametros"
        public void ActualizaParametros(string empId)
        {
            foreach (var item in _context.MasterParms.Where(f => f.MParmId.StartsWith("emp")))
            {
                if(_context.Parms.Count(f=>f.ParmId == item.MParmId && f.EmpId == empId) == 0)
                {
                    _context.Parms.Add(new Parm()
                    {
                        EmpId = empId,
                        ParmDesc = item.MParmDesc,
                        ParmId = item.MParmId,
                        ParmValNum = item.MParmValNum,
                        ParmValTxt = item.MParmValTxt
                    });
                }
            }
        }
        #endregion
        #region "Validaciones"
        private void valid(EmpresaInsertarViewModel model)
        {
            if(ModelState.ContainsKey("EmpId") &&  ModelState["EmpId"].ValidationState != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                if(_context.Empresas.Count(e => e.EmpId == model.EmpId)> 0)
                {
                    ModelState.AddModelError("EmpId", "Ya existe otra empresa con el código especificado");
                }
            }

            if (ModelState.ContainsKey("EmpRFC") && ModelState["EmpRFC"].ValidationState != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                if (_context.Empresas.Count(e => e.EmpRFC == model.EmpRFC) > 0)
                {
                    ModelState.AddModelError("EmpRFC", "Ya existe otra empresa con el RFC especificado");
                }
            }
        }

        private void valid(EmpresaModificarViewModel model)
        {
            if (ModelState.ContainsKey("EmpRFC") && ModelState["EmpRFC"].ValidationState != Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Invalid)
            {
                if (_context.Empresas.Count(e => e.EmpRFC == model.EmpRFC && e.EmpId != model.EmpId ) > 0)
                {
                    ModelState.AddModelError("EmpRFC", "Ya existe otra empresa con el RFC especificado");
                }
            }
        }

        #endregion
    }
}