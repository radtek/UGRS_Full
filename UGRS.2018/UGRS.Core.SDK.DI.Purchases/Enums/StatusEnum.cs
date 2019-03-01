using System.ComponentModel;

namespace UGRS.Core.SDK.DI.Purchases.Enums
{

    public enum StatusEnum : int
    {
        [Description("Pendiente")]
        Pending = 1,

        [Description("Pendiente Autorizar")]
        PendingArea = 2,

        //[Description("Autorizado Area")]
        //AuthorizedArea = 3,

        [Description("Autorizado Ope/Admon")]
        Authorized_Ope_Admon = 4,

        [Description("Cerrado")]
        Closed = 5


    }
}
