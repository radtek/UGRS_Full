using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class ClientsService
    {
        private ClientsDAO lObjClientsDTO;

        public ClientsService()
        {
            lObjClientsDTO = new ClientsDAO();
        }

        public List<ClientsDTO> GetClients()
        {
            return lObjClientsDTO.GetClients();
        }
    }
}
