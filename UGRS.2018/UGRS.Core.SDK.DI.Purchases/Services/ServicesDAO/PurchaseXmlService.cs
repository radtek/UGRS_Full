using System.Collections.Generic;
using UGRS.Core.SDK.DI.Purchases.DAO;
using UGRS.Core.SDK.DI.Purchases.DTO;

namespace UGRS.Core.SDK.DI.Purchases.Services.ServicesDAO
{
    public class PurchaseXmlService
    {
        private PurchaseXmlDAO lObjPurchaseXml;

        public PurchaseXmlService()
        {
            lObjPurchaseXml = new PurchaseXmlDAO();
        }

        public IList<ItemDTO> GetItems(string pStrClasificationCode)
        {
            return lObjPurchaseXml.GetItems(pStrClasificationCode);
        }

        public IList<ItemDTO> GetItems()
        {
            return lObjPurchaseXml.GetItems();
        }


        public string GetRFC()
        {
            return lObjPurchaseXml.GetRFC();
        }

        public string GetBussinesPartner(string pStrRFC)
        {
            return lObjPurchaseXml.GetBussinesPartner(pStrRFC);
        }

        public bool ValidateUUID(string pStrUUID)
        {
            return lObjPurchaseXml.ValidateUUID(pStrUUID);
        }

        public bool IsInvntItem(string pStrItemCode)
        {
            return lObjPurchaseXml.IsInvntItem(pStrItemCode);
        }

        public string GetWhsDropShip()
        {
            return lObjPurchaseXml.GetWhsDropShip();
        }

        public List<string> GetMqSubidas()
        {
            return lObjPurchaseXml.GetMQRise();
        }

        public IList<AssetsDTO> GetRiseAF(string pStrRiseId)
        {
            return lObjPurchaseXml.GetRiseAF(pStrRiseId);
        }

        public PacConfigDTO GetConfigurationPac() {

            return lObjPurchaseXml.GetConfigurationPac();
        }
    }
}
