using UGRS.Core.Services;
using System;
using System.Collections.Generic;
using UGRS.Core.SDK.UI;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;

namespace UGRS.Core.SDK.DI.Purchases.DAO
{
    public class PurchaseNotesDAO
    {
        
        /// <summary>
        /// Obtener cuenta afectable.
        /// </summary>
        public IList<string> GetAffectable()
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            IList<string> lLstAffectable = new List<string>();
            try
            {
                //Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                //lLstStrParameters.Add("WareHouse", "");
                string lStrQuery = this.GetSQL("GetAffectable");
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        lLstAffectable.Add(lObjRecordset.Fields.Item("AcctCode").Value.ToString());
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetAffectable): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstAffectable;
        }

    }
}
