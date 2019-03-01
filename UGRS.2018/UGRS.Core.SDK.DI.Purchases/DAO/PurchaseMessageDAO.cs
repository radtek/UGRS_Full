using UGRS.Core.Services;
using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Purchases.DTO;
using UGRS.Core.SDK.UI;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;


namespace UGRS.Core.SDK.DI.Purchases.DAO
{
    public class PurchaseMessageDAO
    {
        QueryManager mObjQueryManager = new QueryManager();
        #region Message

        /// <summary>
        /// Obtener Mensaje de la tabla definida.
        /// </summary>
        public IList<MessageDTO> GetMessage(string pStrMessageType)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            IList<MessageDTO> lLstMessageDTO = new List<MessageDTO>();
            try
            {

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Clave", pStrMessageType);
                string lStrQuery = this.GetSQL("GetMessage").Inject(lLstStrParameters);
                //this.UIAPIRawForm.DataSources.DataTables.Item("RESULT").ExecuteQuery(lStrQuery);

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        MessageDTO lObjMessageDTO = new MessageDTO();
                        lObjMessageDTO.UserId = lObjRecordset.Fields.Item("U_USERID").Value.ToString();
                        lObjMessageDTO.Message = lObjRecordset.Fields.Item("U_MSG").Value.ToString();
                        lObjMessageDTO.UserCode = GetUserCode(lObjMessageDTO.UserId);
                        lLstMessageDTO.Add(lObjMessageDTO);
                        lObjRecordset.MoveNext();
                    }
                }

            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetMessage): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstMessageDTO;
        }


        /// <summary>
        /// Obtener los usuarios encargados de area para enviar los mensajes.
        /// </summary>
        public IList<MessageDTO> GetUsersMessage(string pStrMessage, string pStrCostCenter)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            IList<MessageDTO> lLstMessageDTO = new List<MessageDTO>();
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("CostCenter", pStrCostCenter);
                string lStrQuery = this.GetSQL("GetMessageUsers").Inject(lLstStrParameters);
                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        MessageDTO lObjMessageDTO = new MessageDTO();
                        lObjMessageDTO.UserCode = lObjRecordset.Fields.Item("USER_CODE").Value.ToString();
                        lObjMessageDTO.UserId = GetUserId(lObjMessageDTO.UserCode);
                        lObjMessageDTO.Message = pStrMessage;
                        lLstMessageDTO.Add(lObjMessageDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitDataSourcesException: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetUsersMessage): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstMessageDTO;
        }


        public string GetUserCode(string pStrUserId)
        {
            string lStrUserCode = "";
            try
            {
                lStrUserCode = mObjQueryManager.GetValue("USER_CODE", "USERID", pStrUserId, "OUSR");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetDocNum: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetUserCode): " + ex.Message);
                LogService.WriteError(ex);

            }

            return lStrUserCode;
        }

        public string GetUserId(string pStrUserCode)
        {
            string lStrUserCode = "";
            try
            {
                lStrUserCode = mObjQueryManager.GetValue("USERID", "USER_CODE", pStrUserCode, "OUSR");
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("GetDocNum: {0}", ex.Message));
                LogService.WriteError("PurchasesDAO (GetUserId): " + ex.Message);
                LogService.WriteError(ex);
            }

            return lStrUserCode;
        }

        #endregion
    }
}
