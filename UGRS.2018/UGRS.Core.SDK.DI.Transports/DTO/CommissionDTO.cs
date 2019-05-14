using System;

namespace UGRS.Core.SDK.DI.Transports.DTO
{
    public class CommissionDTO
    {
        public string DocEntry { get; set; }

        /// <summary>
        /// Fecha
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Folio Factura
        /// </summary>
        public string InvFol { get; set; }

        /// <summary>
        /// Operador
        /// </summary>
        public string OpType { get; set; }

        /// <summary>
        /// Ruta
        /// </summary>
        public string Route { get; set; }

        /// <summary>
        /// Vehiculo
        /// </summary>
        public string Vcle { get; set; }

        /// <summary>
        /// Carga
        /// </summary>
        public string PyId { get; set; }

        /// <summary>
        /// Importe
        /// </summary>
        public double Amnt { get; set; }

        /// <summary>
        /// Seguro
        /// </summary>
        public double Ins { get; set; }

        /// <summary>
        /// Comision
        /// </summary>
        public double Cmsn { get; set; }
    }
}
