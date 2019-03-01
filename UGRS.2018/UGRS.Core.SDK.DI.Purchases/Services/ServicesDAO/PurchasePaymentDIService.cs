using UGRS.Core.SDK.DI.Purchases.DAO;

namespace UGRS.Core.SDK.DI.Purchases.Services.ServicesDAO
{
    public class PurchasePaymentDIService
    {
        private PurchasePaymentDAO lObjPurchasePayment;

        public PurchasePaymentDIService()
        {
            lObjPurchasePayment = new PurchasePaymentDAO();
        }

        public string GetVoucherCode(string pStrFolio, string pStrArea, int pIntType)
        {
            return lObjPurchasePayment.GetVoucherCode(pStrFolio, pStrArea, pIntType);
        }

        public string GetPaymentDocNum(string pStrInvoiceDocEntry)
        {
            return lObjPurchasePayment.GetPaymentDocNum(pStrInvoiceDocEntry);
        }

    }
}
