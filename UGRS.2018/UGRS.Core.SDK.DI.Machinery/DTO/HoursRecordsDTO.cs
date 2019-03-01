using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class HoursRecordsDTO
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

        public int ContractEntry { get; set; }

        public int ContractDocNum { get; set; }

        private string contractClient;
        public string ContractClient
        {
            get
            {
                if (contractClient == null)
                {
                    contractClient = string.Empty;
                }

                return contractClient;
            }
            set
            {
                contractClient = value;
            }
        }

        public int IdRise { get; set; }

        public DateTime DateHour { get; set; }

        public int SupervisorId { get; set; }

        private string supervisor;
        public string Supervisor 
        {
            get
            { 
                if(supervisor == null)
                {
                    supervisor = string.Empty;
                }

                return supervisor;
            }
            set
            {
                supervisor = value;
            }
        }

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

        public double HrFeet { get; set; }

        public int SectionId { get; set; }

        private string section;
        public string Section
        {
            get
            {
                if (section == null)
                {
                    section = string.Empty;
                }

                return section;
            }
            set
            {
                section = value;
            }
        }

        public double KmHt { get; set; }

        public double Pending { get; set; }

        private string close;
        public string Close
        {
            get
            {
                if (close == null)
                {
                    close = string.Empty;
                }

                return close;
            }
            set
            {
                close = value;
            }
        }
    }
}
