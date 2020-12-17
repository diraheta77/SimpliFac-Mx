using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    public class UnidadNegocio
    {
        [Key]
        public string UnId { get; set; }

        public string EmpId { get; set; }

        public string UnRazonSocial { get; set; }

        public string UnRFC { get; set; }

        public int UnEstatus { get; set; } //1 = Activo, 0 = Inactivo, 2 = Pendiente de Carga de Cer en Portal

        public Empresa Empresa { get; set; }

        public ICollection<Certificado> Certificados { get; set; }

        public ICollection<RequisicionSat> Requisiciones { get; set; }
    }
}
