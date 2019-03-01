using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UGRS.Core.Exceptions;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Exceptions;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.WebServicePermissions.DTO;
using UGRS.Core.Services;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.WebServicePermissions.DAO {
    public class PermissionsDAO {
        QueryManager mObjQueryManager;
        DateTime mDblTime;

        public PermissionsDAO() {
            mObjQueryManager = new QueryManager();
        }

        public bool IsRequestPreparedToCreateSaleOrder(string pStrRequestId) {
            return IsPermissionRequestPrepared(pStrRequestId)
                && IsProductRequestPrepared(pStrRequestId)
                && IsDestinationRequestPrepared(pStrRequestId)
                //&& IsPortRequestPrepared(pStrRequestId)
                && IsParameterRequestPrepared(pStrRequestId) ? true : false;
        }

        public bool ExistsSaleOrder(string pStrRequestId) {
            SAPbobsCOM.Recordset lObjRecordSet = null;
            Dictionary<string, string> lLstStrParameters = null;
            bool lBolResult = false;
            string lStrQuery = "";

            try {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RequestId", pStrRequestId);

                lStrQuery = this.GetSQL("CountSaleOrderByRequestId").Inject(lLstStrParameters);
                lObjRecordSet.DoQuery(lStrQuery);

                if(lObjRecordSet.RecordCount > 0) {
                    lBolResult = int.Parse(lObjRecordSet.Fields.Item(0).Value.ToString()) > 0;
                }
            }
            catch(Exception lObjException) {
                throw new QueryException(lObjException.Message, lObjException);
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }

            return lBolResult;
        }

        public int CreateSaleOrder(string pStrRequestId) {
            Documents lObjSaleOrder = null;

            try {

               
                List<String> lLstRequest = GetALLRequest();
              
                //Quitar
               
                    mDblTime = DateTime.Now;
                    lObjSaleOrder = null;
                    lObjSaleOrder = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                    lObjSaleOrder = PopulateSaleOrder(lObjSaleOrder, pStrRequestId);


                    int lIntResult = lObjSaleOrder.Add();
                    if (lIntResult == 0)
                    {
                        LogService.WriteSuccess("[SaleOrder CREATED]");
                        int lIntDocEntry = Convert.ToInt32(DIApplication.Company.GetNewObjectKey());
                        if (OrderIsMinValue(lIntDocEntry))
                        {
                            LogService.WriteSuccess("[SaleOrder < 50]");
                            LogService.WriteSuccess("[SaleOrder Adding MinService]");
                            AddMinService(lIntDocEntry);
                        }

                    }
                    else
                    {
                        LogService.WriteError("CODE ERROR" + lIntResult.ToString());
                        LogService.WriteError(DIApplication.Company.GetLastErrorDescription());
                    }
                   
                    GetTime("Create Sale Order");

                    return lIntResult;
            }
            catch(Exception lObjException) {
                LogService.WriteError(lObjException.Message);
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjSaleOrder);
            }
        }

        private void GetTime(string lStrmesage)
        {
            TimeSpan lTmsTime = DateTime.Now - mDblTime;
            LogService.WriteInfo(lTmsTime.Seconds + "." + lTmsTime.Milliseconds + " Segundos " + lStrmesage);
        }

        public int UpdateSaleOrder(string pStrRequestId) {
            Documents lObjSaleOrder = null;

            try {
                lObjSaleOrder = GetSaleOrderByRequestId(pStrRequestId);
                LogService.WriteInfo("[SaleOrderToUpdate:" + lObjSaleOrder.DocNum + "]");
                lObjSaleOrder = PopulateSaleOrder(lObjSaleOrder, pStrRequestId);
                LogService.WriteInfo("[SaleOrdeUpdate:" + lObjSaleOrder.DocNum + "]");
                return lObjSaleOrder.Update();
            }
            catch(Exception lObjException) {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjSaleOrder);
            }
        }

        public int CancelSaleOrder(string pStrRequestId) {
            Documents lObjSaleOrder = null;

            try {
                lObjSaleOrder = GetSaleOrderByRequestId(pStrRequestId);
                return lObjSaleOrder.Cancel();
            }
            catch(Exception lObjException) {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjSaleOrder);
            }
        }

        public int CloseSaleOrder(string pStrRequestId) {
            Documents lObjSaleOrder = null;

            try {
                lObjSaleOrder = GetSaleOrderByRequestId(pStrRequestId);
                return lObjSaleOrder.Close();
            }
            catch(Exception lObjException) {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjSaleOrder);
            }

        }

        public int GetNextUgrsFolio(string pStrPrefix) {
            SAPbobsCOM.Recordset lObjRecordSet = null;
            Dictionary<string, string> lLstStrParameters = null;
            string lStrQuery = "";

            try {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Prefix", pStrPrefix);

                lStrQuery = this.GetSQL("GetNextUgrsFolio").Inject(lLstStrParameters);
                lObjRecordSet.DoQuery(lStrQuery);

                return lObjRecordSet.RecordCount > 0 ? (int)lObjRecordSet.Fields.Item("UgrsFolio").Value : 0;
            }
            catch(Exception lObjException) {
                throw new QueryException(lObjException.Message, lObjException);
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
        }


        public List<string> GetALLRequest()
        {
            SAPbobsCOM.Recordset lObjRecordSet = null;
           
            string lStrQuery = "";
            List<string> lLstRequest = new List<string>();

            try
            {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);


                lStrQuery = "select U_RequestId from [dbo].[@UG_PE_WS_PERE] ";
                lObjRecordSet.DoQuery(lStrQuery);

                if (lObjRecordSet.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordSet.RecordCount; i++)
                    {
                        lLstRequest.Add(lObjRecordSet.Fields.Item("U_RequestId").Value.ToString());
                        lObjRecordSet.MoveNext();
                    }
                }
                return lLstRequest;
            }
            catch (Exception lObjException)
            {
                throw new QueryException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
        }

        public string GetPrefix(string pStrType)
        {
            return mObjQueryManager.GetValue("U_Value", "Name", pStrType, "[@UG_CONFIG]");
        }

        public string GetRequestIdByPermissionRequestCode(string pStrCode)
        {
            return mObjQueryManager.GetValue("U_RequestId", "Code", pStrCode, "[@UG_PE_WS_PERE]");
        }

        private bool IsPermissionRequestPrepared(string pStrRequestId)
        {
            bool lBolResult = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PERE]") > 0 ? true : false;
            if (!lBolResult)
            {
                LogService.WriteError("IsPermissionRequestPrepared false");
            }
            return lBolResult;
        }

        private bool IsProductRequestPrepared(string pStrRequestId)
        {
            bool lBolResult = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PRRE]") > 0 ? true : false;
            if (!lBolResult)
            {
                LogService.WriteError("IsProductRequestPrepared false");
            }
            return lBolResult;
        }

        private bool IsDestinationRequestPrepared(string pStrRequestId)
        {
            bool lBolResult =  mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_DERE]") > 0 ? true : false;
            if (!lBolResult)
            {
                LogService.WriteError("IsDestinationRequestPrepared false");
            }
            return lBolResult;
        }

        private bool IsPortRequestPrepared(string pStrRequestId) {
            string lStrResult = mObjQueryManager.GetValue("U_MobilizationTypeId", "U_RequestId", pStrRequestId, "[@UG_PE_WS_PERE]");
            bool lBoolFlag = false;
            LogService.WriteInfo("RESULTQUERY " + lStrResult);
            switch(lStrResult) {
                case "1":
                lBoolFlag = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PORE]") == 1 ? true : false;
                break;
                case "2":
                    int lIntPoRe = (mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PORE]"));
                    if (lIntPoRe == 2 || lIntPoRe == 1)
                    {
                        lBoolFlag = true;
                    }
                    else
                    {
                        lBoolFlag = false;
                    }
                //lBoolFlag = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PORE]") == 2 ? true : false;
                break;
                case "3":
                lBoolFlag = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PORE]") == 2 ? true : false;
                break;
                case "4":
                lBoolFlag = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PORE]") == 0 ? true : false;
                break;
                case "5":
                lBoolFlag = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PORE]") == 1 ? true : false;
                break;
            }

            if (!lBoolFlag)
            {
                LogService.WriteError("IsPortRequestPrepared false " + lStrResult);
            }
            return lBoolFlag;
        }

        private bool IsParameterRequestPrepared(string pStrRequestId) {
            string lStrResult = mObjQueryManager.GetValue("U_ParentProductId", "U_RequestId", pStrRequestId, "[@UG_PE_WS_PRRE]");
            bool lBoolReturn = false;
            int lIntResult = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PRRE]");
            // Equino - FaunaSilvestre	
            if(lStrResult == "105" || lStrResult == "118") {
                lBoolReturn = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PARE]") == (3 * lIntResult) ? true : false;
            }
            // Bovino - Caprino - Ovino
            if(lStrResult == "1" || lStrResult == "94" || lStrResult == "99") {
                lBoolReturn = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PARE]") == (4 * lIntResult) ? true : false;
            }
            // Conejos - Lacteos
            if(lStrResult == "169") {
                lBoolReturn = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PARE]") == (1 * lIntResult) ? true : false;
            }

            if (!lBoolReturn)
            {
                LogService.WriteError("IsParameterRequestPrepared false " + lStrResult);
            }

            return lBoolReturn;

        }

        private int GetSaleOrderKeyByRequestId(string pStrRequestId) {
            Recordset lObjRecordset = null;
            int lIntResult = 0;

            try {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RequestId", pStrRequestId);

                string lStrQuery = this.GetSQL("GetSaleOrderByRequestId").Inject(lLstStrParameters);
                lObjRecordset.DoQuery(lStrQuery);

                if(lObjRecordset.RecordCount > 0) {
                    lIntResult = Convert.ToInt32(lObjRecordset.Fields.Item("DocEntry").Value.ToString());
                }
            }
            catch(Exception lObjException) {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lIntResult;
        }

        private Documents GetSaleOrderByRequestId(string pStrRequestId) {
            Documents lObjSaleOrder = null;

            lObjSaleOrder = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
            int x = GetSaleOrderKeyByRequestId(pStrRequestId);

            lObjSaleOrder.GetByKey(x);

            return lObjSaleOrder;
        }

        private bool OrderIsMinValue(int pIntDocEntry)
        {
            Documents lObjDoc = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
            bool lbolResult = false;
            if (lObjDoc.GetByKey(pIntDocEntry))
            {
                lbolResult = lObjDoc.DocTotal< 50 ? true: false;
            }
            return lbolResult;
        }

        private bool AddMinService(int pIntDocEntry)
        {
            Documents lObjDoc = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
              bool lbolResult = false;
              if (lObjDoc.GetByKey(pIntDocEntry))
              {
                  double lDblPrice = 50 - lObjDoc.DocTotal;
                  string lStrItemCode = mObjQueryManager.GetValue("U_Value", "Name", "PE_SERV_MIN", "[@UG_CONFIG]");
                  if (!string.IsNullOrEmpty(lStrItemCode))
                  {
                      double lDblTaxRate = GetItemTaxRate(lStrItemCode);
                      lDblPrice = lDblPrice / (1 + lDblTaxRate / 100);

                      lObjDoc.Lines.Add();
                      lObjDoc.Lines.SetCurrentLine(lObjDoc.Lines.Count - 1);
                      lObjDoc.Lines.ItemCode = lStrItemCode;
                      lObjDoc.Lines.Price = lDblPrice;

                      if (lObjDoc.Update() == 0)
                      {
                          lbolResult = true;
                          LogService.WriteSuccess("[SaleOrder UPDATE MinTotal]");
                      }
                      else
                      {
                          LogService.WriteError("[SaleOrder UPDDATE Fail MinTotal");
                          LogService.WriteError("AddMinService" + DIApplication.Company.GetLastErrorDescription());
                          lbolResult = false;
                      }
                  }
              }
              return lbolResult;
        }

        private double GetItemTaxRate(string pStrItemCode)
        {
            Recordset lObjRecordset = null;
            double ldblResult = 0;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("ItemCode", pStrItemCode);

                string lStrQuery = this.GetSQL("GetItemTaxRate").Inject(lLstStrParameters);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    ldblResult = Convert.ToInt32(lObjRecordset.Fields.Item("Rate").Value.ToString());
                }
            }
            catch (Exception lObjException)
            {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return ldblResult;
        }

        private Documents PopulateSaleOrder(Documents pObjSaleOrder, string pStrRequestId) {
            PermissionRequestDTO lObjPermissionRequest = GetPermissionRequest(pStrRequestId);
            LogService.WriteInfo("[SaleOrder Begin]");
            //Header
            pObjSaleOrder.CardCode = lObjPermissionRequest.CardCode;

            if (string.IsNullOrEmpty(lObjPermissionRequest.CardCode))
            {
                string lStrCustomerCode = mObjQueryManager.GetValue("U_ProducerId", "U_RequestId", pStrRequestId, "[@UG_PE_WS_PERE]");
                string lStrCustomerName = mObjQueryManager.GetValue("U_Producer", "U_RequestId", pStrRequestId, "[@UG_PE_WS_PERE]");
                LogService.WriteError("No fue posible encontrar el socio de negocio: "+ lStrCustomerName+ " Codigo: " + lStrCustomerCode);
            }
            pObjSaleOrder.DocDate = lObjPermissionRequest.Date;
            pObjSaleOrder.DocDueDate = lObjPermissionRequest.CrossingDate;
            pObjSaleOrder.DocumentsOwner = GetPermissionUser();
            
            //pObjSaleOrder.Series = 147;
            lObjPermissionRequest.Entry2 = lObjPermissionRequest.Entry == lObjPermissionRequest.Entry2 ? "": lObjPermissionRequest.Entry2;
            lObjPermissionRequest.Departure2 = lObjPermissionRequest.Departure2 == lObjPermissionRequest.Departure ? "" : lObjPermissionRequest.Departure2;


            pObjSaleOrder.UserFields.Fields.Item("U_PE_IdPermitType").Value = Truncate(lObjPermissionRequest.MobilizationTypeId.ToString(), pObjSaleOrder.UserFields.Fields.Item("U_PE_IdPermitType").Size); //Campo enlazado al catalogo de tipos de movilizaciones
            pObjSaleOrder.UserFields.Fields.Item("U_PE_RequestCodeUGRS").Value = Truncate(lObjPermissionRequest.UgrsRequest, pObjSaleOrder.UserFields.Fields.Item("U_PE_RequestCodeUGRS").Size);
            pObjSaleOrder.UserFields.Fields.Item("U_PE_FolioUGRS").Value = Truncate(lObjPermissionRequest.UgrsFolio, pObjSaleOrder.UserFields.Fields.Item("U_PE_FolioUGRS").Size);
            pObjSaleOrder.UserFields.Fields.Item("U_PE_ChargeTo").Value = Truncate(lObjPermissionRequest.Producer, pObjSaleOrder.UserFields.Fields.Item("U_PE_ChargeTo").Size);
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Requests").Value = Truncate(lObjPermissionRequest.Producer, pObjSaleOrder.UserFields.Fields.Item("U_PE_Requests").Size);
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Representative").Value = Truncate(lObjPermissionRequest.Producer, pObjSaleOrder.UserFields.Fields.Item("U_PE_Representative").Size);
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Asociacion").Value = null; (No se recibe por el WS)
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Phone").Value = Truncate(lObjPermissionRequest.ProducerTelephone, pObjSaleOrder.UserFields.Fields.Item("U_PE_Phone").Size);
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Entry").Value = Truncate(lObjPermissionRequest.Entry, pObjSaleOrder.UserFields.Fields.Item("U_PE_Entry").Size);
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Departure").Value = Truncate(lObjPermissionRequest.Departure, pObjSaleOrder.UserFields.Fields.Item("U_PE_Departure").Size);

            string lStrEntry = Truncate(lObjPermissionRequest.Entry, pObjSaleOrder.UserFields.Fields.Item("U_PE_Entry").Size);
            string lSTrDeparture = Truncate(lObjPermissionRequest.Departure, pObjSaleOrder.UserFields.Fields.Item("U_PE_Departure").Size);
            string lStrEntry2 = Truncate(lObjPermissionRequest.Entry2, pObjSaleOrder.UserFields.Fields.Item("U_PE_Entry2").Size);
            string lStrDeaperture2 = Truncate(lObjPermissionRequest.Departure2, pObjSaleOrder.UserFields.Fields.Item("U_PE_Deaperture2").Size);

            pObjSaleOrder.UserFields.Fields.Item("U_PE_Entry2").Value = lStrEntry == lStrEntry2? "" : Truncate(lObjPermissionRequest.Entry2, pObjSaleOrder.UserFields.Fields.Item("U_PE_Entry2").Size);
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Deaperture2").Value = lSTrDeparture == lStrDeaperture2 ? "" :  Truncate(lObjPermissionRequest.Departure2, pObjSaleOrder.UserFields.Fields.Item("U_PE_Deaperture2").Size);
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Transport").Value = Truncate(lObjPermissionRequest.TransportId.ToString(), pObjSaleOrder.UserFields.Fields.Item("U_PE_Transport").Size); //Campo enlazado al catalogo de transportes
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Origin").Value = Truncate(lObjPermissionRequest.Origin, pObjSaleOrder.UserFields.Fields.Item("U_PE_Origin").Size);
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Customs1").Value = Truncate(lObjPermissionRequest.Customs1 != 0 ? lObjPermissionRequest.Customs1.ToString() : "", pObjSaleOrder.UserFields.Fields.Item("U_PE_Customs1").Size); //Campo enlazado al catalogo de aduanas
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Customs2").Value = Truncate(lObjPermissionRequest.Customs2 != 0 ? lObjPermissionRequest.Customs2.ToString() : "", pObjSaleOrder.UserFields.Fields.Item("U_PE_Customs2").Size); //Campo enlazado al catalogo de aduanas
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Destination").Value = Truncate(lObjPermissionRequest.Destination, pObjSaleOrder.UserFields.Fields.Item("U_PE_Destination").Size);
            pObjSaleOrder.UserFields.Fields.Item("U_PE_Location").Value = Truncate(lObjPermissionRequest.CustomerLocation, pObjSaleOrder.UserFields.Fields.Item("U_PE_Location").Size);


            //pObjSaleOrder.UserFields.Fields.Item("U_PE_IdPermitType").Value = lObjPermissionRequest.MobilizationTypeId.ToString(); //Campo enlazado al catalogo de tipos de movilizaciones
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_RequestCodeUGRS").Value = lObjPermissionRequest.UgrsRequest;
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_FolioUGRS").Value = lObjPermissionRequest.UgrsFolio;
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_ChargeTo").Value = lObjPermissionRequest.Producer;
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Requests").Value = lObjPermissionRequest.Producer;
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Representative").Value = lObjPermissionRequest.Producer;
            ////pObjSaleOrder.UserFields.Fields.Item("U_PE_Asociacion").Value = null; (No se recibe por el WS)
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Phone").Value = lObjPermissionRequest.ProducerTelephone;
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Entry").Value = lObjPermissionRequest.Entry;
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Departure").Value = lObjPermissionRequest.Departure;
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Transport").Value = lObjPermissionRequest.TransportId.ToString(); //Campo enlazado al catalogo de transportes
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Origin").Value = lObjPermissionRequest.Origin;
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Customs1").Value = lObjPermissionRequest.Customs1 != 0 ? lObjPermissionRequest.Customs1.ToString() : ""; //Campo enlazado al catalogo de aduanas
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Customs2").Value = lObjPermissionRequest.Customs2 != 0 ? lObjPermissionRequest.Customs2.ToString() : ""; //Campo enlazado al catalogo de aduanas
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Destination").Value = lObjPermissionRequest.Destination;
            //pObjSaleOrder.UserFields.Fields.Item("U_PE_Location").Value = lObjPermissionRequest.CustomerLocation;

            pObjSaleOrder = GetLines(pObjSaleOrder, pStrRequestId, lObjPermissionRequest.MobilizationTypeId);

            return pObjSaleOrder;
        }

        private string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }




        private Documents GetLines(Documents pObjSaleOrder, string pStrRequestId, int pIntMobilizationTypeId) {
            SAPbobsCOM.Recordset lObjRecordSet = null;
            string lStrInsurenceItemCode = "";
            string lStrQuery = "";
            string lStrResult = mObjQueryManager.GetValue("U_ParentProductId", "U_RequestId", pStrRequestId, "[@UG_PE_WS_PRRE]");
            LogService.WriteInfo("[SaleOrder: TipoGanado " + lStrResult + "]");
            if(lStrResult == "105" && pIntMobilizationTypeId == 2) {
                lStrInsurenceItemCode = mObjQueryManager.GetValue("U_Value", "Name", "PE_SEG_EQUINO", "[@UG_CONFIG]");
                string lStrItem = string.IsNullOrEmpty(lStrInsurenceItemCode.ToString()) ? "Item no encontrado" : lStrInsurenceItemCode.ToString();
                LogService.WriteInfo("[SaleOrder: ItemCode " + lStrItem + "]");

                double lDouInsurencePrice = GetInsurencePrice(lStrInsurenceItemCode.ToString());
                if (lDouInsurencePrice <= 0)
                {
                    LogService.WriteInfo("[SaleOrder: Precio no encontrado");
                }
                LogService.WriteInfo("[SaleOrder: Precio " + lDouInsurencePrice + "]");


            }
            if(lStrResult == "1" && pIntMobilizationTypeId == 2) {
                lStrInsurenceItemCode = mObjQueryManager.GetValue("U_Value", "Name", "PE_SEG_BOVINO", "[@UG_CONFIG]");
                string lStrItem = string.IsNullOrEmpty(lStrInsurenceItemCode.ToString()) ? "Item no encontrado" : lStrInsurenceItemCode.ToString();
                LogService.WriteInfo("[SaleOrder: ItemCode " + lStrItem + "]");

                double lDouInsurencePrice = GetInsurencePrice(lStrInsurenceItemCode.ToString());
                if (lDouInsurencePrice <= 0)
                {
                    LogService.WriteInfo("[SaleOrder: Precio no encontrado");
                }
                LogService.WriteInfo("[SaleOrder: Precio " + lDouInsurencePrice + "]");

            }

          
        
            try {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RequestId", pStrRequestId);
                lLstStrParameters.Add("MobilizationTypeId", pIntMobilizationTypeId.ToString());
                lStrQuery = this.GetSQL("GetItemAndPriceByProductRequests").Inject(lLstStrParameters);

                lObjRecordSet.DoQuery(lStrQuery);

                for (int i = 0; i < pObjSaleOrder.Lines.Count; i++)
                {
                    pObjSaleOrder.Lines.SetCurrentLine(i);
                    pObjSaleOrder.Lines.Delete();
                }


                lStrInsurenceItemCode = mObjQueryManager.GetValue("U_Value", "Name", "PE_SEG_BOVINO", "[@UG_CONFIG]");
                int lIntLintCountRows = mObjQueryManager.Count("U_RequestId", pStrRequestId, "[@UG_PE_WS_PRRE]");

                if(lObjRecordSet.RecordCount > 0 && lObjRecordSet.RecordCount == lIntLintCountRows) {
                    for(int i = 0; i < lObjRecordSet.RecordCount; i++) {
                        //pObjSaleOrder.Lines.SetCurrentLine(i);
                        pObjSaleOrder.Lines.WarehouseCode = "PEHE";
                        pObjSaleOrder.Lines.ItemCode = lObjRecordSet.Fields.Item("U_ItemCode").Value.ToString();
                        pObjSaleOrder.Lines.Quantity = Convert.ToDouble(lObjRecordSet.Fields.Item("U_Quantity").Value.ToString());
                        pObjSaleOrder.Lines.Price = Convert.ToDouble(lObjRecordSet.Fields.Item("Price").Value.ToString());
                        pObjSaleOrder.Lines.ItemDetails = lObjRecordSet.Fields.Item("U_Description").Value.ToString();
                        pObjSaleOrder.Lines.CostingCode = "OG_PERMI";
                        //pObjSaleOrder.Update();
                        pObjSaleOrder.Lines.Add();


                        string lStrInsureCU = string.Empty;
                        string lStrInsureTrans = string.Empty;

                        lStrInsureCU = lObjRecordSet.Fields.Item("U_InsureCU").Value.ToString();
                        lStrInsureTrans = lObjRecordSet.Fields.Item("U_InsureTrans").Value.ToString();

                        if (!string.IsNullOrEmpty(lStrInsureCU)) 
                        {
                            pObjSaleOrder.Lines.WarehouseCode = "PEHE";
                            pObjSaleOrder.Lines.ItemCode = lStrInsureCU;
                            double lDblQuantity = Convert.ToDouble(lObjRecordSet.Fields.Item("U_Quantity").Value.ToString());
                            double lDblPrice = GetInsurencePrice(lStrInsureCU.ToString());
                            pObjSaleOrder.Lines.Quantity = 1;
                            pObjSaleOrder.Lines.Price =  Math.Ceiling(lDblPrice * lDblQuantity);
                            pObjSaleOrder.Lines.CostingCode = "OG_PERMI";
                            pObjSaleOrder.Lines.Add();
                        }

                        if (!string.IsNullOrEmpty(lStrInsureTrans))
                        {
                            pObjSaleOrder.Lines.WarehouseCode = "PEHE";
                            pObjSaleOrder.Lines.ItemCode = lStrInsureTrans;
                            double lDblQuantity = Convert.ToDouble(lObjRecordSet.Fields.Item("U_Quantity").Value.ToString());
                            double lDblPrice = GetInsurencePrice(lStrInsureTrans.ToString());
                            pObjSaleOrder.Lines.Quantity = 1;
                            pObjSaleOrder.Lines.Price = Math.Ceiling(lDblPrice * lDblQuantity);
                            pObjSaleOrder.Lines.CostingCode = "OG_PERMI";
                            pObjSaleOrder.Lines.Add();
                        }
                        lObjRecordSet.MoveNext();
                    }
                    //double lDblTotal = pObjSaleOrder.DocTotal;
                }
                else
                {
                    LogService.WriteError("No fue posible encontrar los articulos favor de revisar la tabla de relación con SAP IdRequest:"+ pStrRequestId);
                }
            }
            catch(Exception ex) {
                LogService.WriteInfo(ex.Message + " " + ex.StackTrace);
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }

            return pObjSaleOrder;
        }

        public PermissionRequestDTO GetPermissionRequest(string pStrRequestId) {
            PermissionRequestDTO lObjResult = new PermissionRequestDTO();
            SAPbobsCOM.Recordset lObjRecordSet = null;
            string lStrQuery = "";

            try {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RequestId", pStrRequestId);

                lStrQuery = this.GetSQL("GetPermissionRequestById").Inject(lLstStrParameters);
                lObjRecordSet.DoQuery(lStrQuery);

                if(lObjRecordSet.RecordCount > 0) {
                    foreach(PropertyInfo lObjProperty in lObjResult.GetType().GetProperties().Where(x => x.GetMethod.IsPublic && !x.GetMethod.IsVirtual)) {
                        try {
                            lObjProperty.SetValue
                            (
                                lObjResult,
                                Convert.ChangeType
                                (
                                    lObjRecordSet.Fields.Item(lObjProperty.Name).Value.ToString(),
                                    lObjProperty.PropertyType
                                )
                            );
                        }
                        catch {
                            //Ignore ;)
                        }
                    }
                }
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }

            return lObjResult;
        }

        /// <summary>
        /// GetRowCode Method
        /// Get RowCode by RequestId
        /// </summary>
        /// <param name="pStrTable"></param>
        /// <param name="pStrRequestId"></param>
        /// <returns></returns>
        public string GetRowCode(string pStrTable, string pStrRequestId) {
            string lStrResult = mObjQueryManager.GetValue("Code", "U_RequestId", pStrRequestId, pStrTable);

            return lStrResult;
        }

        public string GetRowCodeByProduct(string pStrTable, string pStrRequestId, int pIntProductId) {
            SAPbobsCOM.Recordset lObjRecordSet = null;
            string lStrResult = "";
            string lStrQuery;
            try {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RequestId", pStrRequestId);
                lLstStrParameters.Add("ProductId", pIntProductId.ToString());

                lStrQuery = this.GetSQL("GetRowCodeByProduct").Inject(lLstStrParameters);
                lObjRecordSet.DoQuery(lStrQuery);

                lStrResult = lObjRecordSet.Fields.Item(0).Value.ToString();
            }
            catch {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
            finally {

            }


            return lStrResult;
        }

        public string IsPortExist(string pStrRequestId, int pIntPortId, int pIntPortType) {
            SAPbobsCOM.Recordset lObjRecordSet = null;
            string lStrResult = "";
            string lStrQuery;
            try {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RequestId", pStrRequestId);
                lLstStrParameters.Add("PortId", pIntPortId.ToString());
                lLstStrParameters.Add("PortType", pIntPortType.ToString());

                lStrQuery = this.GetSQL("CountPortRequestByPortId").Inject(lLstStrParameters);
                lObjRecordSet.DoQuery(lStrQuery);

                lStrResult = lObjRecordSet.Fields.Item(0).Value.ToString();
            }
            catch {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
            finally {

            }


            return lStrResult;
        }

        public string GetRowCodeByPort(string pStrRequestId, int pIntPortId, int pIntPortType) {
            SAPbobsCOM.Recordset lObjRecordSet = null;
            string lStrResult = "";
            string lStrQuery;
            try {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RequestId", pStrRequestId);
                lLstStrParameters.Add("PortId", pIntPortId.ToString());
                lLstStrParameters.Add("PortType", pIntPortType.ToString());

                lStrQuery = this.GetSQL("GetRowCodeByPort").Inject(lLstStrParameters);
                lObjRecordSet.DoQuery(lStrQuery);

                lStrResult = lObjRecordSet.Fields.Item(0).Value.ToString();
            }
            catch {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
            finally {

            }


            return lStrResult;
        }

        public int GetUgrsFolio(string pStrRequestId) {
            int lStrResult = int.Parse(mObjQueryManager.GetValue("U_UgrsFolio", "U_RequestId", pStrRequestId, "[@UG_PE_WS_PERE]"));

            return lStrResult;
        }

        public int GetPermissionUser()
        {
            int lStrResult = 0;
            try
            {
                lStrResult = int.Parse(mObjQueryManager.GetValue("USERID", "USER_CODE", "PERMISOS", "OUSR"));

               
            }
            catch (Exception)
            {

                throw;
            }
            return lStrResult;
        }

        public double GetInsurencePrice(string pStrItemCode) {
            SAPbobsCOM.Recordset lObjRecordSet = null;
            double lDouResult = 0;
            string lStrQuery = "";

            try {
                lObjRecordSet = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("ItemCode", pStrItemCode);
                lStrQuery = this.GetSQL("GetInsurencePrice").Inject(lLstStrParameters);

                lObjRecordSet.DoQuery(lStrQuery);

                lDouResult = double.Parse(lObjRecordSet.Fields.Item(0).Value.ToString());
            }
            finally {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
            return lDouResult;
        }

        public bool ExistRequest(string pStrRequestId)
        {
            try
            {
                bool lBolExist = mObjQueryManager.Exists("U_RequestId", pStrRequestId, "[@UG_PE_WS_PERE]");
                LogService.WriteInfo("Existe solicitud: "+pStrRequestId+ " " + lBolExist);
                return lBolExist;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
