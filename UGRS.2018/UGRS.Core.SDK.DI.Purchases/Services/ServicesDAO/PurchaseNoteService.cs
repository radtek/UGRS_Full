using System.Collections.Generic;
using UGRS.Core.SDK.DI.Purchases.DAO;

namespace UGRS.Core.SDK.DI.Purchases.Services.ServicesDAO
{
    public class PurchaseNoteService
    {
        private PurchaseNotesDAO lObjPurchaseNote;

        public PurchaseNoteService()
        {
            lObjPurchaseNote = new PurchaseNotesDAO();
        }

        public IList<string> GetAffectable()
        {
            return lObjPurchaseNote.GetAffectable();
        }
    }
}
