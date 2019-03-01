using System.Collections.Generic;
using UGRS.Core.SDK.DI.Auctions.DAO;
using UGRS.Core.SDK.DI.Auctions.DTO;
using CrystalDecisions.CrystalReports.Engine;
using UGRS.Reports.Auctions.Reports.Sellers;
using System;

namespace UGRS.Core.SDK.DI.Auctions.Services
{
    public class AuctionSellersService
    {
        private MailSenderDAO mObjMailSenderDAO = new MailSenderDAO();
        private MailSenderService mObjMailSenderService = new MailSenderService();

        public string GetSellers(string pStrAuction)
        {
            return mObjMailSenderDAO.GetAuctionSellers(pStrAuction);
        }

        public void GetSellersBatches(List<SellerSenderDTO> pLstSellers, string pStrAuction)
        {
            foreach (var lVarSeller in pLstSellers)
            {
                IList<SellerReportDTO> lLstSellerReports = mObjMailSenderDAO.GetSellerBatches(lVarSeller.Seller, pStrAuction);

                GenerateReport(lLstSellerReports, lVarSeller.Seller, pStrAuction, lVarSeller.Mail);
            }


        }

        private void GenerateReport(IList<SellerReportDTO> pLstSellerReports, string pStrSeller, string pStrAuction, string pStrSellerMail)
        {
            try
            {
                ReportDocument lObjReportDocument = null;

                lObjReportDocument = new SellerReportToSend();

                lObjReportDocument.SetDataSource(pLstSellerReports);
                lObjReportDocument.SetParameterValue("AuctionsFolio", pStrAuction);
                lObjReportDocument.SetParameterValue("SellerName", pStrSeller);

                //Crystal Report to MemoryStream Conversion 
                using (System.IO.MemoryStream lObjMemoryStream = new System.IO.MemoryStream())
                {
                    using (
                    System.IO.FileStream lStreamPDF = (System.IO.FileStream)lObjReportDocument.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat))
                    {
                        lStreamPDF.CopyTo(lObjMemoryStream);

                        System.IO.StreamWriter lObjWriter = new System.IO.StreamWriter(lStreamPDF);

                        lObjWriter.Write("x");
                        lObjWriter.Flush();
                        lObjWriter.Dispose();
                        lObjMemoryStream.Position = 0;

                        string lStrFileName = pStrSeller + pStrAuction + DateTime.Now.ToString("DDmmss") + ".PDF";

                        mObjMailSenderService.SendMail(lObjMemoryStream, pStrSellerMail, lStrFileName);
                    }
                }

            }
            catch (System.Exception lObjException)
            {

                //throw; ignore
            }

        }

        public List<string> GetLastAuctions(string pStrCostingCode)
        {
            return mObjMailSenderDAO.GetLastAuctions(pStrCostingCode);
        }
    }
}
