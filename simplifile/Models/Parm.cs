using simplifile.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    public class Parm
    {
        public string ParmId { get; set; }

        public string EmpId { get; set; }

        public string UnId { get; set; }

        public string ParmDesc { get; set; }

        public string ParmValTxt { get; set; }

        public decimal ParmValNum { get; set; }

        public Empresa Empresa { get; set; }

        public int Tipo
        {
            get
            {
                if (ParmValNum > 0) return 1;
                if (!String.IsNullOrEmpty(ParmValTxt) && (ParmValTxt.Contains("true") || ParmValTxt.Contains("false"))) return 2;
                return 0;
            }
        }

    }
}
