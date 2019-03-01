using System;
using System.Collections.Generic;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.SDK.DI.CyC.DTO;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.CyC.Tables;

using UGRS.Core.SDK.DI.DAO;
using System.Linq;


namespace UGRS.Core.SDK.DI.CyC.DAO
{
    public class CyCDAO
    {
        QueryManager mObjQueryManager = new QueryManager();

        public List<string> GetAuctions(string pStrCostCenter, string pStrUserId)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            List<string> lLstAuctions = new List<string>();
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("CenterCost", pStrCostCenter);
                lLstStrParameters.Add("UserID", pStrUserId);
                string lStrQuery = this.GetSQL("GetAuctions").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        
                        string lStrFolio = lObjRecordset.Fields.Item("U_Folio").Value.ToString();
                        lLstAuctions.Add(lStrFolio);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetActions: {0}", ex.Message));
                LogService.WriteError("CyCDAO (GetActions): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstAuctions;
        }


        public List<AuctionDTO> GetAuctionDTO(string pStrFolio, string pStrCostingCode, char pCharCYC)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            List<AuctionDTO> lLstAuctionDTO = new List<AuctionDTO>();
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Folio", pStrFolio);
                lLstStrParameters.Add("OcrCode", pStrCostingCode);
                lLstStrParameters.Add("CYC", pCharCYC.ToString());

                string lStrQuery = this.GetSQL("GetPayments").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        AuctionDTO lObjAuctionDTO = new AuctionDTO();
                        lObjAuctionDTO.CardCode = lObjRecordset.Fields.Item("CardCode").Value.ToString();
                        lObjAuctionDTO.CardName = lObjRecordset.Fields.Item("CardName").Value.ToString();
                        lObjAuctionDTO.TotalSell = lObjRecordset.Fields.Item("Venta").Value.ToString();
                        lObjAuctionDTO.TotalBuy = lObjRecordset.Fields.Item("Invoice").Value.ToString();
                        lObjAuctionDTO.AccountD = lObjRecordset.Fields.Item("AccountD").Value.ToString();

                        lLstAuctionDTO.Add(lObjAuctionDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetPayments: {0}", ex.Message));
                LogService.WriteError("PaymentDAO (GetPayments): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstAuctionDTO;
        }

        public List<InvoiceDTO> GetInvoices(string pStrCardCode, string pStrOcrCode, char pStrType)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            List<InvoiceDTO> lLstCardCodeDTO = new List<InvoiceDTO>();
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("CardCode", pStrCardCode);
                lLstStrParameters.Add("OcrCode", pStrOcrCode);
                lLstStrParameters.Add("CYC", pStrType.ToString());

                string lStrQuery = this.GetSQL("GetInvoices").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);
                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        InvoiceDTO lObjInvoiceDTO = new InvoiceDTO();
                        lObjInvoiceDTO.DocEntry = lObjRecordset.Fields.Item("DocEntry").Value.ToString();
                        lObjInvoiceDTO.DocNum = lObjRecordset.Fields.Item("DocNum").Value.ToString();
                        lObjInvoiceDTO.Date = Convert.ToDateTime(lObjRecordset.Fields.Item("DocDate").Value.ToString());
                        lObjInvoiceDTO.Days = Convert.ToInt32(lObjRecordset.Fields.Item("Days").Value.ToString());
                        lObjInvoiceDTO.Area = lObjRecordset.Fields.Item("OcrCode").Value.ToString();
                        lObjInvoiceDTO.Balance = Convert.ToDecimal(lObjRecordset.Fields.Item("Balance").Value.ToString());
                        lObjInvoiceDTO.Amount = 0;

                        lLstCardCodeDTO.Add(lObjInvoiceDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetPayments: {0}", ex.Message));
                LogService.WriteError("PaymentDAO (GetPayments): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstCardCodeDTO;
        }

        public List<PaymentsDTO> GetPays(string pStrFolio)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            List<PaymentsDTO> lLstPaymentsDTO = new List<PaymentsDTO>();
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Folio", pStrFolio);
                string lStrQuery = this.GetSQL("GetPay").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        PaymentsDTO lObjPaymentDTO = new PaymentsDTO();
                        lObjPaymentDTO.DocEntry = lObjRecordset.Fields.Item("DocEntry").Value.ToString();
                        lObjPaymentDTO.DocNum = lObjRecordset.Fields.Item("DocNum").Value.ToString();
                        lObjPaymentDTO.DocDate = Convert.ToDateTime(lObjRecordset.Fields.Item("DocDate").Value.ToString());
                        lObjPaymentDTO.DocTotal = Convert.ToDouble(lObjRecordset.Fields.Item("DocTotal").Value.ToString());

                        lLstPaymentsDTO.Add(lObjPaymentDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetPayments: {0}", ex.Message));
                LogService.WriteError("PaymentDAO (GetPayments): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstPaymentsDTO;
        }

        public UserDTO GetUser(string pStrUserCode)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            UserDTO lObjUser = new UserDTO();
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("UserCode", pStrUserCode);
                string lStrQuery = this.GetSQL("GetUser").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lObjUser.UserId = lObjRecordset.Fields.Item("USERID").Value.ToString();
                    lObjUser.UserCode = lObjRecordset.Fields.Item("USER_CODE").Value.ToString();
                    lObjUser.Department = lObjRecordset.Fields.Item("Department").Value.ToString();
                    lObjUser.DepartmentName = lObjRecordset.Fields.Item("Name").Value.ToString();
                    lObjUser.CostigCode = lObjRecordset.Fields.Item("U_GLO_CostCenter").Value.ToString();
                    lObjUser.CYC = Convert.ToChar(lObjRecordset.Fields.Item("CYC").Value); 
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetUser: {0}", ex.Message));
                LogService.WriteError("PaymentDAO (GetUser): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lObjUser;
        }

        public List<Coments> GetComents(string pStrFolio, char pCharCyC,string pStrCostingCode, string pStrCardCode)
        {
            QueryManager mObjQueryManager = new QueryManager();
            List<Coments> lLstComents = new List<Coments>();
            try
            {
                List<Coments> lVarResult = new List<Coments>();
                if (pCharCyC != 'Y' )
                {
                    lVarResult = mObjQueryManager.GetObjectsList<Coments>("U_Folio", pStrFolio, "[@UG_CC_CobroSub]").Where(x => x.CostCenter == pStrCostingCode && x.Cardcocde == pStrCardCode).ToList();
                }
                else
                {
                    lVarResult = mObjQueryManager.GetObjectsList<Coments>("U_Folio", pStrFolio, "[@UG_CC_CobroSub]").Where(x => x.Cardcocde == pStrCardCode).ToList();
                }
                if (lVarResult != null)
                {
                    lLstComents = lVarResult;
                }
            }
            catch (Exception lObjException)
            {
                UIApplication.ShowError(string.Format("Account Refound: {0}", lObjException.Message));
                LogService.WriteError("PurchasesDAO (GetVouchesDetail): " + lObjException.Message);
                LogService.WriteError(lObjException);
            }
            return lLstComents;
        }

        public bool GetUserCyC(string pStrUserCode)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("UserCode", pStrUserCode);
                string lStrQuery = this.GetSQL("GetUserCYC").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                   return true;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetUser: {0}", ex.Message));
                LogService.WriteError("PaymentDAO (GetUser): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return false;
        }

        public Auction GetAuction(string pStrFolio)
        {
            Auction lObjAuction = new Auction();

            try
            {
                lObjAuction = mObjQueryManager.GetTableObject<Auction>("U_Folio", pStrFolio, "[@UG_SU_AUTN]");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetAuction: {0}", ex.Message));
                LogService.WriteError("PaymentDAO (GetAuction): " + ex.Message);
                LogService.WriteError(ex);
            }
            return lObjAuction;
        }


        public List<MessageDTO> GetMessagesCyC(string pStrFolio)
        {
            List<MessageDTO> lLstMessages = new List<MessageDTO>();
            SAPbobsCOM.Recordset lObjRecordset = null;
            try
            {
                string lStrQuery = this.GetSQL("GetUsersMessage");

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        MessageDTO lObjMessageDTO = new MessageDTO();
                        lObjMessageDTO.UserCode = lObjRecordset.Fields.Item("USER_CODE").Value.ToString();
                        lObjMessageDTO.UserId = lObjRecordset.Fields.Item("USERID").Value.ToString();
                        lObjMessageDTO.Message = "El depto de credito y cobranza ha finalizado el cobro de la subasta #" + pStrFolio;
                        lLstMessages.Add(lObjMessageDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetLastAuction: {0}", ex.Message));
                LogService.WriteError("PaymentDAO (ExistConfiguration): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstMessages;
        }

    }
}
