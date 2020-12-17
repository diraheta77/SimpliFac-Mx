
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Account
{
    public class RegisterViewModel
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
        public int EmpId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y su confirmación son diferentes.")]
        public string ConfirmPassword { get; set; }
    }
}
