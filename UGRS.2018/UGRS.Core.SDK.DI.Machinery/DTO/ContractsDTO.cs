using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.Core.Extension.Enum;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class ContractsDTO
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

        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public int RiseId { get; set; }
        public string CardName { get; set; }
        public DateTime DocDate { get; set; }
        public double Import { get; set; }

        private string status;
        public string Status
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

        private int type;
        public int Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        public string TypeDescription 
        { 
            get 
            {
                ContractsTypeEnum lEnumType = (ContractsTypeEnum)type;
                return lEnumType.GetDescription();
            }
        }

        public string StatusDescription
        {
            get
            {
                string lStrStatusDesc = string.Empty;
                switch (status)
                {
                    case "O":
                        lStrStatusDesc = "Abierto";
                        break;
                    case "C":
                        lStrStatusDesc = "Cerrado";
                        break;
                    case "A":
                        lStrStatusDesc = "Por Autorizar";
                        break;
                    case "Cancelled":
                        lStrStatusDesc = "Cancelado";
                        break;
                    default:
                        lStrStatusDesc = "Unk";
                        break;
                }
                //ContractsStatusEnum lEnumStatus = (ContractsStatusEnum)status;
                return lStrStatusDesc;
            }
        }
    }
}
