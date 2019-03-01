using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Finances.DTO
{
    public class DocumentDTO
    {
        public int DocEntry;
        public int DocNum;
        public int TransId;
        public int Series;
        public string SeriesName;
        public string CardCode;
        public DateTime DocDate;
        public DateTime DocDueDate;
        public string DocCur;
        public double DocTotal;
        public double DocTotalFC;
        public string ObjType;
    }
}
