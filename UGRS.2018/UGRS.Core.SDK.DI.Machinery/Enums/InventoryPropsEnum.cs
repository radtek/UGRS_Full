using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.Enums
{
    public enum InventoryPropsEnum
    {
        [Description("DIESELM")]
        DIESELM = 102,
        [Description("DIESELT")]
        DIESELT = 103,
        [Description("GAS")]
        GAS = 104,
        [Description("15W40")]
        F15W40 = 31,
        [Description("HIDRAU")]
        HIDRAULIC = 33,
        [Description("SAE40")]
        SAE40 = 34,
        [Description("TRANSM")]
        TRANSMISSION = 35,
        [Description("OILS")]
        OILS = 28,
        [Description("KMHRS")]
        KMHRS = 101,
        [Description("OTHERS")]
        OTHERS = 100,
    }
}
