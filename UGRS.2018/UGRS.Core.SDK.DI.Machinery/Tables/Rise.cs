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
    [Table(Name = "UG_TBL_MQ_RISE", Description = "MQ Subida cabecera", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class Rise : Table
    {
        [Field(Description = "Id de la subida", Type = BoFieldTypes.db_Numeric)]
        public int IdRise { get; set; }

        [Field(Description = "Fecha de creacion de la subida", Type = BoFieldTypes.db_Date)]
        public DateTime CreatedDate { get; set; }

        [Field(Description = "Fecha de inicio de la subida", Type = BoFieldTypes.db_Date)]
        public DateTime StartDate { get; set; }

        [Field(Description = "Fecha fin de la subida", Type = BoFieldTypes.db_Date)]
        public DateTime EndDate { get; set; }

        [Field(Description = "Cardcode de socio de negocio", Type = BoFieldTypes.db_Alpha, Size = 20)]
        public string Client { get; set; }

        [Field(Description = "Id dato maestro de empleado", Type = BoFieldTypes.db_Numeric)]
        public int Supervisor { get; set; }

        [Field(Description = "Estatus (Activo, Cerrado, Cancelado)", Type = BoFieldTypes.db_Numeric)]
        public int DocStatus { get; set; }

        [Field(Description = "Id de la subida referenciada", Type = BoFieldTypes.db_Numeric)]
        public int DocRef { get; set; }

        [Field(Description = "Id del usuario conectado a sap", Type = BoFieldTypes.db_Numeric)]
        public int UserId { get; set; }

        [Field(Description = "Comisión", Type = BoFieldTypes.db_Alpha, Size = 1)]
        public string HasCommission { get; set; }

        [Field(Description = "Entrada de consumible", Type = BoFieldTypes.db_Alpha, Size = 1)]
        public string HasStockTransfer { get; set; }

        [Field(Description = "Id de la subida original", Type = BoFieldTypes.db_Numeric)]
        public int OriginalFolio { get; set; }
    }
}
