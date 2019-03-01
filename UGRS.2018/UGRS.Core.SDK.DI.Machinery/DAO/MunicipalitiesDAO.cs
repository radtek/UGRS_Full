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
    public class MunicipalitiesDAO
    {
        public List<MunicipalitiesDTO> GetMunicipalities()
        {
            List<MunicipalitiesDTO> lLstMunicipalitiesDTO = new List<MunicipalitiesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetMunicipalities");

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        MunicipalitiesDTO lObjMunicipalitiesDTO = new MunicipalitiesDTO
                        {
                            Code = int.Parse(lObjRecordset.Fields.Item("Code").Value.ToString()),
                            Name = lObjRecordset.Fields.Item("Name").Value.ToString(),
                            CommitteeCode = int.Parse(lObjRecordset.Fields.Item("U_Commite").Value.ToString()),
                        };

                        lLstMunicipalitiesDTO.Add(lObjMunicipalitiesDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[MunicipalitiesDAO - GetMunicipalities: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstMunicipalitiesDTO;
        }
    }
}
