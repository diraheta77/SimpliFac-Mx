using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    [Table("ConNomina")]
    public class ConNomina
    {
        //[Key]
        [StringLength(40)]
        public string CnnUUID { get; set; }

        [Required]
        [StringLength(13)]
        public string CnnRFCEmisor { get; set; }

        [StringLength(200)]
        public string CnnNombreEmisor { get; set; }

        [Required]
        [StringLength(13)]
        public string CnnRFCReceptor { get; set; }

        [StringLength(200)]
        public string CnnNombreReceptor { get; set; }

        [StringLength(20)]
        public string CnnRecepNumEmpleado { get; set; }

        [StringLength(20)]
        public string CnnFechaPago { get; set; }

        public decimal CnnSubtotalCFDI { get; set; }

        [StringLength(20)]
        public string CnnTotalPercepciones { get; set; }

        [StringLength(20)]
        public string CnnTotalDeducciones { get; set; }

        public decimal CnnTotalCFDI { get; set; }

        [StringLength(20)]
        public string CnnStatus { get; set; }

        /// Aqui van las propiedades que no estan mapeadas con la base.
        [NotMapped]
        public Nomina NominaHeader { get; set; }

        [NotMapped]
        public decimal Diferencia { get; set; }

        [NotMapped]
        public string StatusMsj { get; set; }

        [NotMapped]
        public string StatusCFDIMsj { get; set; }

        public ConNomina()
        {
            NominaHeader = new Nomina();
            StatusCFDIMsj = "<span class='label label-success'>Activo</span>";
            StatusMsj = "<span class='label label-success'>Existe</span>";
        }
    }
}
