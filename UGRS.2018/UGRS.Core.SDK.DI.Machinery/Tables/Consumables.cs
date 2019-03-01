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
    [Table(Name = "UG_TBL_MQ_RICM", Description = "MQ Solicitudes de consumibles", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class Consumables : Table
    {
        [Field(Description = "Id de la subida", Type = BoFieldTypes.db_Numeric)]
        public int IdRise { get; set; }

        [Field(Description = "DocEntry del documento de Solicitud de traslado", Type = BoFieldTypes.db_Numeric)]
        public int DocEntry { get; set; }
    }
}
