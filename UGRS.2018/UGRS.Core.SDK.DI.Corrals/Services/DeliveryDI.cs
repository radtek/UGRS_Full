/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: DI API Delivery
Date: 16/08/2018
Company: Qualisys
*/

using System;
using UGRS.Core.SDK.DI.Corrals.DAO;
using UGRS.Core.SDK.DI.Corrals.DTO;
using UGRS.Core.SDK.UI;
using System.Threading.Tasks;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Corrals.Services {

    public class DeliveryDI {
        /// <summary>
        /// Create a Food Delivery in Corrals. Data will be stores in Tables ODLN and DLN1 frin SAP B1
        /// </summary>
        public static ResultDTO CreateDelivery(DeliveryDTO deliveryDTO) {

            var result = new ResultDTO();
            var lObjDistributionDAO = new DistributionDAO();

            try {

                SAPbobsCOM.Documents lObjDocDelivery = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes); //ODLN}

                var task = Task.Run(() => {
                    lObjDocDelivery.CardCode = deliveryDTO.CardCode;
                    lObjDocDelivery.CardName = deliveryDTO.CardName;
                    lObjDocDelivery.UserFields.Fields.Item("U_GLO_BusinessPartner").Value = deliveryDTO.CardCode;
                    lObjDocDelivery.DocDate = deliveryDTO.DocDate;
                    lObjDocDelivery.Series = deliveryDTO.Series;
                });

                var task2 = Task.Factory.StartNew(() => {
                    foreach(var docLine in deliveryDTO.DocLines) { //DLN
                        lObjDocDelivery.Lines.ItemCode = docLine.ItemCode;
                        lObjDocDelivery.Lines.ItemDescription = docLine.Dscription;
                        lObjDocDelivery.Lines.Quantity = docLine.Quantity;
                        lObjDocDelivery.Lines.WarehouseCode = docLine.WhsCode;
                        lObjDocDelivery.Lines.UserFields.Fields.Item("U_GLO_BagsBales").Value = docLine.BagsBales;
                        lObjDocDelivery.Lines.UserFields.Fields.Item("U_GLO_Corral").Value = docLine.Corral;
                        lObjDocDelivery.Lines.CostingCode = docLine.Area;
                        lObjDocDelivery.Lines.Price = docLine.Price;
                        lObjDocDelivery.Lines.Add();
                    }
                });

                Task.WaitAll(task, task2);

                if(lObjDocDelivery.Add() != 0) {
                    LogService.WriteError("DeliveryDI (CreateDocument) " + DIApplication.Company.GetLastErrorDescription());
                   
                    result.Message = DIApplication.Company.GetLastErrorDescription();
                    result.Success = false;
                    return result;
                }

            }
            catch(Exception ex) {
                LogService.WriteError(String.Format("{0}: {1}, {2}", "DeliveryDI(Exception)", ex.Message, ex.StackTrace));
                result.Success = false;
                result.Message = ex.Message;
                return result;
            }

            result.Message = "La Entrega se realizó con éxito";
            result.Success = true;
            return result;
        }
    }
}
