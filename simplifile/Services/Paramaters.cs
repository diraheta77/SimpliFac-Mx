using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration.UserSecrets;
using simplifile.Data;
using simplifile.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Services
{
   
    public interface IParamaters
    {
        string getGenParam(string key,string defValue);

        string getEmpParam(string key,string emp,string defValue);

        void AddGen(MasterParm parm);

        bool MustEncript(string parmId);

        bool mustInstall();

        SmtpOptions smtp();
    }

    public class Paramaters : IParamaters
    {
        private ApplicationDbContext _context;
        
        public Paramaters(ApplicationDbContext context)
        {
            _context = context;
        }

        public string getGenParam(string key, string defValue)
        {
            var parm = _context.MasterParms.FirstOrDefault(f => f.MParmId == key);
            if (parm != null)
            {
                if (!string.IsNullOrEmpty(parm.MParmValTxt)) return parm.MParmValTxt;
                return parm.MParmValNum.ToString();
            }
            return defValue;
        }

        public string getEmpParam(string key, string emp, string defValue)
        {
            var parm = _context.Parms.FirstOrDefault(f => f.ParmId == key && f.EmpId == emp);
            if (parm != null)
            {
                if (!string.IsNullOrEmpty(parm.ParmValTxt)) return parm.ParmValTxt;
                return parm.ParmValNum.ToString();
            }
            var gParm = _context.MasterParms.FirstOrDefault(f => f.MParmId == key );
            {
                if (!string.IsNullOrEmpty(gParm.MParmValTxt)) return gParm.MParmValTxt;
                return gParm.MParmValNum.ToString();
            }
            return defValue;
        }

        public SmtpOptions smtp()
        {
            var options = new SmtpOptions()
            {
                smtpUserName = this.getGenParam(ParamsKeys.G05,""),
                smtpPassword = this.getGenParam(ParamsKeys.G06, ""),
                smtpHost= this.getGenParam(ParamsKeys.G04, ""),
                smtpPort= int.Parse(this.getGenParam(ParamsKeys.G07, "0")),
                smtpSSL= bool.Parse(this.getGenParam(ParamsKeys.G11, "false")),
                fromEmail= this.getGenParam(ParamsKeys.G12, "admin@simplifile.com"),
                fromFullName= this.getGenParam(ParamsKeys.G13, "Admin Simplifile")
                //IsDefault= true
            };

            return options;
        }

        public bool mustInstall()
        {
            return _context.Usuarios.Count(f => f.UsuRol == "admin") == 0;
        }

        public void AddGen(MasterParm parm)
        {
            _context.MasterParms.Add(parm);
        }

        public bool MustEncript(string parmId)
        {
            return parmId == ParamsKeys.E04 || parmId == ParamsKeys.E05 || parmId == ParamsKeys.E06;
        }
    }

    public class SmtpOptions
    {
        public string smtpUserName { get; set; }
        public string smtpPassword { get; set; }
        public string smtpHost { get; set; }
        public int smtpPort { get; set; }
        public bool smtpSSL { get; set; }
        public string fromEmail { get; set; }
        public string fromFullName { get; set; }
        public bool IsDefault { get; set; }

    }

    public class ParamsKeys
    {
        public static string G01 = "gen-01";//Mantenimiento
        public static string G02 = "gen-02";//URL Main Page
        public static string G03 = "gen-03";//Ruta Job de Java
        public static string G04 = "gen-04";//Smtp Host
        public static string G05 = "gen-05";//Smtp Usuario
        public static string G06 = "gen-06";//Smtp Password
        public static string G07 = "gen-07";//Smtp Puerto
        public static string G08 = "gen-08";//Comunicado
        public static string G09 = "gen-09";//Nuevas Empresas
        public static string G10 = "gen-10";//LLave encriptación
        public static string G11 = "gen-11";//Smtp SSL
        public static string G12 = "gen-12";//SMTP From Email
        public static string G13 = "gen-13";//
        public static string G14 = "gen-14";//
        public static string G15 = "gen-15";//
        public static string G16 = "gen-16";//
        public static string G17 = "gen-17";//

        public static string E01 = "emp-01";//Solicitudes Automáticas
        public static string E02 = "emp-02";//No. de días hacia atras en que finalizará el calculo de la fecha final para la creación de solicitudes (diasatras)
        public static string E03 = "emp-03";//No. de días hacia atras en a partir del valor de emp-02 para calcular la fecha inicial para la creación de solicitudes (diasbajar)
        public static string E04 = "emp-04";//Ruta Archivos
        public static string E05 = "emp-05";//Ruta ZIP
        public static string E06 = "emp-06";//Ruta XML
        public static string E07 = "emp-07";
        public static string E08 = "emp-08";
        public static string E09 = "emp-09";
        public static string E10 = "emp-10";
        public static string E11 = "emp-11";
        public static string E12 = "emp-12";
        public static string E13 = "emp-13";//color primario del sistema..
        public static string E14 = "emp-14";//ruta del logo del sistema..
        public static string E15 = "emp-15";//XSL para ingresos..
        public static string E16 = "emp-16";//XSL para egresos..
        public static string E17 = "emp-17";//XSL para CRP..
        public static string E18 = "emp-18";//XSL para nomina..
    }

}
