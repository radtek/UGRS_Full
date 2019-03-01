using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class TransitHoursRecordsDTO
    {
        private string code;
        public string Code
        {
            get
            {
                if (code == null)
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

        private string prcCode;
        public string PrcCode
        {
            get
            {
                if (prcCode == null)
                {
                    prcCode = string.Empty;
                }

                return prcCode;
            }
            set
            {
                prcCode = value;
            }
        }

        private string ecoNum;
        public string EcoNum
        {
            get
            {
                if (ecoNum == null)
                {
                    ecoNum = string.Empty;
                }

                return ecoNum;
            }
            set
            {
                ecoNum = value;
            }
        }

        public double Hrs { get; set; }

        public int OperatorId { get; set; }

        private string operatorName;
        public string OperatorName
        {
            get
            {
                if (operatorName == null)
                {
                    operatorName = string.Empty;
                }

                return operatorName;
            }
            set
            {
                operatorName = value;
            }
        }

        public int SupervisorId { get; set; }
        private string supervisorName;
        public string SupervisorName {
            get {
                if(supervisorName == null) {
                    supervisorName = string.Empty;
                }

                return supervisorName;
            }
            set {
                supervisorName = value;
            }
        }
    }
}
