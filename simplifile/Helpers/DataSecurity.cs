using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using simplifile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace simplifile.Helpers
{
    public static class DataSecurity
    {
        public static IEnumerable<Usuario> Filter(this IEnumerable<Usuario> users, ClaimsPrincipal user)
        {
            var role = user.FindFirst("Role").Value;
            switch (role)
            {
                case "admin":
                    return users;
                case "manager":
                    var emp = user.FindFirst("EmpId").Value;
                    return users.Where(f=>f.UsuEmpId == emp && f.UsuRol == "client");
                case "client":
                    return new List<Usuario>() { };
            }
            return new List<Usuario>() { };
        }

        public static IEnumerable<Empresa> Filter(this IEnumerable<Empresa> empresas, ClaimsPrincipal user)
        {
            var role = user.FindFirstValue("Role") ?? "";
            var emp = user.FindFirstValue("EmpId") ?? "";
            
            switch (role)
            {
                case "admin":
                    return empresas;
                case "manager":
                    return empresas.Where(f => f.EmpId == emp);
                case "client":
                    return empresas.Where(f => f.EmpId == emp);
                case "nomina":
                    return empresas.Where(f => f.EmpId == emp);
            }
            return new List<Empresa>() { };
        }

        public static IEnumerable<UnidadNegocio> Filter(this IEnumerable<UnidadNegocio> unidadNegocios, ClaimsPrincipal user)
        {
            var role = user.FindFirst("Role").Value;
            var emp = user.FindFirst("EmpId").Value;
            switch (role)
            {
                case "admin":
                    return unidadNegocios;
                case "manager":
                    return unidadNegocios.Where(f => f.EmpId == emp);
                case "client":
                    return unidadNegocios.Where(f => f.EmpId == emp);
                case "nomina":
                    return unidadNegocios.Where(f => f.EmpId == emp);
            }
            return new List<UnidadNegocio>() { };
        }

        public static IEnumerable<RequisicionSat> Filter(this IEnumerable<RequisicionSat> requisiciones, ClaimsPrincipal user)
        {
            var role = user.FindFirst("Role").Value;
            var emp = user.FindFirst("EmpId").Value;
            switch (role)
            {
                case "admin":
                    return requisiciones;
                case "manager":
                    return requisiciones.Where(f => f.EmpId == emp);
                case "client":
                    return requisiciones.Where(f => f.EmpId == emp);
                case "nomina":
                    return requisiciones.Where(f => f.EmpId == emp);
            }
            return new List<RequisicionSat>() { };
        }

        public static IEnumerable<Parm> Filter(this IEnumerable<Parm> parms, ClaimsPrincipal user)
        {
            var role = user.FindFirst("Role").Value;
            var emp = user.FindFirst("EmpId").Value;
            switch (role)
            {
                case "admin":
                    return parms;
                case "manager":
                    return parms.Where(f => f.EmpId == emp);
            }
            return new List<Parm>() { };
        }

        public static IEnumerable<MasterParm> Filter(this IEnumerable<MasterParm> parms, ClaimsPrincipal user)
        {
            var role = user.FindFirst("Role").Value;
            var emp = user.FindFirst("EmpId").Value;
            switch (role)
            {
                case "admin":
                    return parms;
            }
            return new List<MasterParm>() { };
        }


        #region cfdis
        public static IEnumerable<CFDIDatos> Filter(this IEnumerable<CFDIDatos> cfdis, ClaimsPrincipal user)
        {
            var role = user.FindFirst("Role").Value;
            var emp = user.FindFirst("EmpId").Value;
            //var rfcempresa = user.getEmpId();
            switch (role)
            {
                case "admin":
                    return cfdis;
                case "manager":
                    return cfdis;
                   // return cfdis.Where(d => d.CfdRFCEmisor == rfcempresa || d.CfdRFCReceptor == rfcempresa).ToList();
                   //case "nomina":
                   //    return cfdis.Where(d => d.CfdTipoComprobante == "P");
            }
            return new List<CFDIDatos>() { };
        }
        #endregion
    }
}
