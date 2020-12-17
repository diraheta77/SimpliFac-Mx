using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Routing;
using simplifile.Helpers;
using simplifile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Middleware
{
    public class AppMiddleware
    {
        private readonly RequestDelegate _next;

        public AppMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext,IParamaters paramaters)
        {
            
            if (httpContext.Request.IsIexplorer())
            {
                if (!httpContext.Request.Path.Value.ToLower().Contains("/navegador"))
                {
                    httpContext.Response.Redirect(httpContext.Request.PathBase.Value + "/home/navegador", false);
                    return _next(httpContext);
                }
            }
            if (!httpContext.User.Identity.IsAuthenticated && paramaters.mustInstall())
            {
                if (!httpContext.Request.Path.Value.ToLower().Contains("/instalar"))
                {
                    httpContext.Response.Redirect(httpContext.Request.PathBase.Value + "/instalar", false);
                    return _next(httpContext);
                }
            }
            if(httpContext.User.Identity.IsAuthenticated && !httpContext.User.HasRoles("admin"))
            {
                if (paramaters.getGenParam(ParamsKeys.G01, "false") == "true")
                {
                    if (!httpContext.Request.Path.Value.ToLower().Contains("/home/mantenimiento") && !httpContext.Request.Path.Value.ToLower().Contains("/account") && !httpContext.Request.Path.Value.ToLower().Contains("/acceso"))
                    {
                        httpContext.Response.Redirect(httpContext.Request.PathBase.Value + "/home/mantenimiento", false);
                        return _next(httpContext);

                    }

                }
            }
            return _next(httpContext);
        }
    }

    public static class AppMiddlewareExtensions
    {
        public static IApplicationBuilder UseAppMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AppMiddleware>();
        }
    }
}
