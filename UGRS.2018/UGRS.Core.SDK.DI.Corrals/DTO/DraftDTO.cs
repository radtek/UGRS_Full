using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Corrals.DTO
{
    public class DraftDTO
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string Status { get; set; }
        public string GLO_Status { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Currency { get; set; }
        public double Total { get; set; }
        public string Comments { get; set; }
        public string PE_Origin { get; set; }
        public int ObjType { get; set; }
        public bool IsDraft { get; set; }
    }
}
