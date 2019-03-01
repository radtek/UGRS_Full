using UGRS.Core.SDK.DI.CyC.Tables;
using UGRS.Core.SDK.DI.DAO;

namespace UGRS.Core.SDK.DI.CyC.Services
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
