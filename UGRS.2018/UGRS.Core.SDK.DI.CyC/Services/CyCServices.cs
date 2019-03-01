using System.Collections.Generic;
using UGRS.Core.SDK.DI.CyC.DAO;
using UGRS.Core.SDK.DI.CyC.DTO;
using UGRS.Core.SDK.DI.CyC.Tables;

namespace UGRS.Core.SDK.DI.CyC.Services
{
    public class CyCServices
    {
        public CyCDAO lObjCyCDAO;

        public CyCServices()
        {
            lObjCyCDAO = new CyCDAO();
        }

        public List<string> GetAuctions(string pStrCostCenter, string pStrUserId)
        {
            return lObjCyCDAO.GetAuctions(pStrCostCenter, pStrUserId);
        }

        public List<AuctionDTO> GetAuctionByCustomer(string pStrFolio, string pStrArea)
        {
            return lObjCyCDAO.GetAuctionDTO(pStrFolio, pStrArea);
        }

        public List<InvoiceDTO> GetInvoices(string pStrCardCode, string pStrOcrCode, string pStrType)
        {
            return lObjCyCDAO.GetInvoices(pStrCardCode, pStrOcrCode, pStrType);
        }

        public List<PaymentsDTO> GetPays(string pStrFolio)
        {
            return lObjCyCDAO.GetPays(pStrFolio);
        }

        public UserDTO GetUser(string pStrUserCode)
        {
            return lObjCyCDAO.GetUser(pStrUserCode);
        }

        public List<Coments> GetComents(string pStrFolio, string pStrCostCenter, string pStrCardcode)
        {
            return lObjCyCDAO.GetComents(pStrFolio, pStrCostCenter,pStrCardcode);
        }

        public List<Coments> GetJournalEntryComents(string pStrFolio, string pStrCardcode)
        {
            return lObjCyCDAO.GetJournalEntryComents(pStrFolio, pStrCardcode);
        }


        public bool GetUserCyC(string pStrUserCode)
        {
            return lObjCyCDAO.GetUserCyC(pStrUserCode);
        }

        public Auction GetAuction(string pStrFolio)
        {
            return lObjCyCDAO.GetAuction(pStrFolio);
        }

        public List<MessageDTO> GetMessageDTO(string pStrFolio)
        {
            return lObjCyCDAO.GetMessagesCyC(pStrFolio);
        }
    }
}

