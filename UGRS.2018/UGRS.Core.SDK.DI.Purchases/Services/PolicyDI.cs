using System;
using System.Collections.Generic;
using System.Linq;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Purchases.DTO;
using UGRS.Core.SDK.DI.Purchases.Enums;
using UGRS.Core.SDK.DI.Purchases.Tables;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Purchases.Services
{
    public class PolicyDI
    {
        QueryManager mObjQueryManager = new QueryManager();
        PurchasesServiceFactory mObjPurchaseServiceFactory = new PurchasesServiceFactory();

        public IList<string> CreateDocument(List<PurchaseNote> pLstPurchaseNotes, Vouchers pObjVoucher, TypeEnum.Type pNoteType)
        {
            IList<string> lLstResult = new List<string>();
            
            try
            {
                string lStrDocEntry = string.Empty;
                SAPbobsCOM.JournalEntries lObjJournalEntryes = (SAPbobsCOM.JournalEntries)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries); //SAPbobsCOM.BoObjectTypes.oInvoices);
                lObjJournalEntryes.TransactionCode = "S/CF";
                lObjJournalEntryes.Memo = "CND" + " " + pObjVoucher.Area;
               
                if (pNoteType == TypeEnum.Type.Refund)
                {
                    lObjJournalEntryes.Reference = pObjVoucher.Folio;
                }
                else
                {
                    lObjJournalEntryes.Reference = pObjVoucher.CodeMov;
                }

                var pLstPurchaseNotesAffectable = pLstPurchaseNotes.GroupBy(x => x.Affectable ).Select(x => new PurchaseNote()
                     {
                         Affectable = x.First().Affectable,
                         Area = x.First().Area,
                         CostingCode = x.First().CostingCode,
                         Aux = x.First().AuxAfectable,
                         AF = x.First().AF,
                         CodeMov = x.First().CodeMov,
                         CodeVoucher = x.First().CodeVoucher,
                         Amount = x.Sum(y => y.Amount) //x.Select(y=> y.Amount).Sum().ToString(),

                     }).ToList();

                string lStrAttachPath = mObjPurchaseServiceFactory.GetPurchaseService().GetAttachPath();
                foreach (PurchaseNote lObjNotes in pLstPurchaseNotes)
                {
                    int lIntAttachement = 0;
                    string lStrAttach = string.Empty;
                    if (!string.IsNullOrEmpty(lObjNotes.File))
                    {
                        AttachmentDI lObjAttachmentDI = new AttachmentDI();
                        lIntAttachement = lObjAttachmentDI.AttachFile(lObjNotes.File);
                        if (lIntAttachement > 0)
                        {

                            lStrAttach = lStrAttachPath + System.IO.Path.GetFileName(lObjNotes.File);
                        }
                        else
                        {
                            lStrAttach = lObjNotes.File;
                        }
                    }
                    lObjJournalEntryes.Lines.Credit = 0; // Convert.ToDouble(lObjMovement.Credit);
                    lObjJournalEntryes.Lines.Debit = lObjNotes.Amount; 
                    lObjJournalEntryes.Lines.AccountCode = lObjNotes.Account;
                    lObjJournalEntryes.Lines.ProjectCode = lObjNotes.Project;
                    lObjJournalEntryes.Lines.CostingCode = lObjNotes.Area;
                    lObjJournalEntryes.Lines.CostingCode2 = lObjNotes.AF;
                    lObjJournalEntryes.Lines.CostingCode3 = lObjNotes.AGL;
                    lObjJournalEntryes.Lines.CostingCode4 = lObjNotes.Line;
                    lObjJournalEntryes.Lines.Reference2 = lObjNotes.Folio;
                    lObjJournalEntryes.Lines.LineMemo = lObjNotes.Provider;
                    lObjJournalEntryes.Lines.UserFields.Fields.Item("U_GLO_Coments").Value = lObjNotes.Coments;
                    lObjJournalEntryes.Lines.UserFields.Fields.Item("U_GLO_File").Value = lStrAttach;
                    lObjJournalEntryes.Lines.UserFields.Fields.Item("U_GLO_TypeAux").Value = "2";
                    lObjJournalEntryes.Lines.UserFields.Fields.Item("U_GLO_Auxiliar").Value = lObjNotes.Aux;
                    lObjJournalEntryes.Lines.UserFields.Fields.Item("U_GLO_CodeMov").Value = string.IsNullOrEmpty(lObjNotes.CodeMov) ? pObjVoucher.Folio : lObjNotes.CodeMov;
                    

                    if (!string.IsNullOrEmpty(lObjNotes.Aux))
                    {
                        lObjJournalEntryes.Lines.UserFields.Fields.Item("U_GLO_TypeAux").Value = "2";
                    }

                    lObjJournalEntryes.Lines.Add();
                }

                foreach (PurchaseNote lObjAffectable in pLstPurchaseNotesAffectable)
                {
                    lObjJournalEntryes.Lines.AccountCode = lObjAffectable.Affectable;
                    lObjJournalEntryes.Lines.Credit = lObjAffectable.Amount;
                    lObjJournalEntryes.Lines.Debit = 0;
                    lObjJournalEntryes.Lines.CostingCode = lObjAffectable.CostingCode;
                    lObjJournalEntryes.Lines.CostingCode2 = lObjAffectable.AF;
                    lObjJournalEntryes.Lines.UserFields.Fields.Item("U_GLO_TypeAux").Value = "2";
                    lObjJournalEntryes.Lines.UserFields.Fields.Item("U_GLO_Auxiliar").Value = lObjAffectable.Aux;
                    lObjJournalEntryes.Lines.UserFields.Fields.Item("U_GLO_CodeMov").Value = pNoteType == TypeEnum.Type.Refund ? pObjVoucher.Folio : lObjAffectable.CodeMov;
                                                                                             /*string.IsNullOrEmpty(lObjAffectable.CodeMov) ?
                                                                                             lObjAffectable.CodeVoucher
                                                                                             :lObjAffectable.CodeMov;*/ //lObjAffectable.CodeVoucher;
                }

                if (lObjJournalEntryes.Add() != 0)
                {
                    lLstResult.Add(string.Format("Mensaje: {0} ", DIApplication.Company.GetLastErrorDescription()));
                    LogService.WriteError("PolicyDI (CreateDocument)  Código de comprobante:" + pObjVoucher.RowCode + " Mensaje:" + DIApplication.Company.GetLastErrorDescription());
                }
                else
                {
                    LogService.WriteSuccess("Asiento creado correctamente: Código de comprobante:" +  pObjVoucher.RowCode);
                   // AddVoucherDetail(pLstPurchaseNotes);
                }
               // var ss = pLstPurchaseNotes.GroupBy(x => x.Account, x => x.AF, x => x.Area);

            }
            catch (Exception ex)
            {
                LogService.WriteError("PolicyDI (CreateDocument) Código de comprobante:" + pObjVoucher.RowCode + " Mensaje:" + ex.Message);
                LogService.WriteError(ex);
                lLstResult.Add(ex.Message);
               // throw new Exception(ex.Message);
            }
            return lLstResult;
        }


        //Actualiza status de comprobante
        private bool UpdateCancel(string pStrDocEntry, string pStrCodeVoucher)
        {
            bool lBolResult = false;
            try
            {
                List<VouchersDetail> lLstVouchersDetail = mObjPurchaseServiceFactory.GetPurchaseVouchersService().GetVoucherDetailByTrans(pStrDocEntry);
                 if (lLstVouchersDetail.Count > 0)
                 {
                     VouchersDetail lObjVoucherDetail = lLstVouchersDetail.Where(x => x.DocEntry == pStrDocEntry).FirstOrDefault();
                     lObjVoucherDetail.Status = "Cancelado";
               
                     if (mObjPurchaseServiceFactory.GetVouchersDetailService().Update(lObjVoucherDetail) == 0)
                     {
                         mObjPurchaseServiceFactory.GetVouchersService().UpdateTotal(pStrCodeVoucher);
                         lBolResult = true;
                         LogService.WriteSuccess("PolicyDI (UpdateCancel) Cancelacion realizada correctamente, TransId: " + pStrDocEntry);
                     }
                     else
                     {
                         lBolResult = false;
                         LogService.WriteError("PolicyDI (UpdateCancel)  TransId:" + pStrDocEntry + " Mensaje:" + DIApplication.Company.GetLastErrorDescription());
                     }
                 }

            }
            catch (Exception ex)
            {

                LogService.WriteError("PolicyDI (UpdateCancel) TransId:" + pStrDocEntry + " Mensaje:" + ex.Message);
                LogService.WriteError(ex);
            }
            return lBolResult;
        }

        //Cancela asiento
        public bool CancelJournalEntry(string pStrTransID, string pStrCodeVoucher)
        {
            bool lBolSuccess = false;
            try
            {
                DIApplication.Company.StartTransaction();
                
                SAPbobsCOM.JournalEntries lObjJournalEntryes = (SAPbobsCOM.JournalEntries)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                
                lObjJournalEntryes.GetByKey(Convert.ToInt32(pStrTransID));
                lObjJournalEntryes.Reference2 = pStrTransID;
               
              
               if (lObjJournalEntryes.Update() != 0)
               {
                   UIApplication.ShowMessageBox(string.Format("Mensaje: {0} ", DIApplication.Company.GetLastErrorDescription()));
                   LogService.WriteError("PolicyDI (CancelJournalEntry)  Código:" + pStrTransID + " Mensaje:" + DIApplication.Company.GetLastErrorDescription());
                   lBolSuccess = false;
               }
               else
                if (lObjJournalEntryes.Cancel() != 0)
                {
                    UIApplication.ShowMessageBox(string.Format("Mensaje: {0} ", DIApplication.Company.GetLastErrorDescription()));
                    LogService.WriteError("PolicyDI (CancelJournalEntry)  Código:" + pStrTransID + " Mensaje:" + DIApplication.Company.GetLastErrorDescription());
                    lBolSuccess = false;
                }
                else
                {
                    //if (UpdateJournalEntryCancel(pStrTransID))//solicitado que no se cancelara 


                    if (UpdateCancel(pStrTransID, pStrCodeVoucher))
                    {
                        LogService.WriteSuccess("PolicyDI (CancelJournalEntry) Cancelacion realizada correctamente, Código:" + pStrTransID); 
                        lBolSuccess = true;
                    }
                    
                   
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("PolicyDI (CancelJournalEntry) Código:" + pStrTransID + " Mensaje:" + ex.Message);
                LogService.WriteError(ex);
                return false;
            }
            finally
            {
                try
                {
                    if (lBolSuccess)
                    {
                        DIApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                        UIApplication.ShowMessageBox(string.Format("Documento realizado correctamente"));
                       
                    }
                    else
                    {
                        //mStrCodeVoucher = string.Empty;
                        if (DIApplication.Company.InTransaction)
                        {
                            DIApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogService.WriteError("(btnSave_ClickBefore): " + ex.Message);
                    LogService.WriteError(ex);
                }
            }
            return lBolSuccess;
        }

        //Actualiza asiento como cancelado
        public bool UpdateJournalEntryCancel(string pStrTransID)
        {
            try
            {
                SAPbobsCOM.JournalEntries lObjJournalEntryesCancel = (SAPbobsCOM.JournalEntries)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oJournalEntries);
                lObjJournalEntryesCancel.GetByKey(Convert.ToInt32(pStrTransID));
                lObjJournalEntryesCancel.UserFields.Fields.Item("U_GLO_Cancel").Value = "Y";
                lObjJournalEntryesCancel.Reference2 = "";

                if (lObjJournalEntryesCancel.Update() != 0)
                {
                    UIApplication.ShowMessageBox(string.Format("Mensaje: {0} ", DIApplication.Company.GetLastErrorDescription()));
                    LogService.WriteError("PolicyDI (CancelJournalEntry)  Código:" + pStrTransID + " Mensaje:" + DIApplication.Company.GetLastErrorDescription());
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
               
                LogService.WriteError("PolicyDI (UpdateJournalEntryCancel) Código:" + pStrTransID + " Mensaje:" + ex.Message);
                LogService.WriteError(ex);
                return false;
            }
           
        }
   

    }
}
