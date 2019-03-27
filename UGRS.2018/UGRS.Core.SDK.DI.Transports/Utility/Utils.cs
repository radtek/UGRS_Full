using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbouiCOM.Framework;

namespace UGRS.Core.SDK.DI.Transports.Utility
{
    public class Utils
    {


        public bool FormExists(string pStrFrmName)
        {
            try
            {
                Application.SBO_Application.Forms.Item(pStrFrmName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public bool ItemExist(string pStrItemName, SAPbouiCOM.Form pObjForm)
        {
            try
            {
                pObjForm.Items.Item(pStrItemName);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
