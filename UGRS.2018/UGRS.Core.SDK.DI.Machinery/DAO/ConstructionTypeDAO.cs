using SAPbobsCOM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Extension;
using UGRS.Core.Extension;
using UGRS.Core.Exceptions;
using UGRS.Core.Utility;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class ConstructionTypeDAO
    {
        public List<ConstructionTypeDTO> GetConstructionTypes()
        {
            List<ConstructionTypeDTO> lLstConstructionTypeDTO = new List<ConstructionTypeDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetConstructionType");

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ConstructionTypeDTO lObjConstructionTypeDTO = new ConstructionTypeDTO
                        {
                            Code = int.Parse(lObjRecordset.Fields.Item("Code").Value.ToString()),
                            Name = lObjRecordset.Fields.Item("Name").Value.ToString(),
                        };

                        lLstConstructionTypeDTO.Add(lObjConstructionTypeDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ConstructionTypeDAO - GetConstructionTypes: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstConstructionTypeDTO;
        }
    }
}
