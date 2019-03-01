using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Finances.DTO;

namespace UGRS.Core.SDK.DI.Finances.DAO
{
    public class AuctionDAO
    {
        public AuctionDTO GetLastAuction()
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string lStrQuery = this.GetSQL("GetLastAuction");
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount == 0)
                {
                    return null;
                }
                AuctionDTO lObjAuction = new AuctionDTO();
                lObjAuction.Id = lObjResults.GetColumnValue<int>("U_Id");
                lObjAuction.Folio = lObjResults.GetColumnValue<string>("U_Folio");
                lObjAuction.LocationId = lObjResults.GetColumnValue<int>("U_LocationId");
                lObjAuction.Location = lObjResults.GetColumnValue<string>("U_Location");
                lObjAuction.Type = lObjResults.GetColumnValue<string>("U_Type");
                lObjAuction.TypeId = lObjResults.GetColumnValue<int>("U_TypeId");
                lObjAuction.Commission = lObjResults.GetColumnValue<double>("U_Commission");
                lObjAuction.Date = lObjResults.GetColumnValue<DateTime>("U_Date");
                lObjAuction.AuthCorral = lObjResults.GetColumnValue<string>("U_AutCorral");
                lObjAuction.AuthTransport = lObjResults.GetColumnValue<string>("U_AutTransp");
                lObjAuction.AuthTransport = lObjResults.GetColumnValue<string>("U_AutAuction");
                lObjAuction.AuthCyC = lObjResults.GetColumnValue<string>("U_AutCyC");
                lObjAuction.AuthFinances = lObjResults.GetColumnValue<string>("U_AutFz");
                return lObjAuction;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[AuctionDAO - GetLastAuction] Error al obtener la ultima subasta: {0}", e.Message));
                throw new Exception(string.Format("Error al obtener la última subasta: {0}", e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public IList<AuctionSellerDTO> GetAuctionSellers(string pFolio, int pintUserSign)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            IList<AuctionSellerDTO> lLstObjSellers = new List<AuctionSellerDTO>();
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Folio", pFolio);
                lLstStrParameters.Add("CostingCode", GetCostingCode(pintUserSign));

                string lStrQuery = this.GetSQL("GetAuctionSellers").Inject(lLstStrParameters);
                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount > 0)
                {
                    for (int i = 0; i < lObjResults.RecordCount; i++)
                    {
                        AuctionSellerDTO lObjAuctionSeller = new AuctionSellerDTO();
                        lObjAuctionSeller.Amount = lObjResults.GetColumnValue<double>("Amount");
                        lObjAuctionSeller.CardCode = lObjResults.GetColumnValue<string>("CardCode");
                        lObjAuctionSeller.CardName = lObjResults.GetColumnValue<string>("CardName");
                        lLstObjSellers.Add(lObjAuctionSeller);
                        lObjResults.MoveNext();
                    }
                }
                return lLstObjSellers;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[AuctionDAO - GetAuctionSellers] Error al obtener las subastas: {0}", e.Message));
                throw new Exception(string.Format("Error al obtener las subastas: {0}", e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        private string GetCostingCode(int pintUserSign)
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            string lStrCostingCode = string.Empty;
            try
            {
                string lStrQuery = this.GetSQL("GetCostingCodeBySing").InjectSingleValue("UsrId", pintUserSign.ToString());

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrCostingCode = lObjRecordset.Fields.Item("U_GLO_CostCenter").Value.ToString();
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[AuctionDAO - GetCostingCode] Error al obtener el centro de costos: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al obtener el centro de costos: {0}", lObjException.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lStrCostingCode;   
        }

        public void AutorizeAuction(string pFolio,char pCharAction)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Folio", pFolio);
                lLstStrParameters.Add("Action", pCharAction.ToString());

                string lStrQuery = this.GetSQL("AuthorizeAuction").Inject(lLstStrParameters);

                //string lStrQuery = this.GetSQL("AuthorizeAuction").InjectSingleValue("Folio", pFolio);

                lObjResults.DoQuery(lStrQuery);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[AuctionDAO - AutorizeAuction] Error al autorizar la subasta con el folio {0}: {1}", pFolio, lObjException.Message));
                throw new Exception(string.Format("Error al autorizar la subasta: {0}", lObjException.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public void EnableAuctionForCC(string pFolio)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string lStrQuery = this.GetSQL("UnAuthorizeAuction").InjectSingleValue("Folio", pFolio);

                lObjResults.DoQuery(lStrQuery);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[AuctionDAO - EnableAuctionForCC] Error al desautorizar la subasta con el folio {0}: {1}", pFolio, lObjException.Message));
                throw new Exception(string.Format("Error al desautorizar la subasta: {0}", lObjException.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public bool HasRole(int pUserId, string pRoleName)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string lStrQuery = this.GetSQL("GetRole");
                Dictionary<string, string> lObjParameters = new Dictionary<string, string>();
                lObjParameters.Add("UserId", pUserId.ToString());
                lObjParameters.Add("RoleName", pRoleName);
                lStrQuery = lStrQuery.Inject(lObjParameters);
                lObjResults.DoQuery(lStrQuery);
                return lObjResults.RecordCount > 0;
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(string.Format("[AuctionDAO - HasRole] Error al verificar el rol para el usuario {0} con el rol {1}: {2}", pUserId, pRoleName, lObjException.Message));
                throw new Exception(string.Format("Error al verificar el rol del usuario: {0}", lObjException.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }

        public List<string> GetLastAuctions()
        {
            SAPbobsCOM.Recordset lObjRecordset = null;
            List<string> lLstAuctions = new List<string>();
            try
            {
                string lStrQuery = this.GetSQL("GetLastAuction");

                lObjRecordset = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {

                        string lStrFolio = lObjRecordset.Fields.Item("U_Folio").Value.ToString();
                        lLstAuctions.Add(lStrFolio);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(string.Format("[AuctionDAO - GetLastAuctions] Error al obtener la ultima subasta: {0}", ex.Message));
                throw new Exception(string.Format("Error al obtener la última subasta: {0}", ex.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstAuctions;
        }

        public AuctionDTO GetAuctionByFolio(string pStrFolio)
        {
            SAPbobsCOM.Recordset lObjResults = (SAPbobsCOM.Recordset)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            try
            {
                string lStrQuery = this.GetSQL("GetAuctionByFolio").InjectSingleValue("Folio",pStrFolio);

                lObjResults.DoQuery(lStrQuery);
                if (lObjResults.RecordCount == 0)
                {
                    return null;
                }
                AuctionDTO lObjAuction = new AuctionDTO();
                lObjAuction.Id = Convert.ToInt32(lObjResults.Fields.Item("U_Id").Value.ToString());
                lObjAuction.Folio = lObjResults.Fields.Item("U_Folio").Value.ToString();
                lObjAuction.LocationId = Convert.ToInt32(lObjResults.Fields.Item("U_LocationId").Value.ToString());
                lObjAuction.Location = lObjResults.Fields.Item("U_Location").Value.ToString();
                lObjAuction.Type = lObjResults.Fields.Item("U_Type").Value.ToString();
                lObjAuction.TypeId = Convert.ToInt32(lObjResults.Fields.Item("U_TypeId").Value.ToString());
                lObjAuction.Commission = Convert.ToDouble(lObjResults.Fields.Item("U_Commission").Value.ToString());
                lObjAuction.Date = Convert.ToDateTime(lObjResults.Fields.Item("U_Date").Value.ToString());
                lObjAuction.AuthCorral = lObjResults.Fields.Item("U_AutCorral").Value.ToString();
                lObjAuction.AuthTransport = lObjResults.Fields.Item("U_AutTransp").Value.ToString();
                lObjAuction.AuthTransport = lObjResults.Fields.Item("U_AutAuction").Value.ToString();
                lObjAuction.AuthCyC = lObjResults.Fields.Item("U_AutCyC").Value.ToString();
                lObjAuction.AuthFinances = lObjResults.Fields.Item("U_AutFz").Value.ToString();
                return lObjAuction;
            }
            catch (Exception e)
            {
                LogUtility.WriteError(string.Format("[AuctionDAO - GetAuctionByFolio] Error al obtener la subasta con el folio {0}: {1}", pStrFolio, e.Message));
                throw new Exception(string.Format("Error al obtener la subasta con el folio {0}: {1}", pStrFolio, e.Message));
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjResults);
            }
        }
    }
}
