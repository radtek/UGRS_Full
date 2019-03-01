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
    public class SectionsDAO
    {
        public List<SectionsDTO> GetSections(int pIntMunicipalityCode)
        {
            List<SectionsDTO> lLstSectionsDTO = new List<SectionsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetSectionsByMun").InjectSingleValue("MunicipalityCode", pIntMunicipalityCode);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        SectionsDTO lObjSectionsDTO = new SectionsDTO
                        {
                            Code = int.Parse(lObjRecordset.Fields.Item("Code").Value.ToString()),
                            Name = lObjRecordset.Fields.Item("Name").Value.ToString(),
                            MunicipalityCode = int.Parse(lObjRecordset.Fields.Item("U_Town").Value.ToString()),
                            MunicipalityName = lObjRecordset.Fields.Item("TownName").Value.ToString(),
                            Kilometers = double.Parse(lObjRecordset.Fields.Item("U_Kilometros").Value.ToString()),
                        };

                        lLstSectionsDTO.Add(lObjSectionsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[SectionsDAO - GetSections: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstSectionsDTO;
        }

        public List<SectionsDTO> GetSectionsBySalesOrder(int pIntDocEntry)
        {
            List<SectionsDTO> lLstSectionsDTO = new List<SectionsDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetSectionsBySalesOrder").InjectSingleValue("DocEntry", pIntDocEntry);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        SectionsDTO lObjSectionsDTO = new SectionsDTO
                        {
                            Code = int.Parse(lObjRecordset.Fields.Item("Code").Value.ToString()),
                            Name = lObjRecordset.Fields.Item("Name").Value.ToString(),
                            MunicipalityCode = int.Parse(lObjRecordset.Fields.Item("U_Town").Value.ToString()),
                            MunicipalityName = lObjRecordset.Fields.Item("TownName").Value.ToString(),
                            Kilometers = double.Parse(lObjRecordset.Fields.Item("U_Kilometros").Value.ToString()),
                        };

                        lLstSectionsDTO.Add(lObjSectionsDTO);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[SectionsDAO - GetSectionsBySalesOrder: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstSectionsDTO;
        }
    }
}
