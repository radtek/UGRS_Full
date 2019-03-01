using UGRS.Core.Services;
using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.Purchases.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.DAO;

namespace UGRS.Core.SDK.DI.Purchases.DAO
{
    public class PurchaseXmlDAO
    {
        QueryManager mObjQueryManager = new QueryManager();
        #region PurchaseXML
        /// <summary>
        /// Obtener Articulos dependiendo del codigo de clasificacion.
        /// </summary>
        public IList<ItemDTO> GetItems(string pStrClasificationCode)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            IList<ItemDTO> lLstItems = new List<ItemDTO>();
            try
            {

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("ClasificationCode", pStrClasificationCode);
                string lStrQuery = this.GetSQL("GetItemCodeByNcmCode").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ItemDTO lObjItemDTO = new ItemDTO();
                        lObjItemDTO.ItemCode = lObjRecordset.Fields.Item("ItemCode").Value.ToString();
                        lObjItemDTO.ItemName = lObjRecordset.Fields.Item("ItemName").Value.ToString();
                        lLstItems.Add(lObjItemDTO);
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

        public IList<ItemDTO> GetItems()
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            IList<ItemDTO> lLstItems = new List<ItemDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetItemCode");
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ItemDTO lObjItemDTO = new ItemDTO();
                        lObjItemDTO.ItemCode = lObjRecordset.Fields.Item("ItemCode").Value.ToString();
                        lObjItemDTO.ItemName = lObjRecordset.Fields.Item("ItemName").Value.ToString();
                        lLstItems.Add(lObjItemDTO);
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


        public bool IsInvntItem(string pStrItemCode)
        {
            string InvntItem = "";
            try
            {
                InvntItem = mObjQueryManager.GetValue("InvntItem", "ItemCode", pStrItemCode, "OITM");
            }
            catch (Exception lObjException)
            {
                UIApplication.ShowError(string.Format("CostAccount: {0}", lObjException.Message));
                LogService.WriteError("PurchasesDAO (GetWhouse): " + lObjException.Message);
                LogService.WriteError(lObjException);
            }
            return InvntItem == "Y" ? true : false;
        }


        public string GetWhsDropShip()
        {
            string WhsDropShip = "";
            try
            {
                WhsDropShip = mObjQueryManager.GetValue("WhsCode", "DropShip", "Y", "OWHS");
            }
            catch (Exception lObjException)
            {
                UIApplication.ShowError(string.Format("CostAccount: {0}", lObjException.Message));
                LogService.WriteError("PurchasesDAO (GetWhouse): " + lObjException.Message);
                LogService.WriteError(lObjException);
            }
            return WhsDropShip;
        }

        /// <summary>
        /// Obtener RFC de la empresa.
        /// </summary>
        public string GetRFC()
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrRFC = "";

            try
            {

                //Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                //lLstStrParameters.Add("WareHouse", "");
                string lStrQuery = this.GetSQL("GetRFC");
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrRFC = lObjRecordset.Fields.Item("taxidnum").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetRFC): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lStrRFC;
        }

        /// <summary>
        /// Obtener socio de negocio por RFC.
        /// </summary>
        public string GetBussinesPartner(string pStrRFC)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrCardCode = "";

            try
            {

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RFC", pStrRFC);
                string lStrQuery = this.GetSQL("GetBussinesPartner").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrCardCode = lObjRecordset.Fields.Item("CardCode").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetBussinesPartner): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lStrCardCode;
        }

        /// <summary>
        /// Valida si existe UUID
        /// </summary>
        public bool ValidateUUID(string pStrUUID)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            bool lBolExist = false;
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("UUID", pStrUUID);
                string lStrQuery = this.GetSQL("ValidateUUID").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lBolExist = true;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (ValidateUUID): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lBolExist;
        }


        public List<string> GetMQRise()
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            List<string> lLstRises = new List<string>();
            try
            {
                //Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
               // lLstStrParameters.Add("ClasificationCode", pStrClasificationCode);
                string lStrQuery = this.GetSQL("GetMQRises");//.Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        string lObjItemDTO = lObjRecordset.Fields.Item("U_IdRise").Value.ToString();
                        lLstRises.Add(lObjItemDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("GetMQRise: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetMQRise): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }


            return lLstRises;
        }

        public IList<AssetsDTO> GetRiseAF(string pStrRiseId)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            IList<AssetsDTO> lLstAssetsDTO = new List<AssetsDTO>();

            try
            {

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RiseId", pStrRiseId);
                string lStrQuery = this.GetSQL("GetRiseAF").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        AssetsDTO lObjAssetDTO = new AssetsDTO();
                        lObjAssetDTO.FrgnName = string.Empty;
                        lObjAssetDTO.OcrCode = string.Empty;
                        lObjAssetDTO.PrcCode = lObjRecordset.Fields.Item("OcrCode2").Value.ToString();
                        lLstAssetsDTO.Add(lObjAssetDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesXmlDAO (GetRiseAF): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lLstAssetsDTO;
        }
        #endregion
    }
}
