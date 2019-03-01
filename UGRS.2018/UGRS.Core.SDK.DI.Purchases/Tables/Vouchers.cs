using SAPbobsCOM;
using System;
using System.Collections.Generic;
using UGRS.Core.SDK.Attributes;
using UGRS.Core.SDK.DI.Models;
using UGRS.Core.SDK.DI.Purchases.DTO;


namespace UGRS.Core.SDK.DI.Purchases.Tables
{
    [Table(Name = "UG_GLO_VOUC", Description = "GLO Comprobantes", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class Vouchers : Table
    {
        [Field(Description = "Folio", Size = 64)]
        public string Folio { get; set; }

        [Field(Description = "Area", Size = 64)]
        public string Area { get; set; }

        [Field(Description = "Employee", Size = 64)]
        public string Employee { get; set; }

        [Field(Description = "Coments", Size = 64)]
        public string Coments { get; set; }

        [Field(Description = "Date", Type = BoFieldTypes.db_Date)]
        public DateTime Date { get; set; }

        [Field(Description = "Total", Type = BoFieldTypes.db_Float, SubType = BoFldSubTypes.st_Price)]
        public double Total { get; set; }

        [Field(Description = "Status", Size = 64)]
        public int Status { get; set; }

        [Field(Description = "UserCode", Size = 64)]
        public string UserCode { get; set; }

        [Field(Description = "CodeMov", Size = 64)]
        public string CodeMov { get; set; }

        [Field(Description = "TypeVoucher", Type = BoFieldTypes.db_Numeric)]
        public int TypeVoucher { get; set; }

        public List<VouchersDetail> LstVouchersDetail;
       
    }
}
