using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    public class RequisicionSat
    {
        public int RsId { get; set; }

        public string RsIdRequisicionSAT { get; set; }

        public string EmpId { get; set; }

        public string UnId { get; set; }

        public string RsOrigenCodigo { get; set; }

        public string RsEstatusCodigo { get; set; }


        public string RsMensajeRequisicion { get; set; }

        public DateTime RsFechaRequisicion { get; set; }

        public int RsNumCFDI { get; set; }

        public string RsEstatusRequisicion { get; set; }

        public string RsIdPaquete { get; set; }

        public string RsMensajeCheck { get; set; }

        public string RsEstatusDescarga { get; set; }

        public DateTime RsFechaModificacion { get; set; }

        public DateTime RsFechaInfoInicio { get; set; }

        public string RsRFCRequisicion { get; set; }

        public DateTime RsFechaInfoFinal { get; set; }

        public string RsTipoRequisicion { get; set; }
        public string RsTipoComprobante { get; set; }
        public string RsTipoSolicitante { get; set; }

        public Empresa Empresa { get; set; }

        public UnidadNegocio UnidadNegocio { get; set; }
    }
}
