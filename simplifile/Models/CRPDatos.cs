using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simplifile.Models
{
    public class CRPDatos
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string CrpUUID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(36)]
        public string CrpIdDocumento { get; set; }

        [StringLength(100)]
        public string CrpSerie { get; set; }

        [StringLength(100)]
        public string CrpFolio { get; set; }

        [StringLength(100)]
        public string CrpMetodoPago { get; set; }

        [StringLength(100)]
        public string CrpMoneda { get; set; }

        [StringLength(100)]
        public string CrpTipoCambio { get; set; }

        [StringLength(100)]
        public string CrpImpSaldoAnt { get; set; }

        [StringLength(100)]
        public string CrpImpSaldoInsoluto { get; set; }

        [StringLength(100)]
        public string CrpImpPagado { get; set; }


        [NotMapped]
        public string RFCEmisor { get; set; }

        [NotMapped]
        public DateTime CRPFecha { get; set; }

        [NotMapped]
        public string Status { get; set; }
    }
}
