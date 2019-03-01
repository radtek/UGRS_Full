using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.Enums
{
    public enum ContractsTypeEnum : int
    {
        [Description("Particular")]
        Particular = 1,
        [Description("Caminos")]
        Roads = 2,
        [Description("Conaza")]
        Conaza = 3
    }
}
