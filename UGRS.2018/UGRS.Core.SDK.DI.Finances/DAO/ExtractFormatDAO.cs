using System;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Finances.DTO;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Finances.DAO
{
    public class ExtractFormatDAO
    {
        public ExtractFormatDTO GetAccountExtractFormat(string pAcctCode){
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string lStrQuery = this.GetSQL("GetAccountExtractFormat").InjectSingleValue("AcctCode", pAcctCode);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    ExtractFormatDTO lObjExtractFormat = new ExtractFormatDTO();
                    lObjExtractFormat.Code = lObjResults.GetColumnValue<string>("U_FZ_ExtFormat");
                    lObjExtractFormat.Name = lObjResults.GetColumnValue<string>("Name");
                    if (lObjExtractFormat.Code == null || lObjExtractFormat.Code == "")
                    {
                        return null;
                    }
                    return lObjExtractFormat;
                }
                return null;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[ExtractFormatDAO - GetAccountExtractFormat] Error al obtener el formato de la cuenta {0}: {1}", pAcctCode, e.Message));
                throw new Exception(string.Format("Error al obtener el formato de la cuenta {0}: {1}", pAcctCode, e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }
    }
}
