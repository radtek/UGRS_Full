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


        public bool CreateNewJournal(string pStrInternal, List<DTO.JournalLineDTO> pLstJournalLines)
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
                lObjJournalEntry.TransactionCode = "TR/F";
                lObjJournalEntry.Reference = pStrInternal;
                //lObjJournalEntry.Series = GetJournalEntrySeries();
                lObjJournalEntry.Memo = "Flete interno" + DateTime.Now.ToShortDateString();

                //Add lines
                if (pLstJournalLines != null && pLstJournalLines.Count > 0)
                {
                    foreach (var lObjLine in pLstJournalLines)
                    {
                        lObjJournalEntry.Lines.AccountCode = lObjLine.AccountCode;
                        lObjJournalEntry.Lines.ContraAccount = lObjLine.ContraAccount;
                        lObjJournalEntry.Lines.CostingCode = lObjLine.CostingCode;
                        lObjJournalEntry.Lines.Credit = lObjLine.Credit;
                        lObjJournalEntry.Lines.Debit = lObjLine.Debit;

                        //lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_Auxiliar").Value = lObjLine.Auxiliary;
                        //lObjJournalEntry.Lines.UserFields.Fields.Item("U_GLO_TypeAux").Value = lObjLine.AuxiliaryType.ToString();
                        //lObjJournalEntry.Lines.UserFields.Fields.Item("U_SU_Folio").Value = pObjJournalEntry.Auction.Folio;
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

        public string GetCreditAccount()
        {
            return mObjRouteListDAO.GetCreditAccount();
        }

        public string GetDebitAccount()
        {
            return mObjRouteListDAO.GetDebitAccount();
        }

        public bool ReverseJournal(string pStrFolio)
        {
            SAPbobsCOM.JournalEntries lObjJournalEntry = null;
            int lIntResult = 0;
            try
            {
                lObjJournalEntry = (SAPbobsCOM.JournalEntries)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                lObjJournalEntry.GetByKey(GetTransId(pStrFolio));
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

        private int GetTransId(string pStrFolio)
        {
            return mObjRouteListDAO.GetJournalId(pStrFolio);
        }
    }
}
