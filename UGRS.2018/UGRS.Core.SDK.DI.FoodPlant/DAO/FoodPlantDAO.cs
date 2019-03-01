/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Food Plant Data Access Object
Date: 04/09/2018
Company: Qualisys
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK;
using UGRS.Core.SDK.DI.FoodPlant.DTO;
using SAPbobsCOM;
using UGRS.Core.SDK.DI.Extension;
using System.Data;
using QualisysLog;
using UGRS.Core.SDK.UI;
using UGRS.Core.Utility;
using UGRS.Core.Extension;

namespace UGRS.Core.SDK.DI.FoodPlant.DAO {

    public class FoodPlantDAO {

        #region GetPendingTransfers
        public PendingTransfer[] GetPendingTransfers(){

           Recordset recordset = null;
           PendingTransfer[] pendingTransfers = null;
           Dictionary<string, string> paramaters = new Dictionary<string, string>();

           try {

              // paramaters.Add("Whs", GetUserDefaultWarehouse());
                paramaters.Add("Whs", "CRHE");

               var query = this.GetSQL("GetPendingTransfers").Inject(paramaters); 

               recordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
               recordset.DoQuery(query);

               if(recordset.RecordCount > 0) {
                   pendingTransfers = new PendingTransfer[recordset.RecordCount];
                   for(int i = 0; i < recordset.RecordCount; i++) {
                       var pendingTransfer = new PendingTransfer();
                       Parallel.ForEach(recordset.Fields.OfType<Field>(), field => {
                           pendingTransfer.GetType().GetProperty(field.Name).SetValue(pendingTransfer, field.Value);
                       });
                       pendingTransfers[i] = pendingTransfer;
                       recordset.MoveNext();
                   }
               }
           }
           catch(Exception ex) {
               HandleException(ex, "GetPendingTransfers");
           }
           return pendingTransfers;
       }
        #endregion

        #region GetTransferItems
        public TransferItem[] GetTransferItems(string DocEntry) {

           Recordset recordset = null;
           TransferItem[] transferItems = null;
           Dictionary<string, string> paramaters = new Dictionary<string, string>();

           try {
               paramaters.Add("DocEntry", DocEntry);
               string query = this.GetSQL("GetTransferItems").Inject(paramaters); 
               recordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
               recordset.DoQuery(query);

               if(recordset.RecordCount > 0) {
                   transferItems = new TransferItem[recordset.RecordCount];
                   for(int i = 0; i < recordset.RecordCount; i++) {
                       var transferItem = new TransferItem();
                       Parallel.ForEach(recordset.Fields.OfType<Field>(), field => {
                           transferItem.GetType().GetProperty(field.Name).SetValue(transferItem, field.Value);
                       });
                       transferItems[i] = transferItem;
                       recordset.MoveNext();
                   }
               }
           }
           catch(Exception ex) {
               HandleException(ex, "GetTransferItems");
           }
           return transferItems;
       }
        #endregion

        #region GetTransferRequests
        public RequestTransfer[] GetTransferRequests(string docNum) {

           Recordset recordset = null;
           RequestTransfer[] requestTransfers = null;
           Dictionary<string, string> paramaters = new Dictionary<string, string>();

           try {

               paramaters.Add("DocNum", docNum);
               string query = this.GetSQL("GetTransferRequest").Inject(paramaters);
               recordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
               recordset.DoQuery(query);

               if(recordset.RecordCount > 0) {
                   requestTransfers = new RequestTransfer[recordset.RecordCount];
                   for(int i = 0; i < recordset.RecordCount; i++) {
                       var transferRequest = new RequestTransfer();
                       Parallel.ForEach(recordset.Fields.OfType<Field>(), field => {
                           transferRequest.GetType().GetProperty(field.Name).SetValue(transferRequest, field.Value);
                       });
                       requestTransfers[i] = transferRequest;
                       recordset.MoveNext();
                   }
               }
           }
           catch(Exception ex) {
               HandleException(ex, "GetTransferItems");
           }
           return requestTransfers;
       }
        #endregion

        #region GetUserDefaultWarehouse
        public string GetUserDefaultWarehouse() {

           Recordset lObjRecordset = null;
           Dictionary<string, string> lLstParams = new Dictionary<string, string>();
           string result = String.Empty;
           var currenUser = DIApplication.Company.UserName;

           try {

               lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
               lLstParams.Add("UserCode", currenUser);
               string lStrQuery = this.GetSQL("GetUserDefaultWarehouse").Inject(lLstParams);

               lObjRecordset.DoQuery(lStrQuery);

               if(lObjRecordset.RecordCount > 0) {
                   result = lObjRecordset.Fields.Item("Warehouse").Value.ToString();
               }
           }
           catch(Exception e) {
               HandleException(e, "GetUserDefaultWarehouse");
           }
           finally {
               MemoryUtility.ReleaseComObject(lObjRecordset);
           }
           return !String.IsNullOrEmpty(result) ? result : "PLHE";
       }
        #endregion

        #region GetUserCostCenter
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

               if(lObjRecordset.RecordCount > 0) {
                   result = lObjRecordset.Fields.Item("U_GLO_CostCenter").Value.ToString();
               }
           }
           catch(Exception e) {
               HandleException(e, "GetUserCostCenter");
           }
           finally {
               MemoryUtility.ReleaseComObject(lObjRecordset);
           }
           return !String.IsNullOrEmpty(result)? result: "PL_PLANT";
       }
        #endregion

        #region GetAvailableTransitWarehouse
        public string GetAvailableTransitWarehouse(string itemCode) {

           Recordset lObjRecordset = null;
           string result = String.Empty;
           var currenUser = DIApplication.Company.UserName;

           try {

               lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
               lObjRecordset.DoQuery(this.GetSQL("GetAvailableTransitWarehouse").Inject(new Dictionary<string, string>() { {"ItemCode", itemCode}}));
               if(lObjRecordset.RecordCount > 0) {
                   result = lObjRecordset.Fields.Item("WhsCode").Value.ToString();
               }
           }
           catch(Exception e) {
               HandleException(e, "GetAvailableTransitWarehouse");
           }
           finally {
               MemoryUtility.ReleaseComObject(lObjRecordset);
           }
           return result;
       }
        #endregion

        #region GetSeries
        public int GetSeries(string seriesName, string objectCode) {

            Recordset lObjRecordset = null;
            Dictionary<string, string> lLstParams = new Dictionary<string, string>();
            int result = 0;

            try {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lLstParams.Add("SeriesName", seriesName);
                lLstParams.Add("ObjectCode", objectCode);
                string lStrQuery = this.GetSQL("GetSeries");

                lObjRecordset.DoQuery(lStrQuery.Inject(lLstParams));

                if(lObjRecordset.RecordCount > 0) {
                    result = (int)lObjRecordset.Fields.Item("Series").Value;
                }
            }
            catch(Exception e) {

                HandleException(e, "GetSeries");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return result;
        }
        #endregion

        #region HandleException
        public static void HandleException(Exception ex, string section) {
           UIApplication.ShowMessageBox(String.Format("Error: {0}", ex.Message));
           QsLog.WriteError(String.Format("{0}: {1}", section, ex.Message));
           QsLog.WriteException(ex);
        }
        #endregion
    }
}
