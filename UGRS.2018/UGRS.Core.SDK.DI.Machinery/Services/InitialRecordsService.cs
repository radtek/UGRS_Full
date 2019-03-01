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
    public class InitialRecordsService
    {
        private TableDAO<InitialRecords> mObjInitialRecordsTableDAO;
        //private InitialRecordsDAO mObjInitialRecordsDAO;

        public InitialRecordsService()
        {
            //mObjInitialRecordsDAO = new InitialRecordsDAO();
            mObjInitialRecordsTableDAO = new TableDAO<InitialRecords>();
        }

        #region Entities
        public int Add(InitialRecords pObjInitialRecord)
        {
            try
            {
                return mObjInitialRecordsTableDAO.Add(pObjInitialRecord);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[InitialRecordsService - Add: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(InitialRecords pObjInitialRecord)
        {
            try
            {
                return mObjInitialRecordsTableDAO.Update(pObjInitialRecord);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[InitialRecordsService - Update: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjInitialRecordsTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[InitialRecordsService - GetLastCode: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public List<ConsumablesDocumentsDTO> GetByRiseId(int pIntRiseId)
        {
            try
            {
                List<InitialRecords> lLstInitialRds = new QueryManager().GetObjectsList<InitialRecords>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjInitialRecordsTableDAO.GetUserTableName())).ToList();

                return ToConsumablesDocDTO(lLstInitialRds);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[InitialRecordsService - GetByRiseId: {0}]", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO

        #endregion

        #region Extras
        public List<ConsumablesDocumentsDTO> ToConsumablesDocDTO(List<InitialRecords> pLstInitialRecords)
        {
            return pLstInitialRecords.Select(x => new ConsumablesDocumentsDTO
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
