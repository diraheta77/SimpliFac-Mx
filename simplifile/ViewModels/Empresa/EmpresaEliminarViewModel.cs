
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Empresa
{
    public class EmpresaEliminarViewModel
    {
        [Required(ErrorMessage="Este campo Empresa ID es obligatorio")]
        public string EmpId { get; set; }
    }
}
