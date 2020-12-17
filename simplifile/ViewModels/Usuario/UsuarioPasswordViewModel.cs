
using Microsoft.AspNetCore.Mvc;
using simplifile.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Usuario
{
    public class UsuarioPasswordViewModel
    {
        [Required(ErrorMessage = "Este campo Usuario ID es obligatorio")]
        public int UsuId { get; set; }

        
        public string UsuCorreo { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 carácteres")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y su confirmación son diferentes.")]
        public string ConfirmPassword { get; set; }
    }
}
