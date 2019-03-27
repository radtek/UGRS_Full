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

        public string GetUSerAuthorization(string pStrUser)
        {
            return mObjCommissionDAO.GetUserAuthorization(pStrUser);
        }

        public IList<Commissions> GetCommissionByFolio(string pStrFolio)
        {
            return mObjCommissionDAO.GetCommissionsByFolio(pStrFolio);
        }
    }
}
