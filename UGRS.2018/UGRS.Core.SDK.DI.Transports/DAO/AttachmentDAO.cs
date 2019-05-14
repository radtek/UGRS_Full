using System;
using UGRS.Core.Services;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;

namespace UGRS.Core.SDK.DI.Transports.DAO
{
    public class AttachmentDAO
    {
        public string GetAttachPath()
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrAttachPath = string.Empty;

            try
            {
                string lStrQuery = this.GetSQL("GetAttachPath");
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrAttachPath = lObjRecordset.Fields.Item("AttachPath").Value.ToString();
                }
            }
            catch (Exception ex)
            {
                //UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError(string.Format("[AttachmentDAO - GetAttachPath] {0}", ex.Message));
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lStrAttachPath;
        }
    }
}
