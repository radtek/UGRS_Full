using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.DI.Auctions.DTO;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.Services;
using UGRS.Core.SDK.DI.Auctions.Tables;

namespace UGRS.Core.SDK.DI.Auctions.DAO
{
    public class PaymentDAO
    {
        QueryManager mObjQueryManager = new QueryManager();

        public List<PaymentDTO> GetPayments(string pStrFolio, string pStrCostingCode)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            List<PaymentDTO> lLstPaymentDTO = new List<PaymentDTO>();
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("AuctionId", pStrFolio);
                lLstStrParameters.Add("CostingCode", pStrCostingCode);
                string lStrQuery = this.GetSQL("GetPayments").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        PaymentDTO lObjPaymentDTO = new PaymentDTO();
                        lObjPaymentDTO.CardCode = lObjRecordset.Fields.Item("CardCode").Value.ToString();
                        lObjPaymentDTO.CardName = lObjRecordset.Fields.Item("CardName").Value.ToString();
                        lObjPaymentDTO.TotalSell = lObjRecordset.Fields.Item("Venta").Value.ToString();
                        lObjPaymentDTO.TotalBuy = lObjRecordset.Fields.Item("Compra").Value.ToString();
                        lObjPaymentDTO.AccountD = lObjRecordset.Fields.Item("AccountD").Value.ToString();
                        lObjPaymentDTO.AccountC = lObjRecordset.Fields.Item("AccountC").Value.ToString();

                        lLstPaymentDTO.Add(lObjPaymentDTO);
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
            return lLstPaymentDTO;
        }

        public List<AuctionsDTO> GetActionsByBP(string pStrCardCode,string pStrCostingCode)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            List<AuctionsDTO> lLstAuctionsDTO = new List<AuctionsDTO>();
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("CardCode", pStrCardCode);
                lLstStrParameters.Add("CostingCode", pStrCostingCode);

                string lStrQuery = this.GetSQL("GetAuctionsByBP").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        AuctionsDTO lObjAuctionsDTO = new AuctionsDTO();
                        lObjAuctionsDTO.Folio = lObjRecordset.Fields.Item("U_SU_Folio").Value.ToString();
                        lObjAuctionsDTO.TotalBuyer = lObjRecordset.Fields.Item("Compra").Value.ToString();
                        lLstAuctionsDTO.Add(lObjAuctionsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetActionsByBP: {0}", ex.Message));
                LogService.WriteError("PaymentDAO (GetActionsByBP): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstAuctionsDTO;
        }

        public string GetLastAuction()
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrAuction = string.Empty;
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                string lStrQuery = this.GetSQL("GetLastAuction").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrAuction = lObjRecordset.Fields.Item("U_Folio").Value.ToString();
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
            return lStrAuction;
        }

        public bool ExistConfiguration(string pStrField)
        {
            string InvntItem = "";
            try
            {
                InvntItem = mObjQueryManager.GetValue("U_Value", "Name", pStrField, "[@UG_CONFIG]");
            }
            catch (Exception ex)
            {
                //UIApplication.ShowMessageBox("El registro " + pStrField + " no existe en la tabla de configuración");
                UIApplication.ShowError(string.Format("ExistConfiguration: {0}", ex.Message));
                LogService.WriteError("PaymentDAO (ExistConfiguration): " + ex.Message);
                LogService.WriteError(ex);
            }
            return string.IsNullOrEmpty(InvntItem) ? false : true;
        }


        //public bool GetStatusAuction(string pStrFolio)
        //{
        //    SAPbobsCOM.Recordset lObjRecordset = null;
        //    string lStrAutCorral = string.Empty;
        //    string lSTrAutTrans = string.Empty;

        //    try
        //    {
        //        Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
        //        lLstStrParameters.Add("AuctionId", pStrFolio);
        //        string lStrQuery = this.GetSQL("GetAuctionStatus").Inject(lLstStrParameters);
        //        //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

        //        lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
        //        lObjRecordset.DoQuery(lStrQuery);

        //        if (lObjRecordset.RecordCount > 0)
        //        {
        //            lStrAutCorral = lObjRecordset.Fields.Item("U_AutCorral").Value.ToString();
        //            lSTrAutTrans = lObjRecordset.Fields.Item("U_AutTransp").Value.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        UIApplication.ShowError(string.Format("GetPayments: {0}", ex.Message));
        //        LogService.WriteError("PaymentDAO (GetPayments): " + ex.Message);
        //        LogService.WriteError(ex);
        //    }
        //    finally
        //    {
        //        MemoryUtility.ReleaseComObject(lObjRecordset);
        //    }

        //    if (lStrAutCorral == "Y" && lSTrAutTrans == "Y")
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

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

        public List<MessageDTO> GetMessages(string pStrFolio)
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
                        lObjMessageDTO.Message = "Ya puede realizar el cobro de crédito y cobranza " + pStrFolio;
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


        public List<string> GetLastAuctions(string pStrCostingCode)
        {

            SAPbobsCOM.Recordset lObjRecordset = null;
            List<string> lLstAuctions = new List<string>();
            try
            {
                string lStrQuery = this.GetSQL("GetLastAuction").InjectSingleValue("CostingCode", pStrCostingCode);

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
                LogService.WriteError("MailSenderDAO (GetActions): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstAuctions;
        }




        public string GetCostingCode(int pIntUserSign)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrCostingCode = string.Empty;
            try
            {
                string lStrQuery = this.GetSQL("GetCostingCodeBySign").InjectSingleValue("UsrId", pIntUserSign.ToString());

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrCostingCode = lObjRecordset.Fields.Item("U_GLO_CostCenter").Value.ToString();
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
            return lStrCostingCode;
        }
    }
}
