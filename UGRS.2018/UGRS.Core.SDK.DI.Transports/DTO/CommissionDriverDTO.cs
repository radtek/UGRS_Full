
namespace UGRS.Core.SDK.DI.Transports.DTO
{
    public class CommissionDriverDTO
    {

        /// <summary>
        /// Chofer
        /// </summary>
        public string Driver { get; set; }

        /// <summary> 
        /// ChoferId
        /// </summary>
        public string DriverId { get; set; }

        /// <summary> 
        /// Imp Flete
        /// </summary>
        public double FrgAm { get; set; }

        /// <summary> 
        /// Importe Seguro
        /// </summary>
        public double InsAm { get; set; }

        /// <summary> 
        /// Descuento Anterior
        /// </summary>
        public double LstDisc { get; set; }

        /// <summary> 
        /// Descuento Semana
        /// </summary>
        public double WkDisc { get; set; }

        /// <summary> 
        /// Total Descuento
        /// </summary>
        public double TotDisc { get; set; }

        /// <summary> 
        /// Comisión
        /// </summary>
        public double Comm { get; set; }

        /// <summary> 
        /// Total Comision
        /// </summary>
        public double TotComm { get; set; }

        /// <summary> 
        /// Adeudo
        /// </summary>
        public double Doubt { get; set; }

        public string DocEntry { get; set; }


    }
}
