using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.AddOn.Machinery.Enums
{
    public enum ContractModeEnum : int
    {
        [Description("Compras")]
        Purchase = 1,
        [Description("Ventas")]
        Sales = 2,
    }
}
