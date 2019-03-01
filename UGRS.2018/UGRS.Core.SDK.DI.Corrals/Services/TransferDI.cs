/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: DI API Stock Transfer
Date: 29/08/2018
Company: Qualisys
*/


using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using UGRS.Core.SDK.DI.Corrals.DAO;
using UGRS.Core.SDK.DI.Corrals.DTO;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Corrals.Services {

    public class LivestockTransfer {

        #region Stock Transfer
        public static ResultDTO CreateStockTransfer(List<LivestockDTO> livestock, List<BatchDTO> allBatches, int series) {

            DistributionDAO distributionDAO = new DistributionDAO();
            var result = new ResultDTO();
    
            try {

                var oStockTransfer = (StockTransfer)DIApplication.Company.GetBusinessObject(BoObjectTypes.oStockTransfer); //OWTR

                oStockTransfer.UserFields.Fields.Item("U_GLO_BusinessPartner").Value = livestock[0].Code;
                oStockTransfer.FromWarehouse = "CRHE";
                oStockTransfer.ToWarehouse = "SUHE";
                oStockTransfer.Series = series;
                oStockTransfer.DocDate = DateTime.Now;

                foreach(var line in livestock) {

                    oStockTransfer.Lines.ItemCode = line.ItemCode;
                    oStockTransfer.Lines.ItemDescription = line.ItemName;
                    oStockTransfer.Lines.Quantity = line.Quantity;
                    oStockTransfer.Lines.FromWarehouseCode = line.Corral;
                    oStockTransfer.Lines.WarehouseCode = "SUHE";

                    var batches = allBatches.Where(b => b.Corral == line.Corral && b.AuctDate == line.AuctDate && b.ItemCode == line.ItemCode).AsParallel().ToList();
                    foreach(var batch in batches) {

                        if(batches.Count == 1) {
                            oStockTransfer.Lines.BatchNumbers.Quantity = line.Quantity;
                            oStockTransfer.Lines.BatchNumbers.BatchNumber = batch.Batch;
                        }
                        else {
                            if(line.Quantity - batch.Quantity > 0) {
                                oStockTransfer.Lines.BatchNumbers.BatchNumber = batch.Batch;
                                oStockTransfer.Lines.BatchNumbers.Quantity = batch.Quantity;
                                line.Quantity -= batch.Quantity;
                            }
                            else if(line.Quantity > 0) {
                                oStockTransfer.Lines.BatchNumbers.BatchNumber = batch.Batch;
                                oStockTransfer.Lines.BatchNumbers.Quantity = line.Quantity;
                                line.Quantity -= batch.Quantity;
                            }
                        }
                        oStockTransfer.Lines.BatchNumbers.Add();
                    }
                    oStockTransfer.Lines.Add();
                }

                if(oStockTransfer.Add() != 0) {
                    
                    LogService.WriteError("StockTransfer (CreateDocument) " + DIApplication.Company.GetLastErrorDescription());
                    result.Success = false;
                    result.Message = "Error: " + DIApplication.Company.GetLastErrorDescription();
                }
                else {
                    result.Success = true;
                    result.Message = "El translado de corrales a subsata se realizó con éxito";
                }
            }
            catch(Exception ex) {
                HandleException(ex, "TransferDI");
            }

            return result;
        }
        #endregion

        #region Handle Exception
        public static void HandleException(Exception ex, string section) {
            LogService.WriteError(String.Format("{0}: {1}, {2}", section, ex.Message, ex.StackTrace));
        }
        #endregion
    }
}


