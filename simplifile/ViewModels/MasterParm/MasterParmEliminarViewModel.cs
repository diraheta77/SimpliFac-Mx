
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.MasterParm
{
    public class MasterParmEliminarViewModel
    {
        [Required(ErrorMessage="Este campo Parámetro ID es obligatorio")]
        public string MParmId { get; set; }

    }
}
