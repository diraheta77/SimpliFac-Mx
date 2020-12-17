
using Microsoft.AspNetCore.Mvc;
using simplifile.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace simplifile.ViewModels.Instalar
{
    public class InstalarViewModel
    {
        //[Required(ErrorMessage = "Este campo es obligatorio")]
        //[Display(Name = "Servidor")]
        //public string Servidor { get; set; }


        //[Required(ErrorMessage = "Este campo es obligatorio")]
        //[Display(Name = "Base de datos")]
        //public string BaseDatos { get; set; }

        //[Required(ErrorMessage = "Este campo es obligatorio")]
        //[Display(Name = "Usuario Base de datos")]
        //public string Usuario { get; set; }

        //[Required(ErrorMessage = "Este campo es obligatorio")]
        //[Display(Name = "Contraseña BD")]
        //public string Password { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Correo del administrador")]
        public string EmailAdmin { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Contraseña del administrador")]
        public string EmailPassword { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Confirmar contraseña")]
        [Compare("EmailPassword", ErrorMessage = "La contraseña y su confirmación son diferentes.")]
        public string EmailPasswordConfirmation { get; set; }

        #region "Parametros"

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Mantenimiento")]
        public string gen01 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Url de página principal")]
        public string gen02 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Ruta Job de Java")]
        public string gen03 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Servidor SMPT")]
        public string gen04 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Usuario SMPT")]
        public string gen05 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Contraseña SMPT")]
        public string gen06 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Puerto SMPT")]
        public string gen07 { get; set; }


        [Display(Name = "Comunicado")]
        public string gen08 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nuevas empresas")]
        public string gen09 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "LLave encriptación")]
        public string gen10 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Usar SSL para SMPT")]
        public string gen11 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Dirección de correo que se muestra en el envio de notificaciones")]
        public string gen12 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nombre que se muestra en el envio de notificaciones")]
        public string gen13 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Hora de repeticion para descargar listas negras.")]
        public string gen14 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Hora de repeticion para validar CFDI vs Listas negras.")]
        public string gen15 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Hora de repeticion para validar el status de los CFDI.")]
        public string gen16 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Hora de repeticion para descargar CFDI.")]
        public string gen17 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Activar o Desactivar las Notificaciones")]
        public string gen18 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Enctriptación TSL 1.2 o 1.3 en SMTP")]
        public string gen19 { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Solicitudes Automáticas")]
        public string emp01 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "Debe especificar un valor numérico mayor o igual que {1}")]
        [Display(Name = "No. de días hacia atras en que finalizará el calculo de la fecha final para la creación de solicitudes (diasatras)")]
        public string emp02 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "Debe especificar un valor numérico mayor o igual que {1}")]
        [Display(Name = "No. de días hacia atras en a partir del valor de emp-02 para calcular la fecha inicial para la creación de solicitudes (diasbajar)")]
        public string emp03 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Ruta Archivos")]
        public string emp04 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Ruta ZIP")]
        public string emp05 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Ruta XML")]
        public string emp06 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Guardar contenido del CFDI descargado")]
        public string emp07 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Tipo comprobante nomina")]
        public string emp08 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Tipo comprobante pagos")]
        public string emp09 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Tipo comprobante ingresos")]
        public string emp10 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Tipo comprobante egresos")]
        public string emp11 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Validacion status CFDI dias atras fecha emision")]
        public string emp12 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Color primario")]
        public string emp13 { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Ruta logo")]
        public string emp14 { get; set; }

        [Display(Name = "Ruta XSL para Ingresos")]
        public string emp15 { get; set; }

        [Display(Name = "Ruta XSL para Egresos")]
        public string emp16 { get; set; }

        [Display(Name = "Ruta XSL para CRP")]
        public string emp17 { get; set; }

        [Display(Name = "Ruta XSL para Nomina")]
        public string emp18 { get; set; }

        #endregion

    }
}
