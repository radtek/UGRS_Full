using System.Collections.Generic;
using UGRS.Core.SDK.DI.Expogan.DAO;
using UGRS.Core.SDK.DI.Expogan.DTO;

namespace UGRS.Core.SDK.DI.Expogan.Services
{
    public class LocationService
    {
        private LocationsDAO lObjLocationsDAO;

        public LocationService()
        {
            lObjLocationsDAO = new LocationsDAO();
        }

        public IList<LocationDTO> GetLocations(string pStrGroup)
        {
            return lObjLocationsDAO.GetLocations(pStrGroup);
        }

        public IList<LevelDTO> GetLevels()
        {
            return lObjLocationsDAO.GetGroupLocations();
        }
        public string GetItemCode(string pStrField)
        {
            return lObjLocationsDAO.GetItemCode(pStrField);
        }

        public double GetCost(string pStrLocation)
        {
            return lObjLocationsDAO.GetCostLocation(pStrLocation);
        }

        public string GetLastContractId()
        {
            return lObjLocationsDAO.GetLastFolioContract();
        }

        public string GetCostCenter()
        {
            return lObjLocationsDAO.GetCostCenter();
        }
    }
}
