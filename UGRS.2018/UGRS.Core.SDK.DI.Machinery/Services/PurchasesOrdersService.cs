using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery.Tables;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class PurchasesOrdersService
    {
        private TableDAO<PurchaseOrders> mObjPurchaseOrdersTableDAO;

        public PurchasesOrdersService()
        {
            mObjPurchaseOrdersTableDAO = new TableDAO<PurchaseOrders>();
        }

        #region Entities
        public int Add(PurchaseOrders pObjPurchaseOrders)
        {
            try
            {
                return mObjPurchaseOrdersTableDAO.Add(pObjPurchaseOrders);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[PurchasesOrdersService - Add]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(PurchaseOrders pObjPurchaseOrders)
        {
            try
            {
                return mObjPurchaseOrdersTableDAO.Update(pObjPurchaseOrders);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[PurchasesOrdersService - Update]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetLastCode()
        {
            try
            {
                return new QueryManager().Max<string>("Code", string.Format("[@{0}]", mObjPurchaseOrdersTableDAO.GetUserTableName()));
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[PurchasesOrdersService - GetLastCode]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public List<ConsumablesDocumentsDTO> GetByRiseId(int pIntRiseId)
        {
            try
            {
                List<PurchaseOrders> lLstPurchaseOrders = new QueryManager().GetObjectsList<PurchaseOrders>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjPurchaseOrdersTableDAO.GetUserTableName())).ToList();

                return ToConsumablesDocDTO(lLstPurchaseOrders);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[PurchasesOrdersService - GetByRiseId]: {0}", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO

        #endregion

        #region Extras
        public List<ConsumablesDocumentsDTO> ToConsumablesDocDTO(List<PurchaseOrders> pLstPurchaseOrders)
        {
            return pLstPurchaseOrders.Select(x => new ConsumablesDocumentsDTO
            {
                Code = x.RowCode,
                DocType = x.Type,
                IdRise = x.IdRise,
                ActivoCode = x.PrcCode,
                EcoNum = x.EcoNum,
                DieselM = x.DieselM,
                DieselT = x.DieselT,
                Gas = x.Gas,
                F15W40 = x.F15W40,
                Hidraulic = x.Hidraulic,
                SAE40 = x.SAE40,
                Transmition = x.Transmition,
                Oils = x.Oils,
                KmHr = 0,
            }).ToList();
        }
        #endregion
    }
}
