/*
 * Autor: LCC Abraham SaÚL Sandoval Meneses
 * Description: SAP B1 Exception
 * Date: 06/09/2018
 */



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.AddOnFoodTransfer.Utils {
    public class SAPException {

        public static void Handle(Exception ex, string section) {
            UIApplication.ShowMessageBox(ex.Message);
            LogService.WriteError(String.Format("{0}: {1}, {2}", section, ex.Message, ex.StackTrace));
        }
    }

}
