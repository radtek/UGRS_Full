/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Transfers For Food Plant Data Object
Date: 31/08/2018
Company: Qualisys
*/


using SAPbobsCOM;
using System;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.FoodTransfer.DTO;
using UGRS.Core.Services;
using UGRS.Core.Utility;

namespace UGRS.Core.SDK.DI.FoodTransfer.Services {

    public class ProductionOrderDI {

        public static Result CancelDocument(int docEntry, int entryID, int exitID) {

            var result = new Result();
            Documents oStockEntry = null;
            Documents oStockExit = null;
            ProductionOrders oProductionOrder = null;

            try {

                Parallel.Invoke(
                     () => {
                         oProductionOrder = (ProductionOrders)DIApplication.Company.GetBusinessObject(BoObjectTypes.oProductionOrders); //OWOR
                         oProductionOrder.GetByKey(docEntry);
                     },
                     () => {
                         oStockEntry = (Documents)DIApplication.Company.GetBusinessObject(BoObjectTypes.oInventoryGenEntry); //OIGN
                         oStockEntry.GetByKey(entryID);
                     },
                     () => {
                         oStockExit = (Documents)DIApplication.Company.GetBusinessObject(BoObjectTypes.oInventoryGenExit); //OIGE
                         oStockExit.GetByKey(exitID);
                     });


                oProductionOrder.ProductionOrderStatus = BoProductionOrderStatusEnum.boposClosed;
                oProductionOrder.UserFields.Fields.Item("U_GLO_DocNum").Value = oStockEntry.DocNum;
                oProductionOrder.UserFields.Fields.Item("U_GLO_DocNumSal").Value = oStockExit.DocNum;

                if(oProductionOrder.Update() != 0) {
                    LogService.WriteError("TransferDI (Draft) " + DIApplication.Company.GetLastErrorDescription());
                    result.Success = false;
                    result.Message = "Error: " + DIApplication.Company.GetLastErrorDescription();
                }
                else {
                    result.Success = true;
                    result.Message = String.Format("La Orden de Producción#{0} se cerro con éxito!", oProductionOrder.DocumentNumber);
                }
            }

            catch(Exception ex) {
                HandleException(ex, "StockEntryDI.CreateDocument", ref result);
            }
            finally {
                MemoryUtility.ReleaseComObject(oProductionOrder);
            }

            return result;
        }


        #region HandleException
        public static void HandleException(Exception ex, string section, ref Result result) {
            LogService.WriteError(String.Format("{0}: {1}", section, ex.Message));
            result.Success = false;
            result.Message = "Error: " + DIApplication.Company.GetLastErrorDescription();
        }
        #endregion



    }
}
