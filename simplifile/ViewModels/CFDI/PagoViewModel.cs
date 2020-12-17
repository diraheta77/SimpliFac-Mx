using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace simplifile.ViewModels.CFDI
{
    public class PagoViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha de Pago")]
        public DateTime FechaDePago { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Monto de Pago")]
        public decimal MontoDePago { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Referencia de Pago")]
        public string ReferenciaDePago { get; set; }
        public string UUID { get; set; }
    }
}
