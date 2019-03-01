using UGRS.Core.SDK.DI.Models;
using UGRS.Core.SDK.Attributes;
using SAPbobsCOM;


namespace UGRS.Core.SDK.DI.Transports.Tables
{
    [Table(Name = "UG_TR_DAY", Description = "TR Inicio de año", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class StartDay :Table
    {
        [Field(Description = "Year", Type = BoFieldTypes.db_Numeric)]
        public int Year { get; set; }

        [Field(Description = "Day", Type = BoFieldTypes.db_Numeric)]
        public int FirstDay { get; set; }
    }
}
