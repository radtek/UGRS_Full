﻿using System;
using UGRS.Core.SDK.DI.Purchases.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Purchases.Services
{
    public class PaymentDI
    {
        PurchasesServiceFactory mObjPurchaseServiceFactory = new PurchasesServiceFactory();
        /// <summary>
        /// Crea un pago
        /// </summary>
        public bool CreatePayment(PurchaseXMLDTO pObjPurchase)
        {
            bool lBolIsSuccess = false;
            try
            {
                
                SAPbobsCOM.Payments lObjPayment = (SAPbobsCOM.Payments)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
                lObjPayment.CardCode = pObjPurchase.CardCode;

                lObjPayment.DocObjectCode = SAPbobsCOM.BoPaymentsObjectType.bopot_OutgoingPayments;
                //lObjPayment.DocDate = pObjPurchase.DocDate;
                lObjPayment.TaxDate = pObjPurchase.TaxDate;
                lObjPayment.DocDate = pObjPurchase.DocDate;
                lObjPayment.DocType = SAPbobsCOM.BoRcptTypes.rSupplier;

                lObjPayment.CashSum = Convert.ToDouble(pObjPurchase.Total);

                lObjPayment.CashAccount = pObjPurchase.Account;// lObjPurchasesDAO.GetAccountRefund(pObjPurchase.Area);

                lObjPayment.Invoices.DocEntry = pObjPurchase.DocEntry;
                lObjPayment.Invoices.InvoiceType = SAPbobsCOM.BoRcptInvTypes.it_PurchaseInvoice;
                lObjPayment.Invoices.SumApplied = Convert.ToDouble(pObjPurchase.Total);
                

                lObjPayment.UserFields.Fields.Item("U_GLO_PaymentType").Value = "GLPGO";
                lObjPayment.UserFields.Fields.Item("U_FZ_AuxiliarType").Value = "2";
                lObjPayment.UserFields.Fields.Item("U_FZ_Auxiliar").Value = pObjPurchase.Employee;
                lObjPayment.UserFields.Fields.Item("U_GLO_CostCenter").Value = pObjPurchase.Area;
                lObjPayment.UserFields.Fields.Item("U_GLO_CodeMov").Value = pObjPurchase.CodeMov;

                if (lObjPayment.Add() != 0)
                {
                    UIApplication.ShowMessageBox(string.Format("Exception: {0}", DIApplication.Company.GetLastErrorDescription()));
                    LogService.WriteError("PaymentDI (CreatePayment) DocEntry:" + pObjPurchase.DocEntry + " Mensaje:" + DIApplication.Company.GetLastErrorDescription());
                }
                else
                {
                    LogService.WriteSuccess("pago creado correctamente: InvoiceDocEntry: " + pObjPurchase.DocEntry);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("PaymentDI (CancelDocument) InvoiceDocEntry:" + pObjPurchase.DocEntry + " Mensaje:" + ex.Message);
                LogService.WriteError(ex);
                
            }
            return lBolIsSuccess;
        }

        public bool CancelPayment(string pStrDocEntry)
        {
            try
            {
                int lIntDocNum = Convert.ToInt32(mObjPurchaseServiceFactory.GetPurchasePaymentService().GetPaymentDocNum(pStrDocEntry));
                if (lIntDocNum != 0)
                {
                    SAPbobsCOM.Payments lObjPayment = (SAPbobsCOM.Payments)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oVendorPayments);
                    lObjPayment.GetByKey(lIntDocNum);
                    if (lObjPayment.CancelbyCurrentSystemDate() != 0)
                    {
                        UIApplication.ShowMessageBox(DIApplication.Company.GetLastErrorDescription());
                        LogService.WriteError("PaymentDI (CancelPayment) DocEntry:" + pStrDocEntry + " Mensaje:" + DIApplication.Company.GetLastErrorDescription());
                        return false;
                    }
                    else
                    {
                        LogService.WriteSuccess("Pago cancelado correctamente: PaymentDocEntry: " + pStrDocEntry);
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(ex.Message);
                LogService.WriteError("PaymentDI (CancelPayment) DocEntry:" + pStrDocEntry + " Mensaje:" + ex.Message);
                LogService.WriteError(ex);
                return false;
            }
           
        }
    }
}
