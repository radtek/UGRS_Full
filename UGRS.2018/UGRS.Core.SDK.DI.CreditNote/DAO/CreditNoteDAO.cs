using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.Services;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;

namespace UGRS.Core.SDK.DI.CreditNote.DAO
{
    public class CreditNoteDAO
    {
        QueryManager mObjQueryManager = new QueryManager();

        public string GetInvoicesQuery(DateTime pDtmDateTime)
        {
            Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
            lLstStrParameters.Add("StartDate", pDtmDateTime.ToString("yyyyMMdd"));
            string lStrQuery = this.GetSQL("GetInvoices").Inject(lLstStrParameters);
            LogService.WriteInfo(lStrQuery);
            return lStrQuery;
        }
    }
}
