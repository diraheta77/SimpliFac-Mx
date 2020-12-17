using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace simplifile.ViewModels.CFDI
{
    public class NuevoPagoViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha de Pago")]
        [DataType(DataType.Date)]
        public DateTime FechaDePago { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Monto de Pago")]
        public decimal MontoDePago { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Referencia de Pago")]
        public string ReferenciaDePago { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "UUID")]
        public string UUID { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "RFC Emisor")]
        public string RFCEmisor { get; set; }

        [Display(Name = "Nombre Emisor")]
        public string NombreEmisor { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "RFC Receptor")]
        public string RFCReceptor { get; set; }

        [Display(Name = "Nombre Receptor")]
        public string NombreReceptor { get; set; }

        [Display(Name = "Serie")]
        public string Serie { get; set; }

        [Display(Name = "Folio")]
        public string Folio { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha del Comprobante")]
        [DataType(DataType.Date)]
        public DateTime FechaComprobante { get; set; }

        [Display(Name = "Tipo Comprobante")]
        public string TipoComprobante { get; set; }

        [Display(Name = "Forma de Pago")]
        public string FormaPago { get; set; }

        [Display(Name = "Metodo de Pago")]
        public string MetodoPago { get; set; }

        [Display(Name = "Moneda")]
        public string Moneda { get; set; }

        [Display(Name = "Total Impuestos Trasladados")]
        public decimal? ImpuestosTrasladados { get; set; }

        [Display(Name = "Total Impuestos Retenidos")]
        public decimal? ImpuestosRetenidos { get; set; }

        [Display(Name = "SubTotal CFDI")]
        public decimal SubTotal { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Total CFDI")]
        public decimal Total { get; set; }


    }
}
