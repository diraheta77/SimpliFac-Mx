using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Services
{
    public interface IUpload
    {
        bool deleteFile(string file);

        byte[] GetFileZip(string empresa, string rfc, string rsIdSat);

        string uploadCerFile(IFormFile file, string empId,string rfc,string fileName);

        string uploadKeyFile(IFormFile file, string empId, string rfc, string fileName);
    }

    public class Upload : IUpload
    {
        public readonly  IParamaters _paramaters;
        private readonly IHostingEnvironment _environment;
        private readonly ISecurity _security;

        public Upload(IParamaters paramaters, IHostingEnvironment environment,ISecurity security)
        {
            _paramaters = paramaters;
            _environment = environment;
            _security = security;
        }

        public bool deleteFile(string file)
        {
            try
            {
                System.IO.File.Delete(file);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public byte[] GetFileZip(string empresa, string rfc, string rsIdSat)
        {
            var path = Path.Combine(this._environment.WebRootPath, _security.getZipPath(empresa), rfc, "Fuzzy", $"{rsIdSat}.zip");
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return fileBytes;
        }

        public string uploadCerFile(IFormFile file,string empId, string rfc, string fileName)
        {
            string path = Path.Combine(this._environment.WebRootPath, _security.getFilePath(empId), rfc, "Blossom");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return Path.Combine(path, fileName);
        }

        public string uploadKeyFile(IFormFile file, string empId,string rfc, string fileName)
        {
            string path = Path.Combine(this._environment.WebRootPath, _security.getFilePath(empId), rfc, "Buttercup");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }
            return Path.Combine(path, fileName);
        }
    }
}
