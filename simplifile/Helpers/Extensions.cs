using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using simplifile.Controllers;
using simplifile.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace simplifile.Helpers
{
    public static class Extensions
    {
        public static IActionResult RedirectToLocal(this Controller controller, string returnUrl)
        {
            if (controller.Url.IsLocalUrl(returnUrl))
            {
                return controller.Redirect(returnUrl);
            }
            else
            {
                return controller.RedirectToAction(nameof(RequisicionesController.Index), "Requisiciones");
            }
        }

        public static void AddErrors(this Controller controller,IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                controller.ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public static void AddErrors(this Controller controller, string message)
        {
            controller.ModelState.AddModelError(string.Empty, message);
        }

        public static void ShowError(this Controller controller, string message)
        {
            message = message.Replace("'", "");
            controller.TempData["Error"] = message;
            //controller.ModelState.AddModelError(string.Empty, message);
        }

        public static void ShowSuccess(this Controller controller, string message)
        {
            message = message.Replace("'", "");
            controller.TempData["Success"] = message;
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper,string key, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.Recover),
                controller: "Account",
                values: new { key },
                protocol: scheme);
        }

        public static IActionResult RedirectAjax(this Controller controller, string url)
        {
            return controller.Ok(new { Redirect = true, Url = url });
        }

        public static bool HasRoles(this ClaimsPrincipal user, params string[] roles)
        {
            var usrRole = user.FindFirst("Role");
            if (usrRole != null)
                return roles.Contains(usrRole.Value);
            return false;
        }

        public static bool IsIexplorer(this HttpRequest request)
        {
            if(request.Headers.ContainsKey("User-Agent"))
            {
                return request.Headers["User-Agent"].Count(f => f.Contains("MSIE")) > 0 ||
                    request.Headers["User-Agent"].Count(f => f.Contains("Trident")) > 0 || request.Headers["User-Agent"].Count(f => f.Contains("Edge")) > 0; 
            }
            return false;
        }

        public static string getEmpId(this ClaimsPrincipal user)
        {
            var EmpId = user.FindFirst("EmpId").Value;
            if (!string.IsNullOrEmpty(EmpId))
                return EmpId;
            return "";
        }
    }
}
