
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.MasterParm
{
    public class MasterParmModificarViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "ID Parámetro")]
        [StringLength(10)]
        public string MParmId { get; set; }

        
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Descripción de parámetro")]
        [StringLength(100)]
        public string MParmDesc { get; set; }


        [Display(Name = "Valor de tipo texto")]
        public string MParmValTxt { get; set; }

        [Display(Name = "Valor de tipo numérico")]
        public decimal MParmValNum { get; set; }

        [Display(Name = "Tipo de valor")]
        public int Tipo { get; set; }

        [Display(Name = "Valor de tipo boleano")]
        public string MParmBool { get; set; }

    }
}
