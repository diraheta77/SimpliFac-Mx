using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    [Table("CFDIDatos")]
    public class CFDIDatos
    {
        public string CfdUUID { get; set; }

        public string CfdSerie { get; set; }
        public string CfdFolio { get; set; }

        [Required]
        public string CfdVersionCFDI { get; set; }

        public DateTime CfdFechaEmision { get; set; }

        public DateTime CfdFechaTimbre { get; set; }

        [Required]
        [StringLength(10)]
        public string CfdTipoComprobante { get; set; }

        [StringLength(10)]
        public string CfdFormaPago { get; set; }

        [StringLength(10)]
        public string CfdMetodoPago { get; set; }

        [Required]
        [StringLength(20)]
        public string CfdNoCertificado { get; set; }

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

        public decimal CfdSubtotalCFDI { get; set; }

        public decimal CfdTotalCFDI { get; set; }

        [StringLength(20)]
        public string CfdiStatus { get; set; }

        public DateTime? CrtdOn { get; set; }

        public DateTime? MdfdOn { get; set; }

        [Column(TypeName = "image")]
        public byte[] CfdXml { get; set; }
    }
}
