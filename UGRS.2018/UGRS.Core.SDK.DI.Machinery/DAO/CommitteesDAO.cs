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
    public class CommitteesDAO
    {
        public List<CommitteesDTO> GetCommitteesByCode(int pIntMunicipalityCode)
        {
            List<CommitteesDTO> lLstCommitteesDTO = new List<CommitteesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetCommitteesByCode").InjectSingleValue("MunicipalityCode", pIntMunicipalityCode);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        CommitteesDTO lObjCommitteesDTO = new CommitteesDTO
                        {
                            Code = int.Parse(lObjRecordset.Fields.Item("Code").Value.ToString()),
                            Name = lObjRecordset.Fields.Item("Name").Value.ToString(),
                        };

                        lLstCommitteesDTO.Add(lObjCommitteesDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[CommitteesDAO - GetCommitteesByCode: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstCommitteesDTO;
        }
    }
}
