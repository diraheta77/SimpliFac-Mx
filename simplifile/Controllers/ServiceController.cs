using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simplifile.Data;
using simplifile.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using simplifile.Services;

namespace simplifile.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _context;
        public static IWebHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailSender EmailSender;
        

        public ServiceController(ApplicationDbContext context, IWebHostEnvironment environment, IMemoryCache memoryCache, IEmailSender emailSender)
        {
            _context = context;
            _environment = environment;
            _memoryCache = memoryCache;
            EmailSender = emailSender;
        }


        #region metodosGeneralesAPI
        [Route("api/xml/{uuid}")]
        public FileResult GetXMLFile(string uuid)
        {
            CFDIDatos datos = new CFDIDatos();
            string resultname = "CFDI-" + uuid + ".xml";
            datos = _context.CFDIs.FirstOrDefault(d => d.CfdUUID == uuid);
            return new FileContentResult(datos.CfdXml, "text/xml");
            //return File(datos.CfdXml, "text/xml", uuid.TrimStart().TrimEnd() + ".xml"); //esto si funciona..
        }

        

        #endregion

    }
}
