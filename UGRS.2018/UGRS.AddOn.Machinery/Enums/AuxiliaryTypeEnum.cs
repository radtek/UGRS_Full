using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.AddOn.Machinery.Enums
{
    public enum AuxiliaryTypeEnum : int
    {
        [Description("Socios de negocio")]
        BusinessPartners = 1,
        [Description("Empleados")]
        Employees = 2,
    }
}
