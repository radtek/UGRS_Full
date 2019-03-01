using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.AddOn.Permissions.Tables;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.DAO;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;
using UGRS.Core.Exceptions;
using UGRS.Core.Utility;
using UGRS.AddOn.Permissions.DTO;
using UGRS.Core.SDK.DI.Permissions.DTO;
using UGRS.Core.SDK.UI;

namespace UGRS.Core.SDK.DI.Permissions.DAO
{
    class EarringRanksDAO
    {
        QueryManager mObjQueryManager;

        public EarringRanksDAO()
        {
            mObjQueryManager = new QueryManager();
        }

        //public IList<EarringRanksT> GetLines(string pStrBaseEntry)
        //{
        //    Recordset lObjRecordSet = null;
        //    IList<EarringRanksT> lLstObjResult = new List<EarringRanksT>();

        //    try
        //    {

        //        lObjRecordSet = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);




        //        string lStrQuery = this.GetSQL("GetLines").InjectSingleValue("BaseEntry",pStrBaseEntry);
        //        lObjRecordSet.DoQuery(lStrQuery);

        //        if (lObjRecordSet.RecordCount > 0)
        //        {

        //            for (int i = 0; i < lObjRecordSet.RecordCount; i++)
        //            {
        //                lLstObjResult.Add(new EarringRanksT()
        //                {
        //                    BaseEntry = lObjRecordSet.Fields.Item("U_BaseEntry").Value.ToString(),
        //                    EarringFrom = lObjRecordSet.Fields.Item("U_EarringFrom").Value.ToString(),
        //                    EarringTo = lObjRecordSet.Fields.Item("U_EarringTo").Value.ToString(),
        //                    RowCode = lObjRecordSet.Fields.Item("Code").Value.ToString()

        //                });
        //                lObjRecordSet.MoveNext();
        //            }




        //        }
        //    }
        //    catch (Exception lObjException)
        //    {

        //        throw new DAOException(lObjException.Message, lObjException);
        //    }
        //    finally
        //    {
        //        MemoryUtility.ReleaseComObject(lObjRecordSet);
        //    }
        //    return lLstObjResult;
        //}

        public string GetLinesQuery(string pStrBaseEntry)
        {
            return this.GetSQL("GetLines").InjectSingleValue("BaseEntry", pStrBaseEntry);
        }
       
        public bool CheckBaseEntry(string pStrBaseEntry)
        {
            Recordset lObjRecordSet = null;
            try
            {

                lObjRecordSet = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("CheckBaseEntry").InjectSingleValue("BaseEntry", pStrBaseEntry);

                lObjRecordSet.DoQuery(lStrQuery);

                if (lObjRecordSet.RecordCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception lObjException)
            {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }

        }

        public bool CheckStoredRanks(string pStrEarringFrom, string pStrEarringTo)
        {
            Recordset lObjRecordSet = null;
            try
            {
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("EarringFrom", pStrEarringFrom);
                lLstStrParameters.Add("EarringTo", pStrEarringTo);

                lObjRecordSet = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("CheckStoredRanks").Inject(lLstStrParameters);

                lObjRecordSet.DoQuery(lStrQuery);

                if (lObjRecordSet.RecordCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception lObjException)
            {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }

        }

        public int GetDocEntry(string pStrDocNum)
        {
            Recordset lObjRecordSet = null;
            try
            {

                lObjRecordSet = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetDocEntry").InjectSingleValue("DocNum", pStrDocNum);

                lObjRecordSet.DoQuery(lStrQuery);

                if (lObjRecordSet.RecordCount > 0)
                {
                    return (int)lObjRecordSet.Fields.Item(0).Value;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception lObjException)
            {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
        }

        public int GetTotalCertHeads(int pIntDocEntry)
        {
            Recordset lObjRecordSet = null;
            try
            {

                lObjRecordSet = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetTotalCertHeads").InjectSingleValue("DocEntry", pIntDocEntry);

                lObjRecordSet.DoQuery(lStrQuery);

                if (lObjRecordSet.RecordCount > 0)
                {
                    return Convert.ToInt32(lObjRecordSet.Fields.Item(0).Value);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception lObjException)
            {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
        }

        public List<InvoiceExpDTO> GetInvoices()
        {
            Recordset lObjRecordSet = null;
            List<InvoiceExpDTO> lLstInvoice = new List<InvoiceExpDTO>();
            try
            {

                lObjRecordSet = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetInvoiceForNC");

                lObjRecordSet.DoQuery(lStrQuery);

                if (lObjRecordSet.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordSet.RecordCount; i++)
                    {
                         InvoiceExpDTO lObjInvoiceDTO = new InvoiceExpDTO();
                        lObjInvoiceDTO.DocEntry = lObjRecordSet.Fields.Item("DocEntry").Value.ToString();
                        lObjInvoiceDTO.BussinesPartner = lObjRecordSet.Fields.Item("CardName").Value.ToString();
                        lObjInvoiceDTO.Certificate = lObjRecordSet.Fields.Item("Cert").Value.ToString();
                        lObjInvoiceDTO.DocNum = lObjRecordSet.Fields.Item("DocNum").Value.ToString();
                        lObjInvoiceDTO.HeadInvoice = Convert.ToInt32(lObjRecordSet.Fields.Item("HeadsInv").Value.ToString());
                        lObjInvoiceDTO.HeatExport = Convert.ToInt32(lObjRecordSet.Fields.Item("HeadsExp").Value.ToString());
                        lObjInvoiceDTO.HeadNoCruz = Convert.ToInt32(lObjRecordSet.Fields.Item("HeadsNo").Value.ToString());
                        lObjInvoiceDTO.Amount = Convert.ToDouble(lObjRecordSet.Fields.Item("DocTotal").Value.ToString());
                        lObjInvoiceDTO.PaidToDate = Convert.ToDouble(lObjRecordSet.Fields.Item("PaidToDate").Value.ToString());
                        lLstInvoice.Add(lObjInvoiceDTO);
                        lObjRecordSet.MoveNext();
                    }
                }  
            }
            catch (Exception lObjException)
            {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
              
            }
            return lLstInvoice;
        }

        public List<string> GetCertificates(string pStrCert)
        {
            Recordset lObjRecordSet = null;
            List<string> lLstCertificates = new List<string>();
            try
            {
                lObjRecordSet = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);
                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Cert", pStrCert);
                string lStrQuery = this.GetSQL("GetCertificates").Inject(lLstStrParameters);

                lObjRecordSet.DoQuery(lStrQuery);

                if (lObjRecordSet.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordSet.RecordCount; i++)
                    {
                        lLstCertificates.Add(lObjRecordSet.Fields.Item("Code").Value.ToString());
                        lObjRecordSet.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordSet);

            }
            return lLstCertificates;
        }

        public string GetPrefix()
        {
            Recordset lObjRecordSet = null;
            string lStrPrefix = string.Empty;
            try
            {

                lObjRecordSet = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetPrefix");

                lObjRecordSet.DoQuery(lStrQuery);

                if (lObjRecordSet.RecordCount > 0)
                {
                    lStrPrefix = lObjRecordSet.Fields.Item("U_Prefix").Value.ToString();
                }
                else
                {
                    UIApplication.ShowMessageBox("No se encontro ningun prefijo activo registrado");
                }
            }
            catch (Exception lObjException)
            {
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordSet);
            }
            return lStrPrefix;
        }

    }
}
