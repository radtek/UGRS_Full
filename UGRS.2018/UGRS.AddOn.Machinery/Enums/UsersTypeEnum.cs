using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.AddOn.Machinery.Enums
{
    public enum UsersTypeEnum : int
    {
        [Description("Maquinaria")]
        Machinery = 1,
        [Description("Gremial")]
        Gremial = 2,
    }
}
