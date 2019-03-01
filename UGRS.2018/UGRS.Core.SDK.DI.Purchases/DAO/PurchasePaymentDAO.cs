using System;
using System.Collections.Generic;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Purchases.DAO {
    public class PurchasePaymentDAO {
        #region PaymentDI

        /// <summary>
        /// Obtener Folio del comprobante dependiendo del folio y del area.
        /// </summary>
        public string GetVoucherCode(string pStrFolio, string pStrArea, int pIntType) {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrVoucherCode = "-1";
            try {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Folio", pStrFolio);
                lLstStrParameters.Add("Area", pStrArea);
                lLstStrParameters.Add("Type", pIntType.ToString());
                string lStrQuery = this.GetSQL("GetVoucherCode").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if(lObjRecordset.RecordCount > 0) {
                    lStrVoucherCode = lObjRecordset.Fields.Item("Code").Value.ToString();
                }
            }
            catch(Exception ex) {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetVoucherCode): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lStrVoucherCode;
        }

        /// <summary>
        /// Obtener DocNum del documento de pago.
        /// </summary>
        public string GetPaymentDocNum(string pStrInvoiceDocEntry) {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrPaymentDocNum = "0";
            try {

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("DocEntry", pStrInvoiceDocEntry);
                string lStrQuery = this.GetSQL("GetPaymentDocNum").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if(lObjRecordset.RecordCount > 0) {
                    lStrPaymentDocNum = lObjRecordset.Fields.Item("DocNum").Value.ToString();
                }
            }
            catch(Exception ex) {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetPaymentDocNum): " + ex.Message);
                LogService.WriteError(ex);

            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lStrPaymentDocNum;
        }
        #endregion

        public string GetCodeMov(int docEntry, string type) {

            SAPbobsCOM.Recordset lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset); ;
            string codeMov = String.Empty;

            try {


                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("DocEntry", docEntry.ToString());
                string lStrQuery = this.GetSQL("GetCodeMov" + type).Inject(lLstStrParameters);
                lObjRecordset.DoQuery(lStrQuery);


                if(lObjRecordset.RecordCount > 0) {
                    codeMov = lObjRecordset.Fields.Item(0).Value.ToString();
                }
            }
            catch(Exception ex) {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetVoucherCode): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return codeMov;
        }
    }
}
