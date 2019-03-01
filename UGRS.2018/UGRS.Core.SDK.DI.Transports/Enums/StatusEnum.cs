using System.ComponentModel;

namespace UGRS.Core.SDK.DI.Transports.Enums
{
   public enum StatusEnum : int
    {
       [Description("Abierto")]
       OPEN = 1,
       [Description("Cerrado")]
       CLOSED = 2,
       [Description("Cancelado")]
       CANCELED = 3
    }
}
