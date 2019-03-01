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
    public class TransitHoursRecordsService
    {
        private TableDAO<TransitHoursRecords> mObjTransitHoursRecordsTableDAO;
        private TransitHoursRecordsDAO mObjTransitHoursRecordsDAO;

        public TransitHoursRecordsService()
        {
            mObjTransitHoursRecordsTableDAO = new TableDAO<TransitHoursRecords>();
            mObjTransitHoursRecordsDAO = new TransitHoursRecordsDAO();
        }

        #region DAO
        public List<TransitHoursRecordsDTO> GetUDTByRiseId(int pIntRiseId)
        {
            return mObjTransitHoursRecordsDAO.GetTransitHoursRecordsByRiseId(pIntRiseId);
        }
        #endregion

        #region Entities
        public int Add(TransitHoursRecords pObjTransitHoursRecords)
        {
            try
            {
                int result = mObjTransitHoursRecordsTableDAO.Add(pObjTransitHoursRecords);
                //string lStrCode = new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjEmployeesTableDAO.GetUserTableName()));

                return result;
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TransitHoursRecordsService - Add]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(TransitHoursRecords pObjTransitHoursRecords)
        {
            try
            {
                return mObjTransitHoursRecordsTableDAO.Update(pObjTransitHoursRecords);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TransitHoursRecordsService - Update]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public int Remove(string pStrCode)
        {
            try
            {
                return mObjTransitHoursRecordsTableDAO.Remove(pStrCode);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TransitHoursRecordsService - Remove]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public List<TransitHoursRecordsDTO> GetByRiseId(int pIntRiseId)
        {
            try
            {
                return ToTransitHoursDTO(new QueryManager().GetObjectsList<TransitHoursRecords>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjTransitHoursRecordsTableDAO.GetUserTableName())).ToList());
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TransitHoursRecordsService - GetByRiseId]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjTransitHoursRecordsTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TransitHoursRecordsService - GetLastCode]: {0}", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region Extras
        public List<TransitHoursRecordsDTO> ToTransitHoursDTO(List<TransitHoursRecords> pLstPerformance)
        {
            return pLstPerformance.Select(x => new TransitHoursRecordsDTO
            {
                Code = x.RowCode,
                IdRise = x.IdRise,
                PrcCode = x.PrcCode,
                EcoNum = x.EcoNum,
                Hrs = x.Hrs,
                OperatorId = x.Operator,
            }).ToList();
        }

        public List<TransitHoursRecordsDTO> DataTableToDTO(SAPbouiCOM.DataTable pObjDataTable)
        {
            List<TransitHoursRecordsDTO> lLstTransitHours = new List<TransitHoursRecordsDTO>();

            for (int i = 0; i < pObjDataTable.Rows.Count; i++)
            {
                TransitHoursRecordsDTO lObjHoursRecords = new TransitHoursRecordsDTO();

                lObjHoursRecords.Code = pObjDataTable.GetValue("CodeTH", i).ToString();
                lObjHoursRecords.IdRise = int.Parse(pObjDataTable.GetValue("IdRiTHrs", i).ToString());
                lObjHoursRecords.PrcCode = pObjDataTable.GetValue("MaqTHrs", i).ToString();
                lObjHoursRecords.EcoNum = pObjDataTable.GetValue("NumEcoTH", i).ToString();
                lObjHoursRecords.Hrs = double.Parse(pObjDataTable.GetValue("HrsTH", i).ToString());
                lObjHoursRecords.OperatorId = int.Parse(pObjDataTable.GetValue("OpdTH", i).ToString());
                lObjHoursRecords.OperatorName = pObjDataTable.GetValue("OpdNmTH", i).ToString();

                lLstTransitHours.Add(lObjHoursRecords);
            }

            return lLstTransitHours;
        }
        #endregion
    }
}
