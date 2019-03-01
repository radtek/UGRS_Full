using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class MunicipalitiesService
    {
        private MunicipalitiesDAO lObjMunicipalitiesDAO;

        public MunicipalitiesService()
        {
            lObjMunicipalitiesDAO = new MunicipalitiesDAO();
        }

        public List<MunicipalitiesDTO> GetMunicipalities()
        {
            return lObjMunicipalitiesDAO.GetMunicipalities();
        }
    }
}
