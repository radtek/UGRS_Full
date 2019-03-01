using System.Collections.Generic;
using UGRS.Core.SDK.DI.Purchases.DAO;
using UGRS.Core.SDK.DI.Purchases.DTO;
using UGRS.Core.SDK.DI.Purchases.Tables;

namespace UGRS.Core.SDK.DI.Purchases.Services.ServicesDAO
{
    public class PurchaseVouchersService
    {
        private PurchaseVouchersDAO lObjPurchaseVoucher;

        public PurchaseVouchersService()
        {
            lObjPurchaseVoucher = new PurchaseVouchersDAO();
        }

        public List<VouchersDetail> GetVouchesDetail(string pStrCode)
        {
            return lObjPurchaseVoucher.GetVouchesDetail(pStrCode);
        }

        public List<VouchersDetailDTO> GetInvoiceVouchesDetail(string costCenter, string pStrFolio) {
            return lObjPurchaseVoucher.GetInvoiceVouchesDetail(costCenter, pStrFolio);
        }


        public List<VouchersDetail> GetVoucherDetailByTrans(string pStrTransId)
        {
            return lObjPurchaseVoucher.GetVoucherDetailByTrans(pStrTransId);
        }

        public List<Vouchers> GetVouches(string pStrCode)
        {
            return lObjPurchaseVoucher.GetVouches(pStrCode);
        }

        public IList<Vouchers> GetVouchersList(SearchVouchersDTO pObjSearchVouchersDTO)
        {
            return lObjPurchaseVoucher.GetVouchersList(pObjSearchVouchersDTO);
        }

        public IList<BankDTO> GetBankList()
        {
            return lObjPurchaseVoucher.GetBanks();
        }

        public IList<AccountDTO> GetBankAccountList(string pStrBankCode)
        {
            return lObjPurchaseVoucher.GetBankAccounts(pStrBankCode);
        }

      

    }
}
