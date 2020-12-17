
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Usuario
{
    public class UsuarioEliminarViewModel
    {
        [Required(ErrorMessage="Este campo Usuario ID es obligatorio")]
        public int UsuId { get; set; }

        public string UsuCorreo { get; set; }
    }
}
