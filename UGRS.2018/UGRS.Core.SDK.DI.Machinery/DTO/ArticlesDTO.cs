using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class ArticlesDTO
    {
        public int Code { get; set; }
        public string ArticleCode { get; set; }
        public string Name { get; set; }
        public string EquipmentTypeCode { get; set; }
        public string ContractTypeCode { get; set; }
        public bool UseDrilling { get; set; }
    }
}
