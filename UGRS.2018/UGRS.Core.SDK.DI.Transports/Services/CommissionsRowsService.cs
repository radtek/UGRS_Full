using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Transports.Tables;

namespace UGRS.Core.SDK.DI.Transports.Services
{
    public class CommissionsRowsService
    {
        private TableDAO<CommissionsRows> mObjCommissionRowsDAO = new TableDAO<CommissionsRows>();

        public int UpdateCmsnRow(CommissionsRows pObjCommissionsRows)
        {
            return mObjCommissionRowsDAO.Update(pObjCommissionsRows);
        }

        public int AddCmsnRow(CommissionsRows pObjCommissionsRows)
        {
            return mObjCommissionRowsDAO.Add(pObjCommissionsRows);
        }
    }
}
