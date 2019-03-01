using SAPbobsCOM;
using System;
using UGRS.Core.SDK.Attributes;
using UGRS.Core.SDK.DI.Models;

namespace UGRS.Core.SDK.DI.Purchases.Tables
{
    [Table(Name = "UG_GLO_VODE", Description = "GLO Comprobantes detalle", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class VouchersDetail : Table
    {
        [Field(Description = "NA", Size = 1)]
        public string NA { get; set; }

        [Field(Description = "Line", Size = 64)]
        public string Line { get; set; }

        [Field(Description = "Date", Type = BoFieldTypes.db_Date)]
        public DateTime Date { get; set; }

        [Field(Description = "Tipo", Size = 64)]
        public string Type { get; set; }

        [Field(Description = "CodeVoucher", Size = 64)]
        public string CodeVoucher { get; set; }

        [Field(Description = "DocEntry", Size = 64)]
        public string DocEntry { get; set; }

        [Field(Description = "DocNum", Size = 64)]
        public string DocNum { get; set; }

        [Field(Description = "Proveedor", Size = 64)]
        public string Provider { get; set; }

        [Field(Description = "Estatus", Size = 64)]
        public string Status { get; set; }

        [Field(Description = "Coments", Size = 128)]
        public string Coments { get; set; }

        [Field(Description = "Subtotal", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double Subtotal { get; set; }

        [Field(Description = "IVA", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double IVA { get; set; }

        [Field(Description = "RetIVA", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double RetIVA { get; set; }

        [Field(Description = "ISR", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double ISR { get; set; }

        [Field(Description = "IEPS", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double IEPS { get; set; }

        [Field(Description = "Total", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double Total { get; set; }

        [Field(Description = "Coment", Size = 128)]
        public string Coment { get; set; }

        [Field(Description = "UserCode", Size = 64)]
        public string UserCode { get; set; }

    }
}
