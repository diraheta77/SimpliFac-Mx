
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace simplifile.ViewModels.UnidadNegocio
{
    public class UnidadNegocioModificarViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Unidad Id")]
        [StringLength(5)]
        public string UnId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Empresa")]
        public string EmpId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Razon social")]
        [StringLength(100)]
        public string UnRazonSocial { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [RegularExpression(@"^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1])([A-Z]|[0-9]){2}([A]|[0-9]){1})?$", ErrorMessage = "El RFC debe tener un formato correcto")]
        [Display(Name = "Rfc")]
        [StringLength(13)]
        public string UnRFC { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Estatus de unidad")]
        public int UnEstatus { get; set; }

        [Display(Name = "Carga Fiel")]
        public bool CargaFiel { get; set; }

        [Display(Name = "Contraseña fiel")]
        public string PaswordFiel { get; set; }

        [Display(Name = "Certificado fiel")]
        public IFormFile CerFiel { get; set; }

        [Display(Name = "Key fiel")]
        public IFormFile KeyFiel { get; set; }

        public string EmpNombre { get; set; }

    }
}
