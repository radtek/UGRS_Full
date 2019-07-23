using System;
using UGRS.Core.SDK.DI.CreditNote.DAO;

namespace UGRS.Core.SDK.DI.CreditNote.Services
{
    public class CreditNoteService
    {
        private CreditNoteDAO mObjCreditNoteDAO;

        public CreditNoteService()
        {
            mObjCreditNoteDAO = new CreditNoteDAO();
        }

        public string GetInvoiceQuery(DateTime pDtmDate)
        {
            return mObjCreditNoteDAO.GetInvoicesQuery(pDtmDate);
        }
    }
}
