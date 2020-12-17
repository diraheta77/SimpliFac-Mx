using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    public class Certificado
    {
        public int CerId { get; set; }

        public string UnId { get; set; }

        public string EmpId { get; set; }

        public string CerArchivoCer { get; set; }

        public string CerArchivoKey { get; set; }

        public string CerContrasena { get; set; }

        public string CerRFC { get; set; }

        public DateTime CerFechaInicio { get; set; }

        public DateTime CerFechaFin { get; set; }

        public string CerArchivoPFX { get; set; }

        public int CerEstatus { get; set; }

        public UnidadNegocio UnidadNegocio { get; set; }


    }
}
