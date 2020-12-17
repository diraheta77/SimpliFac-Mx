
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Account
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage="Este campo es obligatorio")]
        [EmailAddress(ErrorMessage = "Este debe ser una dirección de correo válida")]
        [Display(Name = "Correo")]
        public string UsuCorreo { get; set; }

        [Display(Name = "Teléfono")]
        public string UsuTelefono { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nombre")]
        public string UsuNombre1 { get; set; }

        [Display(Name = "Segundo Nombre")]
        public string UsuNombre2 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Primer apellido")]
        public string UsuPaterno { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Segundo apellido")]
        public string UsuMaterno { get; set; }

        [Display(Name = "Empresa")]
        public string EmpId { get; set; }

    }
}
