using UGRS.Core.SDK.DI.Models;
using UGRS.Core.SDK.Attributes;
using SAPbobsCOM;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.Transports.Enums;

namespace UGRS.Core.SDK.DI.Transports.Tables
{
    [Table(Name = "UG_TR_CMSN", Description = "TR Comisiones", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
   public class Commissions :Table
    {
        [Field(Description = "FolioComission", Type = BoFieldTypes.db_Alpha, Size = 30)]
        public string Folio { get; set; }

        [Field(Description = "Importe", Type = BoFieldTypes.db_Numeric)]
        public double Amount { get; set; }

        [Field(Description = "Autorizado Transporte", Size = 1)]
        public bool AutTrans { get; set; }

        [Field(Description = "Autorizado Operaciones", Size = 1)]
        public bool AutOperations { get; set; }

        [Field(Description = "Autorizado Bancos", Size = 1)]
        public bool AutBanks { get; set; }

        [Field(Description = "Estatus", Type=  BoFieldTypes.db_Numeric)]
        public int Status { get; set; }

        public List<CommissionLine> LstCommissionLine;

    }
}
