using System.Collections.Generic;
using UGRS.Core.SDK.DI.Purchases.DAO;
using UGRS.Core.SDK.DI.Purchases.DTO;

namespace UGRS.Core.SDK.DI.Purchases.Services.ServicesDAO
{
    public class PurchaseMessageService
    {
        private PurchaseMessageDAO lObjPurchaseMessageDAO;

        public PurchaseMessageService()
        {
            lObjPurchaseMessageDAO = new PurchaseMessageDAO();
        }

        public IList<MessageDTO> GetMessage(string pStrMessageType)
        {
            return lObjPurchaseMessageDAO.GetMessage(pStrMessageType);
        }

        public IList<MessageDTO> GetUsersMessage(string pStrMessage, string pStrCostCenter)
        {
            return lObjPurchaseMessageDAO.GetUsersMessage(pStrMessage, pStrCostCenter);
        }

        public string GetUserCode(string pStrUserId)
        {
            return lObjPurchaseMessageDAO.GetUserCode(pStrUserId);
        }

        public string GetUserId(string pStrUserCode)
        {
            return lObjPurchaseMessageDAO.GetUserId(pStrUserCode);
        }

    }
}
