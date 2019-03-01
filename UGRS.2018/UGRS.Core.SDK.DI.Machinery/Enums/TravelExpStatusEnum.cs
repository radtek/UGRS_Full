using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.Enums
{
    public enum TravelExpStatusEnum : int
    {
        [Description("Activo")]
        Active = 1,
        [Description("Cancelado")]
        Cancelled = 2,
        [Description("Borrador")]
        Draft = 3,
    }
}
