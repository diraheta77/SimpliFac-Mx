
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Parm
{
    public class ParmEliminarViewModel
    {
        [Required(ErrorMessage="Este campo Parámetro ID es obligatorio")]
        public string ParmId { get; set; }

        public string ParmEmpId { get; set; }
    }
}
