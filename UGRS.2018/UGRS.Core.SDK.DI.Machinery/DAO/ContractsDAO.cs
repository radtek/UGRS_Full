using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.Exceptions;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class ContractsDAO
    {
        public List<ContractsDTO> GetContracts()
        {
            List<ContractsDTO> lLstContractsDTO = new List<ContractsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetSalesOrders");

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ContractsDTO lObjContractsDTO = new ContractsDTO
                        {
                            DocEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString()),
                            DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                            DocDate = DateTime.Parse(lObjRecordset.Fields.Item("DocDate").Value.ToString()),
                            Status = lObjRecordset.Fields.Item("Status").Value.ToString(),
                            Type = int.Parse(lObjRecordset.Fields.Item("U_MQ_TipoCont").Value.ToString()),
                            Import = double.Parse(lObjRecordset.Fields.Item("Total").Value.ToString()),
                            CardName = lObjRecordset.Fields.Item("CardName").Value.ToString(),
                            MunicipalityCode = lObjRecordset.Fields.Item("MunicipalityId").Value.ToString(),
                            Municipality = lObjRecordset.Fields.Item("Municipality").Value.ToString(),
                        };

                        lLstContractsDTO.Add(lObjContractsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ContractsDAO - GetContracts: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstContractsDTO;
        }

        public List<ContractsDTO> GetContractsByRiseId(int pIntRiseId)
        {
            List<ContractsDTO> lLstContractsDTO = new List<ContractsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetContractsByRiseId").InjectSingleValue("RiseId", pIntRiseId);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ContractsDTO lObjContractsDTO = new ContractsDTO
                        {
                            Code = lObjRecordset.Fields.Item("Code").Value.ToString(),
                            DocEntry = int.Parse(lObjRecordset.Fields.Item("U_DocEntry").Value.ToString()),
                            DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                            RiseId = int.Parse(lObjRecordset.Fields.Item("U_IdRise").Value.ToString()),
                            DocDate = DateTime.Parse(lObjRecordset.Fields.Item("DocDate").Value.ToString()),
                            Status = lObjRecordset.Fields.Item("Status").Value.ToString(),
                            Type = int.Parse(lObjRecordset.Fields.Item("U_MQ_TipoCont").Value.ToString()),
                            Import = double.Parse(lObjRecordset.Fields.Item("Total").Value.ToString()),
                            CardName = lObjRecordset.Fields.Item("CardName").Value.ToString(),
                            MunicipalityCode = lObjRecordset.Fields.Item("MunicipalityId").Value.ToString(),
                            Municipality = lObjRecordset.Fields.Item("Municipality").Value.ToString(),
                        };

                        lLstContractsDTO.Add(lObjContractsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ContractsDAO - GetContractsByRiseId: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstContractsDTO;
        }

        public ContractsDTO GetContract(int pIntDocEntry)
        {
            ContractsDTO lObjContractDTO = null;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetSalesOrdersByDocEntry").InjectSingleValue("DocEntry", pIntDocEntry);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        lObjContractDTO = new ContractsDTO
                        {
                            DocEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString()),
                            DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                            DocDate = DateTime.Parse(lObjRecordset.Fields.Item("DocDate").Value.ToString()),
                            Status = lObjRecordset.Fields.Item("Status").Value.ToString(),
                            Type = int.Parse(lObjRecordset.Fields.Item("U_MQ_TipoCont").Value.ToString()),
                            Import = double.Parse(lObjRecordset.Fields.Item("Total").Value.ToString()),
                            CardName = lObjRecordset.Fields.Item("CardName").Value.ToString(),
                            MunicipalityCode = lObjRecordset.Fields.Item("MunicipalityId").Value.ToString(),
                            Municipality = lObjRecordset.Fields.Item("Municipality").Value.ToString(),
                        };

                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ContractsDAO - GetContract: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lObjContractDTO;
        }

        public List<ContractsTypesDTO> GetContractsTypes()
        {
            List<ContractsTypesDTO> lLstContractsTypesDTO = new List<ContractsTypesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetContractsType");

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ContractsTypesDTO lObjContractTypeDTO = new ContractsTypesDTO
                        {
                            Code = int.Parse(lObjRecordset.Fields.Item("FldValue").Value.ToString()),
                            Description = lObjRecordset.Fields.Item("Descr").Value.ToString(),
                        };

                        lLstContractsTypesDTO.Add(lObjContractTypeDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ContractsDAO - GetContractsTypes: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstContractsTypesDTO;
        }

        public int GetLastCode(int pIntRiseId, int pIntDocEntry)
        {
            int lIntCode = 0;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("IdRise", pIntRiseId.ToString());
                lLstStrParameters.Add("DocEntry", pIntDocEntry.ToString());

                string lStrQuery = this.GetSQL("GetContractCode").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lIntCode = int.Parse(lObjRecordset.Fields.Item("Code").Value.ToString());
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ContractsDAO - GetLastCode: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lIntCode;
        }

        public bool IsClosed(int pIntDocEntry)
        {
            bool lBolResult = false;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("ContractDocEntry", pIntDocEntry.ToString());

                string lStrQuery = this.GetSQL("IsContractClosed").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lBolResult = lObjRecordset.Fields.Item("Status").Value.ToString() == "0" ? false : true;
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ContractsDAO - IsClosed]: {0}", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lBolResult;
        }

        public List<ContractsFiltersDTO> GetContracts(string pStrContract, string pStrClient, string pStrStatus, string pStrStartDate, string pStrEndDate, string pStrMunicipality)
        {
            List<ContractsFiltersDTO> lLstContractsFiltersDTO = new List<ContractsFiltersDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("Contract", pStrContract);
                lLstStrParameters.Add("Client", pStrClient);
                lLstStrParameters.Add("Status", pStrStatus);
                lLstStrParameters.Add("StartDate", pStrStartDate);
                lLstStrParameters.Add("EndDate", pStrEndDate);
                lLstStrParameters.Add("Municipality", pStrMunicipality);
                
                string lStrQuery = this.GetSQL("SearchRiseContracts").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ContractsFiltersDTO lObjContracts = new ContractsFiltersDTO
                        {
                            DocEntry = int.Parse(lObjRecordset.Fields.Item("DocEntry").Value.ToString()),
                            DocNum = int.Parse(lObjRecordset.Fields.Item("DocNum").Value.ToString()),
                            CardCode = lObjRecordset.Fields.Item("CardCode").Value.ToString(),
                            CardName = lObjRecordset.Fields.Item("CardName").Value.ToString(),
                            HrsFeet = double.Parse(lObjRecordset.Fields.Item("HrsFeet").Value.ToString()),
                            ExtrasInvoices = int.Parse(lObjRecordset.Fields.Item("ExtrasInv").Value.ToString()),
                            Import = double.Parse(lObjRecordset.Fields.Item("ImportInv").Value.ToString()),
                            RealHrs = double.Parse(lObjRecordset.Fields.Item("RealHrs").Value.ToString()),
                            Difference = double.Parse(lObjRecordset.Fields.Item("Difference").Value.ToString()),
                            Status = int.Parse(lObjRecordset.Fields.Item("Close").Value.ToString()),
                            MunicipalityCode = lObjRecordset.Fields.Item("MunicipalityId").Value.ToString(),
                            Municipality = lObjRecordset.Fields.Item("Municipality").Value.ToString(),
                        };

                        lLstContractsFiltersDTO.Add(lObjContracts);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ContractsDAO - GetContracts: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstContractsFiltersDTO;
        }
    }
}
