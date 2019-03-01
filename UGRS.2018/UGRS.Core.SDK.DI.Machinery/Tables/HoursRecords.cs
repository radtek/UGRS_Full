using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Models;
using UGRS.Core.SDK.Attributes;
using SAPbobsCOM;

namespace UGRS.Core.SDK.DI.Machinery.Tables
{
    [Table(Name = "UG_MQ_RIHR", Description = "MQ Registro de horas", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class HoursRecords : Table
    {
        [Field(Description = "Id de la subida", Type = BoFieldTypes.db_Numeric)]
        public int IdRise { get; set; }

        [Field(Description = "Fecha de creacion", Type = BoFieldTypes.db_Date)]
        public DateTime DateHour { get; set; }

        [Field(Description = "DocEntry del documento de orden de venta", Type = BoFieldTypes.db_Numeric)]
        public int DocEntry { get; set; }

        [Field(Description = "Id dato maestro del empleado", Type = BoFieldTypes.db_Numeric)]
        public int Supervisor { get; set; }

        [Field(Description = "Id dato maestro del operador", Type = BoFieldTypes.db_Numeric)]
        public int Operator { get; set; }

        [Field(Description = "Dato maestro de articulo AF o centro de costo de 2 nivel", Type = BoFieldTypes.db_Alpha, Size = 20)]
        public string PrcCode { get; set; }

        [Field(Description = "Nombre del numero economico del activo fijo", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string EcoNum { get; set; }

        [Field(Description = "Cantidad de horas", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double HrFeet { get; set; }

        [Field(Description = "Numero de linea del documento de OV", Type = BoFieldTypes.db_Numeric)]
        public int Section { get; set; }

        [Field(Description = "Cantidad de KmHt", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double KmHt { get; set; }

        [Field(Description = "Cantidad pendiente", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double Pending { get; set; }

        [Field(Description = "Indica si ya esta cerrado el tramo", Type = BoFieldTypes.db_Numeric)]
        public int Close { get; set; }
    }
}
