using SAPbouiCOM.Framework;
using System;

namespace UGRS.Core.SDK.DI.Transports.Utility
{
    public class Utils
    {


        public bool FormExists(string pStrUniqueId)
        {
            try
            {
                if (Application.SBO_Application.Forms.Item(pStrUniqueId).Visible == true)
                {
                    return true;
                }
                else
                {
                    Application.SBO_Application.Forms.Item(pStrUniqueId).Close();
                    return false;
                }
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
