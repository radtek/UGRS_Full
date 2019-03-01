using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Transports.DTO
{
   public class FreightsParamsDTO
    {
       public string CardCode { get; set; }
       public int FormType { get; set; }
       public int UserSign { get; set; }
       public SalesOrderLinesDTO SalesOrderLines { get; set; }
       public bool Insurance { get; set; }
       public bool Internal { get; set; }
       public bool Loaded { get; set; }
    }
}
