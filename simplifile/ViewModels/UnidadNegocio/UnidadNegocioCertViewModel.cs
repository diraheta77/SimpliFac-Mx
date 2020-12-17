
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using simplifile.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.ViewModels.UnidadNegocio
{
    public class UnidadNegocioCertViewModel
    {
        public string KeyPassword { get; set; }
        public string CerPath { get; set; }
        public string KeyPath { get; set; }

    }
}
