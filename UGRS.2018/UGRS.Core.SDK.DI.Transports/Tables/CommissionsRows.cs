using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Models;
using UGRS.Core.SDK.Attributes;
using SAPbobsCOM;
using UGRS.Core.SDK.DI.Transports.DTO;

namespace UGRS.Core.SDK.DI.Transports.Tables
{
    [Table(Name = "UG_TR_CMRW", Description = "TR Lineas de commisiones ", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class CommissionsRows : Table
    {
        /// <summary>
        /// Chofer
        /// </summary>
        [Field(Description = "Chofer", Type = BoFieldTypes.db_Alpha, Size = 80)]
        public string Driver { get; set; }

        /// <summary> 
        /// ChoferId
        /// </summary>
        [Field(Description = "ChoferId", Type = BoFieldTypes.db_Alpha, Size = 20)]
        public string DriverId { get; set; }

        /// <summary> 
        /// Imp Flete
        /// </summary>
        [Field(Description = "Imp Flete", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double FrgAm { get; set; }

        /// <summary> 
        /// Importe Seguro
        /// </summary>
        [Field(Description = "Importe Seguro", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double InsAm { get; set; }

        /// <summary> 
        /// Descuento Anterior
        /// </summary>
        [Field(Description = "Descuento Anterior", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double LstDisc { get; set; }

        /// <summary> 
        /// Descuento Semana
        /// </summary>
        [Field(Description = "Descuento Semana", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double WkDisc { get; set; }

        /// <summary> 
        /// Total Descuento
        /// </summary>
        [Field(Description = "Total Descuento", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double TotDisc { get; set; }

        /// <summary> 
        /// Comisión
        /// </summary>
        [Field(Description = "Comisión", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double Comm { get; set; }

        /// <summary> 
        /// Total Comision
        /// </summary>
        [Field(Description = "Total Comision", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double TotComm { get; set; }

        /// <summary> 
        /// Adeudo
        /// </summary>
        [Field(Description = "Adeudo", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double Doubt { get; set; }

        [Field(Description = "Tipo", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string Type { get; set; }

        [Field(Description = "Id", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string Id { get; set; }

        [Field(Description = "Fecha", Type = BoFieldTypes.db_Date)]
        public DateTime DocDate { get; set; }

        [Field(Description = "Folio", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string Folio { get; set; }

        [Field(Description = "No generar", Type = BoFieldTypes.db_Alpha, Size = 1)]
        public bool NoGenerate { get; set; }

    }
}
