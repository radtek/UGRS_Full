using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.Attributes;
using UGRS.Core.SDK.DI.Models;

namespace UGRS.Core.SDK.DI.Finances.Tables
{
    [Table(Name = "UG_FZ_BANKEXTRACTS", Description = "FZ Extractos Bancarios", Type = SAPbobsCOM.BoUTBTableType.bott_NoObject)]
    public class ExtractFormat : Table
    {
    }
}
