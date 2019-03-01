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
    public class ContractsService
    {
        private TableDAO<Contracts> mObjContractsTableDAO;
        private ContractsDAO mObjContractsDAO;

        public ContractsService()
        {
            mObjContractsTableDAO = new TableDAO<Contracts>();
            mObjContractsDAO = new ContractsDAO();
        }

        #region Entities
        public int Add(Contracts pObjContract)
        {
            try
            {
                return mObjContractsTableDAO.Add(pObjContract);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ContractsService - Add: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(Contracts pObjContract)
        {
            return mObjContractsTableDAO.Update(pObjContract);
        }

        public int Remove(string pStrCode)
        {
            try
            {
                return mObjContractsTableDAO.Remove(pStrCode);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ContractsService - Remove: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjContractsTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[GetLastCode - Remove: {0}]", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetCode(int pIntRiseId)
        {
            try
            {
                return new QueryManager().GetValue("Code", "U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjContractsTableDAO.GetUserTableName()).ToString());
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[GetLastCode - GetCode: {0}]", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO
        public List<ContractsDTO> GetContracts()
        {
            return mObjContractsDAO.GetContracts();
        }

        public ContractsDTO GetContract(int pIntDocEntry)
        {
            return mObjContractsDAO.GetContract(pIntDocEntry);
        }

        public List<ContractsTypesDTO> GetContractsTypes()
        {
            return mObjContractsDAO.GetContractsTypes();
        }

        public List<ContractsDTO> GetContractsByRiseId(int pIntRiseId)
        {
            return mObjContractsDAO.GetContractsByRiseId(pIntRiseId);
        }

        public int GetLastCode(int pIntRiseId, int pIntDocEntry)
        {
            return mObjContractsDAO.GetLastCode(pIntRiseId, pIntDocEntry);
        }

        public bool IsClosed(int pIntDocEntry)
        {
            return mObjContractsDAO.IsClosed(pIntDocEntry);
        }

        public List<ContractsFiltersDTO> GetContracts(string pStrContract, string pStrClient, string pStrStatus, string pStrStartDate, string pStrEndDate)
        {
            return mObjContractsDAO.GetContracts(pStrContract, pStrClient, pStrStatus, pStrStartDate, pStrEndDate);
        }
        #endregion

        #region Extras
        public List<ContractsDTO> DataTableToDTO(SAPbouiCOM.DataTable pObjDataTable)
        {
            try
            {
                List<ContractsDTO> lLstContracts = new List<ContractsDTO>();

                for (int i = 0; i < pObjDataTable.Rows.Count; i++)
                {
                    ContractsDTO lObjContracts = new ContractsDTO();

                    lObjContracts.DocEntry = int.Parse(pObjDataTable.GetValue("DocECont", i).ToString());
                    lObjContracts.DocNum = int.Parse(pObjDataTable.GetValue("DocNCont", i).ToString());
                    lObjContracts.DocDate = DateTime.Parse(pObjDataTable.GetValue("DateCont", i).ToString());
                    lObjContracts.Type = int.Parse(pObjDataTable.GetValue("TypCCont", i).ToString());
                    lObjContracts.Status = pObjDataTable.GetValue("StsCCont", i).ToString();
                    lObjContracts.Import = double.Parse(pObjDataTable.GetValue("ImpCont", i).ToString());
                    lObjContracts.Code = pObjDataTable.GetValue("CodeTCont", i).ToString();
                    lObjContracts.CardName = pObjDataTable.GetValue("CardName", i).ToString();

                    lLstContracts.Add(lObjContracts);
                }

                return lLstContracts;
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ContractsService - DataTableToDTO: {0}]", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion
    }
}
