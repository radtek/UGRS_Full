using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Expogan.Tables;

namespace UGRS.Core.SDK.DI.Expogan.Services
{
    public class SetupService
    {
        private TableDAO<Locations> mObjLocationsDAO;


        public SetupService()
        {
            mObjLocationsDAO = new TableDAO<Locations>();
        }

        public void InitializeTables()
        {
            mObjLocationsDAO.Initialize();
        }
    }
}
