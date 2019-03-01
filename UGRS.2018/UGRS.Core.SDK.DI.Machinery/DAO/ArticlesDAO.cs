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
    public class ArticlesDAO
    {
        public List<ArticlesDTO> GetArticlesByEquipmentType(string pStrEquipmentTypeCode, string pStrContractTypeCode)
        {
            List<ArticlesDTO> lLstArticlesDTO = new List<ArticlesDTO>();
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                Dictionary<string, string> lLstStrParameters = new Dictionary<string, string>();
                lLstStrParameters.Add("EquipmentTypeCode", pStrEquipmentTypeCode);
                lLstStrParameters.Add("ContractTypeCode", pStrContractTypeCode);

                string lStrQuery = this.GetSQL("GetArticlesByEquipmentType").Inject(lLstStrParameters);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    for (int i = 0; i < lObjRecordset.RecordCount; i++)
                    {
                        ArticlesDTO lObjArticles = new ArticlesDTO
                        {
                            Code = int.Parse(lObjRecordset.Fields.Item("Code").Value.ToString()),
                            ArticleCode = lObjRecordset.Fields.Item("ItemCode").Value.ToString(),
                            Name = lObjRecordset.Fields.Item("ItemName").Value.ToString(),
                            EquipmentTypeCode = lObjRecordset.Fields.Item("TypeEquipId").Value.ToString(),
                            ContractTypeCode = lObjRecordset.Fields.Item("TypeContractId").Value.ToString(),
                            UseDrilling = lObjRecordset.Fields.Item("U_UsePerfora").Value.ToString() == "N" ? false : true,
                        };

                        lLstArticlesDTO.Add(lObjArticles);
                        lObjRecordset.MoveNext();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ArticlesDAO - GetArticlesByEquipmentType: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lLstArticlesDTO;
        }

        public double GetArticlePriceByCode(string pStrArticleCode)
        {
            double lDblArticlePrice = 0;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("GetArticlePrice").InjectSingleValue("ArticleCode", pStrArticleCode);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lDblArticlePrice = double.Parse(lObjRecordset.Fields.Item("Price").Value.ToString());
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ArticlesDAO - GetArticlePriceByCode: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lDblArticlePrice;
        }

        public bool UseDrilling(string pStrArticleCode)
        {
            bool lBolUseDrilling = false;
            Recordset lObjRecordset = null;

            try
            {
                lObjRecordset = (Recordset)DIApplication.Company.GetBusinessObject(BoObjectTypes.BoRecordset);

                string lStrQuery = this.GetSQL("UseDrilling").InjectSingleValue("Code", pStrArticleCode);

                lObjRecordset.DoQuery(lStrQuery);

                if (lObjRecordset.RecordCount > 0)
                {
                    lBolUseDrilling = lObjRecordset.Fields.Item("U_UsePerfora").Value.ToString() == "N" ? false : true;
                }
            }
            catch (Exception lObjException)
            {
                LogService.WriteError(string.Format("[ArticlesDAO - UseDrilling: {0}]", lObjException.Message));
                throw new DAOException(lObjException.Message, lObjException);
            }
            finally
            {
                MemoryUtility.ReleaseComObject(lObjRecordset);
            }
            return lBolUseDrilling;
        }
    }
}
