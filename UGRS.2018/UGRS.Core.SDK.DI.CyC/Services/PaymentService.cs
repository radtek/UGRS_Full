using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.CyC.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.CyC.Services
{
    public class PaymentService
    {


        public bool CreatePayment(AuctionDTO pObjAuctionDTO, List<InvoiceDTO> pLstObjInvoice)
        {
            bool lBolIsSuccess = false;
            try
            {
                SAPbobsCOM.Payments lObjPayment = (SAPbobsCOM.Payments)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oIncomingPayments);

                lObjPayment.CardCode = pObjAuctionDTO.CardCode;
                lObjPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_IncomingPayments;
                lObjPayment.DocDate = DateTime.Now;
                lObjPayment.DocType = SAPbobsCOM.BoRcptTypes.rCustomer;
                lObjPayment.UserFields.Fields.Item("U_GLO_PaymentType").Value = "GLPGO";
                lObjPayment.UserFields.Fields.Item("U_FZ_AuxiliarType").Value = "1";
                lObjPayment.UserFields.Fields.Item("U_FZ_Auxiliar").Value = pObjAuctionDTO.CardCode;
                lObjPayment.UserFields.Fields.Item("U_FZ_FolioAuction").Value = pObjAuctionDTO.AuctionID;
                lObjPayment.UserFields.Fields.Item("U_GLO_CostCenter").Value = pObjAuctionDTO.LocationId;
                lObjPayment.UserFields.Fields.Item("U_FechaPago").Value = DateTime.Now.ToString("dd-MM-yyyy");
                lObjPayment.UserFields.Fields.Item("U_HoraPago").Value = DateTime.Now.ToString("HH:mm");
                lObjPayment.UserFields.Fields.Item("U_B1SYS_PmntMethod").Value = "17";
                
                
                lObjPayment.CashSum = Convert.ToDouble(pObjAuctionDTO.TotalSell);

                lObjPayment.CashAccount = pObjAuctionDTO.AccountD;// lObjPurchasesDAO.GetAccountRefund(pObjPurchase.Area);

                

                foreach (InvoiceDTO lObjInvoice in pLstObjInvoice)
                {
                    lObjPayment.Invoices.DocEntry = Convert.ToInt32(lObjInvoice.DocEntry);
                    lObjPayment.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_Invoice;
                    lObjPayment.Invoices.SumApplied = Convert.ToDouble(lObjInvoice.Amount);
                    lObjPayment.Invoices.Add();
                }


                if (lObjPayment.Add() != 0)
                {
                    UIApplication.ShowMessageBox(string.Format("Exception: {0}", DIApplication.Company.GetLastErrorDescription()));
                    LogService.WriteError("PaymentDI (CreatePayment) DocEntry:" + pObjAuctionDTO.AuctionID + " Mensaje:" + DIApplication.Company.GetLastErrorDescription());
                }
                else
                {
                    LogService.WriteSuccess("pago creado correctamente: InvoiceDocEntry: " + pObjAuctionDTO.AuctionID);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("PaymentDI (Create) InvoiceDocEntry:" + pObjAuctionDTO.AuctionID + " Mensaje:" + ex.Message);
                LogService.WriteError(ex);

            }
            return lBolIsSuccess;
        }

    }
}
