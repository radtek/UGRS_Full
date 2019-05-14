using System.Collections.Generic;
using UGRS.Core.SDK.DI.Transports.DAO;
using UGRS.Core.SDK.DI.Transports.DTO;

namespace UGRS.Core.SDK.DI.Transports.Services
{
    public class BankService
    {
        private BankDAO mObjBankDAO;

        public BankService()
        {
            mObjBankDAO = new BankDAO();
        }

        #region Banks
        public IList<BankDTO> GetBanks()
        {
            return mObjBankDAO.GetBanks();
        }

        public IList<AccountDTO> GetBankAccounts(string pBankCode)
        {
            return mObjBankDAO.GetBankAccounts(pBankCode);
        }

        public AccountDTO GetBankAccount(string pStrBankAcctNumber)
        {
            return mObjBankDAO.GetBankAccount(pStrBankAcctNumber);
        }
        #endregion
    }
}
