using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Finances.DTO;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;

namespace UGRS.Core.SDK.DI.Finances.DAO
{
    public class CheckDraftDAO
    {
        public IList<CheckDraftDTO> GetCheckDraftsByClient(string pCardCode)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<CheckDraftDTO> lLstObjChecks = new List<CheckDraftDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetCheckDraftsByClient").InjectSingleValue("CardCode", pCardCode);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        CheckDraftDTO lObjCheckDraftDTO = new CheckDraftDTO();
                        lObjCheckDraftDTO.DraftDocEntry = Convert.ToInt32(lObjResults.Fields.Item("DraftDocEntry").Value.ToString());
                        lObjCheckDraftDTO.DueDate = Convert.ToDateTime(lObjResults.Fields.Item("DueDate").Value.ToString());
                        lObjCheckDraftDTO.CheckNum = Convert.ToInt32(lObjResults.Fields.Item("CheckNum").Value.ToString());
                        lObjCheckDraftDTO.BankCode = lObjResults.Fields.Item("BankCode").Value.ToString();
                        lObjCheckDraftDTO.CheckSum = Convert.ToDouble(lObjResults.Fields.Item("CheckSum").Value.ToString());
                        lObjCheckDraftDTO.CheckAct = lObjResults.Fields.Item("CheckAct").Value.ToString();
                        lObjCheckDraftDTO.DocDate = Convert.ToDateTime(lObjResults.Fields.Item("DocDate").Value.ToString());
                        lLstObjChecks.Add(lObjCheckDraftDTO);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjChecks;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[CheckDraftDAO - GetCheckDraftsByClient] Error al obtener los borradores de cheques del cliente {0}: {1}", pCardCode, e.Message));
                throw new Exception(string.Format("Error al obtener los borradores de cheques del cliente {0}: {1}", pCardCode, e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }
    }
}
