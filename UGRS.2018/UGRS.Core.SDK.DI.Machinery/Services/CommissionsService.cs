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
    public class CommissionsService
    {
        private TableDAO<Commissions> mObjCommissionsTableDAO;
        private CommissionsDAO mObjCommissionsDAO;

        public CommissionsService()
        {
            mObjCommissionsDAO = new CommissionsDAO();
            mObjCommissionsTableDAO = new TableDAO<Commissions>();
        }

        #region Entities
        public int Add(Commissions pObjCommissions)
        {
            try
            {
                return mObjCommissionsTableDAO.Add(pObjCommissions);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[CommissionsService - Add]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public void Add(List<Commissions> pLstCommissions)
        {
            try
            {
                foreach (var lObjCommission in pLstCommissions)
                {
                    if (Add(lObjCommission) != 0)
                    {
                        throw new Exception("Error al agregar el registro de la comisión");
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[CommissionsService - Add]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(Commissions pObjCommissions)
        {
            try
            {
                return mObjCommissionsTableDAO.Update(pObjCommissions);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[CommissionsService - Update]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjCommissionsTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[CommissionsService - GetLastCode]: {0}", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO
        public List<CommissionDetailsDTO> GetRisesDetailsForCommissions(string pStrRiseId, string pStrAccount)
        {
            return mObjCommissionsDAO.GetRisesDetailsForCommissions(pStrRiseId, pStrAccount);
        }

        public List<SupervisorDebitDTO> GetSupervisorDebit(string pStrRiseId, string pStrAccount, string pStrSupervisorId)
        {
            return mObjCommissionsDAO.GetSupervisorDebit(pStrRiseId, pStrAccount, pStrSupervisorId);
        }

        public double GetCurrentSupervisorDebit(string pStrRiseId, string pStrAccount, string pStrSupervisorId)
        {
            return mObjCommissionsDAO.GetCurrentSupervisorDebit(pStrRiseId, pStrAccount, pStrSupervisorId);
        }
        #endregion

        #region Extras
        public List<CommissionDTO> DataTableToDTO(SAPbouiCOM.DataTable pObjDataTable)
        {
            try
            {
                List<CommissionDTO> lLstCommissionDTO = new List<CommissionDTO>();

                for (int i = 0; i < pObjDataTable.Rows.Count; i++)
                {
                    CommissionDTO lObjCommissionDTO = new CommissionDTO();

                    lObjCommissionDTO.Code = pObjDataTable.GetValue("Code", i).ToString();
                    lObjCommissionDTO.EmployeeId = int.Parse(pObjDataTable.GetValue("EmpId", i).ToString());
                    lObjCommissionDTO.HrsQty = double.Parse(pObjDataTable.GetValue("Hrs", i).ToString());
                    lObjCommissionDTO.Rate = double.Parse(pObjDataTable.GetValue("Tarifa", i).ToString());
                    lObjCommissionDTO.Commission = double.Parse(pObjDataTable.GetValue("Cmson", i).ToString());
                    lObjCommissionDTO.ImportFS = double.Parse(pObjDataTable.GetValue("ImpFS", i).ToString());
                    lObjCommissionDTO.Debit = double.Parse(pObjDataTable.GetValue("Adeudo", i).ToString());
                    lObjCommissionDTO.Total = double.Parse(pObjDataTable.GetValue("Total", i).ToString());
                    lObjCommissionDTO.Pending = double.Parse(pObjDataTable.GetValue("Pend", i).ToString());
                    lObjCommissionDTO.Position = pObjDataTable.GetValue("IsSup", i).ToString() == "Y" ? "S" : "N";
                    lObjCommissionDTO.PositionCs = pObjDataTable.GetValue("PstoId", i).ToString(); //== "Y" ? "S" : "O";
                    
                    //lObjCommissionDTO.RiseId = int.Parse(pObjDataTable.GetValue("RiseId", i).ToString());
                    //lObjCommissionDTO.JournalEntryId = int.Parse(pObjDataTable.GetValue("JournalId", i).ToString());

                    lLstCommissionDTO.Add(lObjCommissionDTO);
                }

                return lLstCommissionDTO;
            }
            catch (Exception pObjException)
            {
                LogService.WriteError(string.Format("[CommissionsService - DataTableToDTO]: {0}", pObjException.Message));
                throw pObjException;
            }
        }

        public List<Commissions> ConvetToEntity(List<CommissionDTO> lLstCommissionDTO)
        {
            return lLstCommissionDTO.Select(p => new Commissions
            {
                RowCode = p.Code,
                EmployeeId = p.EmployeeId,
                HrsQty = p.HrsQty,
                Rate = p.Rate,
                Commission = p.Commission,
                ImportFS = p.ImportFS,
                Debit = p.Debit,
                Total = p.Total,
                Pending = p.Pending,
                RiseId = p.RiseId,
                JournalEntryId = p.JournalEntryId,
                Position = p.Position,
                PositionCs = p.PositionCs
            }).ToList();
        }
        #endregion
    }
}
