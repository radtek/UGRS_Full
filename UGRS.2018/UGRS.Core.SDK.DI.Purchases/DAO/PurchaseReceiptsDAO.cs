using UGRS.Core.Services;
using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Purchases.Enums;
using UGRS.Core.SDK.DI.Purchases.Tables;

namespace UGRS.Core.SDK.DI.Purchases.DAO
{
    public class PurchaseReceiptsDAO
    {
        #region Receipts
        QueryManager mObjQueryManager = new QueryManager();

        public string GetEmployeName(string pStrEmployeId)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrEmployeName = "";
            try
            {

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("EmpId", pStrEmployeId);
                string lStrQuery = this.GetSQL("GetEmployeeName").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrEmployeName = lObjRecordset.Fields.Item("EmployeeName").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetEmployeName: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetEmployeName): " + ex.Message);
                LogService.WriteError(ex);

            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lStrEmployeName;
        }

        /// <summary>
        /// Obtener empleados por departamento.
        /// </summary>
        public IList<string> GetEmployeList(string pStrDept)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            IList<string> lLstEmployee = new List<string>();
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("dept", pStrDept);
                string lStrQuery = this.GetSQL("GetEmployee").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        lLstEmployee.Add(lObjRecordset.Fields.Item("empID").Value.ToString());
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetEmployeList): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lLstEmployee;
        }

        /// <summary>
        /// Obtener Cuenta  Deudora de configuracion.
        /// </summary>
        public string GetConfigValue(string pStrField)
        {
            string lStrCostAccount = "";
            try
            {
                lStrCostAccount = mObjQueryManager.GetValue("U_Value", "Name", pStrField, "[@UG_Config]");
            }
            catch (Exception lObjException)
            {
                UIApplication.ShowError(string.Format("CostAccount: {0}", lObjException.Message));
                LogService.WriteError("PurchasesDAO (GetDUAccount): " + lObjException.Message);
                LogService.WriteError(lObjException);
            }
            return lStrCostAccount;
        }

        /// <summary>
        /// Obtener Cuenta de centro de reembolso.
        /// </summary>
        public string GetAccountRefund(string pStrPrcCode)
        {
            string lStrAccountRefound = "";
            try
            {
                lStrAccountRefound = mObjQueryManager.GetValue("U_AccReem", "U_PrcCode", pStrPrcCode, "[@UG_GLO_ARCC]");
            }
            catch (Exception lObjException)
            {
                UIApplication.ShowError(string.Format("Account Refound: {0}", lObjException.Message));
                LogService.WriteError("PurchasesDAO (GetAccountRefund): " + lObjException.Message);
                LogService.WriteError(lObjException);
            }
            return lStrAccountRefound;
        }

        /// <summary>
        /// Obtener el ultimo folio de comprobante dependiendo del area.
        /// </summary>
        public string GetVoucherFolio(string pStrArea, string pStrType)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrVoucherFolio = "0";
            try
            {

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Area", pStrArea);
                lLstStrParameters.Add("TypeVoucher", pStrType); 
                string lStrQuery = this.GetSQL("GetVoucherFolio").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrVoucherFolio = lObjRecordset.Fields.Item("U_Folio").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetVoucherFolio): " + ex.Message);
                LogService.WriteError(ex);

            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lStrVoucherFolio;
        }

        public int GetLastReceipt()
        {
            int lIntCode = 0;
            try
            {


               
                string lStrCode = mObjQueryManager.Max<string>("Code", "[@UG_GLO_VOUC]");
                if (string.IsNullOrEmpty(lStrCode))
                {
                    lStrCode = "0";
                }
                lIntCode = Convert.ToInt32(lStrCode);
            }
            catch (Exception ex)
            {
                LogService.WriteError("PurchasesDAO (GetLastReceipt): " + ex.Message);
                LogService.WriteError(ex);

            }
            //lIntFolio = (Convert.ToInt32(lStrFolio) + 1);
            return lIntCode;
        }

        //private string GetFirstTicket()
        //{

        //    QueryManager mObjQueryManager = new QueryManager();
        //    string lStrCode = mObjQueryManager.<string>("Code", "[@UG_GLO_VOUC]");
        //    if (string.IsNullOrEmpty(lStrCode))
        //    {
        //        lStrCode = "0";
        //    }
        //    //lIntFolio = (Convert.ToInt32(lStrFolio) + 1);
        //    return lStrCode;
        //}

        public bool GetVoucherEmp(Vouchers pObjVoucher)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            bool lBoolVoucherEmp = false;
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Area", pObjVoucher.Area);
                lLstStrParameters.Add("Type", ((int)pObjVoucher.TypeVoucher).ToString());
                lLstStrParameters.Add("Emp", pObjVoucher.Employee);
                string lStrQuery = this.GetSQL("GetVoucherEmp").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lBoolVoucherEmp = false;
                  
                }
                else
                {
                    lBoolVoucherEmp = true;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetVoucherFolio): " + ex.Message);
                LogService.WriteError(ex);

            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lBoolVoucherEmp;
        }

        public Dictionary<string, string> GetBankInfo(string pStrAccountNumber)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            Dictionary<string, string> lStrBankName = new Dictionary<string, string>();
            try
            {
                string lStrQuery = this.GetSQL("GetBankByAccount").InjectSingleValue("AccountNumber", pStrAccountNumber);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrBankName.Add(lObjRecordset.Fields.Item("Account").Value.ToString(), lObjRecordset.Fields.Item("BankName").Value.ToString());
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetBank): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lStrBankName;
        }

        public bool ExistsPayment(string pStrEmployeeCode, string pStrFolio, string pStrArea)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            bool lBolResult = false;
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("EmpCode", pStrEmployeeCode);
                lLstStrParameters.Add("CodeMov", pStrFolio);
                lLstStrParameters.Add("Area", pStrArea);
                string lStrQuery = this.GetSQL("ExistsPayment").Inject(lLstStrParameters);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lBolResult = true;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (ExistsPayment): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lBolResult;
        }



        public bool ExistsPayment(string pStrDocEntry)
        {
            string lStrStatus = "";
            bool lBolResult = false;
            try
            {
                lStrStatus = mObjQueryManager.GetValue("DocStatus", "DocEntry", pStrDocEntry, "OPCH");

                if (lStrStatus == "O")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception lObjException)
            {
                UIApplication.ShowError(string.Format("CostAccount: {0}", lObjException.Message));
                LogService.WriteError("PurchasesDAO (GetDUAccount): " + lObjException.Message);
                LogService.WriteError(lObjException);
            }
            return lBolResult;
        }


        public string GetMenuId()
        {
            string lStrMenuID = string.Empty;
            SAPbobsCOM.Recordset lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string pStrObjectKey = GetConfigValue("GLO_PURCHASEREPORT");
                // get menu UID of report
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("ObjectKey", pStrObjectKey);
                string lStrQuery = this.GetSQL("GetReportId").Inject(lLstStrParameters);
                lObjRecordset.DoQuery(lStrQuery);
                if (lObjRecordset.RecordCount > 0)
                {
                    lStrMenuID = lObjRecordset.Fields.Item("MenuUID").Value.ToString();
                }

            }
            catch (Exception lObjException)
            {
                LogService.WriteError("PurchasesDAO (GetMenuId): " + lObjException.Message);
                LogService.WriteError(lObjException);
            }
             finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lStrMenuID;
        }

        

        #endregion

    }
}
