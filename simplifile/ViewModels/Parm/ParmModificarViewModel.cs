
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.Parm
{
    public class ParmModificarViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "ID Parámetro")]
        [StringLength(10)]
        public string ParmId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Empresa")]
        public string ParmEmpId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Unidad")]
        public string ParmUniId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Descripción de parámetro")]
        [StringLength(100)]
        public string ParmDesc { get; set; }


        [Display(Name = "Valor de tipo texto")]
        public string ParmValTxt { get; set; }

        [Display(Name = "Valor de tipo numérico")]
        public decimal ParmValNum { get; set; }

        public string ParmEmpNombre { get; set; }
        public string ParmUniNombre { get; set; }

        [Display(Name = "Tipo de valor")]
        public int Tipo { get; set; }

        [Display(Name = "Valor de tipo boleano")]
        public string ParmBool { get; set; }

    }
}
