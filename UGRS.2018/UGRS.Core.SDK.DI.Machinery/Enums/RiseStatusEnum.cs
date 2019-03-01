using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.Enums
{
    public enum RiseStatusEnum : int
    {
        [Description("Activo")]
        Active = 1,
        [Description("Finalizado")]
        Close = 2,
        [Description("Cancelado")]
        Cancelled = 3,
        [Description("Reabierto")]
        ReOpen = 4,
    }
}
