using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Exceptions;
using SAPbobsCOM;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class RiseDAO
    {
        public int GetNexFolioId()
        {
            int lIntFolioId = 0;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetNextRiseFolio");

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lIntFolioId = int.Parse(lObjRecordset.Fields.Item("NextFolioId").Value.ToString());
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseDAO - GetNexFolioId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lIntFolioId;
        }

        public List<RiseDTO> GetRiseRelations()
        {
            List<RiseDTO> lLstRiseDTO = new List<RiseDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetRelationsFolios");

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        RiseDTO lObjRiseDTO = new RiseDTO
                        {
                            Code = int.Parse(lObjRecordset.Fields.Item("Code").Value.ToString()),
                            Client = lObjRecordset.Fields.Item("U_Client").Value.ToString(),
                            CreatedDate = DateTime.Parse(lObjRecordset.Fields.Item("U_CreatedDate").Value.ToString()),
                            FolioRelation = int.Parse(lObjRecordset.Fields.Item("U_DocRef").Value.ToString()),
                            DocStatus = (RiseStatusEnum)int.Parse(lObjRecordset.Fields.Item("U_DocStatus").Value.ToString()),
                            IdRise = int.Parse(lObjRecordset.Fields.Item("U_IdRise").Value.ToString()),
                            SupervisorId = int.Parse(lObjRecordset.Fields.Item("U_Supervisor").Value.ToString()),
                            UserId = int.Parse(lObjRecordset.Fields.Item("U_UserId").Value.ToString()),
                            ClientName = lObjRecordset.Fields.Item("ClientName").Value.ToString(),
                        };

                        lLstRiseDTO.Add(lObjRiseDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseDAO - GetRiseRelations: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstRiseDTO;
        }

        public string GetRiseFoliosQuery()
        {
            string lStrQuery = string.Empty;
            try
            {
                lStrQuery = this.GetSQL("GetRelationsFolios");
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseDAO - GetRiseFoliosQuery: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            return lStrQuery;
        }

        public string GetCommissionsRiseQuery()
        {
            string lStrQuery = string.Empty;
            try
            {
                lStrQuery = this.GetSQL("GetNoCommissionsRises");
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseDAO - GetCommissionsRiseQuery: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            return lStrQuery;
        }

        public string GetNoStockTransferRisesQuery()
        {
            string lStrQuery = string.Empty;
            try
            {
                lStrQuery = this.GetSQL("GetNoStockTransferRises");
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseDAO - GetNoStockTransferRisesQuery: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            return lStrQuery;
        }

        public int GetVoucherStatus(string pStrRise)
        {
            string lStrQuery = string.Empty;
            int lIntStatus = 0;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                lStrQuery = this.GetSQL("GetVoucherStatusByRiseId").InjectSingleValue("FolioRise", pStrRise);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lIntStatus = int.Parse(lObjRecordset.Fields.Item("U_Status").Value.ToString());
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseDAO - GetVoucherStatus: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }

            return lIntStatus;
        }

        public bool ExistsRiseByFolio(int pIntFolio)
        {
            bool lBolResult = false;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Folio", pIntFolio.ToString());

                string lStrQuery = this.GetSQL("ExistsRise").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lBolResult = true;
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseDAO - ExistsRiseByFolio: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lBolResult;
        }

        public RiseDTO GetRiseById(int pIntRiseId)
        {
            RiseDTO lObjRiseDTO = null;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetRiseById").InjectSingleValue("RiseId", pIntRiseId.ToString());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lObjRiseDTO = new RiseDTO
                    {
                        Code = int.Parse(lObjRecordset.Fields.Item("Code").Value.ToString()),
                        IdRise = int.Parse(lObjRecordset.Fields.Item("U_IdRise").Value.ToString()),
                        CreatedDate = DateTime.Parse(lObjRecordset.Fields.Item("U_CreatedDate").Value.ToString()),
                        StartDate = DateTime.Parse(lObjRecordset.Fields.Item("U_StartDate").Value.ToString()),
                        EndDate = DateTime.Parse(lObjRecordset.Fields.Item("U_EndDate").Value.ToString()),
                        Client = lObjRecordset.Fields.Item("U_Client").Value.ToString(),
                        ClientName = lObjRecordset.Fields.Item("CardName").Value.ToString(),
                        SupervisorId = int.Parse(lObjRecordset.Fields.Item("U_Supervisor").Value.ToString()),
                        SupervisorName = lObjRecordset.Fields.Item("SupervisorName").Value.ToString(),
                        DocStatus = (RiseStatusEnum)int.Parse(lObjRecordset.Fields.Item("U_DocStatus").Value.ToString()),
                        FolioRelation = int.Parse(lObjRecordset.Fields.Item("U_DocRef").Value.ToString()),
                        UserId = int.Parse(lObjRecordset.Fields.Item("U_UserId").Value.ToString()),
                    };
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseDAO - GetRiseById: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lObjRiseDTO;
        }

        public List<RiseFiltersDTO> GetRisesByContractId(string pStrDocEntry)
        {
            List<RiseFiltersDTO> lLstRiseFilters = new List<RiseFiltersDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetRisesByContractId").InjectSingleValue("ContractDocEntry", pStrDocEntry);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        RiseFiltersDTO lObjRiseFilters = new RiseFiltersDTO
                        {
                            IdRise = int.Parse(lObjRecordset.Fields.Item("IdRise").Value.ToString()),
                            ContractDocEntry = 0, //int.Parse(lObjRecordset.Fields.Item("ContractDocEntry").Value.ToString()),
                            HrsFeet = double.Parse(lObjRecordset.Fields.Item("HrFeet").Value.ToString()),
                        };

                        lLstRiseFilters.Add(lObjRiseFilters);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseDAO - GetRisesByContractId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstRiseFilters;
        }
    }
}
