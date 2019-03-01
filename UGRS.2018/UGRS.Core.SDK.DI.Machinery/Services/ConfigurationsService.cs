using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery.Enums;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class ConfigurationsService
    {
        private ConfigurationsDAO lObjConfigurationsDAO;

        public ConfigurationsService()
        {
            lObjConfigurationsDAO = new ConfigurationsDAO();
        }

        public ConfigurationsDTO GetConfigurationByName(ConfigurationsEnum pEnumConfig)
        {
            return lObjConfigurationsDAO.GetConfigurationByName(pEnumConfig);
        }

        public string GetAccountCode(ConfigurationsEnum pEnumConfig)
        {
            return lObjConfigurationsDAO.GetAccountCode(pEnumConfig);
        }
    }
}
