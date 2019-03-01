using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery.Tables;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class ConsumablesService
    {
        private TableDAO<Consumables> mObjConsumablesTableDAO;
        private ConsumablesDAO mObjConsumablesDAO;

        public ConsumablesService()
        {
            mObjConsumablesDAO = new ConsumablesDAO();
            mObjConsumablesTableDAO = new TableDAO<Consumables>();
        }

        #region Entities
        public int Add(Consumables pObjConsumables)
        {
            try
            {
                return mObjConsumablesTableDAO.Add(pObjConsumables);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesService - Add: {0}]", lObjException.Message));
                throw lObjException;
            }

        }

        public int Update(Consumables pObjConsumables)
        {
            try
            {
                return mObjConsumablesTableDAO.Update(pObjConsumables);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesService - Update: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjConsumablesTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesService - GetLastCode: {0}]", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO
        /// <summary>
        /// Obtiene la información de una solicitud de consumible por su DocEntry.
        /// </summary>
        /// <param name="pIntDocEntry"></param>
        /// <returns></returns>
        public ConsumablesDTO GetInventoryRequestById(int pIntDocEntry)
        {
            return mObjConsumablesDAO.GetInventoryRequest(pIntDocEntry);
        }

        /// <summary>
        /// Obtiene las solicitudes de consumibles de los documentos de SAP de una subida.
        /// </summary>
        /// <param name="pIntRiseId"></param>
        /// <returns></returns>
        public List<ConsumablesDTO> GetConsumableRequestDocByRiseId(int pIntRiseId)
        {
            return mObjConsumablesDAO.GetConsumableRequestDocByRiseId(pIntRiseId);
        }

        /// <summary>
        /// Obtiene las solicitudes de consumibles de la UDT de una subida.
        /// </summary>
        /// <param name="pIntRiseId"></param>
        /// <returns></returns>
        public List<ConsumablesDTO> GetConsumableRequestUDTByRiseId(int pIntRiseId)
        {
            return mObjConsumablesDAO.GetConsumableRequestUDTByRiseId(pIntRiseId);
        }

        /// <summary>
        /// Obtiene los registros iniciales de una subida
        /// tomando en cuenta la primera transferencia de stock de una subida.
        /// </summary>
        /// <param name="pIntInvRequest"></param>
        /// <returns></returns>
        public List<ConsumablesDocumentsDTO> GetInitialRecordsByRiseId(int pIntInvRequest)
        {
            return mObjConsumablesDAO.GetInitialRecordsByRiseId(pIntInvRequest);
        }

        /// <summary>
        /// Obtiene las compras/traspasos de una subida
        /// sin tomar en cuenta los registros iniciales.
        /// </summary>
        /// <param name="pIntRiseId"></param>
        /// <returns></returns>
        public List<ConsumablesDocumentsDTO> GetPurchasesByRiseId(int pIntRiseId)
        {
            return mObjConsumablesDAO.GetPurchasesByRiseId(pIntRiseId);
        }

        public List<ConsumablesDocumentsDTO> GetInitialRecordsUDTByRiseId(int pIntRiseId)
        {
            return mObjConsumablesDAO.GetInitialRecordsUDTByRiseId(pIntRiseId);
        }

        public List<ConsumablesDocumentsDTO> GetPurchasesRecordsUDTByRiseId(int pIntRiseId)
        {
            return mObjConsumablesDAO.GetPurchasesRecordsUDTByRiseId(pIntRiseId);
        }

        public List<ConsumablesDocumentsDTO> GetFinalsRecordsUDTByRiseId(int pIntRiseId)
        {
            return mObjConsumablesDAO.GetFinalsRecordsUDTByRiseId(pIntRiseId);
        }

        public List<ConsumablesDocumentsDTO> GetFinalsRecordsByRiseId(int pIntRiseId)
        {
            return mObjConsumablesDAO.GetFinalsRecordsByRiseId(pIntRiseId);
        }

        public List<ConsumablesDocumentsDTO> GetTotalsRecordsUDTByRiseId(int pIntRiseId)
        {
            return mObjConsumablesDAO.GetTotalsRecordsUDTByRiseId(pIntRiseId);
        }

        /*public List<ConsumablesDocumentsDTO> GetOthersInventoryRequestsByRiseId(int pIntRiseId, int pIntInvReqDocEntry)
        {
            return mObjConsumablesDAO.GetOthersInventoryRequestsByRiseId(pIntRiseId, pIntInvReqDocEntry);
        }*/
        #endregion

        #region Extras
        public List<ConsumablesDocumentsDTO> CalculateConsumedTotals(SAPbouiCOM.DataTable pObjInitialRecordsDataTable
                                                                    , SAPbouiCOM.DataTable pObjPurchaseDataTable
                                                                    , SAPbouiCOM.DataTable pObjFinalsDataTable
                                                                    , SAPbouiCOM.DataTable pObjTotalsDataTable)
        {
            try
            {
                List<ConsumablesDocumentsDTO> lLstTotalsRecords = new List<ConsumablesDocumentsDTO>();

                List<ConsumablesDocumentsDTO> lLstInitialRecords = InitialRecordsDataTableToDTO(pObjInitialRecordsDataTable);
                List<ConsumablesDocumentsDTO> lLstPurchasesRecordsTemp = PurchasesDataTableToDTO(pObjPurchaseDataTable);
                List<ConsumablesDocumentsDTO> lLstFinalsRecords = FinalsRecordsDataTableToDTO(pObjFinalsDataTable);
                List<ConsumablesDocumentsDTO> lLstOriginalTotalsRecords = TotalsRecordsDataTableToDTO(pObjTotalsDataTable);

                List<ConsumablesDocumentsDTO> lLstPurchasesRecords = lLstPurchasesRecordsTemp
                                                                     .GroupBy(p => new { ActiveCode = p.ActivoCode, EcoNum = p.EcoNum, EqType = p.EquipmentType })
                                                                     .Select(g => new ConsumablesDocumentsDTO
                                                                        {
                                                                            ActivoCode = g.Key.ActiveCode,
                                                                            EcoNum = g.Key.EcoNum,
                                                                            EquipmentType = g.Key.EqType,
                                                                            DieselM = g.Sum(i => i.DieselM),
                                                                            DieselT = g.Sum(i => i.DieselT),
                                                                            Gas = g.Sum(i => i.Gas),
                                                                            F15W40 = g.Sum(i => i.F15W40),
                                                                            Hidraulic = g.Sum(i => i.Hidraulic),
                                                                            SAE40 = g.Sum(i => i.SAE40),
                                                                            Transmition = g.Sum(i => i.Transmition),
                                                                            Oils = g.Sum(i => i.Oils),
                                                                            KmHr = g.Sum(i => i.KmHr),
                                                                        })
                                                                     .ToList();

                var lLstSumList = lLstInitialRecords.Concat(lLstPurchasesRecords)
                                                    .GroupBy(p => new { ActiveCode = p.ActivoCode, EcoNum = p.EcoNum, EqType = p.EquipmentType })
                                                    .Select(g => new ConsumablesDocumentsDTO
                                                    {
                                                        ActivoCode = g.Key.ActiveCode,
                                                        EcoNum = g.Key.EcoNum,
                                                        EquipmentType = g.Key.EqType,
                                                        DieselM = g.Sum(i => i.DieselM),
                                                        DieselT = g.Sum(i => i.DieselT),
                                                        Gas = g.Sum(i => i.Gas),
                                                        F15W40 = g.Sum(i => i.F15W40),
                                                        Hidraulic = g.Sum(i => i.Hidraulic),
                                                        SAE40 = g.Sum(i => i.SAE40),
                                                        Transmition = g.Sum(i => i.Transmition),
                                                        Oils = g.Sum(i => i.Oils),
                                                        KmHr = g.Sum(i => i.KmHr),
                                                    })
                                                    .ToList();

                lLstTotalsRecords = lLstSumList//.Concat(lLstFinalsRecords)
                                                    .GroupBy(p => new { ActiveCode = p.ActivoCode, EcoNum = p.EcoNum, EqType = p.EquipmentType })
                                                    .Select(g => new ConsumablesDocumentsDTO
                                                    {
                                                        ActivoCode = g.Key.ActiveCode,
                                                        EcoNum = g.Key.EcoNum,
                                                        EquipmentType = g.Key.EqType,
                                                        DieselM = g.Sum(i => i.DieselM) - (lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode) == null ? 0 : lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode).DieselM),
                                                        DieselT = g.Sum(i => i.DieselT) - (lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode) == null ? 0 : lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode).DieselT),
                                                        Gas = g.Sum(i => i.Gas) - (lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode) == null ? 0 : lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode).Gas),
                                                        F15W40 = g.Sum(i => i.F15W40) - (lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode) == null ? 0 : lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode).F15W40),
                                                        Hidraulic = g.Sum(i => i.Hidraulic) - (lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode) == null ? 0 : lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode).Hidraulic),
                                                        SAE40 = g.Sum(i => i.SAE40) - (lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode) == null ? 0 : lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode).SAE40),
                                                        Transmition = g.Sum(i => i.Transmition) - (lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode) == null ? 0 : lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode).Transmition),
                                                        Oils = g.Sum(i => i.Oils) - (lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode) == null ? 0 : lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode).Oils),
                                                        KmHr = (lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode) == null ? 0 : lLstFinalsRecords.FirstOrDefault(x => x.ActivoCode == g.Key.ActiveCode).KmHr) - g.Sum(i => i.KmHr),
                                                    })
                                                    .ToList();

                lLstTotalsRecords.ForEach(x => x.Code = (lLstOriginalTotalsRecords.FirstOrDefault(y => y.ActivoCode == x.ActivoCode) == null ? string.Empty : lLstOriginalTotalsRecords.FirstOrDefault(y => y.ActivoCode == x.ActivoCode).Code));

                return lLstTotalsRecords;
            }
            catch (Exception pObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesService - CalculateConsumedTotals: {0}]", pObjException.Message));
                throw pObjException;
            }
        }

        public bool HasConsumedTotalsNegativeValues(SAPbouiCOM.DataTable pObjDataTable)
        {
            try
            {
                bool lBolResult = false;
                List<ConsumablesDocumentsDTO> lLstConsumablesDocuments = TotalsRecordsDataTableToDTO(pObjDataTable);

                lBolResult = lLstConsumablesDocuments.Exists(x => x.F15W40 < 0 || x.Hidraulic < 0 || x.SAE40 < 0 || x.Transmition < 0 || x.Oils < 0 || x.DieselM < 0 || x.DieselT < 0 || x.Gas < 0);
                    //lLstConsumablesDocuments.TrueForAll(x => x.F15W40 < 0 || x.Hidraulic < 0 || x.SAE40 < 0 || x.Transmition < 0 || x.Oils < 0 || x.DieselM < 0 || x.DieselT < 0 || x.Gas < 0);

                return lBolResult;
            }
            catch (Exception pObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesService - InitialRecordsDataTableToDTO: {0}]", pObjException.Message));
                throw pObjException;
            }
        }

        public List<ConsumablesDocumentsDTO> InitialRecordsDataTableToDTO(SAPbouiCOM.DataTable pObjDataTable)
        {
            try
            {
                List<ConsumablesDocumentsDTO> lLstConsumablesDocuments = new List<ConsumablesDocumentsDTO>();

                for (int i = 0; i < pObjDataTable.Rows.Count; i++)
                {
                    ConsumablesDocumentsDTO lObjConsumableDoc = new ConsumablesDocumentsDTO();

                    lObjConsumableDoc.Code = pObjDataTable.GetValue("CodeTIR", i).ToString();
                    lObjConsumableDoc.IdRise = int.Parse(pObjDataTable.GetValue("IdRiIR", i).ToString());
                    lObjConsumableDoc.ActivoCode = pObjDataTable.GetValue("ActCodIR", i).ToString();
                    lObjConsumableDoc.EcoNum = pObjDataTable.GetValue("ActNumIR", i).ToString();
                    lObjConsumableDoc.DieselM = double.Parse(pObjDataTable.GetValue("DslMIR", i).ToString());
                    lObjConsumableDoc.DieselT = double.Parse(pObjDataTable.GetValue("DslTIR", i).ToString());
                    lObjConsumableDoc.Gas = double.Parse(pObjDataTable.GetValue("GasIR", i).ToString());
                    lObjConsumableDoc.F15W40 = double.Parse(pObjDataTable.GetValue("15W40IR", i).ToString());
                    lObjConsumableDoc.Hidraulic = double.Parse(pObjDataTable.GetValue("HidIR", i).ToString());
                    lObjConsumableDoc.SAE40 = double.Parse(pObjDataTable.GetValue("SAE40IR", i).ToString());
                    lObjConsumableDoc.Transmition = double.Parse(pObjDataTable.GetValue("TransIR", i).ToString());
                    lObjConsumableDoc.Oils = double.Parse(pObjDataTable.GetValue("OilsIR", i).ToString());
                    lObjConsumableDoc.KmHr = double.Parse(pObjDataTable.GetValue("KmHrIR", i).ToString());
                    lObjConsumableDoc.EquipmentType = pObjDataTable.GetValue("EqTypIR", i).ToString();

                    lLstConsumablesDocuments.Add(lObjConsumableDoc);
                }

                return lLstConsumablesDocuments;
            }
            catch (Exception pObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesService - InitialRecordsDataTableToDTO: {0}]", pObjException.Message));
                throw pObjException;
            }
        }

        public List<ConsumablesDocumentsDTO> PurchasesDataTableToDTO(SAPbouiCOM.DataTable pObjDataTable)
        {
            try
            {
                List<ConsumablesDocumentsDTO> lLstConsumablesDocuments = new List<ConsumablesDocumentsDTO>();

                for (int i = 0; i < pObjDataTable.Rows.Count; i++)
                {
                    ConsumablesDocumentsDTO lObjConsumableDoc = new ConsumablesDocumentsDTO();

                    lObjConsumableDoc.Code = pObjDataTable.GetValue("CodeTPrch", i).ToString();
                    lObjConsumableDoc.IdRise = int.Parse(pObjDataTable.GetValue("IdRiPrch", i).ToString());
                    lObjConsumableDoc.ActivoCode = pObjDataTable.GetValue("ActCodPrc", i).ToString();
                    lObjConsumableDoc.EcoNum = pObjDataTable.GetValue("ActNumPrc", i).ToString();
                    lObjConsumableDoc.DieselM = 0;
                    lObjConsumableDoc.DieselT = double.Parse(pObjDataTable.GetValue("DslTPrch", i).ToString());
                    lObjConsumableDoc.Gas = double.Parse(pObjDataTable.GetValue("GasPrch", i).ToString());
                    lObjConsumableDoc.F15W40 = double.Parse(pObjDataTable.GetValue("15W40Prch", i).ToString());
                    lObjConsumableDoc.Hidraulic = double.Parse(pObjDataTable.GetValue("HidPrch", i).ToString());
                    lObjConsumableDoc.SAE40 = double.Parse(pObjDataTable.GetValue("SAE40Prch", i).ToString());
                    lObjConsumableDoc.Transmition = double.Parse(pObjDataTable.GetValue("TransPrch", i).ToString());
                    lObjConsumableDoc.Oils = double.Parse(pObjDataTable.GetValue("OilsPrch", i).ToString());
                    lObjConsumableDoc.KmHr = 0;
                    lObjConsumableDoc.DocType = int.Parse(pObjDataTable.GetValue("DocTyPrch", i).ToString());
                    lObjConsumableDoc.EquipmentType = pObjDataTable.GetValue("EqTypPrch", i).ToString();

                    lLstConsumablesDocuments.Add(lObjConsumableDoc);
                }

                return lLstConsumablesDocuments;
            }
            catch (Exception pObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesService - PurchasesDataTableToDTO: {0}]", pObjException.Message));
                throw pObjException;
            }
        }

        public List<ConsumablesDocumentsDTO> FinalsRecordsDataTableToDTO(SAPbouiCOM.DataTable pObjDataTable)
        {
            try
            {
                List<ConsumablesDocumentsDTO> lLstConsumablesDocuments = new List<ConsumablesDocumentsDTO>();

                for (int i = 0; i < pObjDataTable.Rows.Count; i++)
                {
                    ConsumablesDocumentsDTO lObjConsumableDoc = new ConsumablesDocumentsDTO();

                    lObjConsumableDoc.Code = pObjDataTable.GetValue("CodeTFR", i).ToString();
                    lObjConsumableDoc.IdRise = int.Parse(pObjDataTable.GetValue("IdRiFR", i).ToString());
                    lObjConsumableDoc.ActivoCode = pObjDataTable.GetValue("ActCodFR", i).ToString();
                    lObjConsumableDoc.EcoNum = pObjDataTable.GetValue("ActNumFR", i).ToString();
                    lObjConsumableDoc.DieselM = double.Parse(pObjDataTable.GetValue("DslMFR", i).ToString());
                    lObjConsumableDoc.DieselT = double.Parse(pObjDataTable.GetValue("DslTFR", i).ToString());
                    lObjConsumableDoc.Gas = double.Parse(pObjDataTable.GetValue("GasFR", i).ToString());
                    lObjConsumableDoc.F15W40 = double.Parse(pObjDataTable.GetValue("15W40FR", i).ToString());
                    lObjConsumableDoc.Hidraulic = double.Parse(pObjDataTable.GetValue("HidFR", i).ToString());
                    lObjConsumableDoc.SAE40 = double.Parse(pObjDataTable.GetValue("SAE40FR", i).ToString());
                    lObjConsumableDoc.Transmition = double.Parse(pObjDataTable.GetValue("TransFR", i).ToString());
                    lObjConsumableDoc.Oils = double.Parse(pObjDataTable.GetValue("OilsFR", i).ToString());
                    lObjConsumableDoc.KmHr = double.Parse(pObjDataTable.GetValue("KmHrFR", i).ToString());
                    lObjConsumableDoc.EquipmentType = pObjDataTable.GetValue("EqTypFR", i).ToString();

                    lLstConsumablesDocuments.Add(lObjConsumableDoc);
                }

                return lLstConsumablesDocuments;
            }
            catch (Exception pObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesService - FinalsRecordsDataTableToDTO: {0}]", pObjException.Message));
                throw pObjException;
            }
        }

        public List<ConsumablesDocumentsDTO> TotalsRecordsDataTableToDTO(SAPbouiCOM.DataTable pObjDataTable)
        {
            try
            {
                List<ConsumablesDocumentsDTO> lLstConsumablesDocuments = new List<ConsumablesDocumentsDTO>();

                for (int i = 0; i < pObjDataTable.Rows.Count; i++)
                {
                    ConsumablesDocumentsDTO lObjConsumableDoc = new ConsumablesDocumentsDTO();

                    lObjConsumableDoc.Code = pObjDataTable.GetValue("CodeTTR", i).ToString();
                    lObjConsumableDoc.IdRise = int.Parse(pObjDataTable.GetValue("IdRiTR", i).ToString());
                    lObjConsumableDoc.ActivoCode = pObjDataTable.GetValue("ActCodTR", i).ToString();
                    lObjConsumableDoc.EcoNum = pObjDataTable.GetValue("ActNumTR", i).ToString();
                    lObjConsumableDoc.DieselM = double.Parse(pObjDataTable.GetValue("DslMTR", i).ToString());
                    lObjConsumableDoc.DieselT = double.Parse(pObjDataTable.GetValue("DslTTR", i).ToString());
                    lObjConsumableDoc.Gas = double.Parse(pObjDataTable.GetValue("GasTR", i).ToString());
                    lObjConsumableDoc.F15W40 = double.Parse(pObjDataTable.GetValue("15W40TR", i).ToString());
                    lObjConsumableDoc.Hidraulic = double.Parse(pObjDataTable.GetValue("HidTR", i).ToString());
                    lObjConsumableDoc.SAE40 = double.Parse(pObjDataTable.GetValue("SAE40TR", i).ToString());
                    lObjConsumableDoc.Transmition = double.Parse(pObjDataTable.GetValue("TransTR", i).ToString());
                    lObjConsumableDoc.Oils = double.Parse(pObjDataTable.GetValue("OilsTR", i).ToString());
                    lObjConsumableDoc.KmHr = double.Parse(pObjDataTable.GetValue("KmHrTR", i).ToString());
                    lObjConsumableDoc.EquipmentType = pObjDataTable.GetValue("EqTypTR", i).ToString();

                    lLstConsumablesDocuments.Add(lObjConsumableDoc);
                }

                return lLstConsumablesDocuments;
            }
            catch (Exception pObjException)
            {
                LogService.WriteError(string.Format("[ConsumablesService - TotalsRecordsDataTableToDTO: {0}]", pObjException.Message));
                throw pObjException;
            }
        }
        #endregion
    }
}
