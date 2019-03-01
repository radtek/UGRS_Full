using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.Core.SDK.DI.CyC.DTO
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string UserCode { get; set; }
        public string Department { get; set; }
        public string DepartmentName { get; set; }
        public string CostigCode { get; set; }
        public char CYC { get; set; }
    }
}
