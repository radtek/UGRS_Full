using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Finances.DTO
{
    public class PaymentDTO : DocumentDTO
    {
        public string PayNoDoc;
        public double NoDocSum;
        public double NoDocSumFC;
        public double OpenBal;
        public double OpenBalFc;
    }
}
