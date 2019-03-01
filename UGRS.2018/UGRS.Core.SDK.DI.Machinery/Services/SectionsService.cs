using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class SectionsService
    {
        private SectionsDAO mObjSectionsDAO;

        public SectionsService()
        {
            mObjSectionsDAO = new SectionsDAO();
        }

        #region Entities

        #endregion

        #region DAO
        public List<SectionsDTO> GetSections(int pIntMunicipalityCode)
        {
            return mObjSectionsDAO.GetSections(pIntMunicipalityCode);
        }

        public List<SectionsDTO> GetSectionsBySalesOrder(int pIntDocEntry)
        {
            return mObjSectionsDAO.GetSectionsBySalesOrder(pIntDocEntry);
        }
        #endregion
    }
}
