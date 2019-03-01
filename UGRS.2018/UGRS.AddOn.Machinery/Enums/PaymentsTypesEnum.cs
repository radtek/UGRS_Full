using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.AddOn.Machinery.Enums
{
    public class PaymentsTypesEnum
    {
        /*[Description("Solicitud de viáticos")]
        TravelExpenses = 3,*/

        public class TravelExpenses : PaymentsTypesEnum
        {
            public const string Value = "GLSOV";
            public const string Description = "Solicitud de Viáticos";
        }
    }
}
