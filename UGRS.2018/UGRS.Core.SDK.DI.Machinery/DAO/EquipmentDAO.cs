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
    public class EquipmentDAO
    {
        public List<EquipmentDTO> GetEquipmentTypesByContractCode(int pIntContractCode)
        {
            List<EquipmentDTO> lLstEquipmentDTO = new List<EquipmentDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetEquipmentTypes").InjectSingleValue("ContractCode", pIntContractCode);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        EquipmentDTO lObjEquipment = new EquipmentDTO
                        {
                            Code = int.Parse(lObjRecordset.Fields.Item("Code").Value.ToString()),
                            TypeId = lObjRecordset.Fields.Item("TypeEquipId").Value.ToString(),
                            Name = lObjRecordset.Fields.Item("TypeEquipName").Value.ToString(),
                            ContractTypeId = int.Parse(lObjRecordset.Fields.Item("TypeContractId").Value.ToString()),
                        };

                        lLstEquipmentDTO.Add(lObjEquipment);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[EquipmentDAO - GetEquipmentTypesByContractCode: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstEquipmentDTO.GroupBy(x => x.TypeId).Select(x => x.First()).ToList();
        }
    }
}
