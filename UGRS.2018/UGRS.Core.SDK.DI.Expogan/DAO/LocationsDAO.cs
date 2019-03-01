using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Expogan.DTO;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Expogan.Tables;

namespace UGRS.Core.SDK.DI.Expogan.DAO
{
    public class LocationsDAO
    {
        QueryManager mObjQueryManager = new QueryManager();
        public IList<LevelDTO> GetGroupLocations()
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            IList<LevelDTO> LlstLevels = new List<LevelDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetGroupLocation");
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        LevelDTO lObjLevelDTO = new LevelDTO();
                        lObjLevelDTO.IdLevel = lObjRecordset.Fields.Item("Code").Value.ToString();
                        lObjLevelDTO.Name = lObjRecordset.Fields.Item("Name").Value.ToString();
                        LlstLevels.Add(lObjLevelDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetItems): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return LlstLevels;
        }


        public IList<LocationDTO> GetLocations(string lStrGroup)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            IList<LocationDTO> lLstItems = new List<LocationDTO>();
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Group", lStrGroup);
                string lStrQuery = this.GetSQL("GetLocations").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        LocationDTO lObjLocationDTO = new LocationDTO();
                        lObjLocationDTO.IdLocation = lObjRecordset.Fields.Item("Code").Value.ToString();
                        lObjLocationDTO.Name = lObjRecordset.Fields.Item("Name").Value.ToString();
                        lObjLocationDTO.Price = Convert.ToDouble(lObjRecordset.Fields.Item("U_Total").Value.ToString());
                        lLstItems.Add(lObjLocationDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetItems): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstItems;
        }

        /// <summary>
        /// Obtener Articulo de renta de stand
        /// </summary>
        public string GetItemCode(string pStrField)
        {
            string lStrGetItemCode = "";
            try
            {
                lStrGetItemCode = mObjQueryManager.GetValue("U_Value", "Name", pStrField, "[@UG_Config]");

            }
            catch (Exception lObjException)
            {
                UIApplication.ShowError(string.Format("GetItemCode: {0}", lObjException.Message));
                LogService.WriteError("PurchasesDAO (GetItemCode): " + lObjException.Message);
                LogService.WriteError(lObjException);
            }

            if (string.IsNullOrEmpty(lStrGetItemCode))
            {
                UIApplication.ShowMessageBox("No fue posible encontrar el campo \'" + pStrField + "' en la tabla de configuraciones ");
            }
            return lStrGetItemCode;
        }



        public double GetCostLocation(string lStrLocation)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            double lDblCost = 0;
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Location", lStrLocation);
                string lStrQuery = this.GetSQL("GetLocationCost").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                        lDblCost = Convert.ToDouble(lObjRecordset.Fields.Item("U_Total").Value.ToString());
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("LocationDAO (GetCostLocation): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lDblCost;
        }

        public string GetLastFolioContract()
        {
            string lStrCode = mObjQueryManager.Max<string>("Code", "[@UG_EX_LOC_CONTRACT]");
            return mObjQueryManager.GetValue("U_ContractID", "Code", lStrCode, "[@UG_EX_LOC_CONTRACT]");
        }

        /// <summary>
        /// Obtener centro de costo.
        /// </summary>
        public string GetCostCenter()
        {
            string lStrCostCenter = "";
            try
            {
                lStrCostCenter = mObjQueryManager.GetValue("U_GLO_CostCenter", "UserID", DIApplication.Company.UserSignature.ToString(), "OUSR");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("CostCenter: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetCostCenter): " + ex.Message);
                LogService.WriteError(ex);
            }
            return lStrCostCenter;
        }
    }
}
