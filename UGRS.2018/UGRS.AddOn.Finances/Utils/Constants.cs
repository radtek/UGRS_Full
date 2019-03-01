using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UGRS.AddOn.Finances.Utils
{
    public class Constants
    {
        // Form IDs
        public static string STR_AR_INVOICE_FORM = "133"; // Factura de deudores
        public static string STR_ADVANCE_FORM = "65300"; // Factura de anticipo de clientes
        public static string STR_RESERVE_FORM = "60091"; // Factura de reserva de clientes
        public static string STR_CREDIT_MEMO_FORM = "179"; // Nota de crédito de clientes
        public static string STR_REFERENCE_FORM = "54002007"; // Información de referencia

        public static string STR_EXTERNAL_STATEMENT_FORM = "385"; // Tratar estados de cuenta externos

        // Menu entries
        public static string STR_CREDIT_MEMO_MENU = "2055"; // Nota de crédito de clientes

        // Config Entries
        public static string STR_CONFIG_TABLE = "[@UG_CONFIG]";
        public static string STR_ENTRY_BONUS = "GLO_BONIFICACION";
        public static string STR_ENTRY_COMISSION_2 = "GLO_CREDTARJ2";
        public static string STR_ENTRY_COMISSION_15 = "GLO_CREDTARJ15";
    }
}
