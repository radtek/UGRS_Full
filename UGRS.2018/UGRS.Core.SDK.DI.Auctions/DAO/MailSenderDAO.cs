using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.UI;
using UGRS.Core.SDK.DI.Auctions.DTO;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Auctions.DAO
{
    public class MailSenderDAO
    {
        private PaymentDAO lObjPaymentDAO = new PaymentDAO();

        public string GetAuctionSellers(string pStrAuction)
        {
            try
            {
                return this.GetSQL("GetAuctionSellers").InjectSingleValue("Auction", pStrAuction);

            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetActions: {0}", ex.Message));
                LogService.WriteError("MailSenderDAO (GetActions): " + ex.Message);
                LogService.WriteError(ex);
                return string.Empty;
            }
        }

        public string GetCostingCode(string pStrUser)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            try
            {
                string lStrQuery = this.GetSQL("GetCostingCode").InjectSingleValue("UsrName",pStrUser);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    return lObjRecordset.Fields.Item("U_GLO_CostCenter").Value.ToString();
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
            return string.Empty;
        }

        public IList<SellerReportDTO> GetSellerBatches(string pStrSeller, string pStrAuction)
        {

            SAPbobsCOM.Recordset lObjRecordSet = null;

            Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
            List<SellerReportDTO> lLstSellerReports = new List<SellerReportDTO>();

            lLstStrParameters.Add("Auction", pStrAuction);
            lLstStrParameters.Add("Seller", pStrSeller);

            try
            {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                string lStrQuery = this.GetSQL("GetSellerBatches").Inject(lLstStrParameters);
                lObjRecordSet.DoQuery(lStrQuery);

                if (lObjRecordSet.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordSet.RecordCount; i++)
                    {
                        lLstSellerReports.Add(new SellerReportDTO()
                        {
                            BatchNumber = lObjRecordSet.Fields.Item("U_Number").Value.ToString(),
                            Quantity = Convert.ToInt32(lObjRecordSet.Fields.Item("U_Quantity").Value),
                            Article = lObjRecordSet.Fields.Item("U_ItemType").Value.ToString(),
                            AverageWeight = float.Parse(lObjRecordSet.Fields.Item("U_AverageWeight").Value.ToString()),
                            TotalWeight = float.Parse(lObjRecordSet.Fields.Item("U_Weight").Value.ToString()),
                            Price = decimal.Parse(lObjRecordSet.Fields.Item("U_Price").Value.ToString()),
                            Amount = decimal.Parse(lObjRecordSet.Fields.Item("U_Amount").Value.ToString()),
                            Unsold = lObjRecordSet.Fields.Item("U_Unsold").Value.ToString(),
                            UnsoldMotive = (string)lObjRecordSet.Fields.Item("U_UnsoldMotive").Value.ToString(),
                            Reprogrammed = lObjRecordSet.Fields.Item("U_Reprogrammed").Value.ToString(),
                            Gender = lObjRecordSet.Fields.Item("U_Gender").Value.ToString(),
                            Buyer = lObjRecordSet.Fields.Item("U_Buyer").Value.ToString(),
                            Stat = (string)lObjRecordSet.Fields.Item("Stat").Value.ToString(),
                            Orden = (int)lObjRecordSet.Fields.Item("Orden").Value
                        });
                        lObjRecordSet.MoveNext();
                    }
                }


            }
            catch (Exception lObjException)
            {
                UIApplication.ShowError(string.Format("GetActions: {0}", lObjException.Message));
                LogService.WriteError("MailSenderDAO (GetActions): " + lObjException.Message);
                LogService.WriteError(lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
            return lLstSellerReports;
        }

        public List<string> GetLastAuctions(string pStrCostingCode)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            List<string> lLstAuctions = new List<string>();
            try
            {
                string lStrQuery = this.GetSQL("GetLastsAuctions").InjectSingleValue("CostingCode",pStrCostingCode);

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


    }
}
