using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Finances.DTO
{
    public class DownPaymentDTO : DocumentDTO
    {
        public string DocStatus;
        public double PaidToDate;
        public double DocRemaining;
    }
}
