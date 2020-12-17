using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    public class Usuario
    {

        public int UsuId { get; set; }

        public string UsuEmpId { get; set; }

        public string UsuTelefono { get; set; }

        public string UsuCorreo { get; set; }

        public string PasswordHash { get; set; }

        public string UsuNombre1 { get; set; }

        public string UsuNombre2 { get; set; }

        public string UsuPaterno { get; set; }

        public string UsuMaterno { get; set; }

        public string UsuRol { get; set; }

        public int UsuEstatus { get; set; }

        public Empresa Empresa { get; set; }

        public string UsuNombreCompleto
        {
            get
            {
                return $"{UsuNombre1} {UsuPaterno}";
            }
        }
        public string UsuRolDescripcion
        {
            get
            {
                switch (this.UsuRol)
                {
                    case "admin":
                        return "Administrador del la solución";
                    case "manager":
                        return "Administrador de empresa";
                }
                return "Usuario funcional";
            }
        }

    }
}
