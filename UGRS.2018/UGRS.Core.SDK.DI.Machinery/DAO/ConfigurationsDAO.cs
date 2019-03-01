using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Exceptions;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Machinery.DTO;
using SAPbobsCOM;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.Core.Extension.Enum;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class ConfigurationsDAO
    {
        public ConfigurationsDTO GetConfigurationByName(ConfigurationsEnum pEnumConfig)
        {
            ConfigurationsDTO lObjConfigurationsDTO = null;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetConfigByName").InjectSingleValue("ParameterName", pEnumConfig.GetDescription());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lObjConfigurationsDTO = new ConfigurationsDTO
                    {
                        Code = int.Parse(lObjRecordset.Fields.Item("Code").Value.ToString()),
                        Name = lObjRecordset.Fields.Item("Name").Value.ToString(),
                        Value = lObjRecordset.Fields.Item("U_Value").Value.ToString(),
                        Comments = lObjRecordset.Fields.Item("U_Comentario").Value.ToString(),
                    };
                }
                else
                {
                    throw new Exception(string.Format("No se encontró la configuración {0}", pEnumConfig.GetDescription()));
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConfigurationsDAO - GetConfigurationByName: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lObjConfigurationsDTO;
        }

        public string GetAccountCode(ConfigurationsEnum pEnumConfig)
        {
            string lStrAccountCode = string.Empty;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetAccountCode").InjectSingleValue("ParameterName", pEnumConfig.GetDescription());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrAccountCode = lObjRecordset.Fields.Item("AcctCode").Value.ToString();
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConfigurationsDAO - GetAccountCode: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lStrAccountCode;
        }
    }
}
