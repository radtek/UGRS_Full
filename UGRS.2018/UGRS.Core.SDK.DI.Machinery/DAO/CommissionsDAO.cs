using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Exceptions;
using SAPbobsCOM;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class CommissionsDAO
    {
        public List<CommissionDetailsDTO> GetRisesDetailsForCommissions(string pStrRiseId, string pStrAccount)
        {
            List<CommissionDetailsDTO> lLstCommissionDetails = new List<CommissionDetailsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Account", pStrAccount);
                lLstStrParameters.Add("RiseId", pStrRiseId);

                string lStrQuery = this.GetSQL("GetRiseDetailsForCommissions").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        CommissionDetailsDTO lObjRiseFilters = new CommissionDetailsDTO
                        {
                            IdRise = int.Parse(pStrRiseId),
                            EmployeeId = int.Parse(lObjRecordset.Fields.Item("EmpId").Value.ToString()),
                            Employee = lObjRecordset.Fields.Item("EmployeeName").Value.ToString(),
                            Hours = double.Parse(lObjRecordset.Fields.Item("Horas").Value.ToString()),
                            Rate = double.Parse(lObjRecordset.Fields.Item("Rate").Value.ToString()),
                            Commission = double.Parse(lObjRecordset.Fields.Item("Comision").Value.ToString()),
                            Position = lObjRecordset.Fields.Item("Tipo").Value.ToString(), //== "S" ? "Supervisor" : "Operador",
                            ImportFS = double.Parse(lObjRecordset.Fields.Item("ImportFS").Value.ToString()),
                            Adeudo = double.Parse(lObjRecordset.Fields.Item("Adeudo").Value.ToString()),
                            CodeMov = lObjRecordset.Fields.Item("CodeMove").Value.ToString(),
                            IsSupervisor = lObjRecordset.Fields.Item("Puesto").Value.ToString(),
                        };

                        lLstCommissionDetails.Add(lObjRiseFilters);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[CommissionsDAO - GetRisesDetailsForCommissions: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstCommissionDetails;
        }

        public List<SupervisorDebitDTO> GetSupervisorDebit(string pStrRiseId, string pStrAccount, string pStrSupervisorId)
        {
            List<SupervisorDebitDTO> lLstSupervisorDebit = new List<SupervisorDebitDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Account", pStrAccount);
                lLstStrParameters.Add("RiseId", pStrRiseId);
                lLstStrParameters.Add("EmployeeId", pStrSupervisorId);

                string lStrQuery = this.GetSQL("GetRiseSupervisorAdeudo").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        SupervisorDebitDTO lObjSupervisorDebit = new SupervisorDebitDTO
                        {
                            RiseId = pStrRiseId,
                            MovementCode = lObjRecordset.Fields.Item("U_GLO_CodeMov").Value.ToString(),
                            Debit = double.Parse(lObjRecordset.Fields.Item("Adeudo").Value.ToString()),
                        };

                        lLstSupervisorDebit.Add(lObjSupervisorDebit);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[CommissionsDAO - SupervisorDebitDTO: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstSupervisorDebit;
        }

        public double GetCurrentSupervisorDebit(string pStrRiseId, string pStrAccount, string pStrSupervisorId)
        {
            double lDblDebit = 0;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Account", pStrAccount);
                lLstStrParameters.Add("RiseId", pStrRiseId);
                lLstStrParameters.Add("EmployeeId", pStrSupervisorId);

                string lStrQuery = this.GetSQL("GetCurrentRiseSupervisorAdeudo").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lDblDebit = double.Parse(lObjRecordset.Fields.Item("ImportFS").Value.ToString());
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[CommissionsDAO - GetCurrentSupervisorDebit: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lDblDebit;
        }
    }
}
