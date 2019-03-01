using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Permissions.DTO
{
    public class InvoiceExpDTO
    {
        public string Code { get; set; }
        public string DocEntry { get; set; }
        public string BussinesPartner { get; set; }
        public string Certificate { get; set; }
        public string DocNum { get; set; }
        public int HeadInvoice { get; set; }
        public int HeatExport { get; set; }
        public int HeadNoCruz { get; set; }
        public double Amount { get; set; }
        public double PaidToDate { get; set; }
    }
}
