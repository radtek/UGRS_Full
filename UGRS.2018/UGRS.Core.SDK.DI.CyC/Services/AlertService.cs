using System;
using UGRS.Core.SDK.DI.CyC.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Utility;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.CyC.Services
{
   public class AlertService
    {
        /// <summary>
        /// Guardar alerta.
        /// </summary>
       public bool SaveAlert(MessageDTO pObjMessageDTO)
        {
            SAPbobsCOM.Messages lObjMsg = (SAPbobsCOM.Messages)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oMessages);
            try
            {
                lObjMsg.Subject = pObjMessageDTO.Message;
                lObjMsg.Recipients.Add();
                lObjMsg.Recipients.UserCode = pObjMessageDTO.UserCode;
                lObjMsg.Recipients.UserType = SAPbobsCOM.BoMsgRcpTypes.rt_InternalUser;
                lObjMsg.Recipients.SendInternal = SAPbobsCOM.BoYesNoEnum.tYES;
                lObjMsg.Recipients.SendEmail = SAPbobsCOM.BoYesNoEnum.tNO;

                //lObjMsg.AddDataColumn("Factura", "Gasolina", SAPbobsCOM.BoObjectTypes.oPurchaseInvoices, "87");

                //lObjMsg.Priority = SAPbobsCOM.BoMsgPriorities.pr_High;
                int Result = lObjMsg.Add();
                if (Result != 0)
                {
                    UIApplication.ShowMessageBox(DIApplication.Company.GetLastErrorDescription());
                    LogService.WriteError("CreateAlert (SaveAlert) " + DIApplication.Company.GetLastErrorDescription());
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
