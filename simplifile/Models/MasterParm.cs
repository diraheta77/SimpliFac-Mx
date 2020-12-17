using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    public class MasterParm
    {
        public string MParmId { get; set; }

        public string MParmDesc { get; set; }

        public string MParmValTxt { get; set; }

        public decimal MParmValNum { get; set; }

        public int Tipo
        {
            get
            {
                if (MParmValNum > 0) return 1;
                if (!string.IsNullOrEmpty(MParmValTxt) && (MParmValTxt.Contains("true") || MParmValTxt.Contains("false"))) return 2;
                return 0;
            }
        }

    }
}
