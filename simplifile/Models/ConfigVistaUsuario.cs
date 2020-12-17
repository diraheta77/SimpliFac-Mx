using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace simplifile.Models
{
    public enum eTablesType { Requisiciones, CFDI, ConCFDI, Nomina, ConNomina }


    [Table("ConfigVistaUsuario")]
    public class ConfigVistaUsuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Usuario { get; set; }

        [Required]
        [StringLength(64)]
        public string TipoTabla { get; set; }

        [StringLength(128)]
        public string OrderColumns { get; set; }

        [StringLength(128)]
        public string VisibleColumns { get; set; }
    }
}
