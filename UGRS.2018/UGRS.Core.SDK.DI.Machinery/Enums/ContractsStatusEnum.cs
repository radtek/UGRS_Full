using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.Enums
{
    public enum ContractsStatusEnum : int
    {
        [Description("Activo")]
        Active = 1,
        [Description("Cerrado")]
        Closed = 2,
        [Description("Cancelado")]
        Cancelled = 3,
    }
}
