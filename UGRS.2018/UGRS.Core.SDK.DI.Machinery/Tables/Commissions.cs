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
    [Table(Name = "UG_MQ_COMMISSIONS", Description = "MQ Comisiones", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class Commissions : Table
    {
        [Field(Description = "Id del empleado", Type = BoFieldTypes.db_Numeric)]
        public int EmployeeId { get; set; }

        [Field(Description = "Cantidad de horas", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double HrsQty { get; set; }

        [Field(Description = "Tarifa", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double Rate { get; set; }

        [Field(Description = "Comision", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double Commission { get; set; }

        [Field(Description = "ImporteFS", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double ImportFS { get; set; }

        [Field(Description = "Adeudo", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double Debit { get; set; }

        [Field(Description = "Total", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double Total { get; set; }

        [Field(Description = "Pendiente", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double Pending { get; set; }

        [Field(Description = "Folio Subida", Type = BoFieldTypes.db_Numeric)]
        public int RiseId { get; set; }

        [Field(Description = "Id Asiento", Type = BoFieldTypes.db_Numeric)]
        public int JournalEntryId { get; set; }

        [Field(Description = "Puesto", Type = BoFieldTypes.db_Alpha, Size = 1)]
        public string Position { get; set; }

        [Field(Description = "Puesto Comision", Type = BoFieldTypes.db_Alpha, Size = 1)]
        public string PositionCs { get; set; }
    }
}
