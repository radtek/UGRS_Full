using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Exceptions;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class AuthorizationDAO
    {
        public bool IsOperationsUser(int pIntUserId, string pStrObjType, int pIntFunctionId)
        {
            bool lBolOprUser = false;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("UserId", pIntUserId.ToString());
                lLstStrParameters.Add("ObjType", pStrObjType);
                lLstStrParameters.Add("FunctionId", pIntFunctionId.ToString());

                string lStrQuery = this.GetSQL("IsUserOperations").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lBolOprUser = true;
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[AuthorizationDAO - IsOperationsUser: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lBolOprUser;
        }

        public bool IsOperationsUser(int pIntUserId)
        {
            bool lBolOprUser = false;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("IsOperationsUser").InjectSingleValue("UserId", pIntUserId.ToString());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lBolOprUser = lObjRecordset.Fields.Item("ReOpen").Value.ToString() == "Y" ? true : false;
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[AuthorizationDAO - IsOperationsUser: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lBolOprUser;
        }
    }
}
