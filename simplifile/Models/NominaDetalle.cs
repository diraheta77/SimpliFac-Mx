using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    [Table("CRNNominaDetalle")]
    public class NominaDetalle
    {
        public string CrnUUID { get; set; }
        public string CrnNaturaleza { get; set; }
        public string CrnTipo { get; set; }
        public string CrnClave { get; set; }
        public string CrnConcepto { get; set; }
        public string CrnImporte { get; set; }
        public string CrnImporteGravado { get; set; }
        public string CrnImporteExento { get; set; }
        public Nomina nomina { get; set; }
    }
}
