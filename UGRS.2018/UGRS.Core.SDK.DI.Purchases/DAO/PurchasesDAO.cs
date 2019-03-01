using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.Purchases.DTO;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;
using UGRS.Core.Utility;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Purchases.DAO
{
    public class PurchasesDAO
    {  
        QueryManager mObjQueryManager = new QueryManager();

        #region Purchases
       
        /// <summary>
        /// Obtener Activos Fijos.
        /// </summary>
        public IList<AssetsDTO> GetAssets(string lStrOcrCode)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            IList<AssetsDTO> lLstAssetsDTO = new List<AssetsDTO>();
         
            try
            {

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("OcrCode", lStrOcrCode);
                string lStrQuery = this.GetSQL("GetAssets").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        AssetsDTO lObjAssetDTO = new AssetsDTO();
                        lObjAssetDTO.FrgnName = lObjRecordset.Fields.Item("FrgnName").Value.ToString();
                        lObjAssetDTO.OcrCode = lObjRecordset.Fields.Item("OcrCode").Value.ToString();
                        lObjAssetDTO.PrcCode = lObjRecordset.Fields.Item("PrcCode").Value.ToString();
                        lLstAssetsDTO.Add(lObjAssetDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                 UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                 LogService.WriteError("PurchasesDAO (GetAssets): " + ex.Message);
                 LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lLstAssetsDTO;
        }

        /// <summary>
        /// Obtener departamento del cento de costo.
        /// </summary>
        public string GetDepartment(string pStrPrcCode)
        {
            string lStrDepartment = "";
            try
            {
                lStrDepartment = mObjQueryManager.GetValue("Name", "U_PrcCode", pStrPrcCode, "[@UG_GLO_ARCC]");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("Department: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetDepartment): " + ex.Message);
                LogService.WriteError(ex);
            }
            return lStrDepartment;
        }

        public string GetAttachPath()
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrAttachPath = string.Empty;

            try
            {
                string lStrQuery = this.GetSQL("GetAttachPath");
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                        lStrAttachPath = lObjRecordset.Fields.Item("AttachPath").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                //UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetAttachPath): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lStrAttachPath;
        }

        public string GetEmpName(string pStrEmpId)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrEmpName = string.Empty;

            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("EmpId", pStrEmpId);

                string lStrQuery = this.GetSQL("GetEmployeeName").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrEmpName = lObjRecordset.Fields.Item("EmployeeName").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                //UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetAttachPath): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lStrEmpName;
        }


        #endregion

    }
}
