/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Food Distribution Data Access
Date: 16/08/2018
Company: Qualisys
*/

using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Corrals.DTO;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.UI;
using UGRS.Core.Utility;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Corrals.DAO {
    public class DistributionDAO {

        #region GetDistributionFoodInCorrals
        /// <summary>
        /// Get Food Distribution in Corrals for Delivery
        /// </summary>
        /// <returns></returns>
        public IList<DistributionDTO> GetDistributionFoodInCorrals(string pStrDate, string pStrCorral) {

            Recordset lObjRecordset = null;
            var lLstDistribution = new List<DistributionDTO>();
            var lLstStrParameters = new Dictionary<string, string>();

            try {

                string lStrQuery = this.GetSQL("GetDistributionFoodInCorrals");
                string lStrLocation = GetUserDefaultWarehouse();

                lLstStrParameters.Add("InDate", pStrDate);
                lLstStrParameters.Add("Location", lStrLocation);

                if (!String.IsNullOrEmpty(pStrCorral)) {
                    lLstStrParameters.Add("Warehouse", pStrCorral);
                    lStrQuery += " WHERE B0.WhsCode = '{Warehouse}'";
                }

                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery.Inject(lLstStrParameters));

                if (lObjRecordset.RecordCount > 0) {

                    for (int i = 0; i < lObjRecordset.RecordCount; i++) {
                        var lObjDistributionDTO = new DistributionDTO();
                        Parallel.ForEach(lObjRecordset.Fields.OfType<SAPbobsCOM.Field>(), field => {
                            lObjDistributionDTO.GetType().GetProperty(field.Name).SetValue(lObjDistributionDTO, field.Value);
                        });
                        lLstDistribution.Add(lObjDistributionDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception e) {
                HandleException(e, "GetDistributionFoodInCorrals");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstDistribution.OrderBy(d => d.Name).ToList();
        }
        #endregion

        #region GetIsBagRequired
        /// <summary>
        /// To validate if the bags field are required 
        /// </summary>
        /// <param name="pStrItemCode"></param>
        /// <returns></returns>
        public bool GetIsBagRequired(string pStrItemCode) {

            Recordset lObjRecordset = null;
            Dictionary<string, string> lLstParams = new Dictionary<string, string>();
            bool result = false;

            try {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lLstParams.Add("ItemCode", pStrItemCode);
                string lStrQuery = this.GetSQL("GetIsBagsRequired");

                lObjRecordset.DoQuery(lStrQuery.Inject(lLstParams));

                if (lObjRecordset.RecordCount > 0) {
                    result = lObjRecordset.Fields.Item(0).Value.ToString() == "Y" ? true : false;
                }
            }
            catch (Exception e) {
                HandleException(e, "GetIsBagRequired");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return result;
        }
        #endregion

        #region GetSeries
        /// <summary>
        /// Get the doc series for the curren user.
        /// </summary>
        /// <param name="pStrSeriesName"></param>
        /// <returns></returns>
        public int GetSeries(string pStrSeriesName, string pStrObjectCode) {

            Recordset lObjRecordset = null;
            Dictionary<string, string> lLstParams = new Dictionary<string, string>();
            int result = 0;

            try {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lLstParams.Add("SeriesName", pStrSeriesName);
                lLstParams.Add("ObjectCode", pStrObjectCode);
                string lStrQuery = this.GetSQL("GetSeries");

                lObjRecordset.DoQuery(lStrQuery.Inject(lLstParams));

                if (lObjRecordset.RecordCount > 0) {
                    result = (int)lObjRecordset.Fields.Item("Series").Value;
                }
            }
            catch (Exception e) {

                HandleException(e, "GetSeries");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return result;
        }
        #endregion

        #region GetUserDefaultWarehouse
        /// <summary>
        /// Get the default warehouse for the current user
        /// </summary>
        /// <returns></returns>
        public string GetUserDefaultWarehouse() {

            Recordset lObjRecordset = null;
            Dictionary<string, string> lLstParams = new Dictionary<string, string>();
            string result = String.Empty;
            var currenUser = DIApplication.Company.UserName;

            try {

                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lLstParams.Add("UserCode", currenUser);
                string lStrQuery = this.GetSQL("GetUserDefaultWarehouse");

                lObjRecordset.DoQuery(lStrQuery.Inject(lLstParams));

                if (lObjRecordset.RecordCount > 0) {
                    result = lObjRecordset.Fields.Item("Warehouse").Value.ToString();
                }
            }
            catch (Exception e) {
                HandleException(e, "GetUserDefaultWarehouse");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return result;
        }
        #endregion

        #region GetNonLockeditems
        /// <summary>
        /// Get Items where the default user warehouse is not locked
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, double> GetNonLockeditems() {

            Recordset lObjRecordset = null;
            Dictionary<string, double> lDicItems = new Dictionary<string, double>();
            Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();

            try {

                string lStrQuery = this.GetSQL("GetNonLockedItems");
                var currenUser = DIApplication.Company.UserName;

                lLstStrParameters.Add("UserName", currenUser);
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery.Inject(lLstStrParameters));

                if (lObjRecordset.RecordCount > 0) {

                    for (int i = 0; i < lObjRecordset.RecordCount; i++) {
                        lDicItems.Add(lObjRecordset.Fields.Item(0).Value.ToString(), (double)lObjRecordset.Fields.Item(1).Value);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception e) {
                HandleException(e, "GetNonLockeditems");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lDicItems;
        }
        #endregion

        #region GetDraftKey
        public int GetDraftKey(string pStrCardCode) {
            Recordset lObjRecordSet = null;
            try {
                lObjRecordSet = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetDraftKey").InjectSingleValue("CardCode", pStrCardCode);
                lObjRecordSet.DoQuery(lStrQuery);


                if (lObjRecordSet.RecordCount > 0) {
                    return (int)lObjRecordSet.Fields.Item("DocEntry").Value;
                }
                else {
                    return 0;
                }
            }
            catch (Exception e) {
                HandleException(e, "GetNonLockeditems");
                return 0;
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
        }
        #endregion

        #region GetUserCostCenter
        /// <summary>
        /// Get the default warehouse for the current user
        /// </summary>
        /// <returns></returns>
        public string GetUserCostCenter() {

            Recordset lObjRecordset = null;
            Dictionary<string, string> lLstParams = new Dictionary<string, string>();
            string result = String.Empty;
            var currenUser = DIApplication.Company.UserName;

            try {

                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lLstParams.Add("UserCode", currenUser);
                string lStrQuery = this.GetSQL("GetUserCostCenter");
                lObjRecordset.DoQuery(lStrQuery.Inject(lLstParams));

                if (lObjRecordset.RecordCount > 0) {
                    result = lObjRecordset.Fields.Item("U_GLO_CostCenter").Value.ToString();
                }
            }
            catch (Exception e) {
                HandleException(e, "GetUserCostCenter");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return result;
        }
        #endregion

        #region Handle Exception
        /// <summary>
        /// Handle Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="section"></param>
        public static void HandleException(Exception ex, string section) {
            UIApplication.ShowMessageBox(ex.Message);
            LogService.WriteError(String.Format("{0}: {1}, {2}", section, ex.Message, ex.StackTrace));
        }
        #endregion
    }
}
