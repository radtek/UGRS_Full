using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Purchases.Tables;

namespace UGRS.Core.SDK.DI.Purchases.Services
{
    public class SetupService
    {
        private TableDAO<Vouchers> mObjVouchersDAO;
        private TableDAO<VouchersDetail> mObjVouchersDetailDAO;

        public SetupService()
        {
            mObjVouchersDAO = new TableDAO<Vouchers>();
            mObjVouchersDetailDAO = new TableDAO<VouchersDetail>();
        }

        public void InitializeTables()
        {
            mObjVouchersDAO.Initialize();
            mObjVouchersDetailDAO.Initialize();
        }
    }
}
