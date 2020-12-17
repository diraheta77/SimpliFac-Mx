using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    public class Empresa
    {
        [Key]
        public string EmpId { get; set; }
        
        public string EmpRazonSocial { get; set; }
        
        public string EmpRFC { get; set; }
        
        public int EmpEstatus { get; set; } //1 = Activo, 0 = Inactivo		

        public ICollection<UnidadNegocio> UnidadadesNegocios { get; set; } = new List<UnidadNegocio>();

        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

        public ICollection<RequisicionSat> Requisiciones { get; set; }

        public ICollection<Parm> Parms { get; set; }
    }
}
