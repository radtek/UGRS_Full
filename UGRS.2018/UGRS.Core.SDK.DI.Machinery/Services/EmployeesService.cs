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
    public class EmployeesService
    {
        private TableDAO<Employees> mObjEmployeesTableDAO;
        private EmployeesDAO mObjEmployeesDAO;

        public EmployeesService()
        {
            mObjEmployeesTableDAO = new TableDAO<Employees>();
            mObjEmployeesDAO = new EmployeesDAO();
        }

        #region DAO
        public List<EmployeesDTO> GetEmployeesByRiseId(int pIntRiseId)
        {
            return mObjEmployeesDAO.GetEmployeesByRiseId(pIntRiseId);
        }
        #endregion

        #region Entities
        public int Add(Employees pObjEmployees)
        {
            try
            {
                int result = mObjEmployeesTableDAO.Add(pObjEmployees);
                //string lStrCode = new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjEmployeesTableDAO.GetUserTableName()));

                return result;
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[EmployeesService - Add: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(Employees pObjEmployees)
        {
            try
            {
                return mObjEmployeesTableDAO.Update(pObjEmployees);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[EmployeesService - Update: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public int Remove(string pStrCode)
        {
            try
            {
                return mObjEmployeesTableDAO.Remove(pStrCode);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[EmployeesService - Remove: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public IList<Employees> GetByRiseId(int pIntRiseId)
        {
            try
            {
                return new QueryManager().GetObjectsList<Employees>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjEmployeesTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[EmployeesService - GetByRiseId: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjEmployeesTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[EmployeesService - GetLastCode: {0}]", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region Extras
        public IList<EmployeesDTO> ToDTO(List<Employees> pLstEmployees)
        {
            return pLstEmployees.Select(x => new EmployeesDTO
                                        {

                                        }).ToList();
        }
        #endregion
    }
}
