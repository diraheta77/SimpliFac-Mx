using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PagedList;
using simplifile.Data;
using simplifile.Helpers;
using simplifile.Models;
using simplifile.Services;
using simplifile.ViewModels.CFDI;
using simplifile.ViewModels.MasterParm;
using simplifile.ViewModels.Parm;
using simplifile.ViewModels.RequisicionSat;

namespace simplifile.Controllers
{
    [Authorize]
    public class CFDIController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IUpload _upload;
        private readonly IMemoryCache _memoryCache;
        public static IWebHostEnvironment _environment;
        private readonly IEmailSender EmailSender;

        public CFDIController(ApplicationDbContext context, IUpload upload, IWebHostEnvironment environment, IEmailSender emailSender)
        {
            _context = context;
            _upload = upload;
            _environment = environment;
            EmailSender = emailSender;
        }


        #region views
        public ActionResult ConsultaCFDI()
        {
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return View();
        }

        public ActionResult FacturasPagadas()
        {
            return View();
        }

        public ActionResult Conciliacion()
        {
            ViewBag.Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            return View();
        }
        #endregion


        #region methods
        [Produces("application/json")]
        [Route("api/cfdi/consulta")]
        public IActionResult Search(string emisor="", string receptor="", string status_prov="", string fechai="", string fechaf="")
        {
            //tipo comprobante I es ingresos (facturas)...
            //var search = _context.CFDIs.Where(d => d.CfdTipoComprobante == "I" || d.CfdTipoComprobante == "E" || d.CfdTipoComprobante == "P").Filter(User).ToList();
            string query = "select c.CfdUUID,  c.CfdSerie, c.CfdFolio, c.CfdVersionCFDI, c.CfdFechaEmision, c.CfdFechaTimbre, ";
            query += "c.CfdTipoComprobante, c.CfdFormaPago, c.CfdMetodoPago, c.CfdNoCertificado, c.CfdMoneda, c.CfdRFCEmisor, ";
            query += "c.CfdNombreEmisor, c.CfdRFCReceptor, c.CfdNombreReceptor, c.CfdTotalImpuestosRetenidos, c.CfdTotalImpuestosTrasladados, ";
            query += "c.CfdSubtotalCFDI, c.CfdTotalCFDI, c.CfdiStatus, c.CrtdOn, c.MdfdOn, null as CfdXml from CFDIDatos c ";
            query += "where c.CfdTipoComprobante in ('I', 'E', 'P')";

            var search = _context.CFDIs.FromSqlRaw(query).Filter(User).ToList();
            //aplicar filtros...
            if (!string.IsNullOrEmpty(emisor))
            {
                search = search.Where(f => f.CfdRFCEmisor == emisor).ToList();
            }
            if (!string.IsNullOrEmpty(receptor))
            {
                search = search.Where(f => f.CfdRFCReceptor == receptor).ToList();
            }
            if (!string.IsNullOrEmpty(status_prov))
            {
                search = search.Where(f => f.CfdiStatus == status_prov).ToList();
            }
            if (!string.IsNullOrEmpty(fechai))
            {
                var fec = DateTime.ParseExact(fechai, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                search = search.Where(f => f.CfdFechaEmision >= fec).ToList();
            }
            if (!string.IsNullOrEmpty(fechaf))
            {
                var fec_ = DateTime.ParseExact(fechaf, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                search = search.Where(f => f.CfdFechaEmision <= fec_).ToList();
            }

            search = search.OrderByDescending(k => k.CfdFechaEmision).ToList();
            return Ok(search);
        }

        [HttpPost]
        public IActionResult AddPago(PagoViewModel pago)
        {
            try
            {
                var cfdi = _context.CFDIs.First(d => d.CfdUUID == pago.UUID);
                if(_context.CFDIPagados.Any(d=>d.CfdUUID==pago.UUID && d.CfdReferenciaPago == pago.ReferenciaDePago))
                {
                    this.ShowError("Ya existe una referencia de pago registrada.");
                    return View("ConsultaCFDI", new PagoViewModel());
                }
                _context.CFDIPagados.Add(new CFDIPagados
                {
                    CfdUUID = pago.UUID,
                    CfdSerie = cfdi.CfdSerie,
                    CfdFolio = cfdi.CfdFolio,
                    CfdFechaEmision = cfdi.CfdFechaEmision,
                    CfdTipoComprobante = cfdi.CfdTipoComprobante,
                    CfdFormaPago = cfdi.CfdFormaPago,
                    CfdMetodoPago = cfdi.CfdMetodoPago,
                    CfdMoneda = cfdi.CfdMoneda,
                    CfdRFCEmisor = cfdi.CfdRFCEmisor,
                    CfdNombreEmisor = cfdi.CfdNombreEmisor,
                    CfdRFCReceptor = cfdi.CfdRFCReceptor,
                    CfdNombreReceptor = cfdi.CfdNombreReceptor,
                    CfdTotalImpuestosRetenidos = cfdi.CfdTotalImpuestosRetenidos,
                    CfdTotalImpuestosTrasladados = cfdi.CfdTotalImpuestosTrasladados,
                    CfdSubtotalCFDI = cfdi.CfdSubtotalCFDI,
                    CfdTotalCFDI = cfdi.CfdTotalCFDI,
                    //mis campos
                    CfdFechaPago = pago.FechaDePago,
                    CfdMontoPagado = pago.MontoDePago,
                    CfdReferenciaPago = pago.ReferenciaDePago
                });
                _context.SaveChanges();
                this.ShowSuccess("Ingreso Exitoso");
                return View("ConsultaCFDI", new PagoViewModel());
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
            return View("ConsultaCFDI");
        }

        [Produces("application/json")]
        [Route("api/cfdi/pagadas")]
        public IActionResult GetFacturasPagadas(string emisor = "", string receptor = "", string fechai = "")
        {
            var result = _context.CFDIPagados.ToList();
            if (!string.IsNullOrEmpty(emisor))
            {
                result = result.Where(d => d.CfdRFCEmisor == emisor).ToList();
            }
            if (!string.IsNullOrEmpty(receptor))
            {
                result = result.Where(d => d.CfdRFCReceptor == receptor).ToList();
            }
            if (!string.IsNullOrEmpty(fechai))
            {
                var fec = DateTime.ParseExact(fechai, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                result = result.Where(d => d.CfdFechaPago >= fec).ToList();
            }
            return Ok(result);
        }

        [Produces("application/json")]
        [Route("api/cfdi/confacturas")]
        public IActionResult confacturas()
        {
            List<CFDIPagados> lista = new List<CFDIPagados>();
            var init = _context.CFDIPagados.ToList();
            foreach(var item in init)
            {
                item.Existe = "<span class='label label-danger'>No Existe</span>";
                if (_context.CFDIs.Any(d => d.CfdUUID == item.CfdUUID))
                {
                    item.Existe = "<span class='label label-success'>Existe</span>";
                    item.CFDIStatus = _context.CFDIs.FirstOrDefault(d => d.CfdUUID == item.CfdUUID).CfdiStatus;
                }
                    
                item.Detalle = _context.CRPDatos.Where(k => k.CrpIdDocumento == item.CfdUUID).ToList();
                foreach(var det in item.Detalle)
                {
                    det.RFCEmisor = item.CfdRFCEmisor;
                    det.CRPFecha = item.CfdFechaPago;
                    det.Status = "<span class='label label-success'>Activo</span>";
                }
                //item.Detail = _context.CFDIDetalles.Where(d => d.DetCfdUUID == item.CfdUUID).ToList();

                lista.Add(item);
            }
            return Ok(lista);
        }


        [HttpPost]
        public IActionResult NewPago(NuevoPagoViewModel pago)
        {
            var cfdi = _context.CFDIs.First(d => d.CfdUUID == pago.UUID);
            if (_context.CFDIPagados.Any(d => d.CfdUUID == pago.UUID && d.CfdReferenciaPago == pago.ReferenciaDePago))
            {
                this.ShowError("Ya existe una referencia de pago registrada.");
                return View("ConsultaCFDI", new PagoViewModel());
            }

            if (ModelState.IsValid)
            {
                _context.CFDIPagados.Add(new CFDIPagados
                {
                    CfdUUID = pago.UUID,
                    CfdSerie = pago.Serie,
                    CfdFolio = pago.Folio,
                    CfdFechaEmision = pago.FechaComprobante,
                    CfdTipoComprobante = pago.TipoComprobante,
                    CfdFormaPago = pago.FormaPago,
                    CfdMetodoPago = pago.MetodoPago,
                    CfdMoneda = pago.Moneda,
                    CfdRFCEmisor = pago.RFCEmisor,
                    CfdNombreEmisor = pago.NombreEmisor,
                    CfdRFCReceptor = pago.RFCReceptor,
                    CfdNombreReceptor = pago.NombreReceptor,
                    CfdTotalImpuestosRetenidos = pago.ImpuestosRetenidos,
                    CfdTotalImpuestosTrasladados = pago.ImpuestosTrasladados,
                    CfdSubtotalCFDI = pago.SubTotal,
                    CfdTotalCFDI = pago.Total,
                    //mis campos
                    CfdFechaPago = pago.FechaDePago,
                    CfdMontoPagado = pago.MontoDePago,
                    CfdReferenciaPago = pago.ReferenciaDePago
                });
                _context.SaveChanges();
                this.ShowSuccess("Ingreso Exitoso");
                return View("FacturasPagadas");
            }
            else
            {
                this.ShowError("Datos no validos.");
                return View("ConsultaCFDI");
            }
        }

        [HttpPost]
        public async Task<IActionResult> uploadTemplateAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return Content("file not selected");

            var result = new StringBuilder();
            int contador = 0;
            List<string> resultados = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                
                while (reader.Peek() >= 0)
                {
                    result.AppendLine(reader.ReadLine());
                    string linea = reader.ReadLine();
                    var pago = FromCsv(linea);
                    if(pago != null)
                    {
                        var nuevo = new CFDIPagados()
                        {
                            CfdUUID = pago.UUID,
                            CfdSerie = pago.Serie,
                            CfdFolio = pago.Folio,
                            CfdFechaEmision = pago.FechaComprobante,
                            CfdTipoComprobante = pago.TipoComprobante,
                            CfdFormaPago = pago.FormaPago,
                            CfdMetodoPago = pago.MetodoPago,
                            CfdMoneda = pago.Moneda,
                            CfdRFCEmisor = pago.RFCEmisor,
                            CfdNombreEmisor = pago.NombreEmisor,
                            CfdRFCReceptor = pago.RFCReceptor,
                            CfdNombreReceptor = pago.NombreReceptor,
                            CfdTotalImpuestosRetenidos = pago.ImpuestosRetenidos,
                            CfdTotalImpuestosTrasladados = pago.ImpuestosTrasladados,
                            CfdSubtotalCFDI = pago.SubTotal,
                            CfdTotalCFDI = pago.Total,
                            //mis campos
                            CfdFechaPago = pago.FechaDePago,
                            CfdMontoPagado = pago.MontoDePago,
                            CfdReferenciaPago = pago.ReferenciaDePago
                        };
                        resultados.Add(ingresarPago(nuevo));
                    }
                    contador++;
                }
            }
            var resumen = new
            {
                Ingresados = resultados.Where(d => d == "ok").Count(),
                Rechazados = resultados.Where(d => d != "ok").Count()
            };
            this.ShowSuccess("Ingreso Exitosos: " + resumen.Ingresados.ToString());
            this.ShowError("Ingreso Rechazados: " + resumen.Rechazados.ToString());
            return View("FacturasPagadas");
            //return Ok(resumen);
        }


        public NuevoPagoViewModel FromCsv(string csvLine)
        {
            Type type = typeof(NuevoPagoViewModel);
            int NumberOfRecords = type.GetProperties().Length;
            NumberOfRecords = NumberOfRecords - 1;

            try
            {
                if (!String.IsNullOrEmpty(csvLine))
                {
                    string[] values = csvLine.Split(';');
                    NuevoPagoViewModel nuevo = new NuevoPagoViewModel();
                    if (values.Length == NumberOfRecords)
                    {
                        var next = new NuevoPagoViewModel()
                        {
                            UUID = values[0],
                            Folio = values[1],
                            Serie = values[2],
                            FechaComprobante = Convert.ToDateTime(values[3]),
                            FormaPago = values[4],
                            MetodoPago = values[5],
                            Moneda = values[6],
                            RFCEmisor = values[7],
                            NombreEmisor = values[8],
                            RFCReceptor = values[9],
                            NombreReceptor = values[10],
                            MontoDePago = Convert.ToDecimal(values[11]),
                            FechaDePago = Convert.ToDateTime(values[12]),
                            ReferenciaDePago = values[13],
                            ImpuestosRetenidos = Convert.ToDecimal(values[14]),
                            ImpuestosTrasladados = Convert.ToDecimal(values[15]),
                            SubTotal = Convert.ToDecimal(values[16]),
                            Total = Convert.ToDecimal(values[17])
                        };
                        return next;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return null;
        }

        public string ingresarPago(CFDIPagados pago)
        {
            string msj = "ok";
            try
            {
                var cfdi = _context.CFDIs.First(d => d.CfdUUID == pago.CfdUUID);
                if (_context.CFDIPagados.Any(d => d.CfdUUID == pago.CfdUUID && d.CfdReferenciaPago == pago.CfdReferenciaPago))
                {
                    msj = "Ya existe una referencia de pago "+ pago.CfdReferenciaPago + " registrada.";
                    return msj;
                }

                _context.CFDIPagados.Add(pago);
                _context.SaveChanges();
                return msj;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion


        #region archivos
        [Route("api/cfdi/html/{uuid}")]
        public FileResult GetHTMLFile(string uuid)
        {
            Paramaters paramaters = new Paramaters(_context);
            string pathStyle = paramaters.getEmpParam(simplifile.Services.ParamsKeys.E17, "", "especialfolder\\XSL\\CFDI33.xsl");
            var buffer = FilesTransform.generateHTML(uuid, pathStyle, _context, _environment);
            return new FileContentResult(buffer, "text/html");
        }

        [Route("api/cfdi/pdf/{uuid}")]
        public FileResult GetPDF(string uuid)
        {
            Paramaters paramaters = new Paramaters(_context);
            string pathStyle = paramaters.getEmpParam(simplifile.Services.ParamsKeys.E17, "", "especialfolder\\XSL\\CFDI33.xsl");
            var pdf = FilesTransform.generatePDF(uuid, pathStyle, _context, _environment);
            return new FileContentResult(pdf, "application/pdf");
        }
        #endregion




        [HttpGet]
        [Route("api/cfdi/xml/zip/{ids}")]
        public IActionResult XMLZip(string ids)
        {
            List<string> lista = new List<string>();
            var separados = ids.Split(',');
            lista = separados.ToList();
            List<FileBasic> sourceFiles = new List<FileBasic>();

            List<CFDIDatos> datos = new List<CFDIDatos>();
            foreach (var uuid in lista)
            {
                CFDIDatos dato = _context.CFDIs.FirstOrDefault(d => d.CfdUUID == uuid);
                if (dato != null && dato.CfdXml != null)
                {
                    datos.Add(dato);
                }
            }

            string extension = "xml";

            foreach (var item in datos)
            {
                sourceFiles.Add(new FileBasic
                {
                    FileStream = item.CfdXml,
                    filename = item.CfdUUID,
                    extension = extension
                });
            }
            string password = Guid.NewGuid().ToString("d").Substring(1, 6);
            byte[] fileBytes2 = FilesTransform.GetZIPStream(sourceFiles, password);
            SendPasswordEmail(User.Identity.Name, User.Identity.Name, password);
            return File(fileBytes2, "application/zip", "Archivos.zip");
        }


        [HttpGet]
        [Route("api/cfdi/pdf/zip/{ids}")]
        public IActionResult PDFZip(string ids)
        {
            Paramaters paramaters = new Paramaters(_context);
            string pathStyle = paramaters.getEmpParam(simplifile.Services.ParamsKeys.E18, "", "");
            List<string> lista = new List<string>();
            var separados = ids.Split(',');
            lista = separados.ToList();
            List<FileBasic> sourceFiles = new List<FileBasic>();

            List<CFDIDatos> datos = new List<CFDIDatos>();
            foreach (var uuid in lista)
            {
                CFDIDatos dato = _context.CFDIs.FirstOrDefault(d => d.CfdUUID == uuid);
                if (dato != null && dato.CfdXml != null)
                {
                    datos.Add(dato);
                }
            }

            string extension = "pdf";

            foreach (var item in datos)
            {
                sourceFiles.Add(new FileBasic
                {
                    FileStream = FilesTransform.generatePDF(item.CfdUUID, pathStyle, _context, _environment),
                    filename = item.CfdUUID,
                    extension = extension
                });
            }
            string password = Guid.NewGuid().ToString("d").Substring(1, 6);
            var response = FilesTransform.GetZIPStream(sourceFiles, password);
            SendPasswordEmail(User.Identity.Name, User.Identity.Name, password);
            return File(response, "application/zip", DateTime.Now.ToShortDateString() + ".zip");
        }

        [HttpGet]
        [Route("api/cfdi/zip/{ids}/")]
        public IActionResult Zip(string ids)
        {
            Paramaters paramaters = new Paramaters(_context);
            string pathStyle = paramaters.getEmpParam(simplifile.Services.ParamsKeys.E18, "", "");
            List<string> lista = new List<string>();
            var separados = ids.Split(',');
            lista = separados.ToList();
            List<FileBasic> sourceFiles = new List<FileBasic>();

            List<CFDIDatos> datos = new List<CFDIDatos>();
            foreach (var uuid in lista)
            {
                CFDIDatos dato = _context.CFDIs.FirstOrDefault(d => d.CfdUUID == uuid);
                if (dato != null && dato.CfdXml != null)
                {
                    datos.Add(dato);
                }
            }
            //los adjunto como xml
            string extension = "xml";
            foreach (var item in datos)
            {
                sourceFiles.Add(new FileBasic
                {
                    FileStream = item.CfdXml,
                    filename = item.CfdUUID,
                    extension = extension
                });
            }
            //los adjunto como pdf...
            extension = "pdf";
            foreach (var item in datos)
            {
                sourceFiles.Add(new FileBasic
                {
                    FileStream = FilesTransform.generatePDF(item.CfdUUID, pathStyle, _context, _environment),
                    filename = item.CfdUUID,
                    extension = extension
                });
            }
            string password = Guid.NewGuid().ToString("d").Substring(1, 6);
            var response = FilesTransform.GetZIPStream(sourceFiles, password);
            SendPasswordEmail(User.Identity.Name, User.Identity.Name, password);
            return File(response, "application/zip", DateTime.Now.ToShortDateString() + ".zip");
        }

        public void SendPasswordEmail(string email, string user, string pass)
        {
            var result = EmailSender.SendPasswordZip(email, user, pass);
        }

    }
}
