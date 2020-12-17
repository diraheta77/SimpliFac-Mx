
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.UnidadNegocio
{
    public class UnidadNegocioEliminarViewModel
    {
        [Required(ErrorMessage="Este campo Unidad ID es obligatorio")]
        public string UnId { get; set; }
    }
}
