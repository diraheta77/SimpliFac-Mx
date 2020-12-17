using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace simplifile.Models
{
    [Table("CRNNomina")]
    public class Nomina
    {
        public string CrnUUID { get; set; }
        public string CrnVersion { get; set; }
        public string CrnTipoNomina { get; set; }
        public DateTime CrnFechaPago { get; set; }
        public DateTime CrnFechaInicialPago { get; set; }
        public DateTime CrnFechaFinalPago { get; set; }
        public string CrnTotalPercepciones { get; set; }
        public string CrnTotalDeducciones { get; set; }
        public string CrnEmisorRegistroPatronal { get; set; }
        public string CrnRecepCurp { get; set; }
        public string CrnRecepNumSegSoc { get; set; }
        public DateTime CrnRecepFechaInicioRelLaboral { get; set; }
        public string CrnRecepAntiguedad { get; set; }
        public string CrnRecepTipoContrato { get; set; }
        public string CrnRecepTipoJornada { get; set; }
        public string CrnRecepTipoRegimen { get; set; }
        public string CrnRecepNumEmpleado { get; set; }
        public string CrnRecepPeriodicidadPago { get; set; }
        public string CrnRecepDepartamento { get; set; }
        public string CrnRecepPuesto { get; set; }
        public string CrnRecepRiesgoPuesto { get; set; }
        public string CrnRecepCuentaBancaria { get; set; }
        public string CrnRecepSalarioBaseCotApor { get; set; }
        public string CrnRecepSalarioDiarioIntegrado { get; set; }
        public string CrnRecepClaveEntFed { get; set; }
        public string CrnRecepSubContrRfcLabora { get; set; }
        public string CrnRecepSubContrPorcTiempo { get; set; }
        public string CrnPercepTotalSueldos { get; set; }
        public string CrnPercepTotalGravado { get; set; }
        public string CrnPercepTotalExento { get; set; }
        public string CrnDeducTotalOtrasDeducciones { get; set; }
        public string CrnDeducTotalImpRetenidos { get; set; }
        public IEnumerable<NominaDetalle> nominaDetalles { get; set; }

        [NotMapped]
        public CFDIDatos datosCFDI { get; set; }

        public Nomina()
        {
            datosCFDI = new CFDIDatos();
        }
    }
}
