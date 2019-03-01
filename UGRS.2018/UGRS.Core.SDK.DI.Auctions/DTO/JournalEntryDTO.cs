using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Auctions.DTO
{
    public class JournalEntryDTO
    {
        public string  Account { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public string  Area { get; set; }
        public string AuxType { get; set; }
        public string  Aux { get; set; }
        public string AuctionId { get; set; }
        public string Coments { get; set; }
    }
}
