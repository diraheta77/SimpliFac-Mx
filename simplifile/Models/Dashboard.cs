using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    public class Dashboard
    {
        public class PieData
        {
            public double value { get; set; }
            public string color { get; set; }
            public string highlight { get; set; }
            public string label { get; set; }
        }
    }
}
