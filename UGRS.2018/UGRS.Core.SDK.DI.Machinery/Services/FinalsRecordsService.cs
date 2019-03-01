using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery.Tables;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class FinalsRecordsService
    {
        private TableDAO<FinalsRecords> mObjFinalRecordsTableDAO;
        private TableDAO<FinalsRecords> mObjFinalRecordsDAO;

        public FinalsRecordsService()
        {
            mObjFinalRecordsTableDAO = new TableDAO<FinalsRecords>();
        }

        #region Entities
        public int Add(FinalsRecords pObjFinalsRecord)
        {
            try
            {
                return mObjFinalRecordsTableDAO.Add(pObjFinalsRecord);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[FinalsRecordsService - Add: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(FinalsRecords pObjFinalsRecord)
        {
            try
            {
                return mObjFinalRecordsTableDAO.Update(pObjFinalsRecord);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[FinalsRecordsService - Update: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjFinalRecordsTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[FinalsRecordsService - GetLastCode: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public IList<ConsumablesDocumentsDTO> GetByRelatedRiseId(int pIntRiseId)
        {
            try
            {
                return ToConsumablesDTO(new QueryManager().GetObjectsList<FinalsRecords>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjFinalRecordsTableDAO.GetUserTableName())));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[FinalsRecordsService - GetByRelatedRiseId: {0}]", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO

        #endregion

        #region Extras
        public IList<ConsumablesDocumentsDTO> ToConsumablesDTO(IList<FinalsRecords> pLstEmployees)
        {
            return pLstEmployees.Select(x => new ConsumablesDocumentsDTO
            {
                Code = x.RowCode,
                IdRise = x.IdRise,
                ActivoCode = x.PrcCode,
                EcoNum = x.EcoNum,
                DieselM = x.DieselM,
                DieselT = x.DieselT,
                Gas = x.Gas,
                F15W40 = x.F15W40,
                Hidraulic = x.Hidraulic,
                SAE40 = x.SAE40,
                Transmition = x.Transmition,
                Oils = x.Oils,
                KmHr = x.KmHr,
            }).ToList();
        }
        #endregion
    }
}
