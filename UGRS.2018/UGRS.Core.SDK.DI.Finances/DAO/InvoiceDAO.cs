using System;
using System.Collections.Generic;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Finances.DTO;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Finances.DAO
{
    public class InvoiceDAO
    {
        public InvoiceDTO GetInvoice(int pDocNumber)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string lStrQuery = this.GetSQL("GetInvoice").InjectSingleValue("DocNum", pDocNumber);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount == 0)
                {
                    return null;
                }
                InvoiceDTO lObjInvoiceDTO = new InvoiceDTO();
                lObjInvoiceDTO.DocNum = Convert.ToInt32(lObjResults.Fields.Item("DocNum").Value.ToString());
                lObjInvoiceDTO.DocEntry = Convert.ToInt32(lObjResults.Fields.Item("DocEntry").Value.ToString());
                lObjInvoiceDTO.CardCode = lObjResults.Fields.Item("CardCode").Value.ToString();
                return lObjInvoiceDTO;
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public IList<InvoiceRowDTO> GetGroupedLines(int pDocEntry)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<InvoiceRowDTO> lLstObjInvoiceRows = new List<InvoiceRowDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetGroupedLines").InjectSingleValue("DocEntry", pDocEntry);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        InvoiceRowDTO lObjInvoiceRowDTO = new InvoiceRowDTO();
                        lObjInvoiceRowDTO.LineTotal = Convert.ToDouble(lObjResults.Fields.Item("LineTotal").Value.ToString());
                        lObjInvoiceRowDTO.TaxCode = lObjResults.Fields.Item("TaxCode").Value.ToString();
                        lObjInvoiceRowDTO.OcrCode = lObjResults.Fields.Item("OcrCode").Value.ToString();
                        lObjInvoiceRowDTO.OcrCode2 = lObjResults.Fields.Item("OcrCode2").Value.ToString();
                        lObjInvoiceRowDTO.OcrCode3 = lObjResults.Fields.Item("OcrCode3").Value.ToString();
                        lLstObjInvoiceRows.Add(lObjInvoiceRowDTO);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjInvoiceRows;
            }
            catch (Exception e)
            {
                return lLstObjInvoiceRows;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public IList<InvoiceDTO> GetInvoiceByClient(string pCardCode)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<InvoiceDTO> lLstObjInvoices = new List<InvoiceDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetClientOpenInvoices").InjectSingleValue("CardCode", pCardCode);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        InvoiceDTO lObjInvoiceDTO = new InvoiceDTO();
                        lObjInvoiceDTO.DocStatus = lObjResults.Fields.Item("DocStatus").Value.ToString();
                        lObjInvoiceDTO.DocNum = Convert.ToInt32(lObjResults.Fields.Item("DocNum").Value.ToString());
                        lObjInvoiceDTO.DocEntry = Convert.ToInt32(lObjResults.Fields.Item("DocEntry").Value.ToString());
                        lObjInvoiceDTO.DocDate = Convert.ToDateTime(lObjResults.Fields.Item("DocDate").Value.ToString());
                        lObjInvoiceDTO.DocDueDate = Convert.ToDateTime(lObjResults.Fields.Item("DocDueDate").Value.ToString());
                        lObjInvoiceDTO.DocCur = lObjResults.Fields.Item("DocCur").Value.ToString();
                        lObjInvoiceDTO.DocTotal = Convert.ToDouble(lObjResults.Fields.Item("DocTotal").Value.ToString());
                        lObjInvoiceDTO.DocTotalFC = Convert.ToDouble(lObjResults.Fields.Item("DocTotalFC").Value.ToString());
                        lObjInvoiceDTO.PaidToDate = Convert.ToDouble(lObjResults.Fields.Item("PaidToDate").Value.ToString());
                        lObjInvoiceDTO.CardCode = lObjResults.Fields.Item("CardCode").Value.ToString();
                        lObjInvoiceDTO.OcrCode = lObjResults.Fields.Item("OcrCode").Value.ToString();
                        lLstObjInvoices.Add(lObjInvoiceDTO);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjInvoices;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return lLstObjInvoices;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }
    }
}
