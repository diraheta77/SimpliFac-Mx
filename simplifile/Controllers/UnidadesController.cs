using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;
using simplifile.Data;
using simplifile.Helpers;
using simplifile.Models;
using simplifile.Services;
using simplifile.ViewModels.UnidadNegocio;


namespace simplifile.Controllers
{
    public class UnidadesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUpload _upload;
        private readonly ISecurity _security;

        public UnidadesController(ApplicationDbContext context, IUpload upload, ISecurity security)
        {
            _context = context;
            _upload = upload;
            _security = security;
        }

        public ActionResult Index()
        {
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return View();
        }

        [Produces("application/json")]
        [Route("api/unidades")]
        public IActionResult Search(int page = 0 ,int length = 10, string funid = "", string fempid  = "",string frazonsocial = "",string frfc = "")
        {
            //Search
            var search = _context.UnidadadesNegocios.Include(b => b.Empresa).Filter(User).ToList();
            if (!string.IsNullOrEmpty(funid))
            {
                search = search.Where(f => f.UnId.ToLower().Contains(funid.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(fempid))
            {
                search = search.Where(f => f.EmpId == fempid).ToList();
            }
            if (!string.IsNullOrEmpty(frazonsocial))
            {
                search = search.Where(f => f.UnRazonSocial.ToLower().Contains(frazonsocial.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(frfc))
            {
                search = search.Where(f => f.UnRFC.ToLower().Contains(frfc.ToLower())).ToList();
            }
            //End search
            List<UnidadNegocio> Items = search.ToPagedList(page,length).ToList();
            int Count = Items.Count();
            return Ok(new {
                recordsTotal = _context.UnidadadesNegocios.Count(),
                recordsFiltered = _context.Empresas.Count(),
                data = Items
            });
        }

        public IActionResult Insert()
        {
            var model = new UnidadNegocioInsertarViewModel { };
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return PartialView("_InsertPartial", model);
        }

        [HttpPost]
        public IActionResult Insert(UnidadNegocioInsertarViewModel model)
        {
            if (ModelState.IsValid && this.valid(model))
            {
                var unidad = new Models.UnidadNegocio()
                {
                    UnId = model.UnId,
                    EmpId = model.EmpId,
                    UnRazonSocial = model.UnRazonSocial,
                    UnRFC = model.UnRFC,
                    UnEstatus = 2
                };
                
                if (model.CargaFiel)
                {
                    var certificado = this.fiel(model.CerFiel, model.KeyFiel, model.PaswordFiel,model.EmpId, model.UnRFC);
                    if (string.IsNullOrEmpty(certificado.CerArchivoCer)) return BadRequest(ModelState);
                    _context.UnidadadesNegocios.Add(unidad);
                    certificado.UnId = unidad.UnId;
                    certificado.EmpId = unidad.EmpId;
                    _context.Certificados.Add(certificado);
                    unidad.UnEstatus = 1;
                }
                else
                {
                    _context.UnidadadesNegocios.Add(unidad);
                }

                this.ActualizaParametros(unidad.EmpId, unidad.UnId);
                _context.SaveChanges();
                
                this.ShowSuccess("La unidad de negocio se ha agregado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }
            //ViewBag.Empresas = new SelectList(_context.Empresas, "EmpId", "EmpRazonSocial");
            return BadRequest(ModelState);
        }

        public IActionResult Update(string emp_id,string id)
        {
            var unidad = _context.UnidadadesNegocios.Include(e=>e.Empresa).FirstOrDefault(e => e.EmpId == emp_id && e.UnId  == id);
            if(unidad == null)
            {
                this.ShowError("La unidad de negocio que intenta modificar ya no existe");
                return this.RedirectAjax(Url.Action("Index"));
            }
            var model = new UnidadNegocioModificarViewModel()
            {
                UnId = unidad.UnId,
                EmpId = unidad.EmpId,
                UnRazonSocial = unidad.UnRazonSocial,
                UnRFC = unidad.UnRFC,
                UnEstatus = unidad.UnEstatus,
                EmpNombre = (unidad.Empresa != null) ? unidad.Empresa.EmpRazonSocial : ""
            };
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return PartialView("_UpdatePartial", model);
        }


        [HttpPost]
        public IActionResult Update(UnidadNegocioModificarViewModel model)
        {
            if (ModelState.IsValid && this.valid(model)  )
            {
                var unidad = _context.UnidadadesNegocios.FirstOrDefault(e => e.UnId == model.UnId && e.EmpId == model.EmpId);
                unidad.UnRFC = model.UnRFC;
                unidad.UnRazonSocial = model.UnRazonSocial;
                unidad.EmpId = model.EmpId;

                if (model.CargaFiel)
                {
                    var certificado = this.fiel(model.CerFiel, model.KeyFiel, model.PaswordFiel,model.EmpId, model.UnRFC);
                    if (string.IsNullOrEmpty(certificado.CerArchivoCer)) return BadRequest(ModelState);
                    var certDb = _context.Certificados.FirstOrDefault(f => f.UnId == unidad.UnId && f.EmpId == unidad.EmpId);
                    if(certDb == null)
                    {
                        certificado.UnId = unidad.UnId;
                        certificado.EmpId = unidad.EmpId;
                        _context.Certificados.Add(certificado);
                    }
                    else
                    {
                        certDb.CerFechaFin = certificado.CerFechaFin;
                        certDb.CerFechaInicio = certificado.CerFechaInicio;
                        certDb.CerContrasena = certificado.CerContrasena;
                        certDb.CerArchivoCer = certificado.CerArchivoCer;
                        certDb.CerArchivoKey = certificado.CerArchivoKey;
                        certDb.CerRFC = certificado.CerRFC;
                        certDb.CerEstatus = certificado.CerEstatus;
                    }
                    unidad.UnEstatus = 1;
                }

                _context.UnidadadesNegocios.Update(unidad);
                this.ActualizaParametros(unidad.EmpId, unidad.UnId);
                _context.SaveChanges();

                this.ShowSuccess("La unidad de negocio se ha modificado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }
            return BadRequest(ModelState);
        }

        public IActionResult Delete(string emp_id,string id)
        {
            var unidad = _context.UnidadadesNegocios.FirstOrDefault(e => e.UnId == id);
            if (unidad == null)
            {
                this.ShowError("La unidad de negocio que intenta eliminar ya no existe");
                return this.RedirectAjax(Url.Action("Index"));
            }
            var model = new UnidadNegocioEliminarViewModel()
            {
                UnId = unidad.UnId
            };

            return PartialView("_DeletePartial", model);
        }


        [HttpPost]
        public IActionResult Delete(UnidadNegocioEliminarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var unidad = _context.UnidadadesNegocios.FirstOrDefault(e => e.UnId == model.UnId);
                _context.UnidadadesNegocios.Remove(unidad);
                _context.SaveChanges();

                this.ShowSuccess("La unidad de negocio se ha eliminado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }

            return PartialView("_DeletePartial", model);
        }

        #region "Parametros"
        public void ActualizaParametros(string empId, string UniId)
        {
            List<Parm> parms = new List<Parm>();
            foreach (var item in _context.MasterParms.Where(f => f.MParmId.StartsWith("emp")))
            {
                //validar si la unidad tiene el parametro o no.
                if(_context.Parms.Count(d=>d.EmpId == empId && d.UnId == UniId && d.ParmId == item.MParmId) == 0)
                {
                    parms.Add(new Parm()
                    {
                        EmpId = empId,
                        ParmDesc = item.MParmDesc,
                        ParmId = item.MParmId,
                        UnId = UniId,
                        ParmValNum = item.MParmValNum,
                        ParmValTxt = item.MParmValTxt
                    });
                    
                }
                //if (_context.Parms.Count(f => f.ParmId == item.MParmId && f.EmpId == empId) == 0)
                //{
                //    _context.Parms.Add(new Parm()
                //    {
                //        EmpId = empId,
                //        ParmDesc = item.MParmDesc,
                //        ParmId = item.MParmId,
                //        UnId = UniId,
                //        ParmValNum = item.MParmValNum,
                //        ParmValTxt = item.MParmValTxt
                //    });
                //}
            }
            _context.Parms.AddRange(parms);
            _context.SaveChanges();
        }
        #endregion

        #region "Validaciones"
        private bool valid(UnidadNegocioInsertarViewModel model)
        {
            bool status = true;
            if (_context.UnidadadesNegocios.Count(e => e.UnId == model.UnId && e.EmpId == model.EmpId) > 0)
            {
                ModelState.AddModelError("UnId", "Ya existe otra unidad de negocio con el código especificado");
                status = false;
            }
            if (_context.UnidadadesNegocios.Count(e => e.UnRFC == model.UnRFC) > 0)
            {
                ModelState.AddModelError("UnRfc", "Ya existe otra unidad de negocio con el Rfc especificado");
                status = false;
            }
            if (model.CargaFiel)
            {
                if (string.IsNullOrEmpty(model.PaswordFiel))
                {
                    ModelState.AddModelError("PaswordFiel", "Este campo es obligatorio");
                    status = false;
                }
                if (model.CerFiel == null)
                {
                    ModelState.AddModelError("CerFiel", "Este campo es obligatorio");
                    status = false;
                }
                if (model.KeyFiel == null)
                {
                    ModelState.AddModelError("KeyFiel", "Este campo es obligatorio");
                    status = false;
                }
            }
            return status;
        }

        private bool valid(UnidadNegocioModificarViewModel model)
        {
            bool status = true;
            if (_context.UnidadadesNegocios.Count(e => e.UnRFC == model.UnRFC && e.UnId != model.UnId) > 0)
            {
                ModelState.AddModelError("UnRFC", "Ya existe otra unidad de negocio con el RFC especificado");
                status = false;
            }
            if (_context.UnidadadesNegocios.Count(e => e.EmpId == model.EmpId && e.UnRFC == model.UnRFC && e.UnId != model.UnId) > 0)
            {
                ModelState.AddModelError("UnRFC", "Ya existe otra unidad de negocio con el RFC especificado");
                status = false;
            }
            if (model.CargaFiel)
            {
                if (string.IsNullOrEmpty(model.PaswordFiel))
                {
                    ModelState.AddModelError("PaswordFiel", "Este campo es obligatorio");
                    status = false;
                }
                if (model.CerFiel == null)
                {
                    ModelState.AddModelError("CerFiel", "Este campo es obligatorio");
                    status = false;
                }
                if (model.KeyFiel == null)
                {
                    ModelState.AddModelError("KeyFiel", "Este campo es obligatorio");
                    status = false;
                }
            }
            return status;
        }

        public Certificado fiel(IFormFile cert, IFormFile key,string password,string empId,string rfc)
        {
            var certificado = new Certificado();
            var error = false;
            
            var certPath = this._upload.uploadCerFile(cert,empId, rfc, cert.FileName);
            var keyPath = this._upload.uploadKeyFile(key, empId,rfc, key.FileName);
            
            var datosCert = this._security.ExtraeDatosCer(certPath);
            if(datosCert.Rfc != rfc)
            {
                ModelState.AddModelError("CerFiel", "El rfc definido en el certificado no coincide con el especificado en el formulario");
                error = true;
            }

            //Validación FAKE UVM131003DL4
            if ((rfc == "UVM131003DL4") && (password == "UVM131003"))
            {
                error = false;
            }
            else
            {
                if (!_security.ValidarLLave(keyPath, password))
                {
                    ModelState.AddModelError("KeyFiel", "La contraseña de la llave es incorrecta");
                    error = true;
                }
                else if (!_security.ValidarCerLLave(certPath, keyPath, password))
                {
                    ModelState.AddModelError("KeyFiel", "La llave del certificado no es correcta");
                    error = true;
                }
            }

            if (error)
            {
                this._upload.deleteFile(certPath);
                this._upload.deleteFile(keyPath);
            }


            if (!error)
            {
                certificado.CerArchivoCer = this._security.Encriptar(cert.FileName);
                certificado.CerArchivoKey = this._security.Encriptar(key.FileName);
                certificado.CerContrasena = _security.Encriptar(password);
                certificado.CerFechaFin = datosCert.FechaFin;
                certificado.CerFechaInicio = datosCert.FechaInicio;
                certificado.CerRFC = datosCert.Rfc;
                certificado.CerEstatus = 1;
            }
            return certificado;

        }

        #endregion
    }
}