using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using simplifile.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using simplifile.Helpers;
using simplifile.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Xml.Xsl;
using SelectPdf;
using ClosedXML.Excel;
using System.Net;
using System.Buffers.Text;
using Microsoft.Extensions.Caching.Memory;
using ICSharpCode.SharpZipLib.Zip;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Core;
using simplifile.Services;
using simplifile.Helpers;


namespace simplifile.Helpers
{
    public class FilesTransform
    {

        #region genericos
        public static byte[] generatePDF(string uuid, string pathStyle, ApplicationDbContext _context, IWebHostEnvironment _environment)
        {
            List<string> rutasDelete = new List<string>();
            CFDIDatos datos = new CFDIDatos();
            datos = _context.CFDIs.FirstOrDefault(d => d.CfdUUID == uuid);

            var XSLpath = Path.Combine(_environment.WebRootPath, pathStyle);
            //var XSLpath = Path.Combine(_environment.WebRootPath, "images", "CFDI33_Pagos_Nomina.xsl");
            var XMLpath = Path.Combine(_environment.WebRootPath, "temp", uuid + ".xml");//eliminar

            rutasDelete.Add(XMLpath);

            using (FileStream fs = System.IO.File.Create(XMLpath))
            {
                fs.Write(datos.CfdXml, 0, datos.CfdXml.Length);
            }

            var pathPDF = Path.Combine(_environment.WebRootPath, "temp", "CFDI-" + uuid + ".pdf");//eliminar
            string resultname = "CFDI-" + uuid + ".html";
            var result = Path.Combine(_environment.WebRootPath, "temp", resultname);//eliminar
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(XSLpath);
            xslt.Transform(XMLpath, result);

            rutasDelete.Add(result);

            byte[] buffer = null;
            using (FileStream fs = new FileStream(result, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            string contenido = System.IO.File.ReadAllText(result);
            var textReader = new StringReader(contenido);

            // instantiate the html to pdf converter
            HtmlToPdf converter = new HtmlToPdf();
            // convert the url to pdf
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(contenido);
            byte[] pdf = null;
            // save pdf document
            doc.Save(pathPDF);
            // close pdf document
            doc.Close();

            using (FileStream documentStream = new FileStream(pathPDF, FileMode.Open, FileAccess.Read))
            {
                pdf = new byte[documentStream.Length];
                documentStream.Read(pdf, 0, (int)documentStream.Length);
            }
            return pdf;
        }

        public static byte[] generateHTML(string uuid, string pathStyle, ApplicationDbContext _context, IWebHostEnvironment _environment)
        {
            CFDIDatos datos = new CFDIDatos();
            datos = _context.CFDIs.FirstOrDefault(d => d.CfdUUID == uuid);

            var XSLpath = Path.Combine(_environment.WebRootPath, pathStyle);
            //var XSLpath = Path.Combine(_environment.WebRootPath, "images", "CFDI33_Pagos_Nomina.xsl");
            var XMLpath = Path.Combine(_environment.WebRootPath, "images", uuid + ".xml");

            using (FileStream fs = System.IO.File.Create(XMLpath))
            {
                fs.Write(datos.CfdXml, 0, datos.CfdXml.Length);
            }

            string resultname = "CFDI-" + uuid + ".html";
            var result = Path.Combine(_environment.WebRootPath, "images", resultname);
            XslCompiledTransform xslt = new XslCompiledTransform();
            xslt.Load(XSLpath);
            xslt.Transform(XMLpath, result);

            byte[] buffer = null;
            using (FileStream fs = new FileStream(result, FileMode.Open, FileAccess.Read))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, (int)fs.Length);
            }
            return buffer;
        }

        public static byte[] GetZIPStream(List<FileBasic> lista, string passtoZIP)
        {
            MemoryStream outputMemStream = new MemoryStream();
            ZipOutputStream zipStream = new ZipOutputStream(outputMemStream);

            //string password = Guid.NewGuid().ToString("d").Substring(1, 6);
            zipStream.Password = passtoZIP;
            zipStream.SetLevel(5); //0-9, 9 being the highest level of compression

            foreach (var item in lista)
            {
                Stream stream = new MemoryStream(item.FileStream);
                ZipEntry zipEntry = new ZipEntry(item.filename + "." + item.extension);
                zipEntry.DateTime = DateTime.Now;
                zipStream.PutNextEntry(zipEntry);
                StreamUtils.Copy(stream, zipStream, item.FileStream);
                zipStream.CloseEntry();
            }
            zipStream.IsStreamOwner = false;
            zipStream.Close();

            outputMemStream.Position = 0;
            byte[] byteArray = outputMemStream.ToArray();

            return byteArray;
        }
        #endregion 
    }
}
