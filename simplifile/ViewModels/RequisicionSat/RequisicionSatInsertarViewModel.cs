
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using simplifile.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.RequisicionSat
{
    public class RequisicionSatInsertarViewModel
    {

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Empresa")]
        public string EmpId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Unidad de negocio")]
        public string UnId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha inicial")]
        public string RsFechaInfoInicio { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha final")]
        public string RsFechaInfoFinal { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Figura Unidad de Negocio")]
        public string FiguraUnidadNegocio { get; set; }

        public bool Ingreso { get; set; }
        public bool Egreso { get; set; }
        public bool ComplementoPago { get; set; }
        public bool ReciboNomina { get; set; }

        public DateTime RsFechaInfoInicioDate
        {
            get
            {
                if (String.IsNullOrEmpty(RsFechaInfoInicio)) return DateTime.MinValue;
                return DateTime.ParseExact(RsFechaInfoInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }

        public DateTime RsFechaInfoFinalDate
        {
            get
            {
                if (String.IsNullOrEmpty(RsFechaInfoFinal)) return DateTime.MinValue;
                return DateTime.ParseExact(RsFechaInfoFinal, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
        }

       
    }

   
}
