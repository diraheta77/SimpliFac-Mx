using Microsoft.EntityFrameworkCore.Metadata.Internal;
using simplifile.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace simplifile.Services
{
    public interface ISecurity
    {
        dynamic ExtraeDatosCer(string cerPath);

        bool ValidarLLave(string keyPath, string password);

        bool ValidarCerLLave(string cerPath,string keyPath, string password);

        string Encriptar(string text);

        string Encriptar(string text,string llave);

        string DesEncriptar(string text);

        string getFilePath(string empId);

        string getZipPath(string empId);

    }

    public class Security : ISecurity
    {
        public readonly IParamaters _paramaters;


        public Security(IParamaters paramaters)
        {
            _paramaters = paramaters;
        }

        public string Encriptar(string text)
        {
            return this.EncryptData(text, _paramaters.getGenParam(ParamsKeys.G10,""));
        }

        public string DesEncriptar(string text)
        {
            return this.DecryptData(text, _paramaters.getGenParam(ParamsKeys.G10, ""));
        }

        public bool ValidarLLave(string keyPath, string password)
        {
            return OpenSslHelper.ValidKeyPassword(keyPath, password);
        }

        public bool ValidarCerLLave(string cerPath, string keyPath, string password)
        {
            return OpenSslHelper.ValidCerKeyPassword(cerPath, keyPath, password);
        }

        public dynamic ExtraeDatosCer(string cerPath)
        {
            var d = OpenSslHelper.CertificateData(cerPath);
            return new
            {
                FechaFin = d.FinalDate,
                FechaInicio = d.InitDate,
                Rfc = d.Rfc
            };
        }


        private string EncryptData(string plainText,string key)
        {
            MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
            byte[] digestOfPassword = md.ComputeHash(Encoding.UTF8.GetBytes(key));

            byte[] keyBytes = Enumerable.Repeat((byte)0, 24).ToArray();

            Array.Copy(digestOfPassword, keyBytes, 16);

            TripleDESCryptoServiceProvider Tripledes = new TripleDESCryptoServiceProvider();

            byte[] b_input = Encoding.UTF8.GetBytes(plainText);

            MemoryStream tempStream = new MemoryStream();

            CryptoStream encStream = new CryptoStream(tempStream, Tripledes.CreateEncryptor(keyBytes, keyBytes), CryptoStreamMode.Write);
            encStream.Write(b_input, 0, b_input.Length);
            encStream.Close();
            return Convert.ToBase64String(tempStream.ToArray());
        }

        private string DecryptData(string input, string key)
        {
            try
            {
                MD5CryptoServiceProvider md = new MD5CryptoServiceProvider();
                byte[] digestOfPassword = md.ComputeHash(Encoding.UTF8.GetBytes(key));

                byte[] keyBytes = Enumerable.Repeat((byte)0, 24).ToArray();

                Array.Copy(digestOfPassword, keyBytes, 16);

                TripleDESCryptoServiceProvider Tripledes = new TripleDESCryptoServiceProvider();
                byte[] b_input = Convert.FromBase64String(input);

                MemoryStream tempStream = new MemoryStream();

                CryptoStream encStream = new CryptoStream(tempStream, Tripledes.CreateDecryptor(keyBytes, keyBytes), CryptoStreamMode.Write);
                encStream.Write(b_input, 0, b_input.Length);
                encStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(tempStream.ToArray());
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string getFilePath(string empId)
        {
            var p = _paramaters.getEmpParam(ParamsKeys.E04, empId, "");
            return DesEncriptar(p);
        }

        public string getZipPath(string empId)
        {
            var p = _paramaters.getEmpParam(ParamsKeys.E05, empId, "");
            return DesEncriptar(p);
        }

        public string Encriptar(string text, string llave)
        {
            return this.EncryptData(text, llave);
        }
    }
}
