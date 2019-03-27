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
                            Driver = lObjResults.Fields.Item("SlpName").Value.ToString(),
                            DriverId = lObjResults.Fields.Item("SlpCode").Value.ToString(),
                            FrgAm = Convert.ToDouble(lObjResults.Fields.Item("LineTotal").Value.ToString()),
                            InsAm = Convert.ToDouble(lObjResults.Fields.Item("U_Seguro").Value.ToString()),
                            //LstDisc = Convert.ToDouble(lObjResults.Fields.Item("").Value.ToString()),
                            WkDisc = Convert.ToDouble(lObjResults.Fields.Item("WkDisc").Value.ToString()),
                            //TotDisc = Convert.ToDouble(lObjResults.Fields.Item("").Value.ToString()),
                            Comm = Convert.ToDouble(lObjResults.Fields.Item("U_Commission").Value.ToString()),
                            DocEntry = lObjResults.Fields.Item("DocEntry").Value.ToString()
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
           return  mObjQueryManager.GetValue("Name", "U_Value", pStrUser, "[@UG_CONFIG]");
        }

        public IList<Commissions> GetCommissionsByFolio(string pStrFolio)
        {
            return mObjQueryManager.GetObjectsList<Commissions>("U_Folio", pStrFolio, "[@UG_TR_CMSN]");
        }
    }
}
