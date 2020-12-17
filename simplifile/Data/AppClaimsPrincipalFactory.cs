using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using simplifile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace simplifile.Data
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<Usuario>
    {
        public AppClaimsPrincipalFactory(
            UserManager<Usuario> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        public async override Task<ClaimsPrincipal> CreateAsync(Usuario user)
        {
            var principal = await base.CreateAsync(user);
            var rolDes = "";
            switch (user.UsuRol)
            {
                case "admin":
                    rolDes = "Administrador";
                    break;
                case "manager":
                    rolDes = "Supervisor";
                    break;
                default:
                    rolDes = "Cliente";
                    break;
            }
            ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
                    new Claim("FullName", user.UsuNombreCompleto),
                    new Claim("Role", user.UsuRol),
                    new Claim("RoleDes", rolDes),
                    new Claim("EmpId", user.UsuEmpId ?? "")
             });

            return principal;
        }

    }
}
