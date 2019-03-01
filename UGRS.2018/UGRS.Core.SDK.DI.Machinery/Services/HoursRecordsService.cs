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
    public class HoursRecordsService
    {
        private TableDAO<HoursRecords> mObjHoursRecordsTableDAO;
        private HoursRecordsDAO mObjHoursRecordsDAO;

        public HoursRecordsService()
        {
            mObjHoursRecordsTableDAO = new TableDAO<HoursRecords>();
            mObjHoursRecordsDAO = new HoursRecordsDAO();
        }

        #region Entities
        public int Add(HoursRecords pObjHoursRecords)
        {
            try
            {
                int result = mObjHoursRecordsTableDAO.Add(pObjHoursRecords);
                //string lStrCode = new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjEmployeesTableDAO.GetUserTableName()));

                return result;
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[HoursRecordsService - Add: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(HoursRecords pObjHoursRecords)
        {
            try
            {
                return mObjHoursRecordsTableDAO.Update(pObjHoursRecords);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[HoursRecordsService - Update: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public int Remove(string pStrCode)
        {
            try
            {
                return mObjHoursRecordsTableDAO.Remove(pStrCode);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[HoursRecordsService - Remove: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public IList<HoursRecords> GetByRiseId(int pIntRiseId)
        {
            try
            {
                return new QueryManager().GetObjectsList<HoursRecords>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjHoursRecordsTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[HoursRecordsService - GetByRiseId: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjHoursRecordsTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[HoursRecordsService - GetLastCode: {0}]", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO
        public List<HoursRecordsDTO> GetHoursRecordsByRiseId(int pIntRiseId)
        {
            return mObjHoursRecordsDAO.GetHoursRecordsByRiseId(pIntRiseId);
        }
        #endregion

        #region Extras
        public List<HoursRecordsDTO> DataTableToDTO(SAPbouiCOM.DataTable pObjDataTable)
        {
            List<HoursRecordsDTO> lLstHoursRecords = new List<HoursRecordsDTO>();

            for (int i = 0; i < pObjDataTable.Rows.Count; i++)
            {
                HoursRecordsDTO lObjHoursRecords = new HoursRecordsDTO();

                lObjHoursRecords.Code = pObjDataTable.GetValue("CodeHrs", i).ToString();
                lObjHoursRecords.IdRise = int.Parse(pObjDataTable.GetValue("IdRiHrs", i).ToString());
                lObjHoursRecords.ContractEntry = int.Parse(pObjDataTable.GetValue("ContOVHrs", i).ToString());
                lObjHoursRecords.ContractDocNum = int.Parse(pObjDataTable.GetValue("CDEOVHrs", i).ToString());
                lObjHoursRecords.DateHour = DateTime.Parse(pObjDataTable.GetValue("DateHrs", i).ToString());
                lObjHoursRecords.SupervisorId = int.Parse(pObjDataTable.GetValue("SupIdHrs", i).ToString());
                lObjHoursRecords.Supervisor = pObjDataTable.GetValue("SupNmHrs", i).ToString();
                lObjHoursRecords.OperatorId = int.Parse(pObjDataTable.GetValue("OpdIdHrs", i).ToString());
                lObjHoursRecords.OperatorName = pObjDataTable.GetValue("OpdNmHrs", i).ToString();
                lObjHoursRecords.PrcCode = pObjDataTable.GetValue("EqpHrs", i).ToString();
                lObjHoursRecords.EcoNum = pObjDataTable.GetValue("NumEcnHrs", i).ToString();
                lObjHoursRecords.HrFeet = double.Parse(pObjDataTable.GetValue("HrsFeetHr", i).ToString());
                lObjHoursRecords.SectionId = int.Parse(pObjDataTable.GetValue("SctnIdHrs", i).ToString());
                lObjHoursRecords.Section = pObjDataTable.GetValue("SctnNnHrs", i).ToString();
                lObjHoursRecords.KmHt = double.Parse(pObjDataTable.GetValue("KmHcHrs", i).ToString());
                lObjHoursRecords.Pending = double.Parse(pObjDataTable.GetValue("PendHrs", i).ToString());
                lObjHoursRecords.Close = pObjDataTable.GetValue("CloseHrs", i).ToString();

                lLstHoursRecords.Add(lObjHoursRecords);
            }

            return lLstHoursRecords;
        }
        #endregion
    }
}
