using UGRS.Core.SDK.DI.Models;
using UGRS.Core.SDK.Attributes;
using SAPbobsCOM;

namespace UGRS.Core.SDK.DI.Transports.Tables
{
    [Table(Name = "UG_TR_TOWN", Description = "TR Lista de municipios", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class TOWN : Table
    {

        [Field(Description = "Municipio", Type = BoFieldTypes.db_Alpha, Size = 100)]
        public string TR_TOWN { get; set; }

        [Field(Description = "Estado", Type = BoFieldTypes.db_Alpha, Size = 4)]
        public string TR_State { get; set; }

      

    }
}
