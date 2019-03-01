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
    [Table(Name = "UG_MQ_RIPO", Description = "MQ Registros medios/compras", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class PurchaseOrders : Table
    {
        [Field(Description = "Id de la subida", Type = BoFieldTypes.db_Numeric)]
        public int IdRise { get; set; }

        [Field(Description = "Tipo", Type = BoFieldTypes.db_Numeric)]
        public int Type { get; set; }

        [Field(Description = "Dato maestro de articulo AF o centro de costo de 2 nivel", Type = BoFieldTypes.db_Alpha, Size = 20)]
        public string PrcCode { get; set; }

        [Field(Description = "Nombre del numero economico del activo fijo", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string EcoNum { get; set; }

        [Field(Description = "Cantidad de dieselM", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double DieselM { get; set; }

        [Field(Description = "Cantidad de dieselT", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double DieselT { get; set; }

        [Field(Description = "Cantidad de gas", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double Gas { get; set; }

        [Field(Description = "Cantidad de 15W40", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double F15W40 { get; set; }

        [Field(Description = "Cantidad de hidraulico", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double Hidraulic { get; set; }

        [Field(Description = "Cantidad de SAE40", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double SAE40 { get; set; }

        [Field(Description = "Cantidad de transmision", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double Transmition { get; set; }

        [Field(Description = "Cantidad de aceite", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Quantity)]
        public double Oils { get; set; }
    }
}
