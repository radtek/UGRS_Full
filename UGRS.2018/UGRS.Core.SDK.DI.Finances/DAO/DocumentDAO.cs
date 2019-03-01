using System;
using System.Collections.Generic;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Finances.DTO;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Finances.DAO
{
    public class DocumentDAO
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
                LogUtility.WriteError(string.Format("[DocumentDAO - GetInvoice] Error al obtener la factura con DocNum {0}: {1}", pDocNumber, e.Message));
                throw new Exception(string.Format("Error al obtener la factura con DocNum {0}: {1}", pDocNumber, e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public IList<InvoiceRowDTO> GetInvoiceLinesByGroup(int pDocEntry)
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
                        lObjInvoiceRowDTO.BagsBales = lObjResults.Fields.Item("U_GLO_BagsBales").Value.ToString();
                        lLstObjInvoiceRows.Add(lObjInvoiceRowDTO);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjInvoiceRows;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[DocumentDAO - GetInvoiceLinesByGroup] Error al obtener las lineas de la factura con DocEntry {0}: {1}", pDocEntry, e.Message));
                throw new Exception(string.Format("Error al obtener las lineas de la factura con DocEntry {0}: {1}", pDocEntry, e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public IList<InvoiceDTO> GetClientInvoices(string pCardCode, string pDocStatus = "%")
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<InvoiceDTO> lLstObjInvoices = new List<InvoiceDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetClientInvoices").InjectSingleValue("CardCode", pCardCode).InjectSingleValue("DocStatus", pDocStatus);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        //lObjResults.GetColumnValue<int>("CheckKey");
                        InvoiceDTO lObjInvoiceDTO = new InvoiceDTO();
                        lObjInvoiceDTO.DocStatus = lObjResults.GetColumnValue<string>("DocStatus");
                        lObjInvoiceDTO.DocNum = lObjResults.GetColumnValue<int>("DocNum");
                        lObjInvoiceDTO.Series = lObjResults.GetColumnValue<int>("Series");
                        lObjInvoiceDTO.SeriesName = lObjResults.GetColumnValue<string>("SeriesName");
                        lObjInvoiceDTO.DocNum = lObjResults.GetColumnValue<int>("DocNum");
                        lObjInvoiceDTO.DocEntry = lObjResults.GetColumnValue<int>("DocEntry");
                        lObjInvoiceDTO.TransId = lObjResults.GetColumnValue<int>("TransId");
                        lObjInvoiceDTO.DocDate = lObjResults.GetColumnValue<DateTime>("DocDate");
                        lObjInvoiceDTO.DocDueDate = lObjResults.GetColumnValue<DateTime>("DocDueDate");
                        lObjInvoiceDTO.DocCur = lObjResults.GetColumnValue<string>("DocCur");
                        lObjInvoiceDTO.DocTotal = lObjResults.GetColumnValue<double>("DocTotal");
                        lObjInvoiceDTO.DocTotalFC = lObjResults.GetColumnValue<double>("DocTotalFC");
                        lObjInvoiceDTO.PaidToDate = lObjResults.GetColumnValue<double>("PaidToDate");
                        lObjInvoiceDTO.DocRemaining = lObjResults.GetColumnValue<double>("DocRemaining");
                        lObjInvoiceDTO.CardCode = lObjResults.GetColumnValue<string>("CardCode");
                        lObjInvoiceDTO.OcrCode = lObjResults.GetColumnValue<string>("OcrCode");
                        lObjInvoiceDTO.ObjType = lObjResults.GetColumnValue<string>("ObjType");
                        lLstObjInvoices.Add(lObjInvoiceDTO);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjInvoices;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[DocumentDAO - GetClientInvoices] Error al obtener las facturas del cliente {0}: {1}", pCardCode, e.Message));
                throw new Exception(string.Format("Error al obtener las facturas del cliente {0}: {1}", pCardCode, e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public IList<DownPaymentDTO> GetClientDownPayments(string pCardCode)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<DownPaymentDTO> lLstObjInvoices = new List<DownPaymentDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetClientDownPayments").InjectSingleValue("CardCode", pCardCode);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        DownPaymentDTO lObjInvoiceDTO = new DownPaymentDTO();
                        lObjInvoiceDTO.DocStatus = lObjResults.Fields.Item("DocStatus").Value.ToString();
                        lObjInvoiceDTO.DocNum = Convert.ToInt32(lObjResults.Fields.Item("DocNum").Value.ToString());
                        lObjInvoiceDTO.DocEntry = Convert.ToInt32(lObjResults.Fields.Item("DocEntry").Value.ToString());
                        lObjInvoiceDTO.DocDate = Convert.ToDateTime(lObjResults.Fields.Item("DocDate").Value.ToString());
                        lObjInvoiceDTO.DocDueDate = Convert.ToDateTime(lObjResults.Fields.Item("DocDueDate").Value.ToString());
                        lObjInvoiceDTO.DocCur = lObjResults.Fields.Item("DocCur").Value.ToString();
                        lObjInvoiceDTO.DocTotal = Convert.ToDouble(lObjResults.Fields.Item("DocTotal").Value.ToString());
                        lObjInvoiceDTO.DocTotalFC = Convert.ToDouble(lObjResults.Fields.Item("DocTotalFC").Value.ToString());
                        lObjInvoiceDTO.PaidToDate = Convert.ToDouble(lObjResults.Fields.Item("PaidToDate").Value.ToString());
                        lObjInvoiceDTO.DocRemaining = Convert.ToDouble(lObjResults.Fields.Item("DocRemaining").Value.ToString());
                        lObjInvoiceDTO.CardCode = lObjResults.Fields.Item("CardCode").Value.ToString();
                        //lObjInvoiceDTO.OcrCode = lObjResults.Fields.Item("OcrCode").Value.ToString();
                        lLstObjInvoices.Add(lObjInvoiceDTO);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjInvoices;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[DocumentDAO - GetClientDownPayments] Error al obtener los pagos del cliente {0}: {1}", pCardCode, e.Message));
                throw new Exception(string.Format("Error al obtener los pagos del cliente {0}: {1}", pCardCode, e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public IList<PaymentDTO> GetClientNoDocPayments(string pCardCode)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<PaymentDTO> lLstObjPayments = new List<PaymentDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetClientNoDocPayments").InjectSingleValue("CardCode", pCardCode);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        PaymentDTO lObjPayment = new PaymentDTO();
                        lObjPayment.DocNum = Convert.ToInt32(lObjResults.Fields.Item("DocNum").Value.ToString());
                        lObjPayment.DocEntry = Convert.ToInt32(lObjResults.Fields.Item("DocEntry").Value.ToString());
                        lObjPayment.DocDate = Convert.ToDateTime(lObjResults.Fields.Item("DocDate").Value.ToString());
                        lObjPayment.DocDueDate = Convert.ToDateTime(lObjResults.Fields.Item("DocDueDate").Value.ToString());
                        lObjPayment.DocCur = lObjResults.Fields.Item("DocCur").Value.ToString();
                        lObjPayment.DocTotal = Convert.ToDouble(lObjResults.Fields.Item("DocTotal").Value.ToString());
                        lObjPayment.DocTotalFC = Convert.ToDouble(lObjResults.Fields.Item("DocTotalFC").Value.ToString());
                        lObjPayment.CardCode = lObjResults.Fields.Item("CardCode").Value.ToString();
                        lObjPayment.NoDocSum = Convert.ToDouble(lObjResults.Fields.Item("NoDocSum").Value.ToString());
                        lObjPayment.NoDocSumFC = Convert.ToDouble(lObjResults.Fields.Item("NoDocSumFC").Value.ToString());
                        lObjPayment.OpenBal = Convert.ToDouble(lObjResults.Fields.Item("OpenBal").Value.ToString());
                        lObjPayment.OpenBalFc = Convert.ToDouble(lObjResults.Fields.Item("OpenBalFc").Value.ToString());
                        lObjPayment.PayNoDoc = lObjResults.Fields.Item("PayNoDoc").Value.ToString();
                        lObjPayment.TransId = Convert.ToInt32(lObjResults.Fields.Item("TransId").Value.ToString());
                        lLstObjPayments.Add(lObjPayment);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjPayments;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[DocumentDAO - GetClientNoDocPayments] Error al obtener los pagos del cliente {0}: {1}", pCardCode, e.Message));
                throw new Exception(string.Format("Error al obtener los pagos del cliente {0}: {1}", pCardCode, e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }


        public IList<DraftDTO> GetClientInvoiceDrafts(string pCardCode)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<DraftDTO> lLstDrafts = new List<DraftDTO>();
            try
            {
                string lStrQuery = this.GetSQL("GetClientInvoiceDrafts").InjectSingleValue("CardCode", pCardCode);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        DraftDTO lObjDraft = new DraftDTO();
                        lObjDraft.Series = lObjResults.GetColumnValue<int>("Series");
                        lObjDraft.SeriesName = lObjResults.GetColumnValue<string>("SeriesName");
                        lObjDraft.DocNum = lObjResults.GetColumnValue<int>("DocNum");
                        lObjDraft.DocEntry = lObjResults.GetColumnValue<int>("DocEntry");
                        lObjDraft.DocDate = lObjResults.GetColumnValue<DateTime>("DocDate");
                        lObjDraft.DocDueDate = lObjResults.GetColumnValue<DateTime>("DocDueDate");
                        lObjDraft.DocCur = lObjResults.GetColumnValue<string>("DocCur");
                        lObjDraft.DocTotal = lObjResults.GetColumnValue<double>("DocTotal");
                        lObjDraft.DocTotalFC = lObjResults.GetColumnValue<double>("DocTotalFC");
                        lObjDraft.CardCode = lObjResults.GetColumnValue<string>("CardCode");
                        lObjDraft.OcrCode = lObjResults.GetColumnValue<string>("OcrCode");
                        lObjDraft.UserSign = lObjResults.GetColumnValue<int>("UserSign");
                        lObjDraft.U_GLO_CashRegister = lObjResults.GetColumnValue<string>("U_GLO_CashRegister");
                        lObjDraft.ObjType = lObjResults.GetColumnValue<string>("ObjType");
                        lLstDrafts.Add(lObjDraft);
                        lObjResults.MoveNext();
                    }
                }
                return lLstDrafts;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[DocumentDAO - GetClientInvoiceDrafts] Error al obtener los borradores de las facturas del cliente {0}: {1}", pCardCode, e.Message));
                throw new Exception(string.Format("Error al obtener los borradores de las facturas del cliente {0}: {1}", pCardCode, e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        #region Extras
        public IList<InvoiceDTO> DataTableToInvoiceDTO(SAPbouiCOM.DataTable mDtClientInvoices)
        {
            IList<InvoiceDTO> lLstInvoiceDTO = new List<InvoiceDTO>();

            for (int i = 0; i < mDtClientInvoices.Rows.Count; i++)
            {
                InvoiceDTO lObjInvoiceDTO = new InvoiceDTO();
                lObjInvoiceDTO.DocNum = int.Parse(mDtClientInvoices.GetValue("C_DocNum", i).ToString());
                lObjInvoiceDTO.DocDate = DateTime.Parse(mDtClientInvoices.GetValue("C_DocDate", i).ToString());
                lObjInvoiceDTO.DocDueDate = DateTime.Parse(mDtClientInvoices.GetValue("C_DcDueDt", i).ToString());
                lObjInvoiceDTO.DocCur = mDtClientInvoices.GetValue("C_DocCur", i).ToString();
                lObjInvoiceDTO.OcrCode = mDtClientInvoices.GetValue("C_OcrCode", i).ToString();
                lObjInvoiceDTO.DocTotal = double.Parse(mDtClientInvoices.GetValue("C_DocTotal", i).ToString());
                lObjInvoiceDTO.DocRemaining = int.Parse(mDtClientInvoices.GetValue("C_DocRem", i).ToString());
                lObjInvoiceDTO.SeriesName = mDtClientInvoices.GetValue("C_Series", i).ToString();
                lObjInvoiceDTO.DocEntry = int.Parse(mDtClientInvoices.GetValue("C_DocEntry", i).ToString());
                lObjInvoiceDTO.ObjType = mDtClientInvoices.GetValue("C_ObjType", i).ToString();

                lLstInvoiceDTO.Add(lObjInvoiceDTO);

                //mDtClientInvoices.SetValue("C_DocDate", i, lObjinvoice.DocDate);
                //mDtClientInvoices.SetValue("C_DcDueDt", i, lObjinvoice.DocDueDate);
                //mDtClientInvoices.SetValue("C_DocCur", i, lObjinvoice.DocCur);
                //mDtClientInvoices.SetValue("C_OcrCode", i, lObjinvoice.OcrCode);
                //mDtClientInvoices.SetValue("C_DocTotal", i, lObjinvoice.DocTotal);
                //mDtClientInvoices.SetValue("C_DocRem", i, lObjinvoice.DocRemaining);
                //mDtClientInvoices.SetValue("C_Series", i, lObjinvoice.SeriesName);
                //// The following are not shown in the matrix, only internal
                //mDtClientInvoices.SetValue("C_DocEntry", i, lObjinvoice.DocEntry);
                //mDtClientInvoices.SetValue("C_ObjType", i, lObjinvoice.ObjType);
            }

            return lLstInvoiceDTO;
        }
        #endregion
    }
}
