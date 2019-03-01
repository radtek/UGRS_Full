using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class UsersService
    {
        private UsersDAO lObjUsersDAO;

        public UsersService()
        {
            lObjUsersDAO = new UsersDAO();
        }

        #region Entities
        public string GetCostCenter(string pStrUserId)
        {
            try
            {
                return new QueryManager().GetValue("U_GLO_CostCenter", "UserId", pStrUserId, "OUSR");
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[UsersService - GetCostCenter]: {0}", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO
        public int GetUserId(string lStrUsername)
        {
            return lObjUsersDAO.GetUserId(lStrUsername);
        }

        public string GetUserCenterCost(string pStrUsercode)
        {
            return lObjUsersDAO.GetUserCenterCost(pStrUsercode);
        }
        #endregion
    }
}
