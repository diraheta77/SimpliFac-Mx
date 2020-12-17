using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using simplifile.Data;
using simplifile.Helpers;
using simplifile.Models;
using simplifile.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Controllers
{
    [Authorize]
    public class NominaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public static IWebHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailSender EmailSender;

        public NominaController(ApplicationDbContext context, IWebHostEnvironment environment, IMemoryCache memoryCache, IEmailSender emailSender)
        {
            _context = context;
            _environment = environment;
            _memoryCache = memoryCache;
            EmailSender = emailSender;
        }

        [HttpPost]
        public async Task<IActionResult> SetOrderColumns(int[] order, eTablesType etype)
        {
            ConfigVistaUsuario config = new ConfigVistaUsuario();
            string optionTableName = Enum.GetName(typeof(eTablesType), etype);
            string ids = String.Join(",", order.Select(p => p.ToString()).ToArray());

            try
            {
                //si debo actualizar
                if (_context.ConfigVistas.Any(d => d.Usuario == User.Identity.Name))
                {
                    config = _context.ConfigVistas.FirstOrDefault(d => d.Usuario == User.Identity.Name);
                    config.TipoTabla = optionTableName;
                    config.Usuario = User.Identity.Name;
                    config.OrderColumns = ids;
                    var result = await _context.SaveChangesAsync();
                }
                //si debo crear un nuevo registro.
                config.TipoTabla = optionTableName;
                config.Usuario = User.Identity.Name;
                config.OrderColumns = ids;
                _context.ConfigVistas.Add(config);
                var r = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
            
            return Ok();
        }


        [HttpGet]
        public ActionResult getordercolumns()
        {
            int[] arreglo = null;
            var config = _context.ConfigVistas.FirstOrDefault(d => d.Usuario == User.Identity.Name);
            if(config.OrderColumns != null)
            {
                return Ok(config.OrderColumns);
            }
            return Ok(arreglo);
        }

        [HttpGet]
        //[Route("GetExcelDocument/{data}")]
        public FileResult GetExcelDocument()
        {
            //List<Nomina> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Nomina>>(_data);
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = "Recibos de Nómina - simplifile.xlsx";

            List<Nomina> data = new List<Nomina>();
            if (_memoryCache.TryGetValue("nomina", out data))
            {
                var count = data.Count;
            }
            

            using(var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("Nomina");
                //headers..
                worksheet.Cell(1, 1).Value = "UUID";
                worksheet.Cell(1, 2).Value = "RFC Emisor";
                worksheet.Cell(1, 3).Value = "Nombre Emisor";
                worksheet.Cell(1, 4).Value = "RFC Receptor";
                worksheet.Cell(1, 5).Value = "Nombre Receptor";
                worksheet.Cell(1, 6).Value = "Receptor Num Empleado";
                worksheet.Cell(1, 7).Value = "Fecha de Pago";
                worksheet.Cell(1, 8).Value = "Fecha Periodo Inicial";
                worksheet.Cell(1, 9).Value = "Fecha Periodo Final";
                worksheet.Cell(1, 10).Value = "Sub Total CFDI";
                worksheet.Cell(1, 11).Value = "Total Percepciones";
                worksheet.Cell(1, 12).Value = "Total Deducciones";
                worksheet.Cell(1, 13).Value = "Total CFDI";

                for (int index = 1; index <= data.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = data[index - 1].CrnUUID;
                    worksheet.Cell(index + 1, 2).Value = data[index - 1].datosCFDI.CfdRFCEmisor;
                    worksheet.Cell(index + 1, 3).Value = data[index - 1].datosCFDI.CfdNombreEmisor;
                    worksheet.Cell(index + 1, 4).Value = data[index - 1].datosCFDI.CfdRFCReceptor;
                    worksheet.Cell(index + 1, 5).Value = data[index - 1].datosCFDI.CfdNombreReceptor;
                    worksheet.Cell(index + 1, 6).Value = data[index - 1].CrnRecepNumEmpleado;
                    worksheet.Cell(index + 1, 7).Value = data[index - 1].CrnFechaPago;
                    worksheet.Cell(index + 1, 8).Value = data[index - 1].CrnFechaInicialPago;
                    worksheet.Cell(index + 1, 9).Value = data[index - 1].CrnFechaFinalPago;
                    worksheet.Cell(index + 1, 10).Value = data[index - 1].datosCFDI.CfdSubtotalCFDI;
                    worksheet.Cell(index + 1, 11).Value = data[index - 1].CrnTotalPercepciones;
                    worksheet.Cell(index + 1, 12).Value = data[index - 1].CrnTotalDeducciones;
                    worksheet.Cell(index + 1, 13).Value = data[index - 1].datosCFDI.CfdTotalCFDI;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
        }




        public IActionResult Index()
        {
            
            var Empresas = new SelectList(_context.Empresas.Filter(User), "EmpId", "EmpRazonSocial");
            ViewBag.Empresas = Empresas;

            return View();
        }

        [Produces("application/json")]
        [Route("api/nomina")]
        public IActionResult Search(string emisor = "", string receptor = "", string fechap = "", string fechai = "", string fechaf = "")
        {
            List<Nomina> nominas = new List<Nomina>();

            string query = "select cn.* from CRNNomina cn inner join CFDIDatos cd ";
            query += " on cn.CrnUUID = cd.CfdUUID where cd.CfdTipoComprobante = 'N'";
            if(!string.IsNullOrEmpty(fechap))
            {
                query += " and cn.CrnFechaPago = convert(char(10),'" + fechap + "', 126)";
            }
            if(!string.IsNullOrEmpty(fechai))
            {
                query += " and cn.CrnFechaInicialPago >= convert(char(10),'" + fechai + "', 126)";
            }
            if (!string.IsNullOrEmpty(fechaf))
            {
                query += " and cn.CrnFechaFinalPago <= convert(char(10),'" + fechaf + "', 126)";
            }

            nominas = _context.Nominas.FromSqlRaw(query).ToList();

            foreach (var item in nominas)
            {
                query = "";
                query = "select c.CfdUUID,  c.CfdSerie, c.CfdFolio, c.CfdVersionCFDI, c.CfdFechaEmision, c.CfdFechaTimbre, ";
                query += "c.CfdTipoComprobante, c.CfdFormaPago, c.CfdMetodoPago, c.CfdNoCertificado, c.CfdMoneda, c.CfdRFCEmisor, ";
                query += "c.CfdNombreEmisor, c.CfdRFCReceptor, c.CfdNombreReceptor, c.CfdTotalImpuestosRetenidos, c.CfdTotalImpuestosTrasladados, ";
                query += "c.CfdSubtotalCFDI, c.CfdTotalCFDI, c.CfdiStatus, c.CrtdOn, c.MdfdOn, null as CfdXml from CFDIDatos c ";
                query += " where c.CfdUUID = '" + item.CrnUUID + "'";
                item.datosCFDI = _context.CFDIs.FromSqlRaw(query).FirstOrDefault();
                query = "";
                query += "select * from CRNNominaDetalle where CrnUUID = '" + item.CrnUUID + "'";
                item.nominaDetalles = _context.NominaDetalles.FromSqlRaw(query).ToList();
            }
            return Ok(nominas);
        }

        [Produces("application/json")]
        [Route("api/nominaDetalle")]
        public IActionResult Detalle(string crnUUID)
        {
            List<NominaDetalle> lista = new List<NominaDetalle>();
            if(!string.IsNullOrEmpty(crnUUID))
            {
                lista = _context.NominaDetalles.Where(d => d.CrnUUID == crnUUID).ToList();
                return Ok(lista);
            }
            return Ok();
        }

        public IActionResult Conciliacion()
        {
            var receptores = _context.ConNominas.Select(d => d.CnnRFCReceptor).Distinct().ToList();
            ViewBag.receptores = receptores;
            return View();
        }

        [Produces("application/json")]
        [Route("api/nomina/conciliacion")]
        public IActionResult DetalleCon()
        {
            List<ConNomina> nominas = new List<ConNomina>();
            //inicio
            nominas = _context.ConNominas.ToList();
            foreach(var dato in nominas)
            {
                if(!_context.CFDIs.Any(d=>d.CfdUUID == dato.CnnUUID))
                {
                    dato.StatusCFDIMsj = "<span class='label label-info'>No Aplica</span>";
                }
                dato.StatusMsj = CreateStatusConNomina(dato.CnnUUID);
            }
            return Ok(nominas);
            //fin



            var cfdis = _context.CFDIs.Where(d => d.CfdTipoComprobante.ToUpper() == "N").ToList();

            

            if (cfdis.Any())
            {
                foreach(var item in cfdis)
                {
                    if(_context.ConNominas.Any(k => k.CnnUUID == item.CfdUUID))
                    {
                        var list = _context.ConNominas.Where(k => k.CnnUUID == item.CfdUUID).ToList();
                        foreach(var nomina in list)
                        {
                            nomina.Diferencia = nomina.CnnTotalCFDI - item.CfdTotalCFDI;
                            nomina.StatusMsj = CreateStatusConNomina(nomina.CnnUUID);
                        }
                        nominas.AddRange(list);
                    }
                    else
                    {
                        var listNO = _context.ConNominas.Where(k => k.CnnUUID != item.CfdUUID).ToList();
                        foreach (var xx in listNO)
                        {
                            xx.StatusMsj = CreateStatusConNomina(xx.CnnUUID);
                            xx.StatusCFDIMsj = "<span class='label label-secondary'>No Aplica</span>";
                        }
                        nominas.AddRange(listNO);
                    }
                }
            }


            //List<Nomina> nominas = new List<Nomina>();
            //foreach(var cfdi in cfdis)
            //{
            //    if(_context.Nominas.Any(k => k.CrnUUID == cfdi.CfdUUID))
            //    {   
            //        var nomina = _context.Nominas.First(k => k.CrnUUID == cfdi.CfdUUID);
            //        nomina.nominaDetalles = _context.NominaDetalles.Where(d => d.CrnUUID == cfdi.CfdUUID).ToList();
            //        nomina.datosCFDI = cfdi;
            //        nominas.Add(nomina);
            //    }
            //}
            return Ok(nominas);
        }

        public string CreateStatusConNomina(string uuid)
        {
            decimal dif = 0;
            string status = "<span class='label label-success'>Existe</span>";

            if(_context.CFDIs.Any(k=>k.CfdUUID == uuid && k.CfdTipoComprobante == "N"))
            {
                var CFDITotal = _context.CFDIs.FirstOrDefault(d => d.CfdUUID == uuid).CfdTotalCFDI;
                var ConNominaTotal = _context.ConNominas.FirstOrDefault(d => d.CnnUUID == uuid).CnnTotalCFDI;
                dif = ConNominaTotal - CFDITotal;
                if(dif == 0)
                {
                    status = "<span class='label label-success'>Existe</span>";
                }
                else
                {
                    status = "<span class='label label-warning'>Diferencia</span>";
                }
            }
            else
            {
                status = "<span class='label label-danger'>No Existe</span>";
            }
            return status;
        }


        #region archivos
        [Route("api/nomina/html/{uuid}")]
        public FileResult GetHTMLFile(string uuid)
        {
            Paramaters paramaters = new Paramaters(_context);
            string pathStyle = paramaters.getEmpParam(simplifile.Services.ParamsKeys.E18, "", "especialfolder\\XSL\\CFDI33.xsl");
            var buffer = FilesTransform.generateHTML(uuid, pathStyle, _context, _environment);
            return new FileContentResult(buffer, "text/html");
            //return File(buffer, "text/html", resultname); //esto si funciona..
        }

        [Route("api/nomina/pdf/{uuid}")]
        public FileResult GetPDF(string uuid)
        {
            Paramaters paramaters = new Paramaters(_context);
            string pathStyle = paramaters.getEmpParam(simplifile.Services.ParamsKeys.E18, "", "especialfolder\\XSL\\CFDI33.xsl");
            var pdf = FilesTransform.generatePDF(uuid, pathStyle, _context, _environment);
            return new FileContentResult(pdf, "application/pdf");
        }

        [HttpGet]
        [Route("api/nomina/xml/zip/{ids}/{algo}")]
        public IActionResult XMLZip(string ids, string algo = "")
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
        [Route("api/nomina/pdf/zip/{ids}/{algo}")]
        public IActionResult PDFZip(string ids, string algo = "")
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
        [Route("api/nomina/zip/{ids}/{algo}")]
        public IActionResult Zip(string ids, string algo = "")
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

        #endregion

    }
}
