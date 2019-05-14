using System;
using System.Collections.Generic;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;

namespace UGRS.Core.SDK.DI.Transports.DAO
{
    public class CommissionDriverDAO
    {
        public IList<CommissionDriverDetailsDTO> GetCommissionDriversDetails(string pStrFolio, string pStrAccount)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<CommissionDriverDetailsDTO> lLstCommissiosDTO = new List<CommissionDriverDetailsDTO>();
            try
            {
                Dictionary<string, string> lLstParams = new Dictionary<string, string>();
                lLstParams.Add("Folio", pStrFolio);
                lLstParams.Add("Account", pStrAccount);

                string lStrQuery = this.GetSQL("GetCommissionDriverDetails").Inject(lLstParams);

                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        CommissionDriverDetailsDTO lObjCommission = new CommissionDriverDetailsDTO();
                        lObjCommission.Folio = lObjResults.GetColumnValue<string>("Folio");
                        lObjCommission.EmployeeId = lObjResults.GetColumnValue<string>("EmpId");
                        lObjCommission.Employee = lObjResults.GetColumnValue<string>("EmployeeName");
                        lObjCommission.Import = lObjResults.GetColumnValue<double>("Import");
                        lObjCommission.Account = lObjResults.GetColumnValue<string>("Account");
                        lLstCommissiosDTO.Add(lObjCommission);
                        lObjResults.MoveNext();
                    }
                }
                return lLstCommissiosDTO;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[CommissionDriverDAO - GetCommissionsDriversDetails] Error al obtener los detalles de la comisión: {0}", e.Message));
                throw new Exception(string.Format("Error al obtener los detalles de la comisión: {0}", e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public string GetCmsFoliosQuery()
        {
            return this.GetSQL("GetCommissionDriverFolios");
        }
    }
}
