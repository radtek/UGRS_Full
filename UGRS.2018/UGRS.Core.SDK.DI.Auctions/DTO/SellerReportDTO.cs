using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Auctions.DTO
{
   public class SellerReportDTO
    {
       public string BatchNumber { get; set; }
       public int Quantity { get; set; }
       public string Article { get; set; }
       public float AverageWeight { get; set; }
       public float TotalWeight { get; set; }
       public decimal Price { get; set; }
       public decimal Amount { get; set; }
       public string Unsold { get; set; }
       public string Reprogrammed { get; set; }
       public string Gender { get; set; }
       public string Buyer { get; set; }
       public string UnsoldMotive { get; set; }
       public string Stat { get; set; }
       public int Orden { get; set; }
    }
}
