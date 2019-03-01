using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Exceptions;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class UsersDAO
    {
        public int GetUserId(string pStrUsername)
        {
            int lIntUserId = 0;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("UserName", pStrUsername);

                string lStrQuery = this.GetSQL("GetUserId").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lIntUserId = int.Parse(lObjRecordset.Fields.Item("USERID").Value.ToString());
                }

                LogService.WriteInfo(string.Format("[UsersDAO - GetUserId: Usuario obtenido {0}]", lIntUserId));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[UsersDAO - GetUserId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lIntUserId;
        }

        public string GetUserCenterCost(string pStrUsercode)
        {
            string lStrCenterCost = string.Empty;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetUserCenterCost").InjectSingleValue("UserCode", pStrUsercode);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrCenterCost = lObjRecordset.Fields.Item("CenterCost").Value.ToString();
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[UsersDAO - GetUserCenterCost: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lStrCenterCost;
        }
    }
}
