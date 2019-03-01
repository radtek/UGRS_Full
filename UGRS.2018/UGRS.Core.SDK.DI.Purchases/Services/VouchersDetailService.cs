using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Purchases.Tables;

namespace UGRS.Core.SDK.DI.Purchases.Services
{
    public class VouchersDetailService
    {
        private TableDAO<VouchersDetail> mObjVouchersDetailDAO;

        public VouchersDetailService()
        {
            mObjVouchersDetailDAO = new TableDAO<VouchersDetail>();
        }

        public int Add(VouchersDetail pObjVouchersDetail)
        {
            return mObjVouchersDetailDAO.Add(pObjVouchersDetail);
        }

        public int Update(VouchersDetail pObjVouchersDetail)
        {
            return mObjVouchersDetailDAO.Update(pObjVouchersDetail);
        }



    }
}
