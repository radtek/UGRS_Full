using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class ConstructionService
    {
        private ConstructionTypeDAO lObjConstructionTypeDAO;

        public ConstructionService()
        {
            lObjConstructionTypeDAO = new ConstructionTypeDAO();
        }

        public List<ConstructionTypeDTO> GetConstructionTypes()
        {
            return lObjConstructionTypeDAO.GetConstructionTypes();
        }
    }
}
