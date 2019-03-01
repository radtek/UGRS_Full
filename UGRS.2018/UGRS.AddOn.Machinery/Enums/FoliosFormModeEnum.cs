using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.AddOn.Machinery.Enums
{
    public enum FoliosFormModeEnum : int
    {
        [Description("Relacionadas")]
        RelatedRise = 1,
        [Description("Comisiones")]
        Commission = 2,
        [Description("Entrada de consumibles")]
        StockTransfer = 3,
    }
}
