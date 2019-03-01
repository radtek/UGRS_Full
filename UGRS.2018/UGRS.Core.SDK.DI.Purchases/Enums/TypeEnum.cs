using System.ComponentModel;

namespace UGRS.Core.SDK.DI.Purchases.Enums
{
    public class TypeEnum
    {
        public enum Type : int
        {
            [Description("Reembolso")] 
            Refund = 0,
            [Description("Comprobante")] //Viatico
            Voucher = 1
        }
    }
}
