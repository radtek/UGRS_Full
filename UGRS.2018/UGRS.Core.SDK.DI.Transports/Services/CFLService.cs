using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Purchases.DTO;
using UGRS.Core.SDK.DI.Transports.DAO;

namespace UGRS.Core.SDK.DI.Transports.Services
{

   public class CFLService
    {
       RouteListDAO mObjRouteListDAO = new RouteListDAO();

       public string GetCFLTownsQuery(string pStrSearch)
       {
           return mObjRouteListDAO.GetTownsQuery(pStrSearch);
       }

       public string GetCFLItemsQuery(string pStrWHS, string pStrCardCode,string pStrSearch)
       {
           return mObjRouteListDAO.GetItemsQuery(pStrWHS,pStrCardCode, pStrSearch);
       }

       public string GetCFLAFQuery(string pStrCostingCode,string pStrEquip, string pStrSearch)
       {
           return mObjRouteListDAO.GetAFQuery(pStrCostingCode,pStrEquip,pStrSearch);
       }

       public string GetCFLDrivers(string pStrCostingCode,string pStrSearch)
       {
           return mObjRouteListDAO.GetDriversQuery(pStrCostingCode,pStrSearch);
       }

       public string GetCostingCode(int pIntUserSignature)
       {
           return mObjRouteListDAO.GetCostingCode(pIntUserSignature);
       }

       public string GetWhs(int pIntUserSignature)
       {
           return mObjRouteListDAO.GetWhs(pIntUserSignature);
       }

       public string GetCFLFoliosQuery()
       {
           return mObjRouteListDAO.GetFolios();
       }

       public IList<AssetsDTO> GetAssets(string pStrArea)
       {
           return mObjRouteListDAO.GetAssets(pStrArea);
       }
    }
}
