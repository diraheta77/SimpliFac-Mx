
using Microsoft.AspNetCore.Mvc;
using simplifile.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Empresa
{
    public class EmpresaInsertarViewModel
    {
        [Required(ErrorMessage="Este campo es obligatorio")]
        [Display(Name = "ID Empresa")]
        [StringLength(5)]
        public string EmpId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Razon social")]
        [StringLength(100)]
        public string EmpRazonSocial { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [RegularExpression(@"^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])([A-Z]|[0-9]){2}([A]|[0-9]){1})?$", ErrorMessage = "El RFC debe tener un formato correcto")]
        [Display(Name = "Rfc")]
        [StringLength(13)]
        public string EmpRFC { get; set; }

    }
}
