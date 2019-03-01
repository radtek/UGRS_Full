using System.Collections.Generic;
using UGRS.Core.SDK.DI.Purchases.DAO;
using UGRS.Core.SDK.DI.Purchases.Tables;

namespace UGRS.Core.SDK.DI.Purchases.Services.ServicesDAO
{
    public class PurchaseReceiptsService
    {
        private PurchaseReceiptsDAO lObjPurchaseReceiptsDAO;

        public PurchaseReceiptsService()
        {
            lObjPurchaseReceiptsDAO = new PurchaseReceiptsDAO();
        }

        public string GetEmployeName(string pStrEmployeId)
        {
            return lObjPurchaseReceiptsDAO.GetEmployeName(pStrEmployeId);
        }

        public IList<string> GetEmployeList(string pStrDept)
        {
            return lObjPurchaseReceiptsDAO.GetEmployeList(pStrDept);
        }

        public string GetAccountInConfig(string pStrField)
        {
            return lObjPurchaseReceiptsDAO.GetConfigValue(pStrField);
        }

        public string GetAccountRefund(string pStrPrcCode)
        {
            return lObjPurchaseReceiptsDAO.GetAccountRefund(pStrPrcCode);
        }

        public string GetVoucherFolio(string pStrArea, string pStrType)
        {
            return lObjPurchaseReceiptsDAO.GetVoucherFolio(pStrArea, pStrType);
        }

        public int GetLastReceipt()
        {
            return lObjPurchaseReceiptsDAO.GetLastReceipt();
        }

        public bool GetVoucherEmp(Vouchers pObjVouchers)
        {
            return lObjPurchaseReceiptsDAO.GetVoucherEmp(pObjVouchers);
        }

        public Dictionary<string, string> GetBankInfo(string pStrAccountNumber)
        {
            return lObjPurchaseReceiptsDAO.GetBankInfo(pStrAccountNumber);
        }

        public bool ExistsPayment(string pStrEmployeeCode, string pStrFolio, string pStrArea)
        {
            return lObjPurchaseReceiptsDAO.ExistsPayment(pStrEmployeeCode, pStrFolio, pStrArea);
        }

        public bool ExistsPayment(string pStrDocEntry)
        {
            return lObjPurchaseReceiptsDAO.ExistsPayment(pStrDocEntry);
        }

        public string GetMenuId()
        {
            return lObjPurchaseReceiptsDAO.GetMenuId();
        }
    }
}
