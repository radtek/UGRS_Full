/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Mass Invoicing Data Access
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

    public class MassInvoicingDAO {

        DistributionDAO distributionDAO = new DistributionDAO();

        #region GetInvoicesPending
        /// <summary>
        /// Get Food Distribution in Corrals for Delivery
        /// </summary>
        /// <returns></returns>
        public List<PendingInvoiceDTO> GetInvoicesPending(string type) {

            Recordset lObjRecordset = null;
            var lLstPendingInvoce = new List<PendingInvoiceDTO>();

            try {

                string lStrQuery = this.GetSQL("GetPendingInvoices" + type);


                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0) {

                    for (int i = 0; i < lObjRecordset.RecordCount; i++) {
                        var pendingInvoice = new PendingInvoiceDTO();
                        Parallel.ForEach(lObjRecordset.Fields.OfType<SAPbobsCOM.Field>(), field => {
                            pendingInvoice.GetType().GetProperty(field.Name).SetValue(pendingInvoice, field.Value);
                        });
                        lLstPendingInvoce.Add(pendingInvoice);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception e) {
                HandleException(e, "GetInvoicesPending");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lLstPendingInvoce;
        }
        #endregion

        #region GetDistributedLiveStock
        /// <summary>
        /// Get Distribution of Livestock
        /// </summary>
        /// <param name="pStrCardCode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<LivestockDTO> GetDistributedLiveStock(string pStrCardCode, string type) {

            Recordset lObjRecordset = null;
            var lLstLovestock = new List<LivestockDTO>();
            var lDicParameters = new Dictionary<string, string>();

            try {

                string lStrQuery = this.GetSQL("GetLivestock" + type);

                string lStrLocation = distributionDAO.GetUserDefaultWarehouse();

                lDicParameters.Add("CardCode", pStrCardCode);
                lDicParameters.Add("Location", lStrLocation);

                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery.Inject(lDicParameters));

                if (lObjRecordset.RecordCount > 0) {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++) {

                        var livestock = new LivestockDTO();
                        Parallel.ForEach(lObjRecordset.Fields.OfType<SAPbobsCOM.Field>(), field => {
                            livestock.GetType().GetProperty(field.Name).SetValue(livestock, field.Value);
                        });
                        lLstLovestock.Add(livestock);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception e) {
                HandleException(e, "GetDistributedLiveStock");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lLstLovestock;
        }
        #endregion

        #region GetPayCondition
        /// <summary>
        /// Get Payment Condition for Given Client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public int GetPayCondition(string clientCode) {

            Recordset lObjRecordset = null;
            var result = 0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            try {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                parameters.Add("CardCode", clientCode);
                string query = this.GetSQL("GetPaymentGroup").Inject(parameters);


                lObjRecordset.DoQuery(query);

                if (lObjRecordset.RecordCount > 0) {
                    result = (int)lObjRecordset.Fields.Item("GroupNumber").Value;

                    //if(!result.Equals(-1) && !result.Equals(10)) {
                    //    result = 11;
                    //}
                }


            }
            catch (Exception ex) {
                HandleException(ex, "GetPayCondition");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return result;
        }
        #endregion

        #region GetFloorServiceItem
        /// <summary>
        /// Get Floor Charge Service for Corrals 
        /// </summary>
        /// <returns></returns>
        public FloorService GetFloorServiceItem(string whsCode) {

            Recordset lObjRecordset = null;

            var floorService = new FloorService();
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            try {

                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string query = this.GetSQL("GetFloorServiceItem");
                parameters.Add("ServiceName", "CU_GLO_ITEMPAYR");
                parameters.Add("WhsCode", whsCode);

                lObjRecordset.DoQuery(query.Inject(parameters));

                if (lObjRecordset.RecordCount > 0) {
                    floorService.ItemCode = lObjRecordset.Fields.Item(0).Value.ToString();
                    floorService.Price = (double)lObjRecordset.Fields.Item(1).Value;
                }
            }
            catch (Exception ex) {
                HandleException(ex, "GetFloorServiceItem");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return floorService;
        }
        #endregion

        #region Get Delivery Lines
        /// <summary>
        /// Get Lines for Delivery Document that will be added to Invoice Document
        /// </summary>
        /// <returns></returns>
        public List<DeliveryLine> GetDeliveryLines(string cardCode, string whsCode) {

            Recordset lObjRecordset = null;
            var parameters = new Dictionary<string, string>();
            var lLstdeliveryLines = new List<DeliveryLine>();

            try {
                string query = this.GetSQL("GetDeliveryLines");
                parameters.Add("CardCode", cardCode);
                parameters.Add("WhsCode", whsCode);

                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(query.Inject(parameters));

                if (lObjRecordset.RecordCount > 0) {

                    for (int i = 0; i < lObjRecordset.RecordCount; i++) {

                        var deliveryLine = new DeliveryLine();
                        Parallel.ForEach(lObjRecordset.Fields.OfType<SAPbobsCOM.Field>(), field => {
                            deliveryLine.GetType().GetProperty(field.Name).SetValue(deliveryLine, field.Value);
                        });
                        lLstdeliveryLines.Add(deliveryLine);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex) {

                HandleException(ex, "GetDeliveryLines");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lLstdeliveryLines;
        }
        #endregion

        #region GetTransferDocEntries
        /// <summary>
        /// Get the Action Livestock Transfer DocEntries to Update Stock Tranfer When Action Invoicing
        /// </summary>
        /// <returns></returns>
        public int[] GetTransferDocEntries(string cardCode) {

            Recordset lObjRecordset = null;
            var parameters = new Dictionary<string, string>();
            int[] docEntries = null;

            try {

                string query = this.GetSQL("GetTransferDocEntries");
                parameters.Add("CardCode", cardCode);
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(query.Inject(parameters));

                if (lObjRecordset.RecordCount > 0) {

                    docEntries = new int[lObjRecordset.RecordCount];
                    for (int i = 0; i < lObjRecordset.RecordCount; i++) {
                        docEntries[i] = (int)lObjRecordset.Fields.Item("DocEntry").Value;
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex) {
                HandleException(ex, "GetTransferDocEntries");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return docEntries;
        }
        #endregion

        #region GetDebtImport
        /// <summary>
        /// To validate remaining livestock to determine if the client can do the livetock exit. 
        /// </summary>
        /// <param name="cardCode"></param>
        /// <returns></returns>
        public double GetDebtImport(string CardCode, string whsCode) {

            Recordset lObjRecordset = null;
            Dictionary<string, string> lLstParams = new Dictionary<string, string>();
            double result = 0;
            var objectCode = "13";

            try {

                var series = distributionDAO.GetSeries(whsCode, objectCode).ToString();
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lLstParams.Add("Series", series);
                lLstParams.Add("CardCode", CardCode);
                string lStrQuery = this.GetSQL("GetInvoiceTotalSum");

                lObjRecordset.DoQuery(lStrQuery.Inject(lLstParams));

                if (lObjRecordset.RecordCount > 0) {
                    result = (double)lObjRecordset.Fields.Item("DebtImport").Value;
                }
            }
            catch (Exception e) {
                HandleException(e, "GetDebtImport");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return result;
        }
        #endregion

        #region Get Batches
        /// <summary>
        /// Get Batches for a given client for Livestoxk Exit
        /// </summary>
        /// <param name="cardCode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<BatchDTO> GetBatches(string cardCode, string whsCode, string type) {

            Recordset lObjRecordset = null;
            var batches = new List<BatchDTO>();
            var lDicParameters = new Dictionary<string, string>();

            try {

                string lStrQuery = this.GetSQL("GetBathces" + type);

                lDicParameters.Add("CardCode", cardCode);
                lDicParameters.Add("Location", whsCode);

                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery.Inject(lDicParameters));

                if (lObjRecordset.RecordCount > 0) {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++) {

                        var batch = new BatchDTO();
                        Parallel.ForEach(lObjRecordset.Fields.OfType<SAPbobsCOM.Field>(), field => {
                            batch.GetType().GetProperty(field.Name).SetValue(batch, field.Value);
                        });
                        batches.Add(batch);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception e) {
                HandleException(e, "MassInvoicingDAO(GetDistribution)");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return batches;
        }
        #endregion

        #region GetFloorServiceLines
        /// <summary>
        /// Get Floor Service Lines for Mass Invoicing
        /// </summary>
        /// <param name="cardCode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<FloorServiceLineDTO> GetFloorServiceLines(string cardCode, string whsCode, string type) {

            Recordset lObjRecordset = null;
            var results = new List<FloorServiceLineDTO>();
            var lDicParameters = new Dictionary<string, string>();

            try {

                string lStrQuery = this.GetSQL("GetFloorServiceLines" + type);

                lDicParameters.Add("CardCode", cardCode);
                lDicParameters.Add("Location", whsCode);

                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                lObjRecordset.DoQuery(lStrQuery.Inject(lDicParameters));

                if (lObjRecordset.RecordCount > 0) {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++) {

                        var floorServiceLine = new FloorServiceLineDTO();
                        Parallel.ForEach(lObjRecordset.Fields.OfType<SAPbobsCOM.Field>(), field => {
                            floorServiceLine.GetType().GetProperty(field.Name).SetValue(floorServiceLine, field.Value);
                        });
                        results.Add(floorServiceLine);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception e) {
                HandleException(e, "MassInvoicingDAO(GetDistribution)");
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return results;
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
