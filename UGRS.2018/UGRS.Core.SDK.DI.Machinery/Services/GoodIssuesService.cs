using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class GoodIssuesService
    {
        private GoodIssuesDAO mObjGoodIssuesDAO;

        public GoodIssuesService()
        {
            mObjGoodIssuesDAO = new GoodIssuesDAO();
        }

        #region Entities
        public bool ExistsGoodIssue(int pIntRiseId)
        {
            try
            {
                return new QueryManager().Exists("U_MQ_Rise", pIntRiseId.ToString(), "OIGE");
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[GoodIssuesService - ExistsGoodIssue: {0}]", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO
        public List<InventoryItemsLinesDTO> GetItemsByRiseId(int pIntRiseId)
        {
            return mObjGoodIssuesDAO.GetItemsByRiseId(pIntRiseId);
        }

        public List<InventoryItemsLinesDTO> GetTotalsRiseId(int pIntRiseId)
        {
            return mObjGoodIssuesDAO.GetTotalsRiseId(pIntRiseId);
        }

        public List<InventoryItemsLinesDTO> GetRiseItemsForStockTransfer(int pIntRiseId)
        {
            return mObjGoodIssuesDAO.GetRiseItemsForStockTransfer(pIntRiseId);
        }
        #endregion

        #region Extras
        public List<InventoryItemsLinesDTO> DataTableToDTO(SAPbouiCOM.DataTable pObjDataTable)
        {
            List<InventoryItemsLinesDTO> lLstGoodIssuesLines = new List<InventoryItemsLinesDTO>();

            for (int i = 0; i < pObjDataTable.Rows.Count; i++)
            {
                InventoryItemsLinesDTO lObjGoodIssuesLine = new InventoryItemsLinesDTO();

                lObjGoodIssuesLine.ItemCode = pObjDataTable.GetValue("ItemCode", i).ToString();
                lObjGoodIssuesLine.ItemName = pObjDataTable.GetValue("ItemName", i).ToString();
                //lObjGoodIssuesLine.OriginalQty = double.Parse(pObjDataTable.GetValue("OrigQty", i).ToString());
                lObjGoodIssuesLine.Quantity = double.Parse(pObjDataTable.GetValue("Quantity", i).ToString());
                lObjGoodIssuesLine.ActivoFijo = pObjDataTable.GetValue("ActiveF", i).ToString();
                lObjGoodIssuesLine.Category = pObjDataTable.GetValue("Category", i).ToString();

                lLstGoodIssuesLines.Add(lObjGoodIssuesLine);
            }

            return lLstGoodIssuesLines;
        }
        #endregion
    }
}
