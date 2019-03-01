using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.AddOn.Machinery.Enums
{
    public enum EmployeesTypesEnum : int
    {
        [Description("O")]
        Operators = 1,
        [Description("S")]
        Supervisors = 2,
    }
}
