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
    [Table(Name = "UG_MQ_RIHT", Description = "MQ Subida cabecera", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class TransitHoursRecords : Table
    {
        [Field(Description = "Id de la subida", Type = BoFieldTypes.db_Numeric)]
        public int IdRise { get; set; }

        [Field(Description = "Dato maestro de articulo AF o centro de costo de 2 nivel", Type = BoFieldTypes.db_Alpha, Size = 20)]
        public string PrcCode { get; set; }

        [Field(Description = "Nombre del numero economico del activo fijo", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string EcoNum { get; set; }

        [Field(Description = "Cantidad de horas", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double Hrs { get; set; }

        [Field(Description = "Id dato maestro del operador", Type = BoFieldTypes.db_Numeric)]
        public int Operator { get; set; }

        [Field(Description = "Id dato maestro del supervisor", Type = BoFieldTypes.db_Numeric)]
        public int Supervisor { get; set; }
    }
}
