using UGRS.Core.SDK.DI.Models;
using UGRS.Core.SDK.Attributes;
using SAPbobsCOM;


namespace UGRS.Core.SDK.DI.Transports.Tables
{
    [Table(Name = "UG_TR_CMLN", Description = "TR Comisiones Detalle", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class CommissionLine :Table
    {
        [Field(Description = "CommissionId", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string CommisionId { get; set; }

        [Field(Description = "FolioComission", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string Folio { get; set; }

        [Field(Description = "Chofer", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string DriverId { get; set; }

        [Field(Description = "DocEntry", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string DocEntry { get; set; }

        [Field(Description = "Amount", Type = BoFieldTypes.db_Numeric)]
        public double Amount { get; set; }
    }
}
