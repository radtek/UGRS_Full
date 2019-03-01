using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Permissions.DAO;
using UGRS.Core.SDK.DI.Permissions.DTO;

namespace UGRS.Core.SDK.DI.Permissions.Services
{
    public class PermissionsService
    {
        private EarringRanksDAO lObjEarringRankDAO;

        public PermissionsService()
        {
            lObjEarringRankDAO = new EarringRanksDAO();
        }

        public string GetLinesQuery(string pStrBaseEntry)
        {
            return lObjEarringRankDAO.GetLinesQuery(pStrBaseEntry);
        }

        public bool CheckBaseEntry(string pStrBaseEntry)
        {
            return lObjEarringRankDAO.CheckBaseEntry(pStrBaseEntry);
        }

        public bool CheckStoredRank(string pStrEarringFrom, string pStrEarringTo)
        {
            return lObjEarringRankDAO.CheckStoredRanks(pStrEarringFrom, pStrEarringTo);
        }

        public int GetDocEntry(string pStrDocNum)
        {
            return lObjEarringRankDAO.GetDocEntry(pStrDocNum);
        }

        public int GetTotalCertHeads(int pIntDocEntry)
        {
            return lObjEarringRankDAO.GetTotalCertHeads(pIntDocEntry);
        }

        public List<InvoiceExpDTO> GetInvoices()
        {
            return lObjEarringRankDAO.GetInvoices();
        }

        public List<string> GetCertificates(string pStrCert)
        {
            return lObjEarringRankDAO.GetCertificates(pStrCert);
        }

        public string GetPrefix()
        {
            return lObjEarringRankDAO.GetPrefix();
        }
    }
}
