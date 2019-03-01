using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Exceptions;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class ConsumablesDAO
    {
        #region Consumables request
        public ConsumablesDTO GetInventoryRequest(int pIntDocEntry)
        {
            ConsumablesDTO lObjConsumablesDTO = null;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("DocEntry", pIntDocEntry.ToString());

                string lStrQuery = this.GetSQL("GetInventoryRequestById").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lObjConsumablesDTO = new ConsumablesDTO
                    {
                        DocEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString()),
                        IdRise = int.Parse(lObjRecordset.Fields.Item("FolioRise").Value.ToString()),
                        TransferFolio = int.Parse(lObjRecordset.Fields.Item("StockTransferDocEntry").Value.ToString()),
                        DocDate = DateTime.Parse(lObjRecordset.Fields.Item("DocDate").Value.ToString()),
                        DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                    };
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesDAO - GetInventoryRequest: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lObjConsumablesDTO;
        }

        public List<ConsumablesDTO> GetConsumableRequestDocByRiseId(int pIntRiseId)
        {
            List<ConsumablesDTO> lLstConsumablesDTO = new List<ConsumablesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetConsubamleRequestDocByRiseId").InjectSingleValue("RiseId", pIntRiseId.ToString());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConsumablesDTO lObjConsumablesDTO = new ConsumablesDTO
                        {
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString() == "0" ? string.Empty : lObjRecordset.Fields.Item("Code").Value.ToString(),
                            DocEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString()),
                            IdRise = int.Parse(lObjRecordset.Fields.Item("FolioRise").Value.ToString()),
                            TransferFolio = int.Parse(lObjRecordset.Fields.Item("StockTransferDocEntry").Value.ToString()),
                            DocDate = DateTime.Parse(lObjRecordset.Fields.Item("DocDate").Value.ToString()),
                            DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                        };

                        lLstConsumablesDTO.Add(lObjConsumablesDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesDAO - GetConsumableRequestDocByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstConsumablesDTO;
        }

        public List<ConsumablesDTO> GetConsumableRequestUDTByRiseId(int pIntRiseId)
        {
            List<ConsumablesDTO> lLstConsumablesDTO = new List<ConsumablesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetConsubamleRequestUDTByRiseId").InjectSingleValue("RiseId", pIntRiseId.ToString());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConsumablesDTO lObjConsumablesDTO = new ConsumablesDTO
                        {
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString(),
                            DocEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString()),
                            IdRise = int.Parse(lObjRecordset.Fields.Item("FolioRise").Value.ToString()),
                            TransferFolio = int.Parse(lObjRecordset.Fields.Item("StockTransferDocEntry").Value.ToString()),
                            DocDate = DateTime.Parse(lObjRecordset.Fields.Item("DocDate").Value.ToString()),
                            DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                        };

                        lLstConsumablesDTO.Add(lObjConsumablesDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesDAO - GetConsumableRequestUDTByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstConsumablesDTO;
        }
        #endregion

        public List<ConsumablesDocumentsDTO> GetInitialRecordsByRiseId(int pIntRiseId)
        {
            List<ConsumablesDocumentsDTO> lLstInitialRecordsDTO = new List<ConsumablesDocumentsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetInitialRecordsByRiseId").InjectSingleValue("RiseId", pIntRiseId); //GetInitialRecordsByInvRequest

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConsumablesDocumentsDTO lObjInitialRecordsDTO = new ConsumablesDocumentsDTO
                        {
                            IdRise = int.Parse(lObjRecordset.Fields.Item("IdRise").Value.ToString()),
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString() == "0" ? string.Empty : lObjRecordset.Fields.Item("Code").Value.ToString(),
                            ActivoCode = lObjRecordset.Fields.Item("Equipo").Value.ToString(),
                            EcoNum = lObjRecordset.Fields.Item("NumEconomico").Value.ToString(),
                            DocType = 0, //int.Parse(lObjRecordset.Fields.Item("ObjType").Value.ToString()),
                            DieselM = double.Parse(lObjRecordset.Fields.Item("DieselM").Value.ToString()),
                            DieselT = double.Parse(lObjRecordset.Fields.Item("DieselT").Value.ToString()),
                            Gas = double.Parse(lObjRecordset.Fields.Item("Gasolina").Value.ToString()),
                            F15W40 = double.Parse(lObjRecordset.Fields.Item("15W40").Value.ToString()),
                            Hidraulic = double.Parse(lObjRecordset.Fields.Item("Hidraulico").Value.ToString()),
                            SAE40 = double.Parse(lObjRecordset.Fields.Item("SAE_40").Value.ToString()),
                            Transmition = double.Parse(lObjRecordset.Fields.Item("Transm").Value.ToString()),
                            Oils = double.Parse(lObjRecordset.Fields.Item("Grasa").Value.ToString()),
                            EquipmentType = lObjRecordset.Fields.Item("EqType").Value.ToString(),
                            KmHr = double.Parse(lObjRecordset.Fields.Item("Km/Hr").Value.ToString()),
                        };

                        /*lDblQuantity = string.IsNullOrEmpty(lObjRecordset.Fields.Item("Quantity").Value.ToString())
                                       ? 0 : double.Parse(lObjRecordset.Fields.Item("Quantity").Value.ToString());

                        AssignQuantityValue(ref lObjInitialRecordsDTO, lDblQuantity);*/

                        lLstInitialRecordsDTO.Add(lObjInitialRecordsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesDAO - GetInitialRecordsByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstInitialRecordsDTO;
        }

        public List<ConsumablesDocumentsDTO> GetPurchasesByRiseId(int pIntRiseId)
        {
            List<ConsumablesDocumentsDTO> lLstPurchasesDTO = new List<ConsumablesDocumentsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetPurchasesByRiseId").InjectSingleValue("RiseId", pIntRiseId);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConsumablesDocumentsDTO lObjPurchaseDTO = new ConsumablesDocumentsDTO
                        {
                            IdRise = 0, //int.Parse(lObjRecordset.Fields.Item("IdSubida").Value.ToString()),
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString() == "0" ? string.Empty : lObjRecordset.Fields.Item("Code").Value.ToString(), //string.Empty,
                            ActivoCode = lObjRecordset.Fields.Item("Equipo").Value.ToString(),
                            EcoNum = lObjRecordset.Fields.Item("NumEconomico").Value.ToString(),
                            DocType = lObjRecordset.Fields.Item("Tipo").Value.ToString() == "Traslado" ? 1250000001 : 18,
                            Gas = double.Parse(lObjRecordset.Fields.Item("Gasolina").Value.ToString()),
                            F15W40 = double.Parse(lObjRecordset.Fields.Item("15W40").Value.ToString()),
                            Hidraulic = double.Parse(lObjRecordset.Fields.Item("Hidra").Value.ToString()),
                            SAE40 = double.Parse(lObjRecordset.Fields.Item("SAE").Value.ToString()),
                            Transmition = double.Parse(lObjRecordset.Fields.Item("TRANS").Value.ToString()),
                            Oils = double.Parse(lObjRecordset.Fields.Item("Grasa").Value.ToString()),
                            EquipmentType = lObjRecordset.Fields.Item("EqType").Value.ToString(),
                            DieselT = double.Parse(lObjRecordset.Fields.Item("DieselT").Value.ToString()),
                        };

                        /*lDblQuantity = string.IsNullOrEmpty(lObjRecordset.Fields.Item("Quantity").Value.ToString())
                                       ? 0 : double.Parse(lObjRecordset.Fields.Item("Quantity").Value.ToString());

                        AssignQuantityValue(ref lObjPurchaseDTO, lDblQuantity);*/

                        lLstPurchasesDTO.Add(lObjPurchaseDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesDAO - GetPurchasesByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstPurchasesDTO;
        }

        public List<ConsumablesDocumentsDTO> GetInitialRecordsUDTByRiseId(int pIntRiseId)
        {
            List<ConsumablesDocumentsDTO> lLstInitialRecordsDTO = new List<ConsumablesDocumentsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetInitialRecordsUDTByRiseId").InjectSingleValue("RiseId", pIntRiseId); //GetInitialRecordsByInvRequest

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConsumablesDocumentsDTO lObjInitialRecordsDTO = new ConsumablesDocumentsDTO
                        {
                            IdRise = int.Parse(lObjRecordset.Fields.Item("U_IdRise").Value.ToString()),
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString() == "0" ? string.Empty : lObjRecordset.Fields.Item("Code").Value.ToString(),
                            ActivoCode = lObjRecordset.Fields.Item("U_PrcCode").Value.ToString(),
                            EcoNum = lObjRecordset.Fields.Item("U_EcoNum").Value.ToString(),
                            DocType = 0, //int.Parse(lObjRecordset.Fields.Item("ObjType").Value.ToString()),
                            DieselM = double.Parse(lObjRecordset.Fields.Item("U_DieselM").Value.ToString()),
                            DieselT = double.Parse(lObjRecordset.Fields.Item("U_DieselT").Value.ToString()),
                            Gas = double.Parse(lObjRecordset.Fields.Item("U_Gas").Value.ToString()),
                            F15W40 = double.Parse(lObjRecordset.Fields.Item("U_F15W40").Value.ToString()),
                            Hidraulic = double.Parse(lObjRecordset.Fields.Item("U_Hidraulic").Value.ToString()),
                            SAE40 = double.Parse(lObjRecordset.Fields.Item("U_SAE40").Value.ToString()),
                            Transmition = double.Parse(lObjRecordset.Fields.Item("U_Transmition").Value.ToString()),
                            Oils = double.Parse(lObjRecordset.Fields.Item("U_Oils").Value.ToString()),
                            KmHr = double.Parse(lObjRecordset.Fields.Item("U_KmHr").Value.ToString()),
                            EquipmentType = lObjRecordset.Fields.Item("EqType").Value.ToString(),
                        };

                        lLstInitialRecordsDTO.Add(lObjInitialRecordsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesDAO - GetInitialRecordsUDTByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstInitialRecordsDTO;
        }

        public List<ConsumablesDocumentsDTO> GetPurchasesRecordsUDTByRiseId(int pIntRiseId)
        {
            List<ConsumablesDocumentsDTO> lLstInitialRecordsDTO = new List<ConsumablesDocumentsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetPurchasesUDTByRiseId").InjectSingleValue("RiseId", pIntRiseId); //GetInitialRecordsByInvRequest

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConsumablesDocumentsDTO lObjInitialRecordsDTO = new ConsumablesDocumentsDTO
                        {
                            IdRise = int.Parse(lObjRecordset.Fields.Item("U_IdRise").Value.ToString()),
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString() == "0" ? string.Empty : lObjRecordset.Fields.Item("Code").Value.ToString(),
                            ActivoCode = lObjRecordset.Fields.Item("U_PrcCode").Value.ToString(),
                            EcoNum = lObjRecordset.Fields.Item("U_EcoNum").Value.ToString(),
                            DocType = int.Parse(lObjRecordset.Fields.Item("U_Type").Value.ToString()),
                            DieselM = double.Parse(lObjRecordset.Fields.Item("U_DieselM").Value.ToString()),
                            DieselT = double.Parse(lObjRecordset.Fields.Item("U_DieselT").Value.ToString()),
                            Gas = double.Parse(lObjRecordset.Fields.Item("U_Gas").Value.ToString()),
                            F15W40 = double.Parse(lObjRecordset.Fields.Item("U_F15W40").Value.ToString()),
                            Hidraulic = double.Parse(lObjRecordset.Fields.Item("U_Hidraulic").Value.ToString()),
                            SAE40 = double.Parse(lObjRecordset.Fields.Item("U_SAE40").Value.ToString()),
                            Transmition = double.Parse(lObjRecordset.Fields.Item("U_Transmition").Value.ToString()),
                            Oils = double.Parse(lObjRecordset.Fields.Item("U_Oils").Value.ToString()),
                            //KmHr = double.Parse(lObjRecordset.Fields.Item("U_KmHr").Value.ToString()),
                            EquipmentType = lObjRecordset.Fields.Item("EqType").Value.ToString(),
                        };

                        lLstInitialRecordsDTO.Add(lObjInitialRecordsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesDAO - GetPurchasesRecordsUDTByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstInitialRecordsDTO;
        }

        public List<ConsumablesDocumentsDTO> GetFinalsRecordsUDTByRiseId(int pIntRiseId)
        {
            List<ConsumablesDocumentsDTO> lLstInitialRecordsDTO = new List<ConsumablesDocumentsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetFinalsRecordsUDTByRiseId").InjectSingleValue("RiseId", pIntRiseId); //GetInitialRecordsByInvRequest

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConsumablesDocumentsDTO lObjInitialRecordsDTO = new ConsumablesDocumentsDTO
                        {
                            IdRise = int.Parse(lObjRecordset.Fields.Item("U_IdRise").Value.ToString()),
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString() == "0" ? string.Empty : lObjRecordset.Fields.Item("Code").Value.ToString(),
                            ActivoCode = lObjRecordset.Fields.Item("U_PrcCode").Value.ToString(),
                            EcoNum = lObjRecordset.Fields.Item("U_EcoNum").Value.ToString(),
                            DocType = 0, //int.Parse(lObjRecordset.Fields.Item("U_Type").Value.ToString()),
                            DieselM = double.Parse(lObjRecordset.Fields.Item("U_DieselM").Value.ToString()),
                            DieselT = double.Parse(lObjRecordset.Fields.Item("U_DieselT").Value.ToString()),
                            Gas = double.Parse(lObjRecordset.Fields.Item("U_Gas").Value.ToString()),
                            F15W40 = double.Parse(lObjRecordset.Fields.Item("U_F15W40").Value.ToString()),
                            Hidraulic = double.Parse(lObjRecordset.Fields.Item("U_Hidraulic").Value.ToString()),
                            SAE40 = double.Parse(lObjRecordset.Fields.Item("U_SAE40").Value.ToString()),
                            Transmition = double.Parse(lObjRecordset.Fields.Item("U_Transmition").Value.ToString()),
                            Oils = double.Parse(lObjRecordset.Fields.Item("U_Oils").Value.ToString()),
                            KmHr = double.Parse(lObjRecordset.Fields.Item("U_KmHr").Value.ToString()),
                            EquipmentType = lObjRecordset.Fields.Item("EqType").Value.ToString(),
                        };

                        lLstInitialRecordsDTO.Add(lObjInitialRecordsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesDAO - GetFinalsRecordsUDTByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstInitialRecordsDTO;
        }

        public List<ConsumablesDocumentsDTO> GetFinalsRecordsByRiseId(int pIntRiseId)
        {
            List<ConsumablesDocumentsDTO> lLstInitialRecordsDTO = new List<ConsumablesDocumentsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetFinalsRecordsByRiseId").InjectSingleValue("RiseId", pIntRiseId); //GetInitialRecordsByInvRequest

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConsumablesDocumentsDTO lObjInitialRecordsDTO = new ConsumablesDocumentsDTO
                        {
                            IdRise = int.Parse(lObjRecordset.Fields.Item("U_IdRise").Value.ToString()),
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString() == "0" ? string.Empty : lObjRecordset.Fields.Item("Code").Value.ToString(),
                            ActivoCode = lObjRecordset.Fields.Item("U_PrcCode").Value.ToString(),
                            EcoNum = lObjRecordset.Fields.Item("U_EcoNum").Value.ToString(),
                            DocType = 0, //int.Parse(lObjRecordset.Fields.Item("U_Type").Value.ToString()),
                            DieselM = double.Parse(lObjRecordset.Fields.Item("U_DieselM").Value.ToString()),
                            DieselT = double.Parse(lObjRecordset.Fields.Item("U_DieselT").Value.ToString()),
                            Gas = double.Parse(lObjRecordset.Fields.Item("U_Gas").Value.ToString()),
                            F15W40 = double.Parse(lObjRecordset.Fields.Item("U_F15W40").Value.ToString()),
                            Hidraulic = double.Parse(lObjRecordset.Fields.Item("U_Hidraulic").Value.ToString()),
                            SAE40 = double.Parse(lObjRecordset.Fields.Item("U_SAE40").Value.ToString()),
                            Transmition = double.Parse(lObjRecordset.Fields.Item("U_Transmition").Value.ToString()),
                            Oils = double.Parse(lObjRecordset.Fields.Item("U_Oils").Value.ToString()),
                            KmHr = double.Parse(lObjRecordset.Fields.Item("U_KmHr").Value.ToString()),
                            EquipmentType = lObjRecordset.Fields.Item("EqType").Value.ToString(),
                        };

                        lLstInitialRecordsDTO.Add(lObjInitialRecordsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesDAO - GetFinalsRecordsByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstInitialRecordsDTO;
        }

        public List<ConsumablesDocumentsDTO> GetTotalsRecordsUDTByRiseId(int pIntRiseId)
        {
            List<ConsumablesDocumentsDTO> lLstInitialRecordsDTO = new List<ConsumablesDocumentsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetTotalsRecordsUDTByRiseId").InjectSingleValue("RiseId", pIntRiseId); //GetInitialRecordsByInvRequest

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConsumablesDocumentsDTO lObjInitialRecordsDTO = new ConsumablesDocumentsDTO
                        {
                            IdRise = int.Parse(lObjRecordset.Fields.Item("U_IdRise").Value.ToString()),
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString() == "0" ? string.Empty : lObjRecordset.Fields.Item("Code").Value.ToString(),
                            ActivoCode = lObjRecordset.Fields.Item("U_PrcCode").Value.ToString(),
                            EcoNum = lObjRecordset.Fields.Item("U_EcoNum").Value.ToString(),
                            DocType = 0, //int.Parse(lObjRecordset.Fields.Item("U_Type").Value.ToString()),
                            DieselM = double.Parse(lObjRecordset.Fields.Item("U_DieselM").Value.ToString()),
                            DieselT = double.Parse(lObjRecordset.Fields.Item("U_DieselT").Value.ToString()),
                            Gas = double.Parse(lObjRecordset.Fields.Item("U_Gas").Value.ToString()),
                            F15W40 = double.Parse(lObjRecordset.Fields.Item("U_F15W40").Value.ToString()),
                            Hidraulic = double.Parse(lObjRecordset.Fields.Item("U_Hidraulic").Value.ToString()),
                            SAE40 = double.Parse(lObjRecordset.Fields.Item("U_SAE40").Value.ToString()),
                            Transmition = double.Parse(lObjRecordset.Fields.Item("U_Transmition").Value.ToString()),
                            Oils = double.Parse(lObjRecordset.Fields.Item("U_Oils").Value.ToString()),
                            KmHr = double.Parse(lObjRecordset.Fields.Item("U_KmHr").Value.ToString()),
                            EquipmentType = lObjRecordset.Fields.Item("EqType").Value.ToString(),
                        };

                        lLstInitialRecordsDTO.Add(lObjInitialRecordsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesDAO - GetTotalsRecordsUDTByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstInitialRecordsDTO;
        }

        /*public List<ConsumablesDocumentsDTO> GetOthersInventoryRequestsByRiseId(int pIntRiseId, int pIntInvReqDocEntry)
        {
            List<ConsumablesDocumentsDTO> lLstInvRequestDTO = new List<ConsumablesDocumentsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RiseId", pIntRiseId.ToString());
                lLstStrParameters.Add("InvReqDocEntry", pIntInvReqDocEntry.ToString());

                string lStrQuery = this.GetSQL("GetInventoryRequestByRiseId").Inject(lLstStrParameters); //InvReqDocEntry

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConsumablesDocumentsDTO lObjIntRequestDTO = new ConsumablesDocumentsDTO
                        {
                            IdRise = int.Parse(lObjRecordset.Fields.Item("IdSubida").Value.ToString()),
                            ActivoCode = lObjRecordset.Fields.Item("ActivoCode").Value.ToString(),
                            EcoNum = lObjRecordset.Fields.Item("ActivoName").Value.ToString(),
                            DocType = int.Parse(lObjRecordset.Fields.Item("ObjType").Value.ToString()),
                            Gas = double.Parse(lObjRecordset.Fields.Item("Gasolina").Value.ToString()),
                            F15W40 = double.Parse(lObjRecordset.Fields.Item("15w40").Value.ToString()),
                            Hidraulic = double.Parse(lObjRecordset.Fields.Item("Hidraulico").Value.ToString()),
                            SAE40 = double.Parse(lObjRecordset.Fields.Item("SAE40").Value.ToString()),
                            Transmition = double.Parse(lObjRecordset.Fields.Item("Trasmision").Value.ToString()),
                            Oils = double.Parse(lObjRecordset.Fields.Item("Grasas").Value.ToString()),
                        };

                        lLstInvRequestDTO.Add(lObjIntRequestDTO);
                        lObjRecordset.MoveNext();
                    }
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
            return lLstInvRequestDTO;
        }*/

        /*public List<ConsumablesDocumentsDTO> GetFinalsRecordsByRiseId(int pIntRiseId)
        {
            List<ConsumablesDocumentsDTO> lLstInitialRecordsDTO = new List<ConsumablesDocumentsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetInitialRecordsByInvRequest").InjectSingleValue("InvReqDocEntry", pIntRiseId);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConsumablesDocumentsDTO lObjInitialRecordsDTO = new ConsumablesDocumentsDTO
                        {
                            IdRise = int.Parse(lObjRecordset.Fields.Item("IdSubida").Value.ToString()),
                            ActivoCode = lObjRecordset.Fields.Item("ActivoCode").Value.ToString(),
                            EcoNum = lObjRecordset.Fields.Item("ActivoName").Value.ToString(),
                            DocType = int.Parse(lObjRecordset.Fields.Item("ObjType").Value.ToString()),
                            Gas = double.Parse(lObjRecordset.Fields.Item("Gasolina").Value.ToString()),
                            F15W40 = double.Parse(lObjRecordset.Fields.Item("15w40").Value.ToString()),
                            Hidraulic = double.Parse(lObjRecordset.Fields.Item("Hidraulico").Value.ToString()),
                            SAE40 = double.Parse(lObjRecordset.Fields.Item("SAE40").Value.ToString()),
                            Transmition = double.Parse(lObjRecordset.Fields.Item("Trasmision").Value.ToString()),
                            Oils = double.Parse(lObjRecordset.Fields.Item("Grasas").Value.ToString()),
                        };

                        lLstInitialRecordsDTO.Add(lObjInitialRecordsDTO);
                        lObjRecordset.MoveNext();
                    }
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
            return lLstInitialRecordsDTO;
        }*/

        /*private void AssignQuantityValue(ref ConsumablesDocumentsDTO pObjInitialRecord, double pDblQuantity)
        {
            switch (pObjInitialRecord.InventoryPropEnum)
            {
                case InventoryPropsEnum.F15W40:
                    pObjInitialRecord.F15W40 = pDblQuantity;
                    break;
                case InventoryPropsEnum.HIDRAULIC:
                    pObjInitialRecord.Hidraulic = pDblQuantity;
                    break;
                case InventoryPropsEnum.SAE40:
                    pObjInitialRecord.SAE40 = pDblQuantity;
                    break;
                case InventoryPropsEnum.TRANSMISSION:
                    pObjInitialRecord.Transmition = pDblQuantity;
                    break;
                case InventoryPropsEnum.OILS:
                    pObjInitialRecord.Oils = pDblQuantity;
                    break;
                case InventoryPropsEnum.KMHRS:
                    pObjInitialRecord.KmHr = pDblQuantity;
                    break;
                case InventoryPropsEnum.OTHERS:
                    pObjInitialRecord.OtherValue = pDblQuantity;
                    break;
                default:
                    break;
            }
        }*/
    }
}
