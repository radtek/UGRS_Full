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
    [Table(Name = "UG_TBL_MQ_RIEM", Description = "MQ Operadores", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class Employees : Table
    {
        [Field(Description = "Id de la subida", Type = BoFieldTypes.db_Numeric)]
        public int IdRise { get; set; }

        [Field(Description = "Id del dato maestro de empleado", Type = BoFieldTypes.db_Numeric)]
        public int Employee { get; set; }

        [Field(Description = "Indica si esta activo o inactivo", Type = BoFieldTypes.db_Numeric)]
        public int Status { get; set; }
    }
}
