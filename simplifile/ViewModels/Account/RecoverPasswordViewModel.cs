
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Account
{
    public class RecoverPasswordViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [EmailAddress(ErrorMessage = "Este debe ser una dirección de correo válida")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Display(Name = "Key")]
        public string Key { get; set; }

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
