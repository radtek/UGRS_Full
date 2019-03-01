//using QualisysLog;
using System;
using System.Collections.Generic;
using System.Linq;
using UGRS.Core.SDK.DI.Auctions.DTO;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Auctions.Services
{
    public class JounalEntryDI
    {

        public IList<string> CreateDocument(List<JournalEntryDTO> pLstJournalEntry)
        {
            IList<string> lLstResult = new List<string>();
          pLstJournalEntry = pLstJournalEntry.OrderBy(x => x.Aux).ThenBy(y => y.Account).ToList();
            try
            {
                string lStrDocEntry = string.Empty;
                SAPbobsCOM.JournalEntries lObjJournalEntries = (SAPbobsCOM.JournalEntries)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries); //SAPbobsCOM.BoObjectTypes.oInvoices);
                lObjJournalEntries.TransactionCode = "SUB";
               // lObjJournalEntryes.Memo = "CND" + " " + pStrArea
                lObjJournalEntries.Memo = "Cobro compra-venta de ganado " + pLstJournalEntry[0].Area;

                foreach (JournalEntryDTO lObjJournalEntryDTO in pLstJournalEntry)
                {

                    lObjJournalEntries.Lines.Credit = Convert.ToDouble(lObjJournalEntryDTO.Credit);
                    lObjJournalEntries.Lines.Debit = Convert.ToDouble(lObjJournalEntryDTO.Debit);
                    lObjJournalEntries.Lines.AccountCode = lObjJournalEntryDTO.Account;
                    lObjJournalEntries.Lines.CostingCode = lObjJournalEntryDTO.Area;

                    //lObjJournalEntries.Lines.ContraAccount = "";
                    lObjJournalEntries.Lines.UserFields.Fields.Item("U_GLO_Coments").Value = string.IsNullOrEmpty(lObjJournalEntryDTO.Coments) ? "" : lObjJournalEntryDTO.Coments;
                    lObjJournalEntries.Lines.UserFields.Fields.Item("U_GLO_TypeAux").Value = "1";
                    lObjJournalEntries.Lines.UserFields.Fields.Item("U_GLO_Auxiliar").Value = lObjJournalEntryDTO.Aux;
                    lObjJournalEntries.Lines.UserFields.Fields.Item("U_SU_Folio").Value = lObjJournalEntryDTO.AuctionId;
                    lObjJournalEntries.Lines.Add();
                }
             
                if (lObjJournalEntries.Add() != 0)
                {
                    lLstResult.Add(string.Format("Mensaje: {0} ", DIApplication.Company.GetLastErrorDescription()));
                    LogService.WriteError("JournalEntryDI (CreateDocument) Mensaje: " + DIApplication.Company.GetLastErrorDescription());
                }
                else
                {
                   
                    LogService.WriteSuccess("Asiento creado correctamente con :" + pLstJournalEntry.Count() + " lineas");
                    
                }
               // var ss = pLstPurchaseNotes.GroupBy(x => x.Account, x => x.AF, x => x.Area);

            }
            catch (Exception ex)
            {
                UIApplication.ShowError("PolicyDI (CreateDocument)  Mensaje:" + ex.Message);
                LogService.WriteError("PolicyDI (CreateDocument)  Mensaje:" + ex.Message);
                LogService.WriteError(ex);
                lLstResult.Add(ex.Message);
               // throw new Exception(ex.Message);
            }
            return lLstResult;
        }

      
        

       
   

    }
}
