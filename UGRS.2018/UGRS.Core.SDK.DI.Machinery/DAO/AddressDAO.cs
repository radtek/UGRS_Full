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
using SAPbobsCOM;
using UGRS.Core.Services;

namespace UGRS.Core.SDK.DI.Machinery.DAO
{
    public class AddressDAO
    {
        public List<DestinationAddressDTO> GetDestinationAddressClient()
        {
            List<DestinationAddressDTO> lLstDestinationAddressDTO = new List<DestinationAddressDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetDestinationAddress");

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        DestinationAddressDTO lObjDestinationAddressDTO = new DestinationAddressDTO
                        {
                            CardCode = lObjRecordset.Fields.Item("CardCode").Value.ToString(),
                            Address = lObjRecordset.Fields.Item("Address").Value.ToString(),
                            Street = lObjRecordset.Fields.Item("Street").Value.ToString(),
                        };

                        lLstDestinationAddressDTO.Add(lObjDestinationAddressDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[AddressDAO - GetDestinationAddressClient: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstDestinationAddressDTO;
        }
    }
}
