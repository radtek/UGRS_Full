using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.Attributes;
using UGRS.Core.SDK.DI.Models;

namespace UGRS.Core.SDK.DI.Machinery.Tables
{
    [Table(Name = "UG_TBL_MQ_RIDC", Description = "MQ Contratos de subida", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class Contracts : Table
    {
        [Field(Description = "Id de la subida", Type = BoFieldTypes.db_Numeric)]
        public int IdRise { get; set; }

        [Field(Description = "DocEntry del documento de orden de venta", Type = BoFieldTypes.db_Numeric)]
        public int DocEntry { get; set; }
    }
}
