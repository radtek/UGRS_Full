/*
Autor: LCC Abraham Saúl Sandoval Meneses
Description: DI API Invoice Document And Invoice Draft
Date: 16/08/2018
Company: Qualisys
*/

using SAPbobsCOM;
using System;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Corrals.DAO;
using UGRS.Core.SDK.DI.Corrals.DTO;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Corrals.Services {
    public class InvoiceDI {


        #region Create Invoice Document
        /// <summary>
        /// Mass Invoicing
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public static ResultDTO CreateInvoice(DocumentDTO invoice, UserValues user, FloorService floorServiceItem, string type) {

            var result = new ResultDTO();

            if(invoice.Document.Debt == 0) {
                result.Success = true;
                result.Message = "Ya Se Habia Facturado";
                return result;
            }

            var massInvoicingDAO = new MassInvoicingDAO();
            var distributionDAO = new DistributionDAO();
            var objectCode = BoObjectTypes.oInvoices.ToString();
            Documents lObjDocInvoice = null;
            //var containMainUsage = true;

            try {

                //Documents lObjDocInvoice = (Documents)DIApplication.Company.GetBusinessObject(BoObjectTypes.oDrafts); //OINV
                lObjDocInvoice = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts);

                lObjDocInvoice.DocDate = DateTime.Now;
                lObjDocInvoice.CardCode = invoice.Document.Code;
                lObjDocInvoice.CardName = invoice.Document.Name;
                lObjDocInvoice.PaymentMethod = "99";
                lObjDocInvoice.DocObjectCodeEx = "13";
                lObjDocInvoice.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = "P01";
                lObjDocInvoice.Series = distributionDAO.GetSeries(user.WhsCode, objectCode);
                lObjDocInvoice.PaymentGroupCode = massInvoicingDAO.GetPayCondition(invoice.Document.Code);
                lObjDocInvoice.UserFields.Fields.Item("U_PE_Origin").Value = type;
                lObjDocInvoice.UserFields.Fields.Item("U_GLO_BusinessPartner").Value = invoice.Document.Code;
                //lObjDocInvoice.EDocExportFormat = 5;
                lObjDocInvoice.EDocGenerationType = EDocGenerationTypeEnum.edocGenerate;
                lObjDocInvoice.NumAtCard = string.Format("CM_CR_{0}", DateTime.Today.ToString("ddMMyy"));

                #region Comments
                //BusinessPartners BPartner = (BusinessPartners)DIApplication.Company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                //BPartner.GetByKey(lObjDocInvoice.CardCode);

                //if (BPartner.UserFields.Fields.Item("U_B1SYS_MainUsage").Value.ToString() == "")
                //{
                //    BPartner.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = "P01";
                //    containMainUsage = false;
                //}
                //lObjDocInvoice.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = BPartner.UserFields.Fields.Item("U_B1SYS_MainUsage").Value;
                #endregion

                foreach(var line in invoice.FloorServiceLines) {
                    lObjDocInvoice.Lines.ItemCode = floorServiceItem.ItemCode;
                    lObjDocInvoice.Lines.Quantity = (line.Existence * line.TotalDays);
                    lObjDocInvoice.Lines.WarehouseCode = line.Corral;
                    lObjDocInvoice.Lines.Price = floorServiceItem.Price;
                    lObjDocInvoice.Lines.CostingCode = user.Area;
                    lObjDocInvoice.Lines.UserFields.Fields.Item("U_SU_BatchAuc").Value = (type.Equals("N")) ? line.Batch : line.DocEntry.ToString();

                    lObjDocInvoice.Lines.Add();

                }

                foreach(var line in invoice.DeliveryLines) {

                    lObjDocInvoice.Lines.ItemCode = line.ItemCode;
                    lObjDocInvoice.Lines.Quantity = line.Quantity;
                    lObjDocInvoice.Lines.Price = line.Price;
                    lObjDocInvoice.Lines.CostingCode = user.Area;
                    lObjDocInvoice.Lines.BaseEntry = line.DocEntry;
                    lObjDocInvoice.Lines.BaseLine = line.LineNum;
                    lObjDocInvoice.Lines.BaseType = 15; //ODLN
                    lObjDocInvoice.Lines.Add();
                }


                if(lObjDocInvoice.Add() != 0) {
                    var error = DIApplication.Company.GetLastErrorDescription();
                    LogService.WriteError("InvoiceDI (CreateDocument) " + error);
                    result.Success = false;
                    result.Message = "Error: " + error;
                    return result;
                }
                else {
                    string lStrDocEntry = DIApplication.Company.GetNewObjectKey();

                    result.Success = true;
                    result.Message = string.Format("La Factura se realizó con éxito con el DocEntry: {0}", lStrDocEntry);

                    //Update StockTransfer When Auction Invoicing 
                    if(type.Equals("S")) {

                        var transferDocEntries = massInvoicingDAO.GetTransferDocEntries(invoice.Document.Code);

                        if(transferDocEntries != null) {
                            if(transferDocEntries.Length > 0) {
                                Task.Factory.StartNew(() => {
                                    Parallel.ForEach(transferDocEntries, docEntry => {
                                        StockTransfer lObjDocTransfer = (StockTransfer)DIApplication.Company.GetBusinessObject(BoObjectTypes.oStockTransfer); //OWTR
                                        lObjDocTransfer.GetByKey(docEntry);
                                        lObjDocTransfer.UserFields.Fields.Item("U_GLO_Status").Value = "C";
                                        var resultCode = lObjDocTransfer.Update();
                                        if(resultCode != 0) {
                                            var error = DIApplication.Company.GetLastErrorDescription();
                                            LogService.WriteError("InvoiceDI (Update Stock Transfers) " + error);
                                            result.Message += ", " + error;
                                        }
                                    });
                                });
                            }
                        }
                        else {
                            result.Success = false;
                            result.Message += string.Format(" - No se encontró la transferencia para el SN {0}", invoice.Document.Code);
                        }
                    }
                    return result;
                }
            }
            catch(AggregateException ae) {
                ae.Handle(e => {
                    HandleException(e, "InvoiceDI(Document)");
                    return true;
                });
            }
            catch(Exception ex) {
                HandleException(ex, "InvoiceDI(Document)" + ex.Message + " " + ex.InnerException.Message);
            }
            return new ResultDTO() { Success = false, Message = "Error: No Se Pudo Crear La Factura Para el Cliente " + invoice.Document.Name };
        }
        #endregion

        #region Create Invoice Draft
        /// <summary>
        /// Mass Invoicing
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public static ResultDTO CreateDraft(DocumentDTO invoice, UserValues user, FloorService floorServiceItem, string type) {

            var result = new ResultDTO();
            var massInvoicingDAO = new MassInvoicingDAO();
            var distributionDAO = new DistributionDAO();
            var objectCode = "13";
            var containMainUsage = true;

            try {

                Documents lObjDocInvDrf = (Documents)DIApplication.Company.GetBusinessObject(BoObjectTypes.oDrafts); //ODRF

                var task = Task.Run(() => {
                    lObjDocInvDrf.CardCode = invoice.Document.Code;
                    lObjDocInvDrf.CardName = invoice.Document.Name;
                    lObjDocInvDrf.PaymentMethod = "99";

                    BusinessPartners BPartner = (BusinessPartners)DIApplication.Company.GetBusinessObject(BoObjectTypes.oBusinessPartners);
                    BPartner.GetByKey(lObjDocInvDrf.CardCode);

                    if(BPartner.UserFields.Fields.Item("U_B1SYS_MainUsage").Value.ToString() == "") {
                        BPartner.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = "P01";
                        containMainUsage = false;
                    }
                    lObjDocInvDrf.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = BPartner.UserFields.Fields.Item("U_B1SYS_MainUsage").Value;


                    //lObjDocInvDrf.UserFields.Fields.Item("U_B1SYS_MainUsage").Value = "Por Definir";
                    lObjDocInvDrf.UserFields.Fields.Item("U_PE_Origin").Value = type;
                    lObjDocInvDrf.DocObjectCode = BoObjectTypes.oInvoices;
                    lObjDocInvDrf.PaymentGroupCode = massInvoicingDAO.GetPayCondition(invoice.Document.Code);
                    lObjDocInvDrf.Series = distributionDAO.GetSeries(user.WhsCode, objectCode);
                    lObjDocInvDrf.EDocGenerationType = EDocGenerationTypeEnum.edocGenerate;
                });

                var task2 = Task.Factory.StartNew(() => {
                    foreach(var line in invoice.FloorServiceLines) {

                        lObjDocInvDrf.Lines.ItemCode = floorServiceItem.ItemCode;
                        lObjDocInvDrf.Lines.WarehouseCode = line.Corral;
                        lObjDocInvDrf.Lines.Quantity = (line.Existence * line.TotalDays);
                        lObjDocInvDrf.Lines.Price = floorServiceItem.Price;
                        lObjDocInvDrf.Lines.CostingCode = user.Area;
                        lObjDocInvDrf.Lines.UserFields.Fields.Item("U_SU_BatchAuc").Value = line.Batch;
                        lObjDocInvDrf.Lines.Add();
                    }

                    foreach(var line in invoice.DeliveryLines) {

                        lObjDocInvDrf.Lines.ItemCode = line.ItemCode;
                        lObjDocInvDrf.Lines.Quantity = line.Quantity;
                        lObjDocInvDrf.Lines.Price = line.Price;
                        lObjDocInvDrf.Lines.CostingCode = user.Area;
                        lObjDocInvDrf.Lines.BaseEntry = line.DocEntry;
                        lObjDocInvDrf.Lines.BaseLine = line.LineNum;
                        lObjDocInvDrf.Lines.BaseType = 15; //ODLN
                        lObjDocInvDrf.Lines.Add();
                    }
                });

                //lObjDocInvDrf.EDocGenerationType = SAPbobsCOM.EDocGenerationTypeEnum.edocGenerate;
                //lObjDocInvDrf.EDocExportFormat = 9;
                Task.WaitAll(task, task2);



                if(lObjDocInvDrf.Add() != 0) {
                    LogService.WriteError("InvoiceDI (Draft) " + DIApplication.Company.GetLastErrorDescription());
                    result.Success = false;
                    result.Message = "Error: " + DIApplication.Company.GetLastErrorDescription();
                }
                else {
                    string lStrDocEntry = DIApplication.Company.GetNewObjectKey();

                    result.Success = true;
                    result.Message = DIApplication.Company.GetNewObjectKey();
                    if(containMainUsage == false) {
                        result.Success = true;
                        result.Message = DIApplication.Company.GetNewObjectKey();
                        SAPbouiCOM.Framework.Application.SBO_Application.StatusBar.SetText("El socio de negocios no contenia Uso de CFDI, se asigno le asigno por default 'Por Definir'"
                                       , SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                    }
                }
            }
            catch(AggregateException ae) {
                LogService.WriteInfo(String.Format("Error: {0}", ae.Message));
                LogService.WriteError(ae);

                ae.Handle(e => {
                    HandleException(e, "InvoiceDI(Draft)");
                    result.Message = "Error: " + e.Message;
                    result.Success = false;
                    return true;
                });
            }
            catch(Exception ex) {

                HandleException(ex, "InvoiceDI(Draft)");
                result.Message = "Error: " + ex.Message;
                result.Success = false;
            }
            return result;
        }

        #endregion

        #region Handle Exception
        /// <summary>
        /// Handle Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="section"></param>
        public static void HandleException(Exception ex, string section) {
            LogService.WriteInfo(String.Format("{0}: {1}", section, ex.Message));
            LogService.WriteError(ex);
        }
        #endregion
    }
}


