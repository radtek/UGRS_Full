using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.AddOn.Purchases.Enums
{
    public class NoteTypeEnum
    {
        public enum NoteType : int
        {
            [Description("Reembolso")]
            Refund = 0,
            [Description("Comprobante")]
            Voucher = 1
        }
    }
}
