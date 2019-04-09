using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Transports.Tables;

namespace UGRS.Core.SDK.DI.Transports.Services
{
    public class CommissionLineService
    {
        private TableDAO<CommissionLine> mObjCommissionLineDAO = new TableDAO<CommissionLine>();


        public int UpdateCommissionLine(CommissionLine pObjCommissionLine)
        {
            return mObjCommissionLineDAO.Update(pObjCommissionLine);
        }

        public int AddCommissionLine(CommissionLine pObjCommissionLine)
        {
            return mObjCommissionLineDAO.Add(pObjCommissionLine);
        }
    }
}
