using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.Core.Extension.Enum;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class TravelExpensesDTO
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
        public DateTime DocDate { get; set; }
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string Folio { get; set; }
        public double Total { get; set; }

        private int status;
        public int Status
        {
            get
            {
                return status;
            }
            set
            {
                status = value;
            }
        }

        public string StatusDescription
        {
            get
            {
                TravelExpStatusEnum lEnumStatus = (TravelExpStatusEnum)status;
                return lEnumStatus.GetDescription();
            }
        }
    }
}
