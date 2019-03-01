using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DAO;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class AuthorizationService
    {
        private AuthorizationDAO mObjAuthorizationDAO;

        public AuthorizationService()
        {
            mObjAuthorizationDAO = new AuthorizationDAO();
        }

        public bool IsOperationsUser(int lIntUserId, string lStrObjType, int lIntFunctionId)
        {
            return mObjAuthorizationDAO.IsOperationsUser(lIntUserId, lStrObjType, lIntFunctionId);
        }

        public bool IsOperationsUser(int pIntUserId)
        {
            return mObjAuthorizationDAO.IsOperationsUser(pIntUserId);
        }
    }
}
