using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using simplifile.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using simplifile.Models;
using simplifile.Services;
using Microsoft.AspNetCore.Http;
using simplifile.Middleware;

namespace simplifile
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /*services.AddIdentity<User, Role>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 0;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();*/

            services.AddDefaultIdentity<Usuario>(options => 
            {
                
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

            }).AddUserStore<UserDbProvider>().AddClaimsPrincipalFactory<AppClaimsPrincipalFactory>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/account/login";
                options.LogoutPath = $"/account/logout";
                options.AccessDeniedPath = $"/account/access-denied";
            });

            //services.AddScoped<IUserClaimsPrincipalFactory<Usuario>, AppClaimsPrincipalFactory>();


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //services.Configure<SmtpOptions>(Configuration.GetSection("SmtpOptions"));

            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IParamaters, Paramaters>();
            services.AddTransient<IUpload, Upload>();
            services.AddTransient<ISecurity, Security>();

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //para usar cache en el sitio..
            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            /*app.Use(async (context, next) =>
            {
                var install = Configuration["install"];
                if (install == "0" && !context.Request.Path.StartsWithSegments("/instalar"))
                {
                    context.Response.Redirect("/instalar");
                }
                else
                {
                    await next.Invoke();
                }
            });*/

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAppMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Requisiciones}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
