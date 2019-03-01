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
    public class HoursRecordsDAO
    {
        public List<HoursRecordsDTO> GetHoursRecordsByRiseId(int pIntRiseId)
        {
            List<HoursRecordsDTO> lLstHoursRecordsDTO = new List<HoursRecordsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetHoursUDTByRiseId").InjectSingleValue("RiseId", pIntRiseId.ToString());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        HoursRecordsDTO lObjHoursRecordsDTO = new HoursRecordsDTO
                        {
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString(),
                            ContractEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString()),
                            ContractDocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                            IdRise = int.Parse(lObjRecordset.Fields.Item("U_IdRise").Value.ToString()),
                            DateHour = DateTime.Parse(lObjRecordset.Fields.Item("U_DateHour").Value.ToString()),
                            SupervisorId = int.Parse(lObjRecordset.Fields.Item("U_Supervisor").Value.ToString()),
                            Supervisor = lObjRecordset.Fields.Item("SupervisorName").Value.ToString(),
                            OperatorId = int.Parse(lObjRecordset.Fields.Item("U_Operator").Value.ToString()),
                            OperatorName = lObjRecordset.Fields.Item("OperatorName").Value.ToString(),
                            PrcCode = lObjRecordset.Fields.Item("U_PrcCode").Value.ToString(),
                            EcoNum = lObjRecordset.Fields.Item("U_EcoNum").Value.ToString(),
                            HrFeet = double.Parse(lObjRecordset.Fields.Item("U_HrFeet").Value.ToString()),
                            SectionId = int.Parse(lObjRecordset.Fields.Item("U_Section").Value.ToString()),
                            Section = lObjRecordset.Fields.Item("SectionName").Value.ToString(), //string.Empty, 
                            KmHt = double.Parse(lObjRecordset.Fields.Item("U_KmHt").Value.ToString()),
                            Pending = double.Parse(lObjRecordset.Fields.Item("U_Pending").Value.ToString()),
                            Close = lObjRecordset.Fields.Item("U_Close").Value.ToString(),
                            ContractClient = lObjRecordset.Fields.Item("ClientName").Value.ToString(),
                        };

                        lLstHoursRecordsDTO.Add(lObjHoursRecordsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[HoursRecordsDAO - GetHoursRecordsByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstHoursRecordsDTO;
        }
    }
}
