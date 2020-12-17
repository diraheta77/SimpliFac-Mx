using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using simplifile.Helpers;
using simplifile.Models;
using simplifile.Services;
using simplifile.ViewModels.Account;

namespace simplifile.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Usuario> UserManager;
        private readonly SignInManager<Usuario> SignInManager;
        private readonly IEmailSender EmailSender;
        public AccountController(UserManager<Usuario> userManager,SignInManager<Usuario> signInManager, IEmailSender emailSender)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            EmailSender = emailSender;
        }

        #region "Register"
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new Usuario() 
                {
                    UsuCorreo = model.UsuCorreo,
                    UsuTelefono = model.UsuTelefono,
                    UsuNombre1 = model.UsuNombre1,
                    UsuNombre2 = model.UsuNombre2,
                    UsuPaterno = model.UsuPaterno,
                    UsuMaterno = model.UsuMaterno,
                    UsuEstatus = 1,
                    UsuEmpId = "",
                    UsuRol = "client"
                };
                
                var result = await UserManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    await EmailSender.SendSignupNotification(user,model.Password, Url.Action("Login", "Account", null, Request.Scheme));
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    
                    return this.RedirectToLocal(returnUrl);
                }
                this.AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        #endregion

        #region "Login"

        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    return this.RedirectToLocal(returnUrl);
                }
                else
                {
                    this.ShowError("El usuario o la contraseña son incorrectos");
                }
            }
            return View();
        }

        #endregion

        #region "Logout"
        //[HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await SignInManager.SignOutAsync();
            return this.RedirectToAction("Login");
        }
        #endregion


        #region "Forgot"

        public IActionResult Forgot()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Forgot(ForgotViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var code = await UserManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.ResetPasswordCallbackLink(code, Request.Scheme);
                    var result = EmailSender.SendResetPassword(model.Email,user.UsuNombreCompleto, callbackUrl);
                    if (result.IsCompleted)
                    {
                        ViewData["Status"] = true;
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "No se encuentra ningun usuario con el email especificado");
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Recover(string key)
        {
            if (key == null)
            {
                this.ShowError("El codigo de recuperación es obligatorio");
                return this.RedirectToAction("Forgot");
            }
            var model = new RecoverPasswordViewModel { Key = key };
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Recover(RecoverPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(model.Email);
                
                if (user != null)
                {
                    var result = await UserManager.ResetPasswordAsync(user,model.Key,model.Password );
                    if (result.Succeeded)
                    {
                        this.ShowSuccess("Lo contraseña se ha modificado correctamente");
                        return this.RedirectToAction("Login");
                    }
                    else
                    {
                        var erroMessage = "";
                        foreach (var item in result.Errors)
                        {
                            erroMessage += item.Description + "/n";
                        }
                        this.ShowError(erroMessage);
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "No se encuentra ningun usuario con el email especificado");
                }
            }
            return View();
        }

        #endregion

        #region "Profile"
        public async Task<IActionResult> Profile()
        {
            var user = await UserManager.FindByEmailAsync(User.Identity.Name);
            var model = new ProfileViewModel
            { 
                EmpId = user.UsuEmpId,
                UsuCorreo = user.UsuCorreo,
                UsuNombre1 = user.UsuNombre1,
                UsuNombre2 = user.UsuNombre2,
                UsuPaterno = user.UsuPaterno,
                UsuMaterno = user.UsuMaterno,
                UsuTelefono = user.UsuTelefono
            };
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(User.Identity.Name);

                user.UsuCorreo = model.UsuCorreo;
                user.UsuNombre1 = model.UsuNombre1;
                user.UsuNombre2 = model.UsuNombre2;
                user.UsuPaterno = model.UsuPaterno;
                user.UsuMaterno = model.UsuMaterno;
                user.UsuTelefono = model.UsuTelefono;
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    this.ShowSuccess("Lo datos del perfil se han modificado correctamente");
                    return this.RedirectToAction("Index","Requisiciones");
                }
                else
                {
                    var erroMessage = "";
                    foreach (var item in result.Errors)
                    {
                        erroMessage += item.Description + "/n";
                    }
                    this.ShowError(erroMessage);
                }
            }
            return View();
        }

        public async Task<IActionResult> Password()
        {
            var user = await UserManager.FindByEmailAsync(User.Identity.Name);
            var model = new ProfilePasswordViewModel 
            {
                UsuCorreo = user.UsuCorreo
            };


            return PartialView("_PasswordPartial", model);
        }

        [HttpPost]
        public async Task<IActionResult> Password(ProfilePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByEmailAsync(User.Identity.Name);

                var check = await UserManager.CheckPasswordAsync(user, model.Password);
                if (!check)
                {
                    ModelState.AddModelError("Password", "La contraseña es incorrecta");
                }
                else
                {
                    var result = await UserManager.ChangePasswordAsync(user,model.Password,model.NewPassword);
                   
                    if (result.Succeeded)
                    {
                        this.ShowSuccess("La contraseña se ha cambiado correctamente");
                        return this.RedirectAjax(Url.Action("Index", "Requisiciones"));
                    }
                    else
                    {
                        var erroMessage = "";
                        foreach (var item in result.Errors)
                        {
                            erroMessage += item.Description + "/n";
                        }
                        ModelState.AddModelError("NewPassword", erroMessage);
                    }
                }
            }

            return PartialView("_PasswordPartial", model);
        }

        #endregion
    }
}