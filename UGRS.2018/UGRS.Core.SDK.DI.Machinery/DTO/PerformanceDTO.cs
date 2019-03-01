using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class PerformanceDTO
    {
        private string code;
        public string Code
        {
            get
            {
                if (string.IsNullOrEmpty(code))
                {
                    code = string.Empty;
                }

                return code;
            }
            set
            {
                code = value;
            }
        }

        public int IdRise { get; set; }

        public string PrcCode { get; set; }

        public string EcoNum { get; set; }

        public int Type { get; set; }

        public double HrKm { get; set; }

        public double PerformanceF { get; set; }
    }
}
