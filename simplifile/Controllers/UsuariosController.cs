using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;
using simplifile.Data;
using simplifile.Helpers;
using simplifile.Models;
using simplifile.Services;
using simplifile.ViewModels.Usuario;

namespace simplifile.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly IEmailSender _emailSender;


        public UsuariosController(ApplicationDbContext context, UserManager<Usuario> userManager,IEmailSender emailSender)
        {
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public ActionResult Index()
        {
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return View();
        }

        [Produces("application/json")]
        [Route("api/usuarios")]
        public IActionResult Buscar(int page = 0 ,int length = 10, string femail = "", string fempid = "", string fnombre  = "",string frol = "")
        {
            //Search
            var search = _context.Usuarios.Include(b => b.Empresa).Filter(User).ToList();

            if (!string.IsNullOrEmpty(femail))
            {
                search = search.Where(f => f.UsuCorreo.ToLower().Contains(femail.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(fnombre))
            {
                search = search.Where(f => f.UsuNombreCompleto.ToLower().Contains(fnombre.ToLower())).ToList();
            }
            if (!string.IsNullOrEmpty(fempid))
            {
                search = search.Where(f => f.UsuEmpId == fempid).ToList();
            }
            if (!string.IsNullOrEmpty(frol))
            {
                search = search.Where(f => f.UsuRol.ToLower().Contains(frol)).ToList();
            }

            //End search
            List<Usuario> Items = search.ToPagedList(page,length).ToList();
            
            int Count = Items.Count();
            return Ok(new {
                recordsTotal = _context.Usuarios.Count(),
                recordsFiltered = search.Count(),
                data = Items
            });
        }

        public IActionResult Insert()
        {
            var model = new UsuarioInsertarViewModel { };
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return PartialView("_InsertPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> Insert(UsuarioInsertarViewModel model)
        {
            if (ModelState.IsValid && this.valid(model))
            {
                var user = new Usuario()
                {
                    UsuCorreo = model.UsuCorreo,
                    UsuNombre1 = model.UsuNombre1,
                    UsuNombre2 = model.UsuNombre2,
                    UsuPaterno = model.UsuPaterno,
                    UsuMaterno = model.UsuMaterno,
                    UsuTelefono = model.UsuTelefono,
                    UsuEstatus = 1,
                    UsuEmpId = model.UsuEmpId,
                    UsuRol = model.UsuRol
                };
                var result =  await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded )
                {
                    await _emailSender.SendSignupNotification(user, model.Password, Url.Action("Login", "Account", null, Request.Scheme));

                    this.ShowSuccess("El usuario se ha agregado correctamente");
                    return this.RedirectAjax(Url.Action("Index"));
                }
                else
                {
                    ModelState.AddModelError("UsuCorreo", "Ha ocurrido un error y no se puede completar la operación.");
                }
                
            }
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return PartialView("_InsertPartial", model);
        }


        public IActionResult Update(string id)
        {
            var user = _context.Usuarios.FirstOrDefault(x => x.UsuId.ToString() == id);
            if (user == null)
            {
                this.ShowError("El usuario que intenta modificar ya no existe");
                return this.RedirectAjax(Url.Action("Index"));
            }
            var model = new UsuarioModificarViewModel()
            {
                UsuId = user.UsuId,
                UsuEmpId = user.UsuEmpId,
                UsuCorreo = user.UsuCorreo,
                UsuNombre1 = user.UsuNombre1,
                UsuNombre2 = user.UsuNombre2,
                UsuPaterno = user.UsuPaterno,
                UsuMaterno = user.UsuMaterno,
                UsuEstatus = user.UsuEstatus,
                UsuRol = user.UsuRol,
                UsuTelefono = user.UsuTelefono
            };
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return PartialView("_UpdatePartial", model);
        }


        [HttpPost]
        public async Task<IActionResult> Update(UsuarioModificarViewModel model)
        {
            if (ModelState.IsValid && this.valid(model))
            {
                var user = await _userManager.FindByIdAsync(model.UsuId.ToString());
                user.UsuEmpId = model.UsuEmpId;
                user.UsuCorreo = model.UsuCorreo;
                user.UsuNombre1 = model.UsuNombre1;
                user.UsuNombre2 = model.UsuNombre2;
                user.UsuPaterno = model.UsuPaterno;
                user.UsuMaterno = model.UsuMaterno;
                user.UsuEstatus = model.UsuEstatus;
                user.UsuRol = model.UsuRol;
                user.UsuTelefono = model.UsuTelefono;

                var result = await _userManager.UpdateAsync(user);
                
                if (result.Succeeded)
                {
                    this.ShowSuccess("El usuario se ha modificado correctamente");
                    return this.RedirectAjax(Url.Action("Index"));
                }
                else
                {
                    ModelState.AddModelError("Email", "Ha ocurrido un error y no se puede completar la operación.");
                }
            }
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return PartialView("_UpdatePartial", model);
        }

        
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                this.ShowError("El usuario que intenta eliminar ya no existe");
                return this.RedirectAjax(Url.Action("Index"));
            }
            var model = new UsuarioEliminarViewModel()
            {
                UsuId = user.UsuId,
                UsuCorreo = user.UsuCorreo
            };

            return PartialView("_DeletePartial", model);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(UsuarioEliminarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UsuId.ToString());
                await _userManager.DeleteAsync(user);
                this.ShowSuccess("El usuario se ha eliminado correctamente");
                return this.RedirectAjax(Url.Action("Index"));
            }

            return PartialView("_DeletePartial", model);
        }

        public async Task<IActionResult> Password(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                this.ShowError("El usuario al que intenta actualizar contraseña ya no existe");
                return this.RedirectAjax(Url.Action("Index"));
            }
            var model = new UsuarioPasswordViewModel()
            {
                UsuId = user.UsuId,
                UsuCorreo = user.UsuCorreo
            };
            return PartialView("_PasswordPartial", model);
        }


        [HttpPost]
        public async Task<IActionResult> Password(UsuarioPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UsuId.ToString());
                await _userManager.RemovePasswordAsync(user);
                var result = await _userManager.AddPasswordAsync(user,model.Password);
                if (result.Succeeded)
                {
                    this.ShowSuccess("La contraseña se ha modificado correctamente");
                    return this.RedirectAjax(Url.Action("Index"));
                }
                else
                {
                    ModelState.AddModelError("Email", "Ha ocurrido un error y no se puede completar la operación.");
                }
            }
            return PartialView("_PasswordPartial", model);
        }

        #region "Validaciones"
        private bool valid(UsuarioInsertarViewModel model)
        {
            bool status = true;
            if (_context.Usuarios.Count(e => e.UsuCorreo == model.UsuCorreo) > 0)
            {
                ModelState.AddModelError("Email", "Ya existe otro usuario con el email especificado");
                status = false;
            }
            if (model.UsuRol != "admin" && string.IsNullOrEmpty(model.UsuEmpId))
            {
                ModelState.AddModelError("UsuEmpId", "Este campo es obligatorio");
                status = false;
            }
            return status;
        }

        private bool valid(UsuarioModificarViewModel model)
        {
            bool status = true;
            if (_context.Usuarios.Count(e => e.UsuCorreo == model.UsuCorreo && e.UsuId != model.UsuId) > 0)
            {
                ModelState.AddModelError("Email", "Ya existe otro usuario con el email especificado");
                status = false;
            }
            if (model.UsuRol != "admin" && string.IsNullOrEmpty(model.UsuEmpId))
            {
                ModelState.AddModelError("UsuEmpId", "Este campo es obligatorio");
                status = false;
            }
            return status;
        }

        #endregion
    }
}