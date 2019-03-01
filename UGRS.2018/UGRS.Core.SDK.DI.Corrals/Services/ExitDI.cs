/*
Autor: LCC Abraham Saúl Sandoval Meneses
Collaborator: UG Luis Yanez
Description: DI API Livestock Exit
Date: 16/08/2018
Company: Qualisys
*/


using SAPbobsCOM;
using System;
using System.Linq;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Corrals.DAO;
using UGRS.Core.SDK.DI.Corrals.DTO;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Corrals.Services {
    public class IExitDI {

        /// <summary>
        /// Mass Invoicing
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public static ResultDTO CreateInventoryExit(DocumentDTO iExit, string type, UserValues user) {

            var objectCode = BoObjectTypes.oInventoryGenExit.ToString();
            var result = new ResultDTO();
            var distributionDAO = new DistributionDAO();

            try {
                if (iExit.AuthProcess) {
                    OpenGoodsIssuesForm(iExit, type, user);
                }
                else {

                    Documents lObjDocIExit = (Documents)DIApplication.Company.GetBusinessObject(BoObjectTypes.oInventoryGenExit); //OIGE

                    var task = Task.Factory.StartNew(() => {
                        lObjDocIExit.Series = distributionDAO.GetSeries(user.WhsCode, objectCode);
                        lObjDocIExit.UserFields.Fields.Item("U_GLO_BusinessPartner").Value = iExit.Document.Code;
                        lObjDocIExit.UserFields.Fields.Item("U_PE_Origin").Value = type;
                        lObjDocIExit.UserFields.Fields.Item("U_GLO_Status").Value = !user.AppraisalValidation ? "O" : "C";
                        lObjDocIExit.UserFields.Fields.Item("U_GLO_InMo").Value = "S-GAN";
                    });

                    var task2 = Task.Factory.StartNew(() => {
                        foreach (var line in iExit.Lines) {

                            lObjDocIExit.Lines.ItemCode = line.ItemCode;
                            lObjDocIExit.Lines.Quantity = line.Quantity;
                            lObjDocIExit.Lines.WarehouseCode = line.Corral;
                            lObjDocIExit.Lines.CostingCode = user.Area;

                            var batches = iExit.Batches.Where(b => b.Corral == line.Corral && b.AuctDate == line.AuctDate && b.ItemCode == line.ItemCode).AsParallel().ToList();
                            foreach (var batch in batches) {

                                if (batches.Count == 1) {
                                    lObjDocIExit.Lines.BatchNumbers.Quantity = line.Quantity;
                                    lObjDocIExit.Lines.BatchNumbers.BatchNumber = batch.Batch;
                                }
                                else {
                                    if (line.Quantity - batch.Quantity > 0) {
                                        lObjDocIExit.Lines.BatchNumbers.BatchNumber = batch.Batch;
                                        lObjDocIExit.Lines.BatchNumbers.Quantity = batch.Quantity;
                                        line.Quantity -= batch.Quantity;
                                    }
                                    else if (line.Quantity > 0) {
                                        lObjDocIExit.Lines.BatchNumbers.BatchNumber = batch.Batch;
                                        lObjDocIExit.Lines.BatchNumbers.Quantity = line.Quantity;
                                        line.Quantity -= batch.Quantity;
                                    }
                                }
                                lObjDocIExit.Lines.BatchNumbers.Add();
                            }
                            lObjDocIExit.Lines.Add();
                        }
                    });

                    Task.WaitAll(task, task2);

                    if (lObjDocIExit.Add() != 0) {
                        LogService.WriteError("InventoryExitDI (CreateDocument) " + DIApplication.Company.GetLastErrorDescription());
                        result.Success = false;
                        result.Message = "Error: " + DIApplication.Company.GetLastErrorDescription();
                    }
                    else {
                        int lIntInvGenExit = int.Parse(DIApplication.Company.GetNewObjectKey());
                        result.Success = true;
                        result.Message = string.Format("La Salida de Inventario se Realizó con Éxito con DocEntry: {0}", lIntInvGenExit);
                    }
                }
            }
            catch (AggregateException ae) {
                ae.Handle(e => {
                    HandleException(e, "InventoryExitDI (AE)");
                    result.Message = "Error: " + e.Message;
                    result.Success = false;
                    return true;
                });
            }
            catch (Exception ex) {
                HandleException(ex, "InventoryExitDI (Document)");
                result.Message = "Error: " + ex.Message;
                result.Success = false;
            }

            return result;
        }


        private static void OpenGoodsIssuesForm(DocumentDTO pObjExitDTO, string pStrType, UserValues pObjUser) {
            int lIntDraftKey = 0;
            string lStrObjectCode = "60";
            var result = new ResultDTO();
            var distributionDAO = new DistributionDAO();

            //lIntDraftKey = distributionDAO.GetDraftKey(pObjExitDTO.Document.Code);
            //if (lIntDraftKey == 0)
            //{
            SAPbobsCOM.Documents lObjDraftInvoice = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

            lObjDraftInvoice.DocObjectCodeEx = "60";
            lObjDraftInvoice.Series = distributionDAO.GetSeries(pObjUser.WhsCode, lStrObjectCode);
            lObjDraftInvoice.UserFields.Fields.Item("U_GLO_BusinessPartner").Value = pObjExitDTO.Document.Code;
            lObjDraftInvoice.UserFields.Fields.Item("U_PE_Origin").Value = pStrType;
            lObjDraftInvoice.UserFields.Fields.Item("U_GLO_Status").Value = !pObjUser.AppraisalValidation ? "O" : "A";
            lObjDraftInvoice.UserFields.Fields.Item("U_GLO_InMo").Value = "S-GAN";


            foreach (var line in pObjExitDTO.Lines) {

                lObjDraftInvoice.Lines.ItemCode = line.ItemCode;
                lObjDraftInvoice.Lines.Quantity = line.Quantity;
                lObjDraftInvoice.Lines.WarehouseCode = line.Corral;
                lObjDraftInvoice.Lines.CostingCode = pObjUser.Area;

                var batches = pObjExitDTO.Batches.Where(b => b.Corral == line.Corral && b.AuctDate == line.AuctDate && b.ItemCode == line.ItemCode).AsParallel().ToList();
                foreach (var batch in batches) {

                    if (batches.Count == 1) {
                        lObjDraftInvoice.Lines.BatchNumbers.Quantity = line.Quantity;
                        lObjDraftInvoice.Lines.BatchNumbers.BatchNumber = batch.Batch;
                    }
                    else {
                        if (line.Quantity - batch.Quantity > 0) {
                            lObjDraftInvoice.Lines.BatchNumbers.BatchNumber = batch.Batch;
                            lObjDraftInvoice.Lines.BatchNumbers.Quantity = batch.Quantity;
                            line.Quantity -= batch.Quantity;
                        }
                        else if (line.Quantity > 0) {
                            lObjDraftInvoice.Lines.BatchNumbers.BatchNumber = batch.Batch;
                            lObjDraftInvoice.Lines.BatchNumbers.Quantity = line.Quantity;
                            line.Quantity -= batch.Quantity;
                        }
                    }
                    lObjDraftInvoice.Lines.BatchNumbers.Add();
                }
                lObjDraftInvoice.Lines.Add();
            }

            if (lObjDraftInvoice.Add() != 0) {
                string gg = DIApplication.Company.GetLastErrorDescription();
                LogService.WriteError("InventoryExitDI (CreateDocument) " + DIApplication.Company.GetLastErrorDescription());
            }
            else {

                //lIntDraftKey = distributionDAO.GetDraftKey(pObjExitDTO.Document.Code);
                lIntDraftKey = int.Parse(DIApplication.Company.GetNewObjectKey());

                if (lIntDraftKey > 0) {
                    OpenDraft(lIntDraftKey);
                }
            }
            //}
            //else
            //{
            //    OpenDraft(lIntDraftKey);
            //}
        }

        private static void OpenDraft(int lIntDraftKey) {
            SAPbouiCOM.Form lObjFormDraft = SAPbouiCOM.Framework.Application.SBO_Application.OpenForm((SAPbouiCOM.BoFormObjectEnum)112, "", lIntDraftKey.ToString());
        }

        #region Handle Exception
        /// <summary>
        /// Handle Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="section"></param>
        public static void HandleException(Exception ex, string section) {
            LogService.WriteError(String.Format("{0}: {1}, {2}", section, ex.Message, ex.StackTrace));
        }
        #endregion
    }
}





