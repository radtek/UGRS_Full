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
using QualisysLog;
using UGRS.Core.SDK.UI;

namespace UGRS.AddOnFoodPlant.Utils {
    public class SAPException {

        public static void Handle(Exception ex, string section) {
            UIApplication.ShowMessageBox(ex.Message);
            QsLog.WriteError(String.Format("{0}: {1}", section, ex.Message));
            QsLog.WriteException(ex);
        }
    }

}
