using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using simplifile.Data;
using simplifile.Models;
using simplifile.Services;
using simplifile.ViewModels.Instalar;

namespace simplifile.Controllers
{
    public class InstalarController : Controller
    {
        private readonly UserManager<Usuario> UserManager;
        private readonly SignInManager<Usuario> SignInManager;
        private readonly ApplicationDbContext _context;
        private readonly ISecurity _security;
        public InstalarController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, ApplicationDbContext context,ISecurity security )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _context = context;
            _security = security;

        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> IndexAsync(InstalarViewModel model)
        {
            //try
            //{

            //    var filePath = Path.Combine(AppContext.BaseDirectory, "appSettings.json");
            //    string json = System.IO.File.ReadAllText(filePath);
            //    dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            //    jsonObj.ConnectionStrings.DefaultConnection = "adasdasd";
            //    jsonObj.install = "1";

            //    /*if (!string.IsNullOrEmpty(sectionPath))
            //    {
            //        var keyPath = key.Split(":")[1];
            //        jsonObj[sectionPath][keyPath] = value;
            //    }
            //    else
            //    {
            //        jsonObj[sectionPath] = value; // if no sectionpath just set the value
            //    }*/
            //    string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            //    System.IO.File.WriteAllText(filePath, output);

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Error writing app settings");
            //}

            if (ModelState.IsValid)
            {
                var user = new Usuario()
                {
                    UsuCorreo = model.EmailAdmin,
                    UsuTelefono = "",
                    UsuNombre1 = "Administrador",
                    UsuNombre2 = "",
                    UsuPaterno = "",
                    UsuMaterno = "",
                    UsuEstatus = 1,
                    UsuEmpId = "",
                    UsuRol = "admin"
                };

                var result = await UserManager.CreateAsync(user, model.EmailPassword);

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                }

                #region parametros generales
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-01",
                    MParmDesc = "Mantenimiento",
                    MParmValTxt = model.gen01
                });
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-02",
                    MParmDesc = "URL Main Page",
                    MParmValTxt = model.gen02
                });
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-03",
                    MParmDesc = "Ruta Job de Java",
                    MParmValTxt = model.gen03
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-04",
                    MParmDesc = "Servidor Smtp",
                    MParmValTxt = model.gen04
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-05",
                    MParmDesc = "Usuario Smtp",
                    MParmValTxt = model.gen05
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-06",
                    MParmDesc = "Contraseña Smtp",
                    MParmValTxt = model.gen06
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-07",
                    MParmDesc = "Puerto Smtp",
                    MParmValNum = int.Parse(model.gen07)
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-11",
                    MParmDesc = "Usar SSL para Smtp",
                    MParmValTxt = model.gen11
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-12",
                    MParmDesc = "Dirección de correo que se muestra en el envio de notificaciones",
                    MParmValTxt = model.gen12
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-13",
                    MParmDesc = "Nombre que se muestra en el envio de notificaciones",
                    MParmValTxt = model.gen13
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-08",
                    MParmDesc = "Comunicado",
                    MParmValTxt = model.gen08
                });
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-09",
                    MParmDesc = "Nuevas Empresas",
                    MParmValTxt = model.gen09
                });
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-10",
                    MParmDesc = "LLave encriptación",
                    MParmValTxt = model.gen10
                });
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-14",
                    MParmDesc = "Hora repeticion para descargar listas negras.",
                    MParmValTxt = model.gen14
                });
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-15",
                    MParmDesc = "Hora de repeticion para validar CFDI vs Listas negras",
                    MParmValTxt = model.gen15
                });
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-16",
                    MParmDesc = "Hora de repeticion para validar el status de los CFDI",
                    MParmValTxt = model.gen16
                });
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-17",
                    MParmDesc = "Hora de repeticion para descargar CFDI",
                    MParmValTxt = model.gen17
                });
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-18",
                    MParmDesc = "Activar o desactivar notificaciones",
                    MParmValTxt = model.gen18
                });
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "gen-19",
                    MParmDesc = "Enctriptación TSL 1.2 o 1.3 en SMTP",
                    MParmValTxt = model.gen19
                });
                #endregion

                #region parametros empresa

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-01",
                    MParmDesc = "Solicitudes Automáticas",
                    MParmValTxt = model.emp01
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-02",
                    MParmDesc = "Diasatras",
                    MParmValNum = int.Parse(model.emp02)
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-03",
                    MParmDesc = "Diasbajar",
                    MParmValNum = int.Parse(model.emp03)
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-04",
                    MParmDesc = "Ruta Archivos",
                    MParmValTxt = _security.Encriptar(model.emp04, model.gen10)
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-05",
                    MParmDesc = "Ruta ZIP",
                    MParmValTxt = _security.Encriptar(model.emp05, model.gen10)
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-06",
                    MParmDesc = "Ruta XML",
                    MParmValTxt = _security.Encriptar(model.emp06, model.gen10)
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-07",
                    MParmDesc = "Guardar contenido del CFDI descargado",
                    MParmValTxt = model.emp07
                });
                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-08",
                    MParmDesc = "Tipo comprobante nomina",
                    MParmValTxt = model.emp08
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-09",
                    MParmDesc = "Tipo comprobante pagos",
                    MParmValTxt = model.emp09
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-10",
                    MParmDesc = "Tipo comprobante ingresos",
                    MParmValTxt = model.emp10
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-11",
                    MParmDesc = "Tipo comprobante egresos",
                    MParmValTxt = model.emp11
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-12",
                    MParmDesc = "Validacion status CFDI dias atras fecha emision",
                    MParmValTxt = model.emp12
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-13",
                    MParmDesc = "Color primario",
                    MParmValTxt = model.emp13
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-14",
                    MParmDesc = "Ruta logo",
                    MParmValTxt = model.emp14
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-15",
                    MParmDesc = "XSL para Ingresos",
                    MParmValTxt = model.emp15
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-16",
                    MParmDesc = "XSL para Egresos",
                    MParmValTxt = model.emp16
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-17",
                    MParmDesc = "XSL para CRP",
                    MParmValTxt = model.emp17
                });

                _context.MasterParms.Add(new MasterParm()
                {
                    MParmId = "emp-18",
                    MParmDesc = "XSL para Nomina",
                    MParmValTxt = model.emp18
                });
                #endregion

                _context.SaveChanges();

                return this.RedirectToAction("Index", "Requisiciones");
            }
            return PartialView("Index", model);
        }

    }
}