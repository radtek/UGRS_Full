using System;
using System.Collections.Generic;

using UGRS.Core.SDK.DI.Extension;

using UGRS.Core.Extension;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Transports.Tables;

namespace UGRS.Core.SDK.DI.Transports.DAO
{
    public class CommissionDAO
    {
        QueryManager mObjQueryManager = new QueryManager();
        public List<CommissionDriverDTO> GetComissionsDrivers(string pStrDateStart, string pStrDateEnd)
        {
            List<CommissionDriverDTO> lLstComissionDTO = new List<CommissionDriverDTO>();
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();

                lLstParams.Add("DateStart", pStrDateStart);
                lLstParams.Add("DateEnd", pStrDateEnd);

                string lStrQuery = this.GetSQL("GetCommissionsDriver").Inject(lLstParams);

                lObjResults.DoQuery(lStrQuery);

                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        lLstComissionDTO.Add(new CommissionDriverDTO()
                        {
                            Type = lObjResults.Fields.Item("Type").Value.ToString(),
                            Id = lObjResults.Fields.Item("Id").Value.ToString(),
                            DocNum = lObjResults.Fields.Item("Id").Value.ToString(),
                            DocDate = Convert.ToDateTime(lObjResults.Fields.Item("DocDate").Value.ToString()),
                            DriverId = lObjResults.Fields.Item("empID").Value.ToString(),
                            Driver = lObjResults.Fields.Item("SlpName").Value.ToString(),
                            ItemName = lObjResults.Fields.Item("ItemName").Value.ToString(),
                            Route = lObjResults.Fields.Item("Route").Value.ToString(),
                            AF = lObjResults.Fields.Item("AF").Value.ToString(),
                            TypLoad = lObjResults.Fields.Item("TypLoad").Value.ToString(),
                            FrgAm = Convert.ToDouble(lObjResults.Fields.Item("LineTotal").Value.ToString()),
                            InsAm = Convert.ToDouble(lObjResults.Fields.Item("U_Seguro").Value.ToString()),
                            //LstDisc = Convert.ToDouble(lObjResults.Fields.Item("").Value.ToString()),
                            WkDisc = Convert.ToDouble(lObjResults.Fields.Item("WkDisc").Value.ToString()),
                            //TotDisc = Convert.ToDouble(lObjResults.Fields.Item("").Value.ToString()),
                            Comm = Convert.ToDouble(lObjResults.Fields.Item("U_Commission").Value.ToString()),
                            // NoGenerate = lObjResults.Fields.Item("U_NoGenerate").Value.ToString() == "N" ? false : true
                            //TotComm = 
                            // Doubt = Convert.ToDouble(lObjResults.Fields.Item("").Value.ToString()),
                        });
                        lObjResults.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("CommissionDAO (GetComissionsDrivers): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowError(string.Format("GetComissionsDrivers: {0}", ex.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
            return lLstComissionDTO;
        }

        public List<CommissionDriverDTO> GetComissionsDriverSaved(string pStrCommissionSaved)
        {
            List<CommissionDriverDTO> lLstComissionDTO = new List<CommissionDriverDTO>();
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();

                lLstParams.Add("CommissionID", pStrCommissionSaved);

                string lStrQuery = this.GetSQL("GetCommissionsDriverSaved").Inject(lLstParams);

                lObjResults.DoQuery(lStrQuery);

                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        lLstComissionDTO.Add(new CommissionDriverDTO()
                        {
                            Type = lObjResults.Fields.Item("Type").Value.ToString(),
                            Id = lObjResults.Fields.Item("Id").Value.ToString(),
                            DocNum = lObjResults.Fields.Item("Id").Value.ToString(),
                            DocDate = Convert.ToDateTime(lObjResults.Fields.Item("DocDate").Value.ToString()),
                            DriverId = lObjResults.Fields.Item("empID").Value.ToString(),
                            Driver = lObjResults.Fields.Item("SlpName").Value.ToString(),
                            ItemName = lObjResults.Fields.Item("ItemName").Value.ToString(),
                            Route = lObjResults.Fields.Item("Route").Value.ToString(),
                            AF = lObjResults.Fields.Item("AF").Value.ToString(),
                            TypLoad = lObjResults.Fields.Item("TypLoad").Value.ToString(),
                            FrgAm = Convert.ToDouble(lObjResults.Fields.Item("LineTotal").Value.ToString()),
                            InsAm = Convert.ToDouble(lObjResults.Fields.Item("U_Seguro").Value.ToString()),
                            //LstDisc = Convert.ToDouble(lObjResults.Fields.Item("").Value.ToString()),
                            WkDisc = Convert.ToDouble(lObjResults.Fields.Item("WkDisc").Value.ToString()),
                            //TotDisc = Convert.ToDouble(lObjResults.Fields.Item("").Value.ToString()),
                            Comm = Convert.ToDouble(lObjResults.Fields.Item("U_Commission").Value.ToString()),

                            //TotComm = 
                            // Doubt = Convert.ToDouble(lObjResults.Fields.Item("").Value.ToString()),
                        });
                        lObjResults.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("CommissionDAO (GetComissionsDrivers): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowError(string.Format("GetComissionsDrivers: {0}", ex.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
            return lLstComissionDTO;
        }


        public List<CommissionDTO> GetComission(string pStrDriverId, string pStrDateStart, string pStrDateEnd)
        {
            List<CommissionDTO> lLstComissionDTO = new List<CommissionDTO>();
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("DateStart", pStrDateStart);
                lLstParams.Add("DateEnd", pStrDateEnd);
                lLstParams.Add("DriverId", pStrDriverId);
                string lStrQuery = this.GetSQL("GetCommissionsInv").Inject(lLstParams);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        lLstComissionDTO.Add(new CommissionDTO()
                        {
                            DocEntry = lObjResults.Fields.Item("DocEntry").Value.ToString(),
                            Date = Convert.ToDateTime(lObjResults.Fields.Item("DocDate").Value.ToString()),
                            InvFol = lObjResults.Fields.Item("DocNum").Value.ToString(),
                            OpType = lObjResults.Fields.Item("OpType").Value.ToString(),
                            Route = lObjResults.Fields.Item("U_TR_Paths").Value.ToString(),
                            Vcle = lObjResults.Fields.Item("U_TR_VehicleType").Value.ToString(),
                            PyId = lObjResults.Fields.Item("U_TR_LoadType").Value.ToString(),
                            Amnt = Convert.ToDouble(lObjResults.Fields.Item("LineTotal").Value.ToString()),
                            Ins = Convert.ToDouble(lObjResults.Fields.Item("U_Seguro").Value.ToString()),
                            Cmsn = Convert.ToDouble(lObjResults.Fields.Item("U_Commission").Value.ToString()),
                        });
                        lObjResults.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("CommissionDAO (GetComission): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowError(string.Format("GetComission: {0}", ex.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
            return lLstComissionDTO;
        }

        public string GetFirstDay(int pIntYear)
        {
            string lStrFirstDay = "";

            try
            {
                lStrFirstDay = mObjQueryManager.GetValue("U_FirstDay", "U_Year", pIntYear.ToString(), "[@UG_TR_DAY]");

            }
            catch (Exception lObjException)
            {

                LogService.WriteError("GetFirstDay: " + lObjException.Message);
                LogService.WriteError(lObjException);
                UIApplication.ShowError(string.Format("GetFirstDay: {0}", lObjException.Message));
            }
            return lStrFirstDay;
        }

        public string GetLastComisionId()
        {
            string lStrValue = mObjQueryManager.Max<string>("Code", "[@UG_TR_CMSN]");
            return lStrValue;
        }

        public List<string> GetAuthorizers(string pStrConfigName)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            List<string> lStrUsers = new List<string>();
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("ConfigName", pStrConfigName);
                string lStrQuery = this.GetSQL("GetAuthorizers").Inject(lLstParams);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        lStrUsers.Add(lObjResults.Fields.Item("U_Value").Value.ToString());
                        lObjResults.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("CommissionDAO (GetAauthorizers): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowError(string.Format("GetAauthorizers: {0}", ex.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
            return lStrUsers;
        }

        public string GetUserAuthorization(string pStrUser)
        {
            return mObjQueryManager.GetValue("Name", "U_Value", pStrUser, "[@UG_CONFIG]"); ;
        }

        public string GetAsset(string pStrOcrName)
        {
            return mObjQueryManager.GetValue("OcrCode", "OcrName", pStrOcrName, "OOCR");
        }

        public string GetAccountConfig(string pStrAccountConfig)
        {
            string lStrValue = mObjQueryManager.GetValue("U_Value", "Name", pStrAccountConfig, "[@UG_CONFIG]");

            if (string.IsNullOrEmpty(lStrValue))
            {
                LogService.WriteError("CommissionDAO (GetAccountConfig): Configuracion no encontrada:" + pStrAccountConfig);
                UIApplication.ShowError(string.Format("CommissionDAO (GetAccountConfig): Configuracion no encontrada: {0}", pStrAccountConfig));
            }
            return lStrValue;
        }

        public IList<Commissions> GetCommissionsByFolio(string pStrFolio)
        {
            return mObjQueryManager.GetObjectsList<Commissions>("U_Folio", pStrFolio, "[@UG_TR_CMSN]");
        }

        public IList<CommissionLine> GetCommissionLine(string pStrCmsnId)
        {
            return mObjQueryManager.GetObjectsList<CommissionLine>("U_CommisionId", pStrCmsnId, "[@UG_TR_CMLN]");
        }

        public Commissions GetCommission(string pStrFolio)
        {
            return mObjQueryManager.GetTableObject<Commissions>("U_Folio", pStrFolio, "[@UG_TR_CMSN]");
        }

        public IList<Commissions> GetCommissionsByRowCode(string pStrCode)
        {
            return mObjQueryManager.GetObjectsList<Commissions>("Code", pStrCode, "[@UG_TR_CMSN]");
        }

        public List<CommissionDebtDTO> GetCommissionDebt(string pStrFolio, string pStrAccount, string pStrTypeAux, string pStrAux)
        {
            List<CommissionDebtDTO> lLstDebt = new List<CommissionDebtDTO>();
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("Folio", pStrFolio);
                lLstParams.Add("Account", pStrAccount);
                lLstParams.Add("TypeAux", pStrTypeAux);
                lLstParams.Add("Aux", pStrAux);


                string lStrQuery = this.GetSQL("GetCommissionDebt").Inject(lLstParams);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        CommissionDebtDTO lObjDebt = new CommissionDebtDTO();
                        lObjDebt.Debit = Convert.ToDouble(lObjResults.Fields.Item("Debit").Value.ToString());
                        lObjDebt.Credit = Convert.ToDouble(lObjResults.Fields.Item("Credit").Value.ToString());
                        lObjDebt.Folio = lObjResults.Fields.Item("Ref1").Value.ToString();
                        
                        lObjDebt.Importe = Convert.ToDecimal(lObjDebt.Debit) - Convert.ToDecimal(lObjDebt.Credit);
                        lLstDebt.Add(lObjDebt);
                        lObjResults.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("CommissionDAO (GetCommissionDebt): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowError(string.Format("GetCommissionDebt: {0}", ex.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
            return lLstDebt;
        }


        public List<CommissionDriverDTO> GetListDrivers()
        {
            List<CommissionDriverDTO> lLstDrivers = new List<CommissionDriverDTO>();
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                string lStrQuery = this.GetSQL("GetCmsnDrivers").Inject(lLstParams);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {

                        lLstDrivers.Add(new CommissionDriverDTO()
                        {
                            DriverId = lObjResults.Fields.Item("empID").Value.ToString(),
                            Driver = lObjResults.Fields.Item("SlpName").Value.ToString(),
                            WkDisc = Convert.ToDouble(lObjResults.Fields.Item("WkDisc").Value.ToString()),
                        });
                        lObjResults.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("CommissionDAO (GetComission): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowError(string.Format("GetComission: {0}", ex.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
            return lLstDrivers;
        }


        public SalesOrderLinesDTO GetSalesOrderLinesDTO(string pStrDocNum, string pSTrTicket)
        {
            SalesOrderLinesDTO mObjSalesOrderLines = new SalesOrderLinesDTO();
            List<CommissionDebtDTO> lLstDebt = new List<CommissionDebtDTO>();
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("DocNum", pStrDocNum);
                lLstParams.Add("Ticket", pSTrTicket);
                string lStrQuery = this.GetSQL("GetSOLines").Inject(lLstParams);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        mObjSalesOrderLines.ItemCode = lObjResults.Fields.Item("ItemCode").Value.ToString();
                        mObjSalesOrderLines.Description = lObjResults.Fields.Item("ItemName").Value.ToString();
                        mObjSalesOrderLines.Folio = lObjResults.Fields.Item("U_GLO_Ticket").Value.ToString();
                        mObjSalesOrderLines.Shared = lObjResults.Fields.Item("U_TR_Compartido").Value.ToString() == "N" ? false : true;
                        mObjSalesOrderLines.PayloadType = lObjResults.Fields.Item("U_TR_LoadType").Value.ToString();
                        mObjSalesOrderLines.VehicleType = lObjResults.Fields.Item("U_TR_VehicleType").Value.ToString();

                        mObjSalesOrderLines.Route = Convert.ToInt16(lObjResults.Fields.Item("U_TR_Paths").Value.ToString());
                        mObjSalesOrderLines.Employee = lObjResults.Fields.Item("SlpName").Value.ToString();
                        mObjSalesOrderLines.Asset = lObjResults.Fields.Item("OcrCode2").Value.ToString();
                        mObjSalesOrderLines.TotKm = lObjResults.Fields.Item("U_TR_TotKm").Value.ToString();
                        mObjSalesOrderLines.Extra = lObjResults.Fields.Item("U_TR_AdditionalExpense").Value.ToString();
                        mObjSalesOrderLines.KmA = lObjResults.Fields.Item("U_TR_TypeA").Value.ToString();
                        mObjSalesOrderLines.KmB = lObjResults.Fields.Item("U_TR_TypeB").Value.ToString();
                        mObjSalesOrderLines.KmC = lObjResults.Fields.Item("U_TR_TypeC").Value.ToString();
                        mObjSalesOrderLines.KmD = lObjResults.Fields.Item("U_TR_TypeD").Value.ToString();
                        mObjSalesOrderLines.KmE = lObjResults.Fields.Item("U_TR_TypeE").Value.ToString();
                        mObjSalesOrderLines.KmF = lObjResults.Fields.Item("U_TR_TypeF").Value.ToString();
                        mObjSalesOrderLines.Heads = lObjResults.Fields.Item("U_TR_Heads").Value.ToString();
                        mObjSalesOrderLines.TotKg = lObjResults.Fields.Item("U_TR_TotKilos").Value.ToString();
                        mObjSalesOrderLines.Bags = lObjResults.Fields.Item("U_GLO_BagsBales").Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("CommissionDAO (GetCommissionDebt): " + ex.Message);
                LogService.WriteError(ex);
                UIApplication.ShowError(string.Format("GetCommissionDebt: {0}", ex.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
            return mObjSalesOrderLines;
        }
    }
}
