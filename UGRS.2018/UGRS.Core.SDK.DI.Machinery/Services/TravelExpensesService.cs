using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery.Tables;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class TravelExpensesService
    {
        private TableDAO<TravelExpenses> mObjTravelExpensesTableDAO;
        private TravelExpensesDAO mObjTravelExpensesDAO;

        public TravelExpensesService()
        {
            mObjTravelExpensesDAO = new TravelExpensesDAO();
            mObjTravelExpensesTableDAO = new TableDAO<TravelExpenses>();
        }

        #region Entities
        public int Add(TravelExpenses pObjTravelExpenses)
        {
            try
            {
                return mObjTravelExpensesTableDAO.Add(pObjTravelExpenses);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TravelExpensesService - Add]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(TravelExpenses pObjTravelExpenses)
        {
            try
            {
                return mObjTravelExpensesTableDAO.Update(pObjTravelExpenses);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TravelExpensesService - Update]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjTravelExpensesTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TravelExpensesService - GetLastCode]: {0}", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO
        public string GetNexFolio(string pStrCostCenter)
        {
            return mObjTravelExpensesDAO.GetNexFolio(pStrCostCenter);
        }

        public string GetCurrentFolio(string pStrRiseId)
        {
            return mObjTravelExpensesDAO.GetCurrentFolio(pStrRiseId);
        }

        public TravelExpensesDTO GetPayment(int pIntDocEntry)
        {
            return mObjTravelExpensesDAO.GetPayment(pIntDocEntry);
        }

        public List<TravelExpensesDTO> GetDraftPaymentsByRiseId(int pIntRiseId, string pStrObjType)
        {
            return mObjTravelExpensesDAO.GetDraftPaymentsByRiseId(pIntRiseId, pStrObjType);
        }

        public List<TravelExpensesDTO> GetPaymentsByRiseId(int pIntRiseId, string pStrObjType)
        {
            return mObjTravelExpensesDAO.GetPaymentsByRiseId(pIntRiseId, pStrObjType);
        }

        public List<TravelExpensesDTO> GetTravelExpensesByRiseId(int pIntRiseId, bool pBolForDraftPymts = false)
        {
            return mObjTravelExpensesDAO.GetTravelExpensesByRiseId(pIntRiseId, pBolForDraftPymts);
        }
        #endregion
    }
}
