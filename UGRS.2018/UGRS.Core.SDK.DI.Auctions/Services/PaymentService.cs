using System.Collections.Generic;
using UGRS.Core.SDK.DI.Auctions.DAO;
using UGRS.Core.SDK.DI.Auctions.DTO;
using UGRS.Core.SDK.DI.Auctions.Tables;

namespace UGRS.Core.SDK.DI.Auctions.Services
{
    public class PaymentService
    {
        private PaymentDAO lObjPayment;

        public PaymentService()
        {
            lObjPayment = new PaymentDAO();
        }

        public IList<PaymentDTO> GetPayments(string lStrAuctionId, int pIntUserSign)
        {
            return lObjPayment.GetPayments(lStrAuctionId,GetCostingCode(pIntUserSign));
        }

        public string GetCostingCode(int pIntUserSign)
        {
            return lObjPayment.GetCostingCode(pIntUserSign);
        }

        public IList<AuctionsDTO> GetAuctions(string pStrCardCode, int pIntUserSign)
        {
            return lObjPayment.GetActionsByBP(pStrCardCode,GetCostingCode(pIntUserSign));
        }

        public string GetLastAuction()
        {
            return lObjPayment.GetLastAuction();
        }

        public bool ExistConfiguration(string pStrField)
        {
            return lObjPayment.ExistConfiguration(pStrField);
        }

        public Auction GetAuction(string lStrFolio)
        {
            return lObjPayment.GetAuction(lStrFolio);
        }

        public List<MessageDTO> GetMessages(string pStrFolio)
        {
            return lObjPayment.GetMessages(pStrFolio);
        }


        public List<string> GetLastAuctions(string pStrCostingCode)
        {
            return lObjPayment.GetLastAuctions(pStrCostingCode);
        }

    }
}
