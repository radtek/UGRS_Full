using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Finances.DTO
{
    public class CheckDraftDTO
    {
        public int DraftDocEntry;
        public DateTime DueDate;
        public int CheckNum;
        public string BankCode;
        public double CheckSum;
        public string CheckAct;
        public DateTime DocDate;
    }
}
