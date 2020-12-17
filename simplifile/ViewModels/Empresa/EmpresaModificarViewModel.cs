
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Empresa
{
    public class EmpresaModificarViewModel
    {
        [Required(ErrorMessage="Este campo es obligatorio")]
        [Display(Name = "ID Empresa")]
        public string EmpId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Razon social")]
        public string EmpRazonSocial { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Rfc")]
        public string EmpRFC { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Estatus de empresa")]
        public int EmpEstatus { get; set; }

    }
}
