using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    public class FileBasic
    {
        public byte[] FileStream { get; set; }
        public string filename { get; set; }
        public string extension { get; set; }
    }
}
