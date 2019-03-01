using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;

namespace UGRS.Core.SDK.DI.Finances.DAO
{
    public class PaymentDAO
    {
        public void InsertDocumentReference(int pIntCredMemoDocNum, int pIntObjType, int pIntLineNum, int pIntRefDocEntry, int pIntRefDocNum, int pIntRefObjType, string pStrLinkRefType, string pObjDate)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string lStrQuery = this.GetSQL("AddDraftReference");
                Dictionary<string, string> lObjParameters = new Dictionary<string, string>();
                lObjParameters.Add("DocEntry", pIntCredMemoDocNum.ToString());
                lObjParameters.Add("ObjectType", pIntObjType.ToString());
                lObjParameters.Add("LineNum", pIntLineNum.ToString());
                lObjParameters.Add("RefDocEntr", pIntRefDocEntry.ToString());
                lObjParameters.Add("RefDocNum", pIntRefDocNum.ToString());
                lObjParameters.Add("RefObjType", pIntRefObjType.ToString());
                lObjParameters.Add("LinkRefTyp", pStrLinkRefType);
                lObjParameters.Add("IssueDate", pObjDate);
                lStrQuery = lStrQuery.Inject(lObjParameters);
                lObjResults.DoQuery(lStrQuery);
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[PaymentDAO - InsertDocumentReference] Error al agregar la referencia de documento con DocEntry {0}, ObjectType {1}, LineNum {2}, RefDocEntr {3}, RefDocNum {4}, RefObjType {5}, LinkRefTyp {6}, IssueDate {7}: {8}"
                                                    , pIntCredMemoDocNum, pIntObjType, pIntLineNum, pIntRefDocEntry, pIntRefDocNum, pIntRefObjType, pStrLinkRefType, pObjDate, e.Message));
                throw new Exception(string.Format("Error al agregar la referencia de documento: {0}", e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }
    }
}
