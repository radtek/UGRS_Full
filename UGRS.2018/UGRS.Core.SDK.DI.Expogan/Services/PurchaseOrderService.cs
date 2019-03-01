using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.Expogan.Tables;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Expogan.Services
{
    public class PurchaseOrderService
    {
        public string CreateDocument(string pStrCardCode, double pDblTotal, string pStrRef)//(List<Locations> pObjLocation, string pStrCardCode)
        {

            ExpoganServiceFactory mObjExpoganService = new ExpoganServiceFactory();
            string lStrDocEntry = string.Empty;

            SAPbobsCOM.Documents lObjDocument = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

            lObjDocument.CardCode = pStrCardCode;
            lObjDocument.DocDueDate = DateTime.Now;
            lObjDocument.Lines.COGSCostingCode = mObjExpoganService.GetLocationService().GetCostCenter();
            lObjDocument.Lines.ItemCode = mObjExpoganService.GetLocationService().GetItemCode("EX_ITEMSTAND");
            lObjDocument.Lines.Price = pDblTotal; //GetPryce
            lObjDocument.Lines.Add();
            if (lObjDocument.Add() != 0)
            {
                string lStrError = DIApplication.Company.GetLastErrorDescription();
                UIApplication.ShowMessageBox(string.Format("Exception: {0}", DIApplication.Company.GetLastErrorDescription()));
                LogService.WriteError("PurchaseOrderDI (CreateDocument) " + DIApplication.Company.GetLastErrorDescription());
            }
            else
            {
                return DIApplication.Company.GetNewObjectKey().ToString();

            }

            return lStrDocEntry;
        }

    }
}
