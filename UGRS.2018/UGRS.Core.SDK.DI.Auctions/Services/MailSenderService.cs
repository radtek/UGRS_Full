using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Auctions.DAO;

namespace UGRS.Core.SDK.DI.Auctions.Services
{
    public class MailSenderService
    {

        private MailSenderDAO mObjMailSenderDAO = new MailSenderDAO();

        public void SendMail(System.IO.MemoryStream pMSReport, string pStrSellerMail, string pStrFileName)
        {

            string lStrSMTClient = "";
            string lStrFromMail = "";
            string lStrPassword = "";
            string lStr = "";
            int lIntPort = 0;


            MailMessage lObjMail = new MailMessage();
            SmtpClient lObjSmtpClient = new SmtpClient("smtp.gmail.com");
            System.Net.Mail.Attachment lObjAttachment = null;
            System.Net.Mime.ContentType lObjContentType = null;
            System.Net.NetworkCredential NetworkCredential = null;
            try
            {
                //MemoryStream Report to PDF Attachment
                lObjContentType = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                lObjAttachment = new System.Net.Mail.Attachment(pMSReport, lObjContentType);
                lObjAttachment.ContentDisposition.FileName = pStrFileName;

                //Credentials and SMTP client config
                NetworkCredential = new System.Net.NetworkCredential("alechuga717@gmail.com", "29091984");
                lObjSmtpClient.Credentials = NetworkCredential;
                lObjSmtpClient.Port = 587;
                lObjSmtpClient.EnableSsl = true;


                //Mail 
                lObjMail.From = new MailAddress("alechuga717@gmail.com", "Andres");
                lObjMail.To.Add(pStrSellerMail);

                lObjMail.Subject = "test email";
                lObjMail.Body = "test mail body ";
                lObjMail.Attachments.Add(lObjAttachment);

                //Send Mail
                lObjSmtpClient.Send(lObjMail);
            }
            catch (Exception e)
            {
                string dd = e.Message;
            }
            finally
            {
                lObjMail.Dispose();
                lObjSmtpClient.Dispose();
                lObjAttachment.Dispose();
            }

        }


        public string GetCostingCode(string pStrUserName)
        {
            return mObjMailSenderDAO.GetCostingCode(pStrUserName);
        }
    }
}
