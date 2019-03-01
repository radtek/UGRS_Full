using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Exceptions;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Machinery.Tables;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class RiseService
    {
        private TableDAO<Rise> mObjRiseTableDAO;
        private RiseDAO mObjRiseDAO;

        public RiseService()
        {
            mObjRiseTableDAO = new TableDAO<Rise>();
            mObjRiseDAO = new RiseDAO();
        }

        #region Entities
        public int Add(Rise pObjRise)
        {
            try
            {
                return mObjRiseTableDAO.Add(pObjRise);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseService - Add]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public int Update(Rise pObjRise)
        {
            try
            {
                return mObjRiseTableDAO.Update(pObjRise);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseService - Update]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public string GetCode(int pIntRiseId)
        {
            try
            {
                return new QueryManager().GetValue("Code", "U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjRiseTableDAO.GetUserTableName()).ToString());
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseService - GetCode]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public RiseDTO GetByRiseId(int pIntRiseId)
        {
            try
            {
                List<Rise> lLstRise = new QueryManager().GetObjectsList<Rise>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjRiseTableDAO.GetUserTableName()).ToString()).ToList();

                return ConvetToDTO(lLstRise).FirstOrDefault();
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseService - GetByRiseId]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public int GetOriginalFolio(int pIntRiseId)
        {
            try
            {
                Rise lObjRise = new QueryManager().GetObjectsList<Rise>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjRiseTableDAO.GetUserTableName()).ToString()).ToList().FirstOrDefault();

                if (lObjRise == null)
                {
                    throw new Exception(string.Format("No existe una subida con el folio {0}", pIntRiseId));
                }

                return lObjRise.OriginalFolio;
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseService - GetOriginalFolio]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public void MarkRiseAsCommissioned(int pIntRiseId)
        {
            try
            {
                Rise lObjRise = new QueryManager().GetObjectsList<Rise>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjRiseTableDAO.GetUserTableName()).ToString()).ToList().FirstOrDefault();

                if (lObjRise == null)
                {
                    throw new Exception(string.Format("No existe una subida con el folio {0}", pIntRiseId));
                }

                lObjRise.HasCommission = "Y";
                int lIntResult = Update(lObjRise);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseService - MarkRiseAsCommissioned]: {0}", lObjException.Message));
                throw lObjException;
            }
        }

        public void MarkRiseAsStockTransfer(int pIntRiseId)
        {
            try
            {
                Rise lObjRise = new QueryManager().GetObjectsList<Rise>("U_IdRise", pIntRiseId.ToString(), string.Format("[@{0}]", mObjRiseTableDAO.GetUserTableName()).ToString()).ToList().FirstOrDefault();

                if (lObjRise == null)
                {
                    throw new Exception(string.Format("No existe una subida con el folio {0}", pIntRiseId));
                }

                lObjRise.HasStockTransfer = "Y";
                int lIntResult = Update(lObjRise);
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[RiseService - MarkRiseAsStockTransfer]: {0}", lObjException.Message));
                throw lObjException;
            }
        }
        #endregion

        #region DAO
        public int GetNexFolioId()
        {
            return mObjRiseDAO.GetNexFolioId();
        }

        public List<RiseDTO> GetRelationsFolios()
        {
            return mObjRiseDAO.GetRiseRelations();
        }

        public string GetRiseFoliosQuery()
        {
            return mObjRiseDAO.GetRiseFoliosQuery();
        }

        public string GetCommissionsRiseQuery()
        {
            return mObjRiseDAO.GetCommissionsRiseQuery();
        }

        public string GetNoStockTransferRisesQuery()
        {
            return mObjRiseDAO.GetNoStockTransferRisesQuery();
        }

        public bool ExistsRise(int pIntFolio)
        {
            return mObjRiseDAO.ExistsRiseByFolio(pIntFolio);
        }

        public RiseDTO GetRiseById(int pIntRiseId)
        {
            return mObjRiseDAO.GetRiseById(pIntRiseId);
        }

        public List<RiseFiltersDTO> GetRisesByContractId(string pStrDocEntry)
        {
            return mObjRiseDAO.GetRisesByContractId(pStrDocEntry);
        }
        #endregion

        #region Extras
        public List<RiseDTO> ConvetToDTO(List<Rise> lLstRise)
        {
            return lLstRise.Select(p => new RiseDTO
            {
                Client = p.Client,
                CreatedDate = p.CreatedDate,
                FolioRelation = p.DocRef,
                DocStatus = (RiseStatusEnum)p.DocStatus,
                IdRise = p.IdRise,
                SupervisorId = p.Supervisor,
                UserId = p.UserId,
            }).ToList();
        }

        public Rise ToEntity(RiseDTO pObjRise)
        {
            if (pObjRise == null)
                return null;

            return new Rise
            {
                RowCode = pObjRise.Code.ToString(),
                IdRise = pObjRise.IdRise,
                CreatedDate = pObjRise.CreatedDate,
                StartDate = pObjRise.StartDate,
                EndDate = pObjRise.EndDate,
                Client = pObjRise.Client,
                Supervisor = pObjRise.SupervisorId,
                DocStatus = (int)pObjRise.DocStatus,
                DocRef = pObjRise.FolioRelation,
                UserId = pObjRise.UserId,
            };
        }
        #endregion
    }
}
