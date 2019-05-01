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

        [Field(Description = "Importe", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double Amount { get; set; }

        [Field(Description = "Autorizado Transporte", Size = 1)]
        public bool AutTrans { get; set; }

        [Field(Description = "Autorizado Operaciones", Size = 1)]
        public bool AutOperations { get; set; }

        [Field(Description = "Autorizado Bancos", Size = 1)]
        public bool AutBanks { get; set; }

        [Field(Description = "Estatus", Type=  BoFieldTypes.db_Numeric)]
        public int Status { get; set; }

        [Field(Description = "Usuario",  Type = BoFieldTypes.db_Alpha, Size = 50)]
        public string User { get; set; }

        [Field(Description = "Pago Chofer", Type = BoFieldTypes.db_Alpha, Size = 1)]
        public string HasDriverCms { get; set; }

        [Field(Description = "Comentario", Type = BoFieldTypes.db_Alpha, Size = 50)]
        public string Coments { get; set; }

        [Field(Description = "Año", Type = BoFieldTypes.db_Numeric)]
        public int Year { get; set; }

        [Field(Description = "Semana", Type = BoFieldTypes.db_Numeric)]
        public int Week { get; set; }

        [Field(Description = "PagoLimpieza", Size = 1)]
        public bool PagoLimpieza { get; set; }

        public List<CommissionLine> LstCommissionLine;

    }
}
