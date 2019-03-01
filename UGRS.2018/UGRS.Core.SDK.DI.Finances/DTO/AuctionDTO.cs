using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Finances.DTO
{
    public class AuctionDTO
    {
        public int Id;
        public string Folio;
        public int LocationId;
        public string Location;
        public string Type;
        public int TypeId;
        public double Commission;
        public DateTime Date;
        public string AuthCorral;
        public string AuthTransport;
        public string AuthAuction;
        public string AuthCyC;
        public string AuthFinances;
    }
}
