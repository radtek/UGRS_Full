using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class EquipmentsService
    {
        private EquipmentDAO lObjEquipmentDAO;

        public EquipmentsService()
        {
            lObjEquipmentDAO = new EquipmentDAO();
        }

        public List<EquipmentDTO> GetEquipmentTypesByContract(int pIntContractCode)
        {
            return lObjEquipmentDAO.GetEquipmentTypesByContractCode(pIntContractCode);
        }
    }
}
