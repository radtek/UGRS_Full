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
    public class TotalsRecordsService
    {
        private TableDAO<TotalRecords> mObjTotalRecordsTableDAO;

        public TotalsRecordsService()
        {
            mObjTotalRecordsTableDAO = new TableDAO<TotalRecords>();
        }

        #region Entities
        public int Add(TotalRecords pObjTotalRecords)
        {
            try
            {
                return mObjTotalRecordsTableDAO.Add(pObjTotalRecords);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TotalsRecordsService - Add]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public int Remove(string pStrCode)
        {
            try
            {
                int lIntResult = 0;
                if (!string.IsNullOrEmpty(pStrCode))
                {
                    lIntResult = mObjTotalRecordsTableDAO.Remove(pStrCode);
                }

                return lIntResult;
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TotalsRecordsService - Remove]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public void RemoveByRiseId(int pIntRiseId)
        {
            try
            {
                List<ConsumablesDocumentsDTO> lLstTotalsRecords = GetByRelatedRiseId(pIntRiseId).ToList();

                foreach (var lObjTotalRecord in lLstTotalsRecords)
                {
                    int lIntResult = 0;
                    if (!string.IsNullOrEmpty(lObjTotalRecord.Code))
                    {
                        lIntResult = mObjTotalRecordsTableDAO.Remove(lObjTotalRecord.Code);
                        LogService.WriteInfo(string.Format("[TotalsRecordsService - RemoveByRiseId] Se eliminó el registro total con Code: {0}", lObjTotalRecord.Code));
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TotalsRecordsService - RemoveByRiseId]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(TotalRecords pObjTotalRecords)
        {
            try
            {
                return mObjTotalRecordsTableDAO.Update(pObjTotalRecords);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TotalsRecordsService - Update]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjTotalRecordsTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TotalsRecordsService - GetLastCode]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public IList<ConsumablesDocumentsDTO> GetByRelatedRiseId(int pIntRiseId)
        {
            try
            {
                return ToConsumablesDTO(new QueryManager().GetObjectsList<TotalRecords>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjTotalRecordsTableDAO.GetUserTableName())));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TotalsRecordsService - GetByRelatedRiseId]: {0}", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO

        #endregion

        #region Extras
        public IList<ConsumablesDocumentsDTO> ToConsumablesDTO(IList<TotalRecords> pLstTotalRecords)
        {
            return pLstTotalRecords.Select(x => new ConsumablesDocumentsDTO
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
