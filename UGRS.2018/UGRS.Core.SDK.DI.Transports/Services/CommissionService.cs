using System.Collections.Generic;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Transports.DAO;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.SDK.DI.Transports.Tables;

namespace UGRS.Core.SDK.DI.Transports.Services
{
    public class CommissionService
    {
        private CommissionDAO mObjCommissionDAO = new CommissionDAO();
        private TableDAO<Commissions> mObjCommissionsDAO = new TableDAO<Commissions>();

        public int UpdateCommission(Commissions pObjCommissions)
        {
            return mObjCommissionsDAO.Update(pObjCommissions);
        }

        public int AddCommission(Commissions pObjCommissions)
        {
            return mObjCommissionsDAO.Add(pObjCommissions);
        }

        public List<CommissionDriverDTO> GetCommissionDriver(string pStrDateStart, string pStrDateEnd)
        {
            return mObjCommissionDAO.GetComissionsDrivers(pStrDateStart, pStrDateEnd);
        }

        public List<CommissionDriverDTO> GetCommissionDriverSaved(string pStrCommissionId)
        {
            return mObjCommissionDAO.GetComissionsDriverSaved(pStrCommissionId);
        }

        public List<CommissionDriverDTO> GetCommissionDriverLine(string pStrFolio)
        {
            return mObjCommissionDAO.GetComissionsDriversLineSaved(pStrFolio);
        }

        public List<CommissionDTO> GetCommission(string pStrDriverId, string pStrDateStart, string pStrDateEnd)
        {
            return mObjCommissionDAO.GetComission(pStrDriverId, pStrDateStart, pStrDateEnd);
        }

        public string GetFirstDay(int pIntYear)
        {
            return mObjCommissionDAO.GetFirstDay(pIntYear);
        }

        public string GetLastCommissionId()
        {
            return mObjCommissionDAO.GetLastComisionId();
        }

        public List<string> GetAuthorizers(string pStrConfigName)
        {
            return mObjCommissionDAO.GetAuthorizers(pStrConfigName);
        }

        public string GetAccountConfig(string pStrAccountConfig)
        {
            return mObjCommissionDAO.GetAccountConfig(pStrAccountConfig);
        }

        public string GetUSerAuthorization(string pStrUser)
        {
            return mObjCommissionDAO.GetUserAuthorization(pStrUser);
        }

        public IList<Commissions> GetCommissionByFolio(string pStrFolio)
        {
            return mObjCommissionDAO.GetCommissionsByFolio(pStrFolio);
        }

        public Commissions GetCommission(string pStrFolio)
        {
            return mObjCommissionDAO.GetCommission(pStrFolio);
        }
        public IList<Commissions> GetCommissionByRowCode(string pStrCode)
        {
            return mObjCommissionDAO.GetCommissionsByRowCode(pStrCode);
        }

        public List<CommissionDebtDTO> GetCommissionDebtDTO(string pStrFolio, string pStrAccount, string pStrTypeAux, string pStrAux)
        {
            return mObjCommissionDAO.GetCommissionDebt(pStrFolio, pStrAccount, pStrTypeAux, pStrAux);
        }

        public List<CommissionDriverDTO> GetListDrivers()
        {
            return mObjCommissionDAO.GetListDrivers();
        }

        public IList<CommissionLine> GetCommissionLine(string pStrFolio)
        {
            return mObjCommissionDAO.GetCommissionLine(pStrFolio);
        }

        public SalesOrderLinesDTO SalesOrdeLines(string pStrFolio, string pStrTicket)
        {
            return mObjCommissionDAO.GetSalesOrderLinesDTO(pStrFolio, pStrTicket);
        }

        public string GetAsset(string pStrOcrName)
        {
            return mObjCommissionDAO.GetAsset(pStrOcrName);
        }
    }
}
