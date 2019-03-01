using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.CyC.DTO
{
    public class InvoiceDTO
    {
        public string DocEntry { get; set; }
        public string DocNum { get; set; }
        public int Days { get; set; }
        public DateTime Date { get; set; }
        public string Area { get; set; }
        public decimal Balance { get; set; }
        public decimal Amount { get; set; }
    }
}
