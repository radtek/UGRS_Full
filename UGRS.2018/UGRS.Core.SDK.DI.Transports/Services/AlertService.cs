using System;
using UGRS.Core.SDK.DI.Tramsport.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.Transports.Services
{
    public class AlertService
    {
        public bool SaveAlert(MessageDTO pObjMessageDTO)
        {
            SAPbobsCOM.Messages lObjMsg = (SAPbobsCOM.Messages)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages);
            try
            {
                lObjMsg.Subject = pObjMessageDTO.Message;
                foreach (string lStrUserCode in pObjMessageDTO.UserCode)
                {
                    lObjMsg.Recipients.Add();
                    lObjMsg.Recipients.UserCode = lStrUserCode;
                    lObjMsg.Recipients.UserType = SAPbobsCOM.BoMsgRcpTypes.rt_InternalUser;
                    lObjMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES;
                    lObjMsg.Recipients.SendEmail = SAPbobsCOM.BoYesNoEnum.tNO;

                }
          
                //lObjMsg.AddDataColumn("Factura", "Gasolina", SAPbobsCOM.BoObjectTypes.oPurchaseInvoices, "87");

                //lObjMsg.Priority = SAPbobsCOM.BoMsgPriorities.pr_High;
                int Result = lObjMsg.Add();
                if (Result != 0)
                {
                    LogService.WriteError("CreateAlert (SaveAlert) " + DIApplication.Company.GetLastErrorDescription());
                    UIApplication.ShowMessageBox(DIApplication.Company.GetLastErrorDescription());
                    
                    return false;
                }
                else
                {
                    LogService.WriteSuccess("CreateAlert (SaveAlert) Alerta guardada correctamente " + pObjMessageDTO.Message);
                    return true;
                }
            }
            catch (Exception ex)
            {

                UIApplication.ShowMessageBox(string.Format(ex.Message + "\n" + "Exception: {0}", DIApplication.Company.GetLastErrorDescription()));
                LogService.WriteError("CreateAlert (SaveAlert) " + ex.Message);
                LogService.WriteError("CreateAlert (SaveAlert) " + DIApplication.Company.GetLastErrorDescription());
                LogService.WriteError(ex);
                return false;
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjMsg);
            }
        }
    }
}
