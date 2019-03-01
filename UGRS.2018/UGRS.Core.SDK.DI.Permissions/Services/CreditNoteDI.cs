using System;
using UGRS.Core.SDK.DI.Permissions.DTO;
using UGRS.Core.Services;
using UGRS.Core.SDK.UI;

namespace UGRS.Core.SDK.DI.Permissions.Services
{
    public class CreditNoteDI
    {
        public int CrateCreditNote(string pStrDocEntry, bool lBolPaid)
        {
            int lIntResult = -1;
            try
            {
                System.Console.WriteLine("Creando nota de credito basado en DocEntry: " + pStrDocEntry);
                LogService.WriteInfo("Creando nota de credito basado en DocEntry: " + pStrDocEntry);

                SAPbobsCOM.Documents lObjInvoice = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);
                lObjInvoice.GetByKey(Convert.ToInt32(pStrDocEntry));

                SAPbobsCOM.Documents lObjCreditNote = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oCreditNotes);
                lObjCreditNote.CardCode = lObjInvoice.CardCode;
                //lObjCreditNote.DocDate = new DateTime(2018, 08, 31);
                //lObjCreditNote.DocDueDate = new DateTime(2018, 08, 31);
                lObjCreditNote.EDocGenerationType = SAPbobsCOM.EDocGenerationTypeEnum.edocNotRelevant;
                lObjCreditNote.PaymentGroupCode = lObjInvoice.PaymentGroupCode;
                //lObjCreditNote.EDocExportFormat = ;
                for (int i = 0; i < lObjInvoice.Lines.Count; i++)
                {
                    lObjInvoice.Lines.SetCurrentLine(i);

                    if(lBolPaid)
                    {
                        lObjCreditNote.Lines.Price = lObjInvoice.Lines.Price;
                        lObjCreditNote.Lines.ItemCode = lObjInvoice.Lines.ItemCode;
                        lObjCreditNote.Lines.LineTotal = lObjInvoice.Lines.LineTotal;
                        lObjCreditNote.Lines.TaxCode = lObjInvoice.Lines.TaxCode;
                        lObjCreditNote.Lines.UserFields.Fields.Item("U_FolioFiscal").Value = lObjInvoice.Lines.UserFields.Fields.Item("U_FolioFiscal").Value;
                    }
                    else
                    {
                        lObjCreditNote.Lines.BaseEntry = lObjInvoice.Lines.DocEntry;
                        lObjCreditNote.Lines.BaseType = Convert.ToInt32(lObjInvoice.DocObjectCode);
                        lObjCreditNote.Lines.BaseLine = lObjInvoice.Lines.LineNum;
                        //lObjCreditNote.Lines.WithoutInventoryMovement = SAPbobsCOM.BoYesNoEnum.tYES;
                    }

                    lObjCreditNote.Lines.Add();
                }

                for (int i = 0; i < lObjInvoice.WithholdingTaxData.Count; i++)
                {
                    lObjInvoice.WithholdingTaxData.SetCurrentLine(i);
                    lObjCreditNote.WithholdingTaxData.SetCurrentLine(i);
                    if (!string.IsNullOrEmpty(lObjInvoice.WithholdingTaxData.WTCode))
                    {
                        lObjCreditNote.WithholdingTaxData.WTCode = lObjInvoice.WithholdingTaxData.WTCode;
                        lObjCreditNote.WithholdingTaxData.Add();
                    }
                }


                lIntResult = lObjCreditNote.Add();
                if (lIntResult != 0)
                {
                    string lStrLastError = DIApplication.Company.GetLastErrorDescription();
                    UIApplication.ShowMessageBox(lStrLastError);
                    LogService.WriteError(lStrLastError);
                    //UIApplication.ShowMessageBox(string.Format("Error al generar el pago: {0}", DIApplication.Company.GetLastErrorDescription()));
                }
                else
                {
                    string lStrDocEntry = DIApplication.Company.GetNewObjectKey().ToString();
                    System.Console.WriteLine("Nota de credito creada correctamente con el DocEntry: " + lStrDocEntry);
                    LogService.WriteSuccess("Nota de credito creada correctamente con el DocEntry: " + lStrDocEntry);
                }
            }
            catch (Exception lObjException)
            {
                System.Console.WriteLine(string.Format("Error al generar la nota de crédito basada en la factura {0}: {1}", pStrDocEntry, lObjException.Message));
                LogService.WriteError(string.Format("Error al generar la nota de crédito basada en la factura {0}: {1}", pStrDocEntry, lObjException.Message));
                LogService.WriteError(lObjException);
                lIntResult = -1;
            }
            return lIntResult;
        }
    }
}
