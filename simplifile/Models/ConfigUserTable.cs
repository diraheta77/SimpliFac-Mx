using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    
    public class ConfigUserTable
    {
        public string User { get; set; }
        public int[] OrderColumTable { get; set; }
        public eTablesType TablesType { get; set; }
    }
}
