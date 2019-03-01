using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.CyC.Tables;
using UGRS.Core.SDK.DI.DAO;

namespace UGRS.Core.SDK.DI.CyC.Services
{
    public class SetupService
    {
        private TableDAO<Coments> mObjComentsDAO;

        public SetupService()
        {
            mObjComentsDAO = new TableDAO<Coments>();
        }

        public void InitializeTables()
        {
            mObjComentsDAO.Initialize();
        }
    }
}
