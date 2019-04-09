using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Transports.DTO
{
   public class SalesOrderLinesDTO
    {
       public string ItemCode { get; set; }
       public string Description { get; set; }
       public int Quantity { get; set; }
       public float UnitPrice { get;set;}
       public float Tax { get; set; }
       public float TaxWT { get; set; }
       public string PayloadType { get; set; }
       public string VehicleType { get; set; }
       public string Folio { get; set; }
       public int Route { get; set; }
       public string RouteName { get; set; }
       public string KmA { get; set; }
       public string KmB { get; set; }
       public string KmC { get; set; }
       public string KmD { get; set; }
       public string KmE { get; set; }
       public string KmF { get; set; }
       public string Employee { get; set; }
       public string Asset { get; set; }
       public string TotKm { get; set; }
       public string TotKg { get; set; }
       public string Extra { get; set; }
       public string AnotherPyld { get; set; }
       public string Origin { get; set; }
       public string MOrigin { get; set; }
       public string MDestination { get; set; }
       public string Destination { get; set; }
       public bool Shared { get; set; }
       public string Varios { get; set; }
       public string Heads { get; set; }
       public string Bags { get; set; }
       public float KG { get; set; }

    }
}
