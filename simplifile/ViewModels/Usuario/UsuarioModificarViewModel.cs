
using Microsoft.AspNetCore.Mvc;
using simplifile.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Usuario
{
    public class UsuarioModificarViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Id")]
        public int UsuId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Correo")]
        [StringLength(256)]
        public string UsuCorreo { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un rol para el usuario")]
        [Display(Name = "Rol")]
        public string UsuRol { get; set; }

        [Display(Name = "Teléfono")]
        public string UsuTelefono { get; set; }

        [Display(Name = "Empresa")]
        public string UsuEmpId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nombre")]
        public string UsuNombre1 { get; set; }

        [Display(Name = "Segundo nombre")]
        public string UsuNombre2 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Apellido paterno")]
        public string UsuPaterno { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Apellido materno")]
        public string UsuMaterno { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Estatus")]
        public int UsuEstatus { get; set; }

    }
}
