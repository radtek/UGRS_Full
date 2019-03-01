using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.Exceptions;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class EmployeesDAO
    {
        public List<EmployeesDTO> GetEmployeesByRiseId(int pIntRiseId)
        {
            List<EmployeesDTO> lLstEmployeesDTO = new List<EmployeesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetEmployeesByRiseId").InjectSingleValue("RiseId", pIntRiseId);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        EmployeesDTO lObjInitialRecordsDTO = new EmployeesDTO
                        {
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString(),
                            IdRise = int.Parse(lObjRecordset.Fields.Item("U_IdRise").Value.ToString()),
                            Id = int.Parse(lObjRecordset.Fields.Item("U_Employee").Value.ToString()),
                            Employee = lObjRecordset.Fields.Item("lastName").Value.ToString(),
                            Status = lObjRecordset.Fields.Item("Status").Value.ToString(),
                        };

                        lLstEmployeesDTO.Add(lObjInitialRecordsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[EmployeesDAO - GetEmployeesByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstEmployeesDTO;
        }
    }
}
