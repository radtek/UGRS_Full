/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Alert Message Service DI
Date: 04/10/2018
Company: Qualisys
*/


using System;
using UGRS.Core.SDK.DI.FoodTransfer.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Utility;
using SAPbobsCOM;
using UGRS.Core.Services;


namespace UGRS.Core.SDK.DI.FoodTransfer.Services {

    public class AlertMessageDI {

        public static void Create(MessageDTO message) {

            Messages oMessage = (Messages)DIApplication.Company.GetBusinessObject(BoObjectTypes.oMessages);
            try {

                oMessage.Subject = message.Message;
                oMessage.Recipients.Add();
                oMessage.Recipients.UserCode = message.UserCode;
                oMessage.Recipients.UserType = BoMsgRcpTypes.rt_InternalUser;
                oMessage.Recipients.SendInternal = BoYesNoEnum.tYES;
                oMessage.Recipients.SendEmail = BoYesNoEnum.tNO;

                if(oMessage.Add() != 0) {
                    UIApplication.ShowMessageBox(DIApplication.Company.GetLastErrorDescription());
                    LogService.WriteError("MessageDI (CreateMessage) " + DIApplication.Company.GetLastErrorDescription());
                }
                else {
                    LogService.WriteError(String.Format("MessageDI (CreateMessage) Alerta guardada correctamente para el usuario {0}, {1}", message.UserCode, message.Message));
                }
            }
            catch(Exception ex) {
                LogService.WriteError("MessageDI (CreateMessage) " + ex.Message + "," + ex.StackTrace);
            }
            finally {
                MemoryUtility.ReleaseComObject(oMessage);
            }
        }
    }
}
