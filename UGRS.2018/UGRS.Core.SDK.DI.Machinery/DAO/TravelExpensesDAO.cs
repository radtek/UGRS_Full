using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Exceptions;
using UGRS.Core.Utility;
using UGRS.Core.Extension;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class TravelExpensesDAO
    {
        public string GetNexFolio(string pStrCostCenter)
        {
            string lStrFolio = string.Empty;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetNextTEFolio").InjectSingleValue("CostCenter", pStrCostCenter);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrFolio = lObjRecordset.Fields.Item("Folio").Value.ToString();
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[TravelExpensesDAO - GetNexFolio: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lStrFolio;
        }

        public string GetCurrentFolio(string pStrRiseId)
        {
            string lStrFolio = string.Empty;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetCurrentTEFolio").InjectSingleValue("RiseId", pStrRiseId);
                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lStrFolio = lObjRecordset.Fields.Item("Folio").Value.ToString();
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[GetCurrentFolio - GetNexFolio: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lStrFolio;
        }

        public TravelExpensesDTO GetPayment(int pIntDocEntry)
        {
            TravelExpensesDTO lObjTravelExpensesDTO = null;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetPaymentByDocEntry").InjectSingleValue("DocEntry", pIntDocEntry);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lObjTravelExpensesDTO = new TravelExpensesDTO
                    {
                        DocDate = DateTime.Parse(lObjRecordset.Fields.Item("DocDate").Value.ToString()),
                        DocEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString()),
                        DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                        Folio = lObjRecordset.Fields.Item("U_GLO_CodeMov").Value.ToString(),
                        Total = double.Parse(lObjRecordset.Fields.Item("Total").Value.ToString()),
                        Status = int.Parse(lObjRecordset.Fields.Item("Status").Value.ToString()),
                    };
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[GetCurrentFolio - GetPayment: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lObjTravelExpensesDTO;
        }

        public List<TravelExpensesDTO> GetDraftPaymentsByRiseId(int pIntRiseId, string pStrObjType)
        {
            List<TravelExpensesDTO> lLstTravelExpensesDTO = new List<TravelExpensesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RiseId", pIntRiseId.ToString());
                lLstStrParameters.Add("ObjType", pStrObjType);

                string lStrQuery = this.GetSQL("GetDraftPaymentsByRiseId").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        TravelExpensesDTO lObjTravelExpensesDTO = new TravelExpensesDTO
                        {
                            DocDate = DateTime.Parse(lObjRecordset.Fields.Item("DocDate").Value.ToString()),
                            DocEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString()),
                            DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                            Folio = lObjRecordset.Fields.Item("U_GLO_CodeMov").Value.ToString(),
                            Total = double.Parse(lObjRecordset.Fields.Item("Total").Value.ToString()),
                            IdRise = int.Parse(lObjRecordset.Fields.Item("RiseId").Value.ToString()),
                            Status = 3, //int.Parse(lObjRecordset.Fields.Item("Status").Value.ToString()),
                        };

                        lLstTravelExpensesDTO.Add(lObjTravelExpensesDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[GetCurrentFolio - GetDraftPaymentsByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstTravelExpensesDTO;
        }

        public List<TravelExpensesDTO> GetPaymentsByRiseId(int pIntRiseId, string pStrObjType)
        {
            List<TravelExpensesDTO> lLstTravelExpensesDTO = new List<TravelExpensesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("RiseId", pIntRiseId.ToString());
                lLstStrParameters.Add("ObjType", pStrObjType);

                string lStrQuery = this.GetSQL("GetPaymentsByRiseId").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        TravelExpensesDTO lObjTravelExpensesDTO = new TravelExpensesDTO
                        {
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString(),
                            DocDate = DateTime.Parse(lObjRecordset.Fields.Item("DocDate").Value.ToString()),
                            DocEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString()),
                            DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                            Folio = lObjRecordset.Fields.Item("U_GLO_CodeMov").Value.ToString(),
                            Total = double.Parse(lObjRecordset.Fields.Item("Total").Value.ToString()),
                            IdRise = int.Parse(lObjRecordset.Fields.Item("RiseId").Value.ToString()),
                            Status = int.Parse(lObjRecordset.Fields.Item("Status").Value.ToString()),
                        };

                        lLstTravelExpensesDTO.Add(lObjTravelExpensesDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[GetCurrentFolio - GetPaymentsByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstTravelExpensesDTO;
        }

        public List<TravelExpensesDTO> GetTravelExpensesByRiseId(int pIntRiseId, bool pBolForDraftPymts = false)
        {
            List<TravelExpensesDTO> lLstTravelExpensesDTO = new List<TravelExpensesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = string.Empty;

                if (pBolForDraftPymts)
                    lStrQuery = this.GetSQL("GetDraftTravelExpensesByRiseId").InjectSingleValue("RiseId", pIntRiseId.ToString());
                else
                    lStrQuery = this.GetSQL("GetTravelExpensesByRiseId").InjectSingleValue("RiseId", pIntRiseId.ToString());

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        TravelExpensesDTO lObjTravelExpensesDTO = new TravelExpensesDTO();
                        lObjTravelExpensesDTO.DocDate = DateTime.Parse(lObjRecordset.Fields.Item("DocDate").Value.ToString());
                        lObjTravelExpensesDTO.DocEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString());
                        lObjTravelExpensesDTO.DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString());
                        lObjTravelExpensesDTO.Folio = lObjRecordset.Fields.Item("U_GLO_CodeMov").Value.ToString();
                        lObjTravelExpensesDTO.Total = double.Parse(lObjRecordset.Fields.Item("Total").Value.ToString());
                        lObjTravelExpensesDTO.IdRise = int.Parse(lObjRecordset.Fields.Item("RiseId").Value.ToString());

                        if (pBolForDraftPymts)
                        {
                            lObjTravelExpensesDTO.Status = 3;
                            lObjTravelExpensesDTO.Code = lObjRecordset.Fields.Item("Code").Value.ToString();
                        }
                        else
                        {
                            lObjTravelExpensesDTO.Status = int.Parse(lObjRecordset.Fields.Item("Status").Value.ToString());
                            lObjTravelExpensesDTO.Code = lObjRecordset.Fields.Item("Code").Value.ToString();
                        }

                        lLstTravelExpensesDTO.Add(lObjTravelExpensesDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[GetCurrentFolio - GetTravelExpensesByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstTravelExpensesDTO;
        }
    }
}
