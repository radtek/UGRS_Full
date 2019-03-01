using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.DAO;
using UGRS.Core.SDK.DI.Machinery.DTO;

namespace UGRS.Core.SDK.DI.Machinery.Services
{
    public class ArticlesService
    {
        private ArticlesDAO lObjArticlesDAO;

        public ArticlesService()
        {
            lObjArticlesDAO = new ArticlesDAO();
        }

        public List<ArticlesDTO> GetArticlesByEquipmentType(string pStrEquipmentTypeCode, string pStrContractTypeCode)
        {
            return lObjArticlesDAO.GetArticlesByEquipmentType(pStrEquipmentTypeCode, pStrContractTypeCode);
        }

        public double GetArticlePriceByCode(string pStrArticleCode)
        {
            return lObjArticlesDAO.GetArticlePriceByCode(pStrArticleCode);
        }

        public bool UseDrilling(string pStrArticleCode)
        {
            return lObjArticlesDAO.UseDrilling(pStrArticleCode);
        }
    }
}
