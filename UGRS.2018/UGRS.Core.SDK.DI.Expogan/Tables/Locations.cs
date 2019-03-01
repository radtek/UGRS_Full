using SAPbobsCOM;
using UGRS.Core.SDK.Attributes;
using UGRS.Core.SDK.DI.Models;

namespace UGRS.Core.SDK.DI.Expogan.Tables
{

    [Table(Name = "UG_EX_LOC_CONTRACT", Description = "Expo Locations Contract ", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class Locations : Table
    {
        [Field(Description = "ContractID", Size = 64)]
        public string ContractID { get; set; }

        [Field(Description = "LocalId", Size = 64)]
        public string LocalID { get; set; }

        [Field(Description = "DocEntry PO", Size = 64)]
        public string DocEntryO { get; set; }

        [Field(Description = "Status", Size = 64)]
        public int Status { get; set; }

    }
}
