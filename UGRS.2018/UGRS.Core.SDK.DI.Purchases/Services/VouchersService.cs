using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Purchases.Tables;
using UGRS.Core.SDK.UI;
using System.Linq;
using UGRS.Core.Services;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.Purchases.Enums;
using UGRS.Core.SDK.DI.Purchases.DTO;

namespace UGRS.Core.SDK.DI.Purchases.Services
{
    public class VouchersService
    {
        private TableDAO<Vouchers> mObjVouchersDAO;

        public VouchersService()
        {
            mObjVouchersDAO = new TableDAO<Vouchers>();
        }

        public int Add(Vouchers pObjVouchers)
        {
            return mObjVouchersDAO.Add(pObjVouchers);
        }

        public int Update(Vouchers pObjVouchers)
        {
            return mObjVouchersDAO.Update(pObjVouchers);
        }

        public int Update(string pStrCode, StatusEnum pEnumStatus)
        {
            PurchasesServiceFactory mObjPurchaseServiceFactory = new PurchasesServiceFactory();
           var  lObjVoucher = mObjPurchaseServiceFactory.GetPurchaseVouchersService().GetVouches(pStrCode);
            
            if (lObjVoucher.Count > 0)
            {
                lObjVoucher[0].Status = (int)pEnumStatus;
                return mObjVouchersDAO.Update(lObjVoucher[0]);
            }
            return -1;
        }
   

        /// <summary>
        /// Actualiza el total
        /// </summary>
        public int UpdateTotal(string pStrCode)
        {
            int lIntResult = -1;
            try
            {
                PurchasesServiceFactory mObjPurchaseServiceFactory = new PurchasesServiceFactory();
                var lObjVoucher = mObjPurchaseServiceFactory.GetPurchaseVouchersService().GetVouches(pStrCode);
                if (lObjVoucher.Count > 0)
                {
                    List<VouchersDetailDTO> lLstVouchersDetail = mObjPurchaseServiceFactory.GetPurchaseVouchersService().GetInvoiceVouchesDetail(lObjVoucher[0].Area, lObjVoucher[0].Folio);
                    lObjVoucher[0].Total = lLstVouchersDetail.Where(x => x.Status != "Cancelado").Sum(x => x.Total);

                   // lObjVoucher[0].Total = pDblTotal;
                   // lObjVoucher[0].Total = mObjPurchaseServiceFactory.GetPurchaseVouchersService().GetVouchesDetail(pStrCode).Where(x => x.Status != "Cancelado").Sum(x => x.Total);
                  lIntResult = mObjPurchaseServiceFactory.GetVouchersService().Update(lObjVoucher[0]);
                    if (lIntResult != 0)
                    {
                        UIApplication.ShowMessageBox(string.Format("Exception: {0}", DIApplication.Company.GetLastErrorDescription()));
                        LogService.WriteError("InvoiceDI (AddVoucherDetail) " + DIApplication.Company.GetLastErrorDescription());
                    }
                }
                else
                {
                    LogService.WriteError("VouchersService: Comprobante no encontrado: "+ pStrCode);

                }
            }
            catch (System.Exception ex)
            {
                LogService.WriteError("VouchersService (UpdateTotal) " + ex.Message);
                LogService.WriteError(ex);
            }

            return lIntResult;
        }



    }
}
