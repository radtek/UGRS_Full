using System;
using System.Collections.Generic;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.Services;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Transports.Tables;
using UGRS.Core.SDK.DI.Purchases.DTO;


namespace UGRS.Core.SDK.DI.Transports.DAO
{
    public class RouteListDAO
    {
        QueryManager mObjQueryManager = new QueryManager();

        public string GetRouteQuery()
        {
            try
            {
                string lStrQuery = string.Empty;

                Dictionary<string, string> lObjParameters = new Dictionary<string, string>();
                lObjParameters.Add("Orign", string.Empty);
                lObjParameters.Add("Destiny", string.Empty);
                lObjParameters.Add("TownOrig", string.Empty);
                lObjParameters.Add("TownDest", string.Empty);
                lStrQuery = this.GetSQL("SearchRoutes").Inject(lObjParameters);

                return lStrQuery;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetRouteQuery: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetRouteQuery): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public string GetRouteQueryByFilters(string pStrOrign, string pStrDestiny, string pStrTownOrign, string pStrTownDest)
        {
            try
            {
                string lStrQuery = string.Empty;

                Dictionary<string, string> lObjParameters = new Dictionary<string, string>();
                lObjParameters.Add("Orign", pStrOrign);
                lObjParameters.Add("Destiny", pStrDestiny);
                lObjParameters.Add("TownOrig", pStrTownOrign);
                lObjParameters.Add("TownDest", pStrTownDest);
                lStrQuery = this.GetSQL("SearchRoutes").Inject(lObjParameters);

                return lStrQuery;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetRouteQueryByFilters: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetRouteQueryByFilters): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public string GetTownsQuery(string pStrSearch)
        {
            Dictionary<string, string> lObjParameters = new Dictionary<string, string>();
            try
            {
                lObjParameters.Add("Town", string.Empty);
                lObjParameters.Add("Search", pStrSearch);
                string ss = this.GetSQL("GetTowns").Inject(lObjParameters);
                return this.GetSQL("GetTowns").Inject(lObjParameters);
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetTownsQuery: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetTownsQuery): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public bool CheckTown(string pStrTown)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lObjParameters = new Dictionary<string, string>();
                lObjParameters.Add("Town", pStrTown);
                lObjParameters.Add("Search", string.Empty);

                string lStrQuery = this.GetSQL("GetTowns").Inject(lObjParameters);

                lObjResults.DoQuery(lStrQuery);

                return lObjResults.RecordCount > 0 ? true : false;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("CheckTown: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (CheckTown): " + ex.Message);
                LogService.WriteError(ex);
                return false;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public bool CheckDriver(string pStrDriver, string pStrCostingCode)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lObjParameters = new Dictionary<string, string>();
                lObjParameters.Add("CostingCode", pStrCostingCode);
                lObjParameters.Add("Name", pStrDriver);
                lObjParameters.Add("Search", string.Empty);

                string lStrQuery = this.GetSQL("GetDrivers").Inject(lObjParameters);

                lObjResults.DoQuery(lStrQuery);

                return lObjResults.RecordCount > 0 ? true : false;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("CheckDriver: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (CheckDriver): " + ex.Message);
                LogService.WriteError(ex);
                return false;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }

        }

        public bool CheckItem(string pStrWHS, string pStritem)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lObjParameters = new Dictionary<string, string>();
                lObjParameters.Add("ItemCode", pStritem);
                lObjParameters.Add("WHS", pStrWHS);
                lObjParameters.Add("Search", string.Empty);

                string lStrQuery = this.GetSQL("GetDrivers").Inject(lObjParameters);

                lObjResults.DoQuery(lStrQuery);

                return lObjResults.RecordCount > 0 ? true : false;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("CheckItem: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (CheckItem): " + ex.Message);
                LogService.WriteError(ex);
                return false;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public bool CheckAF(string pStrCostingCode, string pStrEquip, string pStrPrcCode)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {

                Dictionary<string, string> lObjParameters = new Dictionary<string, string>();
                lObjParameters.Add("PrcCode", pStrPrcCode);
                lObjParameters.Add("Equip", pStrEquip);
                lObjParameters.Add("CostingCode", pStrCostingCode);
                lObjParameters.Add("Search", string.Empty);

                string lStrQuery = this.GetSQL("GetDrivers").Inject(lObjParameters);

                lObjResults.DoQuery(lStrQuery);

                return lObjResults.RecordCount > 0 ? true : false;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("CheckAF: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (CheckAF): " + ex.Message);
                LogService.WriteError(ex);
                return false;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public List<VehiclesDTO> GetVehiclesTypeList()
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            List<VehiclesDTO> lLstVehicleTypes = new List<VehiclesDTO>();
            try
            {
                string lStrQuery = this.GetSQL("VehicleTypes");

                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        lLstVehicleTypes.Add(new VehiclesDTO()
                        {
                            Name = lObjResults.Fields.Item("Name").Value.ToString(),
                            PathA = lObjResults.Fields.Item("U_TypeA").Value.ToString(),
                            PathB = lObjResults.Fields.Item("U_TypeB").Value.ToString(),
                            PathC = lObjResults.Fields.Item("U_TypeC").Value.ToString(),
                            PathD = lObjResults.Fields.Item("U_TypeD").Value.ToString(),
                            PathE = lObjResults.Fields.Item("U_TypeE").Value.ToString(),
                            PathF = lObjResults.Fields.Item("U_TypeF").Value.ToString(),
                            Comission = lObjResults.Fields.Item("U_Commission").Value.ToString(),
                            EquipType = lObjResults.Fields.Item("U_EquipType").Value.ToString()
                        });
                        lObjResults.MoveNext();
                    }
                }


            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetVehiclesTypeList: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetVehiclesTypeList): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
            return lLstVehicleTypes;
        }

        public List<PayLoadTypeDTO> GetPayloadTypeList()
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            List<PayLoadTypeDTO> lLstPayloadType = new List<PayLoadTypeDTO>();
            try
            {
                string lStrQuery = this.GetSQL("PayloadTypes");

                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        lLstPayloadType.Add(new PayLoadTypeDTO()
                        {
                            Name = lObjResults.Fields.Item("Name").Value.ToString(),
                            Code = lObjResults.Fields.Item("Code").Value.ToString(),

                        });
                        lObjResults.MoveNext();
                    }
                }


            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetPayloadTypeList: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetPayloadTypeList): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
            return lLstPayloadType;
        }

        public string GetTypePayload(string pStrCode)
        {
            string lStrType = string.Empty;
            try
            {
                lStrType = mObjQueryManager.GetValue("U_Type", "Code", pStrCode, "[@UG_TR_TRTY]");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetPayloadTypeList: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetPayloadTypeList): " + ex.Message);
                LogService.WriteError(ex);
            }
            return lStrType;
        }


        public string GetItemsQuery(string pStrWhs,string pStrCardCode, string pStrSearch)
        {
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("WHS", pStrWhs);
                lLstParams.Add("CardCode", pStrCardCode);
                lLstParams.Add("ItemCode", string.Empty);
                lLstParams.Add("Search", pStrSearch);

                string d = this.GetSQL("GetItems").Inject(lLstParams);

                return this.GetSQL("GetItems").Inject(lLstParams);
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetItemsQuery: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetItemsQuery): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public string GetAFQuery(string pStrCostingCode, string pStrEquip, string pStrSearch)
        {
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("CostingCode", pStrCostingCode);
                lLstParams.Add("PrcCode", "");
                lLstParams.Add("Equip", pStrEquip);
                lLstParams.Add("Search", pStrSearch);

                string dd = this.GetSQL("GetAF").Inject(lLstParams);
                return this.GetSQL("GetAF").Inject(lLstParams);
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetAFQuery: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetAFQuery): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public Routes GetRoute(long pLonCode)
        {
            Routes lObjRoute = new Routes();

            try
            {
                lObjRoute = mObjQueryManager.GetTableObject<Routes>("Code", pLonCode.ToString(), "[@UG_TR_RODS]");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetRoute: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetRoute): " + ex.Message);
                LogService.WriteError(ex);
            }
            return lObjRoute;
        }

        public string GetCostingCode(int pIntUserSignature)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string lStrQuery = this.GetSQL("GetCostingCode").InjectSingleValue("UsrId", pIntUserSignature);

                lObjResults.DoQuery(lStrQuery);

                return lObjResults.RecordCount > 0 ? lObjResults.Fields.Item("U_GLO_CostCenter").Value.ToString() : string.Empty;


            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetCostingCode: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetCostingCode): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public string GetWhs(int pIntUserSignature)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string lStrQuery = this.GetSQL("GetWhs").InjectSingleValue("UsrId", pIntUserSignature.ToString());

                lObjResults.DoQuery(lStrQuery);

                return lObjResults.RecordCount > 0 ? lObjResults.Fields.Item("Warehouse").Value.ToString() : string.Empty;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetWhs: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetWhs): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public string GetDriversQuery(string pStrCostingCode, string pStrSearch)
        {
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("CostingCode", pStrCostingCode);
                lLstParams.Add("Name", "");
                lLstParams.Add("Search", pStrSearch);
                string de = this.GetSQL("GetDrivers").Inject(lLstParams);

                return this.GetSQL("GetDrivers").Inject(lLstParams);
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetDriversQuery: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetDriversQuery): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public string GetInsuranceItem()
        {
            try
            {
                return mObjQueryManager.GetValue("U_Value", "Name", "TR_INSURANCE", "[@UG_CONFIG] ");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetInsuranceItem: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetInsuranceItem): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public bool CheckTransportsItem(string pStrItemCode, int pIntUserSign)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("WHS", GetWhs(pIntUserSign));
                lLstParams.Add("ItemCode", pStrItemCode);
                lLstParams.Add("CardCode", string.Empty);
                lLstParams.Add("Search", string.Empty);

                string lStrQuery = this.GetSQL("GetItems").Inject(lLstParams);

                lObjResults.DoQuery(lStrQuery);

                return lObjResults.RecordCount > 0 ? true : false;

            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("CheckTransportsItem: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (CheckTransportsItem): " + ex.Message);
                LogService.WriteError(ex);
                return false;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public bool CheckInsuranceItem(string pStrItemCode, int pIntUserSign)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("WHS", GetWhs(pIntUserSign));
                lLstParams.Add("ItemCode", pStrItemCode);

                string lStrQuery = this.GetSQL("GetInsuranceItem").Inject(lLstParams);

                lObjResults.DoQuery(lStrQuery);

                return lObjResults.RecordCount > 0 ? true : false;

            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("CheckInsuranceItem: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (CheckInsuranceItem): " + ex.Message);
                LogService.WriteError(ex);
                return false;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public InsuranceDTO GetInsuranceObject(int pIntUserSign)
        {
            InsuranceDTO lObjInsurance = new InsuranceDTO();
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("WHS", GetWhs(pIntUserSign));
                lLstParams.Add("ItemCode", GetInsuranceItem());

                string lStrQuery = this.GetSQL("GetInsuranceItem").Inject(lLstParams);

                lObjResults.DoQuery(lStrQuery);

                if (lObjResults.RecordCount > 0)
                {
                    lObjInsurance.ItemCode = lObjResults.Fields.Item("ItemCode").Value.ToString();
                    lObjInsurance.Price = float.Parse(lObjResults.Fields.Item("Price").Value.ToString());
                }

            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetInsuranceObject: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetInsuranceObject): " + ex.Message);
                LogService.WriteError(ex);
                return lObjInsurance;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
            return lObjInsurance;
        }

        public string GetFolios()
        {
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("Folio", string.Empty);
                lLstParams.Add("Search", string.Empty);

                return this.GetSQL("GetInvFolios").Inject(lLstParams);
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetFolios: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetFolios): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public bool CheckFolio(string pStrShared)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("Folio", pStrShared);
                lLstParams.Add("Search", string.Empty);

                string lStrQuery = this.GetSQL("GetInvFolios").Inject(lLstParams);

                lObjResults.DoQuery(lStrQuery);

                return lObjResults.RecordCount > 0 ? true : false;

            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("CheckFolio: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (CheckFolio): " + ex.Message);
                LogService.WriteError(ex);
                return false;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public int GetFolio(string pStrFolio)
        {
            string lStrValue = string.Empty;
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {

                string lStrQuery = this.GetSQL("GetIntrnlFolio").InjectSingleValue("Folio", pStrFolio);

                lObjResults.DoQuery(lStrQuery);

                if(lObjResults.RecordCount > 0)
                {
                    lStrValue = lObjResults.Fields.Item("Folio").Value.ToString();
                }

                return !string.IsNullOrEmpty(lStrValue) ? Convert.ToInt32(lStrValue) : 0;

            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetFolio: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetFolio): " + ex.Message);
                LogService.WriteError(ex);
                return 0;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }

        }

        public string GetInternalStatus(string pStrFolio)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {

                string lStrQuery = this.GetSQL("GetInternalStatus").InjectSingleValue("Folio", pStrFolio);

                lObjResults.DoQuery(lStrQuery);

                string lStr = lObjResults.Fields.Item("Stat").Value.ToString();
                return lObjResults.RecordCount > 0 ? lObjResults.Fields.Item("Stat").Value.ToString() : string.Empty; ;

            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetInternalStatus: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetInternalStatus): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public string GetKey(string pStrFolio)
        {
            try
            {
                return mObjQueryManager.GetValue("Code", "U_InternalFolio", pStrFolio, "[@UG_TR_INTLFRGHT]");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetKey: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetKey): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public int GetLastFolio()
        {
            try
            {
                string lStrValue = mObjQueryManager.Max<string>("U_InternalFolio", "[@UG_TR_INTLFRGHT]");

                return !string.IsNullOrEmpty(lStrValue) ? Convert.ToInt32(lStrValue) : 0;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetLastFolio: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetLastFolio): " + ex.Message);
                LogService.WriteError(ex);
                return 0;
            }
        }

        public int GetFirstFolio()
        {
            try
            {
                string lStrValue = mObjQueryManager.Min<string>("U_InternalFolio", "[@UG_TR_INTLFRGHT]");

                return !string.IsNullOrEmpty(lStrValue) ? Convert.ToInt32(lStrValue) : 0;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetFirstFolio: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetFirstFolio): " + ex.Message);
                LogService.WriteError(ex);
                return 0;
            }
        }

        internal SAPbobsCOM.UserTable GetFreight()
        {
            SAPbobsCOM.UserTable lObjsboTable = (SAPbobsCOM.UserTable)DIApplication.Company.UserTables.Item("UG_TR_INTLFRGHT");
            try
            {
                return lObjsboTable;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetFreight: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetFreight): " + ex.Message);
                LogService.WriteError(ex);
                return null;
            }

        }

        public List<CostingCodesDTO> GetCostingCodeList()
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            List<CostingCodesDTO>  lLstCostingCodes = new List<CostingCodesDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetCostingCodeList");

                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        lLstCostingCodes.Add(new CostingCodesDTO()
                        {
                            Name = lObjResults.Fields.Item("PrcName").Value.ToString(),
                            Code = lObjResults.Fields.Item("PrcCode").Value.ToString(),
                            Account = lObjResults.Fields.Item("U_TR_Account").Value.ToString(),

                        });

                        lObjResults.MoveNext();
                    }
                }


            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetCostingCodeList: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetCostingCodeList): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
            return lLstCostingCodes;
        }

        public string GetCreditAccount()
        {
            try
            {
                return mObjQueryManager.GetValue("U_Value", "Name", "TR_FLETE_INT", "[@UG_CONFIG] ");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetCreditAccount: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetCreditAccount): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public string GetDebitAccount()
        {
            try
            {
                return mObjQueryManager.GetValue("U_Value", "Name", "TR_FLETE_COSTO", "[@UG_CONFIG] ");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetDebitAccount: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetDebitAccount): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public float GetRetention(string pStrItemCode)
        {
            try
            {
                string lStrReturn = mObjQueryManager.GetValue("U_GLO_RetTax", "ItemCode", pStrItemCode, "OITM");

                return lStrReturn != null && !string.IsNullOrEmpty(lStrReturn) && float.Parse(lStrReturn) > 0 ? float.Parse(lStrReturn) : 0;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetRetention: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetRetention): " + ex.Message);
                LogService.WriteError(ex);
                return 0;
            }
        }

        internal int GetLastRouteId()
        {
            try
            {
                int lIntReturn = mObjQueryManager.Max<int>("Code", "[@UG_TR_RODS]");

                return lIntReturn;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetLastRouteId: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetLastRouteId): " + ex.Message);
                LogService.WriteError(ex);
                return 0;
            }
        }

        public int GetJournalId(string pStrFolio, string pStrTransCode)
        {
            SAPbobsCOM.Recordset lObjRecordSet = null;
            string lStrQuery = "";

            try
            {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                 Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("Folio", pStrFolio);
                lLstParams.Add("TransCode",  pStrTransCode);//TR/F
                lStrQuery = this.GetSQL("GetJournalId").Inject(lLstParams);

                lObjRecordSet.DoQuery(lStrQuery);

                return (int)lObjRecordSet.Fields.Item("TransId").Value;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetJournalId: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetJournalId): " + ex.Message);
                LogService.WriteError(ex);
                return 0;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
        }


        public string GetCancelJournal(string pStrFolio)
        {
            try
            {
                string lStrReturn = mObjQueryManager.GetValue("TransId", "StornoToTr", pStrFolio, "OJDT");

                return lStrReturn;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetRetention: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetRetention): " + ex.Message);
                LogService.WriteError(ex);
                return "0";
            }
        }

        public float GetTax(string pStrItemCode)
        {
            SAPbobsCOM.Recordset lObjRecordSet = null;
            string lStrQuery = "";

            try
            {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();

                lLstParams.Add("ItemCode", pStrItemCode);
                lStrQuery = this.GetSQL("GetTax").Inject(lLstParams);

                lObjRecordSet.DoQuery(lStrQuery);

                return float.Parse(lObjRecordSet.Fields.Item("Rate").Value.ToString());
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetJournalId: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetJournalId): " + ex.Message);
                LogService.WriteError(ex);
                return 0;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
        }


        public float GetTaxWT(string pStrItemCode)
        {
            string pStrWT = mObjQueryManager.GetValue("WtLiable", "ItemCode", pStrItemCode, "OITM");
            if (pStrWT == "Y")
            {
                SAPbobsCOM.Recordset lObjRecordSet = null;
                string lStrQuery = "";
                try
                {
                    lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    Dictionary<string, string> lLstParams = new Dictionary<string, string>();

                    lLstParams.Add("ItemCode", pStrItemCode);
                    lStrQuery = this.GetSQL("GetTaxWT").Inject(lLstParams);

                    lObjRecordSet.DoQuery(lStrQuery);
                    return float.Parse(lObjRecordSet.Fields.Item("Rate").Value.ToString());
                }
                catch (Exception ex)
                {
                    UIApplication.ShowError(string.Format("GetJournalId: {0}", ex.Message));
                    LogService.WriteError("RouteListDAO (GetJournalId): " + ex.Message);
                    LogService.WriteError(ex);
                    return 0;
                }
                finally
                {
                    MemoryUtility.ReleaseComObject(lObjRecordSet);
                }
            }
            else
            {
                return 0;
            }

        }

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


        public string ValidateItemSinKM(string pStrItemCode)
        {
            try
            {
                string lStrReturn = mObjQueryManager.GetValue("QryGroup33", "ItemCode", pStrItemCode, "OITM");

                return lStrReturn;
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetRetention: {0}", ex.Message));
                LogService.WriteError("RouteListDAO (GetRetention): " + ex.Message);
                LogService.WriteError(ex);
                return "0";
            }
        }
    }
}
