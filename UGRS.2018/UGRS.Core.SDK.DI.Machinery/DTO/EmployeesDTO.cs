using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.Machinery.DTO
{
    public class EmployeesDTO
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
        public int Id { get; set; }
        public string Employee { get; set; }

        private string status;
        public string Status
        {
            get
            {
                if(status == null)
                {
                    status = "N";
                }

                return status;
            }
            set
            {
                status = value;
            }
        }
    }
}
