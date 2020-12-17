using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace simplifile.Models
{
    public class CFDIPagados
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(40)]
        public string CfdUUID { get; set; }

        [StringLength(50)]
        public string CfdSerie { get; set; }

        [StringLength(50)]
        public string CfdFolio { get; set; }

        public DateTime CfdFechaEmision { get; set; }

        [StringLength(10)]
        public string CfdTipoComprobante { get; set; }

        [StringLength(10)]
        public string CfdFormaPago { get; set; }

        [StringLength(10)]
        public string CfdMetodoPago { get; set; }

        [StringLength(20)]
        public string CfdMoneda { get; set; }

        [Required]
        [StringLength(13)]
        public string CfdRFCEmisor { get; set; }

        [StringLength(200)]
        public string CfdNombreEmisor { get; set; }

        [Required]
        [StringLength(13)]
        public string CfdRFCReceptor { get; set; }

        [StringLength(200)]
        public string CfdNombreReceptor { get; set; }

        public decimal? CfdTotalImpuestosRetenidos { get; set; }

        public decimal? CfdTotalImpuestosTrasladados { get; set; }

        public decimal? CfdSubtotalCFDI { get; set; }

        public decimal CfdTotalCFDI { get; set; }

        public decimal CfdMontoPagado { get; set; }

        public DateTime CfdFechaPago { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(200)]
        public string CfdReferenciaPago { get; set; }

        [NotMapped]
        public string CFDIStatus { get; set; }

        [NotMapped]
        public string Existe { get; set; }

        [NotMapped]
        public List<CRPDatos> Detalle { get; set; }

        //[NotMapped]
        //public List<CFDIDetalle> Detail { get; set; }

    }
}
