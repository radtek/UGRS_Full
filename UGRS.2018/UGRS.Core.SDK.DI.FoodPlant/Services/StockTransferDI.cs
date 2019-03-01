/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: Transfers For Food Plant Data Object
Date: 31/08/2018
Company: Qualisys
*/

using QualisysLog;
using SAPbobsCOM;
using System;
using UGRS.Core.SDK.DI.FoodPlant.DAO;
using UGRS.Core.SDK.DI.FoodPlant.DTO;
using UGRS.Core.SDK.UI;

namespace UGRS.Core.SDK.DI.FoodPlant.Services {

    public class StockTransferDI {


        public static ResultDTO CreateDraft(RequestTransfer[] transferRequest, User user) {

            var result = new ResultDTO();
            string transitWhs = String.Empty;
            var objectCode = BoObjectTypes.oInventoryTransferRequest;
            var item = String.Empty;
            FoodPlantDAO foodPlantDAO = new FoodPlantDAO();

            try {

                var oStockTransferDraft = (Documents)DIApplication.Company.GetBusinessObject(BoObjectTypes.oDrafts); //ODRF

                oStockTransferDraft.CardCode = transferRequest[0].Code;
                oStockTransferDraft.DocObjectCode = BoObjectTypes.oStockTransfer;
                oStockTransferDraft.UserFields.Fields.Item("U_GLO_BusinessPartner").Value = transferRequest[0].Code;
                oStockTransferDraft.UserFields.Fields.Item("U_PL_WhsReq").Value = transferRequest[0].FromWhs;
                oStockTransferDraft.Series = user.Series;
                oStockTransferDraft.Comments = transferRequest[0].Observations;

                foreach(var line in transferRequest) {

                    transitWhs = foodPlantDAO.GetAvailableTransitWarehouse(line.Item);
                    if(String.IsNullOrEmpty(transitWhs)) {
                        item = line.Item;
                        break;
                    }

                    oStockTransferDraft.Lines.ItemCode = line.Item;
                    oStockTransferDraft.Lines.BaseEntry = line.Folio;
                    oStockTransferDraft.Lines.BaseLine = line.LineNum;
                    oStockTransferDraft.Lines.WarehouseCode = foodPlantDAO.GetAvailableTransitWarehouse(line.Item);
                    oStockTransferDraft.Lines.Quantity = line.Quantity;
                    oStockTransferDraft.Lines.BaseType = (int)objectCode;
                    oStockTransferDraft.Lines.CostingCode = user.Area;
                    oStockTransferDraft.Lines.Add();
                }

                if(String.IsNullOrEmpty(transitWhs)) {
                    return new ResultDTO() { Success = false, Message = String.Format("Error: No hay almacenes de transito disponibles para este artículo {0}", item) };
                }

                if(oStockTransferDraft.Add() != 0) {
                    UIApplication.ShowMessageBox(string.Format("Exception: {0}", DIApplication.Company.GetLastErrorDescription()));
                    QsLog.WriteError("TransferDI (Draft) " + DIApplication.Company.GetLastErrorDescription());
                    result.Success = false;
                    result.Message = "Error: " + DIApplication.Company.GetLastErrorDescription();
                }
                else {
                    result.Success = true;
                    result.Message = DIApplication.Company.GetNewObjectKey();
                }
            }
            catch(AggregateException ae) {
                ae.Handle(e => {
                    HandleException(e, "TransferDI(Draft)");
                    result.Message = "Error: " + e.Message;
                    result.Success = false;
                    return true;
                });
            }
            catch(Exception ex) {

                HandleException(ex, "TransferDI(Draft)");
                result.Message = "Error: " + ex.Message;
                result.Success = false;
            }

            return result;
        }

        public static ResultDTO TransferPlant(DocumentTransfer transfer, bool switchWhs) {

            var result = new ResultDTO();
            string transitWhs = String.Empty;
            var objectCode = BoObjectTypes.oStockTransfer;

            FoodPlantDAO foodPlantDAO = new FoodPlantDAO();

            try {

                var oStockTransfer = (StockTransfer)DIApplication.Company.GetBusinessObject(objectCode); //ODRF

                oStockTransfer.FromWarehouse = (switchWhs) ? transfer.Document.FromWhs : transfer.Document.ToWhs;
                oStockTransfer.ToWarehouse = (switchWhs) ? transfer.Document.ToWhs : transfer.Document.FromWhs;
                oStockTransfer.UserFields.Fields.Item("U_GLO_Status").Value = "O";

                foreach(var line in transfer.Lines) {

                    oStockTransfer.Lines.ItemCode = line.Item;
                    oStockTransfer.Lines.ItemDescription = line.Desc;
                    oStockTransfer.Lines.Quantity = line.Quantity;
                    oStockTransfer.Lines.UserFields.Fields.Item("U_GLO_BagsBales").Value = line.Bags;
                    oStockTransfer.Lines.FromWarehouseCode = (switchWhs) ? line.FromWhs : line.ToWhs;
                    oStockTransfer.Lines.WarehouseCode = (switchWhs) ? oStockTransfer.ToWarehouse : oStockTransfer.FromWarehouse;
                    oStockTransfer.Lines.Add();
                }


                if(oStockTransfer.Add() != 0) {
                    UIApplication.ShowMessageBox(string.Format("Exception: {0}", DIApplication.Company.GetLastErrorDescription()));
                    QsLog.WriteError("TransferDI (Document) " + DIApplication.Company.GetLastErrorDescription());
                    result.Success = false;
                    result.Message = "Error: " + DIApplication.Company.GetLastErrorDescription();
                }
                else {
                    result.Success = true;
                    result.Message = String.Format("El translado del almacen {0} al almacen {1} se realizó con éxito", oStockTransfer.FromWarehouse, oStockTransfer.ToWarehouse);
                }
            }
            catch(AggregateException ae) {
                ae.Handle(e => {
                    HandleException(e, "TransferDI(Document)");
                    result.Message = "Error: " + e.Message;
                    result.Success = false;
                    return true;
                });
            }
            catch(Exception ex) {
                HandleException(ex, "TransferDI(Document)");
                result.Message = "Error: " + ex.Message;
                result.Success = false;
            }
            return result;
        }

        #region HandleException
        public static void HandleException(Exception ex, string section) {
            QsLog.WriteError(String.Format("{0}: {1}", section, ex.Message));
            QsLog.WriteException(ex);
        }
        #endregion



    }
}
