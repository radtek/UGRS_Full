using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Exceptions;
using UGRS.Core.SDK.DI.Corrals.DTO;
using UGRS.Core.Services;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;
using UGRS.Core.SDK.UI;


namespace UGRS.Core.SDK.DI.Corrals.DAO
{
    public class DraftDAO
    {
        public List<DraftDTO> GetDraftInvoices(string pStrType)
        {
            List<DraftDTO> lLstInvoiceDTO = new List<DraftDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrDate = DateTime.Now.ToString("ddMMyy");

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Date", lStrDate);
                lLstStrParameters.Add("Type", pStrType);

                string lStrQuery = this.GetSQL("GetDraftsInvoice").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        DraftDTO lObjInvoiceDTO = new DraftDTO
                        {
                            DocEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString()),
                            DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                            Status = lObjRecordset.Fields.Item("DocStatus").Value.ToString(),
                            GLO_Status = lObjRecordset.Fields.Item("U_GLO_Status").Value.ToString(),
                            DocDate = DateTime.Parse(lObjRecordset.Fields.Item("DocDate").Value.ToString()),
                            CardCode = lObjRecordset.Fields.Item("CardCode").Value.ToString(),
                            CardName = lObjRecordset.Fields.Item("CardName").Value.ToString(),
                            Currency = lObjRecordset.Fields.Item("DocCur").Value.ToString(),
                            Total = double.Parse(lObjRecordset.Fields.Item("Total").Value.ToString()),
                            Comments = lObjRecordset.Fields.Item("Comments").Value.ToString(),
                            PE_Origin = lObjRecordset.Fields.Item("U_PE_Origin").Value.ToString(),
                            ObjType = int.Parse(lObjRecordset.Fields.Item("ObjType").Value.ToString()),
                            IsDraft = true,
                        };

                        lLstInvoiceDTO.Add(lObjInvoiceDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                HandleException(lObjException, "GetDraftInvoices");
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstInvoiceDTO;
        }

        #region Handle Exception
        /// <summary>
        /// Handle Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="section"></param>
        public static void HandleException(Exception ex, string section)
        {
            UIApplication.ShowMessageBox(ex.Message);
            LogService.WriteError(String.Format("{0}: {1}, {2}", section, ex.Message, ex.StackTrace));
        }
        #endregion
    }
}
