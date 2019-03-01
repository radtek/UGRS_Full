using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Expogan.Tables;

namespace UGRS.Core.SDK.DI.Expogan.Services
{
    public class LocationTableService
    {
        private TableDAO<Locations> mObjLocationsDAO;

        public LocationTableService()
        {
            mObjLocationsDAO = new TableDAO<Locations>();
        }

        public int Add(Locations pObjLocations)
        {
            return mObjLocationsDAO.Add(pObjLocations);
        }

        public int Update(Locations pObjLocations)
        {
            return mObjLocationsDAO.Update(pObjLocations);
        }
    }
}
