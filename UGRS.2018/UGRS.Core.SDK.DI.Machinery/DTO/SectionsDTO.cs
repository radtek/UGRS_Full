using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class SectionsDTO
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public int MunicipalityCode { get; set; }
        public string MunicipalityName { get; set; }
        public double Kilometers { get; set; }
    }
}
