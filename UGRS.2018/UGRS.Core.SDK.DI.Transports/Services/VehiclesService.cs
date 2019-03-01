using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Transports.DTO;

namespace UGRS.Core.SDK.DI.Transports.Services
{
    public class VehiclesService
    {
        DAO.RouteListDAO mObjRouteListDAO = new DAO.RouteListDAO();

        public List<VehiclesDTO> GetVehiclesTypeList()
        {
            return mObjRouteListDAO.GetVehiclesTypeList();
        }

        public List<PayLoadTypeDTO> GetPayloadTypeList()
        {
            return mObjRouteListDAO.GetPayloadTypeList();
        }

    }
}
