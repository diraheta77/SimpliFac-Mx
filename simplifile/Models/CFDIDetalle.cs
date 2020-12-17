using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simplifile.Models
{
    [Table("CFDIDetalle")]
    public partial class CFDIDetalle
    {
        public long CfdiDetalleId { get; set; }

        [StringLength(40)]
        public string DetCfdUUID { get; set; }

        [StringLength(20)]
        public string DetClaveProdServ { get; set; }

        [StringLength(100)]
        public string DetNoIdentificacion { get; set; }

        [StringLength(20)]
        public string DetClaveUnidad { get; set; }

        [StringLength(20)]
        public string DetUnidad { get; set; }

        [StringLength(20)]
        public string DetCantidad { get; set; }

        [StringLength(200)]
        public string DetDescripcion { get; set; }

        [StringLength(20)]
        public string DetValorUnitario { get; set; }

        [StringLength(20)]
        public string DetImporte { get; set; }

        [StringLength(20)]
        public string DetDescuento { get; set; }

        [StringLength(20)]
        public string DetTrasladoBase { get; set; }

        [StringLength(20)]
        public string DetTrasladoImpuesto { get; set; }

        [StringLength(20)]
        public string DetTrasladoTasaOCuota { get; set; }

        [StringLength(20)]
        public string DetTrasladoImporte { get; set; }

        [StringLength(20)]
        public string DetRetencionBase { get; set; }

        [StringLength(20)]
        public string DetRetencionImpuesto { get; set; }

        [StringLength(20)]
        public string DetRetencionTasaOCuota { get; set; }

        [StringLength(20)]
        public string DetRetencionImporte { get; set; }

    }

}
