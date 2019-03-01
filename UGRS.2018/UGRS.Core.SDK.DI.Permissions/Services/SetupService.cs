using UGRS.AddOn.Permissions.Tables;
using UGRS.Core.SDK.DI.DAO;

namespace UGRS.AddOn.Permissions.Services
{
    public class SetupService
    {
        private TableDAO<EarringRanksT> mObjEarringRanksDAO;

        public SetupService()
        {
            mObjEarringRanksDAO = new TableDAO<EarringRanksT>();
        }

        public void InitializeTables()
        {
            mObjEarringRanksDAO.Initialize();
        }
    }
}
