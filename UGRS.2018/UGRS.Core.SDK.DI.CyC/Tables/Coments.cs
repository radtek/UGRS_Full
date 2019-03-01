using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.Attributes;
using UGRS.Core.SDK.DI.Models;

namespace UGRS.Core.SDK.DI.CyC.Tables
{
    [Table(Name = "UG_CC_CobroSub", Description = "CC Comentarios subasta", Type = BoUTBTableType.bott_NoObjectAutoIncrement)]
    public class Coments : Table
    {
        [Field(Description = "SU_Folio", Size = 64)]
        public string Folio { get; set; }

        [Field(Description = "User", Size = 64)]
        public string User { get; set; }

        [Field(Description = "Department", Size = 64)]
        public string Department { get; set; }

        [Field(Description = "DepartmentName", Size = 64)]
        public string DepartmentName { get; set; }

        [Field(Description = "Coment", Size = 128)]
        public string Coment { get; set; }

        [Field(Description = "CardCode", Size = 128)]
        public string Cardcocde { get; set; }

        [Field(Description = "CostCenter", Size = 128)]
        public string CostCenter { get; set; }

    }
}
