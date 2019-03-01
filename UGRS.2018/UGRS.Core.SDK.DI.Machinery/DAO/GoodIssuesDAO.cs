using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Exceptions;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Machinery.DTO;
using SAPbobsCOM;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class GoodIssuesDAO
    {
        public List<InventoryItemsLinesDTO> GetItemsByRiseId(int pIntRiseId)
        {
            List<InventoryItemsLinesDTO> lLstGoodIssuesLines = new List<InventoryItemsLinesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetItemsForGoodIssue").InjectSingleValue("RiseId", pIntRiseId.ToString());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        InventoryItemsLinesDTO lObjGoodIssueLine = new InventoryItemsLinesDTO
                        {
                            ItemCode = lObjRecordset.Fields.Item("ItemCode").Value.ToString(),
                            ItemName = lObjRecordset.Fields.Item("ItemName").Value.ToString(),
                            OriginalQty = double.Parse(lObjRecordset.Fields.Item("Qty").Value.ToString()),
                            Quantity = double.Parse(lObjRecordset.Fields.Item("Quantity").Value.ToString()),
                            Category = lObjRecordset.Fields.Item("Category").Value.ToString(),
                        };

                        lLstGoodIssuesLines.Add(lObjGoodIssueLine);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[GoodIssuesDAO - GetItemsByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstGoodIssuesLines;
        }

        public List<InventoryItemsLinesDTO> GetTotalsRiseId(int pIntRiseId)
        {
            List<InventoryItemsLinesDTO> lLstGoodIssuesLines = new List<InventoryItemsLinesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetGoodIssueItemsTotalsByRiseId").InjectSingleValue("RiseId", pIntRiseId.ToString());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        InventoryItemsLinesDTO lObjGoodIssueLine = new InventoryItemsLinesDTO
                        {
                            ItemCode = lObjRecordset.Fields.Item("ItemCode").Value.ToString(),
                            ItemName = lObjRecordset.Fields.Item("ItemName").Value.ToString(),
                            OriginalQty = 0,
                            Quantity = double.Parse(lObjRecordset.Fields.Item("Quantity").Value.ToString()),
                            Category = lObjRecordset.Fields.Item("Category").Value.ToString(),
                        };

                        lLstGoodIssuesLines.Add(lObjGoodIssueLine);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[GoodIssuesDAO - GetTotalsRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstGoodIssuesLines;
        }

        public List<InventoryItemsLinesDTO> GetRiseItemsForStockTransfer(int pIntRiseId)
        {
            List<InventoryItemsLinesDTO> lLstGoodIssuesLines = new List<InventoryItemsLinesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetRiseItemsForStockTransfer").InjectSingleValue("RiseId", pIntRiseId.ToString());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        InventoryItemsLinesDTO lObjGoodIssueLine = new InventoryItemsLinesDTO
                        {
                            ItemCode = lObjRecordset.Fields.Item("ItemCode").Value.ToString(),
                            ItemName = lObjRecordset.Fields.Item("ItemName").Value.ToString(),
                            OriginalQty = double.Parse(lObjRecordset.Fields.Item("Qty").Value.ToString()),
                            Quantity = double.Parse(lObjRecordset.Fields.Item("Quantity").Value.ToString()),
                            Category = lObjRecordset.Fields.Item("Category").Value.ToString(),
                        };

                        lLstGoodIssuesLines.Add(lObjGoodIssueLine);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[GoodIssuesDAO - GetRiseItemsForStockTransfer: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstGoodIssuesLines;
        }
    }
}
