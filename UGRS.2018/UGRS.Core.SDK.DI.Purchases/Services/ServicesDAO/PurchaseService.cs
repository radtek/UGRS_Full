using System.Collections.Generic;
using UGRS.Core.SDK.DI.Purchases.DAO;
using UGRS.Core.SDK.DI.Purchases.DTO;

namespace UGRS.Core.SDK.DI.Purchases.Services.ServicesDAO
{
    public class PurchaseService
    {
        private PurchasesDAO lObjPurchase;

        public PurchaseService()
        {
            lObjPurchase = new PurchasesDAO();
        }

        public IList<AssetsDTO> GetAssets(string lStrOcrCode)
        {
            return lObjPurchase.GetAssets(lStrOcrCode);
        }

        public string GetDepartment(string pStrPrcCode)
        {
            return lObjPurchase.GetDepartment(pStrPrcCode);
        }

        public string GetAttachPath()
        {
            return lObjPurchase.GetAttachPath();
        }

        public string GetEmpName(string pStrEmpName)
        {
            return lObjPurchase.GetEmpName(pStrEmpName);
        }

    }
}
