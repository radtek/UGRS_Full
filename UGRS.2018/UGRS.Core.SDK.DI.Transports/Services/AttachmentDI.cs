using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Transports.DAO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Transports.Services
{
    public class AttachmentDI
    {
        private AttachmentDAO mObjAttachmentDAO;

        public AttachmentDI()
        {
            mObjAttachmentDAO = new AttachmentDAO();
        }

        public string GetAttachPath()
        {
            return mObjAttachmentDAO.GetAttachPath();
        }

        public int AttachFile(string pStrFile)
        {
            int lIntAttEntry = -1;
            SAPbobsCOM.Attachments2 lObjAttachment = (SAPbobsCOM.Attachments2)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oAttachments2);
            try
            {
                string lStrFileName = pStrFile;
                lObjAttachment.Lines.Add();
                lObjAttachment.Lines.FileName = System.IO.Path.GetFileNameWithoutExtension(lStrFileName);
                lObjAttachment.Lines.FileExtension = System.IO.Path.GetExtension(lStrFileName).Substring(1);
                lObjAttachment.Lines.SourcePath = System.IO.Path.GetDirectoryName(lStrFileName);
                lObjAttachment.Lines.Override = SAPbobsCOM.BoYesNoEnum.tYES;

                if (lObjAttachment.Add() == 0)
                {

                    lIntAttEntry = int.Parse(DIApplication.Company.GetNewObjectKey());
                }
                else
                {
                    LogService.WriteError(string.Format("[AttachmentDI - AttachFile] {0}", DIApplication.Company.GetLastErrorDescription()));
                    throw new Exception(DIApplication.Company.GetLastErrorDescription());
                }

            }
            catch (Exception ex)
            {
                LogService.WriteError(string.Format("[AttachmentDI - AttachFile] {0}", ex.Message));
                LogService.WriteError(ex);
                throw new Exception(string.Format("Error al guardar el archivo adjunto {0}: {1}", pStrFile, ex.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjAttachment);
            }
            return lIntAttEntry;
        }
    }
}
