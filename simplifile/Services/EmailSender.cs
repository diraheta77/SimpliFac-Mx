using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using simplifile.Models;
using Microsoft.AspNetCore.Mvc.Routing;

namespace simplifile.Services
{
    public interface IEmailSender
    {
        Task SendResetPassword(string email, string name, string link);
        public Task SendSignupNotification(Usuario usuario, string plainPassword, string urlLogin);
        Task SendPasswordZip(string toEmail, string toName, string password);
    }

    public class EmailSender : IEmailSender
    {

        private IHostingEnvironment _enviroment;
        private IParamaters _params;
        private SmtpOptions _smtpOptions;
        public EmailSender(IParamaters paramaters,IHostingEnvironment enviroment)
        {
            _params = paramaters;
            _enviroment = enviroment;
            _smtpOptions = _params.smtp();
        }

        private string GetHtmlTemplate(string template, Dictionary<string,string> data)
        {
            var templatePath = _enviroment.WebRootPath + Path.DirectorySeparatorChar.ToString() + "emails" + Path.DirectorySeparatorChar.ToString() + template;
            string body = "";
            using (StreamReader SourceReader = System.IO.File.OpenText(templatePath))
            {
                body = SourceReader.ReadToEnd();
            }
            foreach (var p in data)
            {
                body = body.Replace("{" + p.Key + "}", p.Value);
            }
            return body;
        }

        public async Task SendEmailSmtpAsync(string subject, string messageBody, string toEmail, string toFullName)
        {
            var body = messageBody;
            var message = new MailMessage();
            message.To.Add(new MailAddress(toEmail, toFullName));
            message.From = new MailAddress(_smtpOptions.fromEmail, _smtpOptions.fromFullName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = _smtpOptions.smtpUserName,
                    Password = _smtpOptions.smtpPassword
                };
                smtp.Credentials = credential;
                smtp.Host = _smtpOptions.smtpHost;
                smtp.Port = _smtpOptions.smtpPort;
                smtp.EnableSsl = _smtpOptions.smtpSSL;
                await smtp.SendMailAsync(message);
            }
        }

        public Task SendResetPassword(string email,string name, string link)
        {
            var message = this.GetHtmlTemplate("recover-password.html",new Dictionary<string, string>() 
            {
                {"FULLNAME",name },
                {"LINK",link }
            });

            this.SendEmailSmtpAsync("Recuperar contraseña",message,email,email).Wait();
            return Task.CompletedTask;
        }

        public Task SendPasswordZip(string toEmail, string toName, string password)
        {
            var message = this.GetHtmlTemplate("send-password-zip.html", new Dictionary<string, string>()
            {
                {"FULLNAME", toName},
                {"PASSWORD", password }
            });
            string titulo = "Password para grupo de archivos";
            this.SendEmailSmtpAsync(titulo, message, toEmail, toName);
            return Task.CompletedTask;
        }

        public Task SendSignupNotification(Usuario usuario,string plainPassword,string urlLogin)
        {
            var message = this.GetHtmlTemplate("signup-user.html", new Dictionary<string, string>()
            {
                {"FULLNAME",usuario.UsuNombreCompleto },
                {"ROL",usuario.UsuRolDescripcion},
                {"EMAIL",usuario.UsuCorreo },
                {"PHONE",usuario.UsuTelefono },
                {"PASSWORD",plainPassword },
                {"URL", urlLogin }
            });

            this.SendEmailSmtpAsync("Nuevo registro de usuario", message, usuario.UsuCorreo, usuario.UsuNombreCompleto).Wait();
            return Task.CompletedTask;
        }
    }
}
