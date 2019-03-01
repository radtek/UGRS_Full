using System;
using System.Collections.Generic;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Finances.DTO;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Finances.DAO
{
    public class CheckDAO
    {
        public IList<CheckDTO> GetChecks()
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<CheckDTO> lLstChecks = new List<CheckDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetChecks");
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        CheckDTO lObjBank = new CheckDTO();
                        lObjBank.CheckKey = lObjResults.GetColumnValue<int>("CheckKey");
                        lObjBank.CheckNum = lObjResults.GetColumnValue<int>("CheckNum");
                        lObjBank.CheckSum = lObjResults.GetColumnValue<double>("CheckSum");
                        lObjBank.CardName = lObjResults.GetColumnValue<string>("CardName");
                        lObjBank.CardCode = lObjResults.GetColumnValue<string>("CardCode");
                        lObjBank.CheckDate = lObjResults.GetColumnValue<DateTime>("CheckDate");
                        lObjBank.Deposited = lObjResults.GetColumnValue<string>("Deposited");
                        lObjBank.BankAcct = lObjResults.GetColumnValue<string>("BankAcct");
                        lObjBank.Currency = lObjResults.GetColumnValue<string>("Currency");
                        lLstChecks.Add(lObjBank);
                        lObjResults.MoveNext();
                    }
                }
                return lLstChecks;
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[CheckDAO - GetChecks] Error al obtener los cheques: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al obtener los cheques: {0}", lObjException.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public CheckDTO GetCheckByAttributes(int pDocEntry, string pBankAcct, int pCheckNum, double pCheckSum)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                Dictionary<string, string> lObjParameters = new Dictionary<string, string>();
                lObjParameters.Add("DocEntry", pDocEntry.ToString());
                lObjParameters.Add("BankAcct", pBankAcct);
                lObjParameters.Add("CheckNum", pCheckNum.ToString());
                lObjParameters.Add("CheckSum", pCheckSum.ToString());

                string lStrQuery = this.GetSQL("GetCheckByAttributes").Inject(lObjParameters);

                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    CheckDTO lObjBank = new CheckDTO();
                    lObjBank.CheckKey = lObjResults.GetColumnValue<int>("CheckKey");
                    lObjBank.CheckNum = lObjResults.GetColumnValue<int>("CheckNum");
                    lObjBank.CheckSum = lObjResults.GetColumnValue<double>("CheckSum");
                    lObjBank.CardName = lObjResults.GetColumnValue<string>("CardName");
                    lObjBank.CardCode = lObjResults.GetColumnValue<string>("CardCode");
                    lObjBank.CheckDate = lObjResults.GetColumnValue<DateTime>("CheckDate");
                    lObjBank.Deposited = lObjResults.GetColumnValue<string>("Deposited");
                    lObjBank.BankAcct = lObjResults.GetColumnValue<string>("BankAcct");
                    return lObjBank;
                }
                return null;

            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[CheckDAO - GetCheckByAttributes] Error al obtener el cheque: {0}", e.Message));
                throw new Exception(string.Format("Error al obtener el cheque: {0}", e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public string GetCheckAcct(string pStrUserCode)
        {
            SAPbobsCOM.Recordset lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            string lStrChkAcct = string.Empty;
            try
            {
                string lStrQuery = this.GetSQL("GetCheckAccount").InjectSingleValue("UserCode", pStrUserCode);

                lObjRecordset.DoQuery(lStrQuery);
                if (lObjRecordset.RecordCount > 0)
                {
                    lStrChkAcct = lObjRecordset.GetColumnValue<string>("CashAcct");
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[CheckDAO - GetCheckAcct] Error al obtener la cuenta del cheque: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al obtener la cuenta del cheque: {0}", lObjException.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lStrChkAcct;
        }
    }
}
