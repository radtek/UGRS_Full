using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Transports.DTO
{
    public class JournalLineDTO
    {

        public string AccountCode { get; set; }
        public string ContraAccount { get; set; }
        public string CostingCode { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }

    }
}
