
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Account
{
    public class ForgotViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [EmailAddress(ErrorMessage = "Este debe ser una dirección de correo válida")]
        [Display(Name = "Correo")]
        public string Email { get; set; }

    }
}
