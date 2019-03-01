using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class AddressService
    {
        private AddressDAO lObjAddressDAO;

        public AddressService()
        {
            lObjAddressDAO = new AddressDAO();
        }

        public List<DestinationAddressDTO> GetDestinationAddressClient()
        {
            return lObjAddressDAO.GetDestinationAddressClient();
        }
    }
}
