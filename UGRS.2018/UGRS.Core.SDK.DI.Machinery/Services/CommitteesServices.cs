using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class CommitteesServices
    {
        private CommitteesDAO lObjCommitteesDAO;

        public CommitteesServices()
        {
            lObjCommitteesDAO = new CommitteesDAO();
        }

        public List<CommitteesDTO> GetCommitteesByMunCode(int pIntMunicipalityCode)
        {
            return lObjCommitteesDAO.GetCommitteesByCode(pIntMunicipalityCode);
        }
    }
}
