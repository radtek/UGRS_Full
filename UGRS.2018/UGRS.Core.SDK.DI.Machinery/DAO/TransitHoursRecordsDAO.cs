using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Exceptions;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;
using SAPbobsCOM;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class TransitHoursRecordsDAO
    {
        public List<TransitHoursRecordsDTO> GetTransitHoursRecordsByRiseId(int pIntRiseId)
        {
            List<TransitHoursRecordsDTO> lLstTransitHoursRecordsDTO = new List<TransitHoursRecordsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetTransitHoursUDTByRiseId").InjectSingleValue("RiseId", pIntRiseId.ToString());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        TransitHoursRecordsDTO lObjTransitHoursRecordsDTO = new TransitHoursRecordsDTO
                        {
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString(),
                            Hrs = double.Parse(lObjRecordset.Fields.Item("U_Hrs").Value.ToString()),
                            PrcCode = lObjRecordset.Fields.Item("U_PrcCode").Value.ToString(),
                            IdRise = int.Parse(lObjRecordset.Fields.Item("U_IdRise").Value.ToString()),
                            EcoNum = lObjRecordset.Fields.Item("U_EcoNum").Value.ToString(),
                            OperatorId = int.Parse(lObjRecordset.Fields.Item("U_Operator").Value.ToString()),
                            OperatorName = lObjRecordset.Fields.Item("OperatorName").Value.ToString(),
                            SupervisorId = int.Parse(lObjRecordset.Fields.Item("U_Supervisor").Value.ToString()),
                            SupervisorName = lObjRecordset.Fields.Item("SupervisorName").Value.ToString(),
                        };

                        lLstTransitHoursRecordsDTO.Add(lObjTransitHoursRecordsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TransitHoursRecordsDAO - GetTransitHoursRecordsByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstTransitHoursRecordsDTO;
        }
    }
}
