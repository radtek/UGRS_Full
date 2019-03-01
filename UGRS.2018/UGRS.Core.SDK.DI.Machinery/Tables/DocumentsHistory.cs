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
    [Table(Name = "UG_MQ_RIOP", Description = "MQ Historial de subidas", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class DocumentsHistory : Table
    {
        [Field(Description = "Id de la subida", Type = BoFieldTypes.db_Numeric)]
        public int IdRise { get; set; }

        [Field(Description = "Fecha de la operacion", Type = BoFieldTypes.db_Date)]
        public DateTime CreatedDate { get; set; }

        [Field(Description = "Id del usuario conectado a sap", Type = BoFieldTypes.db_Numeric)]
        public int UserId { get; set; }

        [Field(Description = "Comentarios", Type = BoFieldTypes.db_Alpha)]
        public string Comments { get; set; }
    }
}
