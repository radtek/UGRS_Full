using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Exceptions;
using UGRS.Core.SDK.DI.Transports.DAO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Transports.Services
{
    public class JournalService
    {
        private RouteListDAO mObjRouteListDAO = new RouteListDAO();


        public bool CreateNewJournal(List<DTO.JournalLineDTO> pLstJournalLines, string pStrInternal, string pStrTransactionCode, string pStrMemo)
        {
            SAPbobsCOM.JournalEntries lObjJournalEntry = null;
            int lIntResult;
            try
            {
                //Pupulate header
                lObjJournalEntry = (SAPbobsCOM.JournalEntries)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                lObjJournalEntry.DueDate = DateTime.Today;
                lObjJournalEntry.TaxDate = DateTime.Today;
                lObjJournalEntry.AutoVAT = SAPbobsCOM.BoYesNoEnum.tYES;
                lObjJournalEntry.TransactionCode = pStrTransactionCode; //"TR/F";
                lObjJournalEntry.Reference = pStrInternal;
                //lObjJournalEntry.Series = GetJournalEntrySeries();
                lObjJournalEntry.Memo = pStrMemo; // "Flete interno" + DateTime.Now.ToShortDateString();

                //Add lines
                if (pLstJournalLines != null && pLstJournalLines.Count > 0)
                {
                    var Debit = pLstJournalLines.Sum(x => x.Debit);
                    var Credit = pLstJournalLines.Sum(x => x.Credit);
                    foreach (var lObjLine in pLstJournalLines)
                    {
                        lObjJournalEntry.Lines.AccountCode = lObjLine.AccountCode;
                        lObjJournalEntry.Lines.ContraAccount = lObjLine.ContraAccount;
                        lObjJournalEntry.Lines.CostingCode = lObjLine.CostingCode;
                        lObjJournalEntry.Lines.CostingCode2 = lObjLine.CostingCode2;
                        lObjJournalEntry.Lines.Credit = lObjLine.Credit;
                        lObjJournalEntry.Lines.Debit = lObjLine.Debit;
                        lObjJournalEntry.Lines.Reference1 = string.IsNullOrEmpty(lObjLine.Ref1) ? "" : lObjLine.Ref1;
                        lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_Auxiliar").Value = string.IsNullOrEmpty(lObjLine.Auxiliar) ? "" : lObjLine.Auxiliar;
                        lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_TypeAux").Value = string.IsNullOrEmpty(lObjLine.TypeAux) ? "" : lObjLine.TypeAux;
                        //lObjJournalEntry.Lines.UserFields.Fields.Item("U_SU_Folio").Value = pObjJournalEntry.Auction.Folio;
                        lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_CodeMov").Value = string.IsNullOrEmpty(lObjLine.CodeMov) ? "" : lObjLine.CodeMov;
                        lObjJournalEntry.Lines.Add();
                    }
                }

                //Handle operation
                lIntResult = lObjJournalEntry.Add();
                if (lIntResult != 0)
                {
                    throw new SapBoException(string.Format("Error code: {0} \nError message: {1}", lIntResult, DIApplication.Company.GetLastErrorDescription()));
                }


            }
            catch (Exception lObjException)
            {
                UIApplication.GetApplication().Forms.ActiveForm.Freeze(false);
                LogService.WriteError("JournalService (CreateAction): " + lObjException.Message);
                LogService.WriteError(lObjException); 
                UIApplication.ShowMessageBox(string.Format("CreateAction: {0}", lObjException.Message));
                return false;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjJournalEntry);
            }
            return lIntResult == 0 ? true : false;
        }

        public string GetCreditAccount()
        {
            return mObjRouteListDAO.GetCreditAccount();
        }

        public string GetDebitAccount()
        {
            return mObjRouteListDAO.GetDebitAccount();
        }

        public bool ReverseJournal(string pStrFolio, string pStrTransCode)
        {
            SAPbobsCOM.JournalEntries lObjJournalEntry = null;
            int lIntResult = 0;
            try
            {
                lObjJournalEntry = (SAPbobsCOM.JournalEntries)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                lObjJournalEntry.GetByKey(GetTransId(pStrFolio, pStrTransCode));
                //Cancel JournalEntry
                lIntResult = lObjJournalEntry.Cancel();

            }
            catch (Exception lObjException)
            {
                UIApplication.ShowError(string.Format("CreateAction: {0}", lObjException.Message));
                LogService.WriteError("JournalService (CreateAction): " + lObjException.Message);
                LogService.WriteError(lObjException);
                return false;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjJournalEntry);
            }
            return lIntResult == 0 ? true : false;
        }

        public int GetTransId(string pStrFolio, string pStrTransCode)
        {
            return mObjRouteListDAO.GetJournalId(pStrFolio, pStrTransCode);
        }

        public string GetCancelJournal(string pStrFolio)
        {
            return mObjRouteListDAO.GetCancelJournal(pStrFolio);
        }
    }
}
