
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [EmailAddress(ErrorMessage = "Este debe ser una dirección de correo válida")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

    }
}
