using System.Collections.Generic;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Transports.DAO;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.Core.SDK.DI.Transports.Tables;


namespace UGRS.Core.SDK.DI.Transports.Services
{
    public class RoutesService
    {
        private RouteListDAO mObjRouteListDAO = new RouteListDAO();
        private TableDAO<Routes> mObjRouteDAO = new TableDAO<Routes>();

        public int UpdateRoute(Routes pObjRoute)
        {
            return mObjRouteDAO.Update(pObjRoute);
        }

        public int AddRoute(Routes pObjRoute)
        {
            return mObjRouteDAO.Add(pObjRoute);
        }

        public string GetRouteQuery()
        {
            return mObjRouteListDAO.GetRouteQuery();
        }

        public string GetRoutesByFiltersQuery(string pStrOrign, string pStrDestiny, string pStrTownOrig, string pStrTownDest)
        {
            return mObjRouteListDAO.GetRouteQueryByFilters(pStrOrign, pStrDestiny, pStrTownOrig, pStrTownDest);
        }

        public Routes GetRoute(long pLonCode)
        {
            return mObjRouteListDAO.GetRoute(pLonCode);
        }

        public bool CheckTown(string pStrTown)
        {
            return mObjRouteListDAO.CheckTown(pStrTown);
        }

        public InsuranceDTO GetInsuranceLine(int pIntUserSig)
        {
            return mObjRouteListDAO.GetInsuranceObject(pIntUserSig);
        }

        public bool CheckTransportsItem(string pStrItemCode, int pIntUserSign)
        {
            return mObjRouteListDAO.CheckTransportsItem(pStrItemCode, pIntUserSign);
        }

        public bool CheckInsuranceLine(string pStrItemCode)
        {
            return mObjRouteListDAO.GetInsuranceItem() == pStrItemCode ? true : false;
        }

        public bool CheckIfShared(string pStrShared)
        {
            return mObjRouteListDAO.CheckFolio(pStrShared);
        }

        public string GetFolio(string pStrFolio)
        {
            int lIntFolio = mObjRouteListDAO.GetFolio(pStrFolio);

            return lIntFolio == 0 ? "20000000" : lIntFolio.ToString() == pStrFolio ? lIntFolio.ToString()
                : string.IsNullOrEmpty(pStrFolio) && lIntFolio != 0 ? (lIntFolio + 1).ToString() : "0";
        }

        public string GetStatus(string pStrFolio)
        {
            return mObjRouteListDAO.GetInternalStatus(pStrFolio);
        }

        public string GetKeyByFolio(string pStrFolio)
        {
            return mObjRouteListDAO.GetKey(pStrFolio);
        }

        public int GetLastFolio()
        {
            int lIntLastFolio = mObjRouteListDAO.GetLastFolio();
            return lIntLastFolio != 0 ? lIntLastFolio : 20000000;
        }

        public int GetFirstFolio()
        {
            int lIntFirstFolio = mObjRouteListDAO.GetFirstFolio();

            return lIntFirstFolio != 0 ? lIntFirstFolio : 20000000;
        }

        public bool FolioExists(string pStrFolio)
        {
            int lIntFolio = mObjRouteListDAO.GetFolio(pStrFolio);

            return lIntFolio == 0 ? false : true;
        }

        public SAPbobsCOM.UserTable GetFreight()
        {
            return mObjRouteListDAO.GetFreight();
        }

        public List<CostingCodesDTO> GetCostingCodes()
        {
            return mObjRouteListDAO.GetCostingCodeList();
        }

        public string GetCostingCode(int pIntUserSign)
        {
            return mObjRouteListDAO.GetCostingCode(pIntUserSign);
        }

        public float CheckIfRetention(string pStrItemCode)
        {
            return mObjRouteListDAO.GetRetention(pStrItemCode);
        }

        public int GetNextRouteId()
        {
            return mObjRouteListDAO.GetLastRouteId() + 1;
        }

        public float GetTax(string pStrItemCode)
        {
            return mObjRouteListDAO.GetTax(pStrItemCode);
        }

        public float GetTaxWT(string pStrItemCode)
        {
            return mObjRouteListDAO.GetTaxWT(pStrItemCode);
        }

        public string GetTypeTRTY(string pStrCode)
        {
            return mObjRouteListDAO.GetTypePayload(pStrCode);
        }
    }
}
