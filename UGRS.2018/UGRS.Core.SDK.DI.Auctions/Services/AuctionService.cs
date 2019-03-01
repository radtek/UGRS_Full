using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Auctions.Tables;
using UGRS.Core.SDK.DI.DAO;

namespace UGRS.Core.SDK.DI.Auctions.Services
{
    public class AuctionService
    {
        private TableDAO<Auction> mObjAuctionDAO;

        public AuctionService()
        {
            mObjAuctionDAO = new TableDAO<Auction>();
        }

        public int Update(Auction pObjAuction)
        {
            return mObjAuctionDAO.Update(pObjAuction);
        }
    }
}
