using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;

namespace UGRS.Core.SDK.DI.Transports.DAO
{
    public class BankDAO
    {
        public IList<BankDTO> GetBanks()
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<BankDTO> lLstObjBanks = new List<BankDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetBanks");
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        BankDTO lObjBank = new BankDTO();
                        lObjBank.BankCode = lObjResults.GetColumnValue<string>("BankCode");
                        lObjBank.BankName = lObjResults.GetColumnValue<string>("BankName");
                        lLstObjBanks.Add(lObjBank);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjBanks;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[BankDAO - GetBanks] Error al obtener los bancos: {0}", e.Message));
                throw new Exception(string.Format("Error al obtener los bancos: {0}", e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public IList<AccountDTO> GetBankAccounts(string pBankCode)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<AccountDTO> lLstObjBanks = new List<AccountDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetBankAccounts").InjectSingleValue("BankCode", pBankCode);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        AccountDTO lObjAccount = new AccountDTO();
                        lObjAccount.BankCode = lObjResults.GetColumnValue<string>("BankCode");
                        lObjAccount.Account = lObjResults.GetColumnValue<string>("Account");
                        lObjAccount.Branch = lObjResults.GetColumnValue<string>("Branch");
                        lObjAccount.GLAccount = lObjResults.GetColumnValue<string>("GLAccount");
                        lLstObjBanks.Add(lObjAccount);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjBanks;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[BankDAO - GetBankAccounts] Error al obtener las cuentas para el banco {0}: {1}", pBankCode, e.Message));
                throw new Exception(string.Format("Error al obtener las cuentas para el banco {0}: {1}", pBankCode, e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public AccountDTO GetBankAccount(string pStrBankAcctNumber)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            AccountDTO lObjAccount = new AccountDTO();
            try
            {
                string lStrQuery = this.GetSQL("GetAccountByAcctNumber").InjectSingleValue("BankAcctNumber", pStrBankAcctNumber);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        lObjAccount.BankCode = lObjResults.GetColumnValue<string>("BankCode");
                        lObjAccount.Account = lObjResults.GetColumnValue<string>("Account");
                        lObjAccount.Branch = lObjResults.GetColumnValue<string>("Branch");
                        lObjAccount.GLAccount = lObjResults.GetColumnValue<string>("GLAccount");
                        lObjResults.MoveNext();
                    }
                }
                return lObjAccount;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[BankDAO - GetBankAccount] Error al obtener la cuenta para el banco {0}: {1}", pStrBankAcctNumber, e.Message));
                throw new Exception(string.Format("Error al obtener las cuenta para el banco {0}: {1}", pStrBankAcctNumber, e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }
    }
}
