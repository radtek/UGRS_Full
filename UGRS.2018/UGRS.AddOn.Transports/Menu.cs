using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Transports.DTO;
using UGRS.AddOn.Transports.Forms;

namespace UGRS.AddOn.Transports
{
    class Menu
    {
        FreightsParamsDTO mObjFreightsParams = null;

        public void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;


            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            //string lstrPath = PathUtilities.GetCurrent("Icon\\Trans.bmp");

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "Transportes";
            oCreationPackage.String = "Transportes";
            oCreationPackage.Enabled = true;
            //oCreationPackage.Image = lstrPath;
            oCreationPackage.Position = -1;

            oMenus = oMenuItem.SubMenus;
            try
            {
                //  If the manu already exists this code will fail
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception )
            {

            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("Transportes");
                oMenus = oMenuItem.SubMenus;

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Transports.Forms.frmFreights";
                oCreationPackage.String = "Fletes";
                oMenus.AddEx(oCreationPackage);


                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Transports.Forms.frmCommissions";
                oCreationPackage.String = "Comisiones";
                oMenus.AddEx(oCreationPackage);

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Transports.Forms.frmDriversCommissions";
                oCreationPackage.String = "Pago choferes";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception )
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Transports.Forms.frmFreights")
                {
                    if (!FormExists("freights"))
                    {
                        SetInternalParameters();
                        Forms.frmFreights lObjActiveForm = new Forms.frmFreights(mObjFreightsParams);
                        lObjActiveForm.UIAPIRawForm.Left = 500;
                        lObjActiveForm.UIAPIRawForm.Top = 10;
                        lObjActiveForm.Show();
                    }
                }

                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Transports.Forms.frmCommissions")
                {
                    if (!FormExists("cmsns"))
                    {
                        Forms.frmCommissions lObjActiveForm = new Forms.frmCommissions();
                        lObjActiveForm.UIAPIRawForm.Left = 500;
                        lObjActiveForm.UIAPIRawForm.Top = 10;
                        lObjActiveForm.Show();
                    }
                }

                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Transports.Forms.frmDriversCommissions")
                {
                    //SetInternalParameters();
                    frmDriversCommissions lObjActiveForm = new frmDriversCommissions();
                    lObjActiveForm.UIAPIRawForm.Left = 500;
                    lObjActiveForm.UIAPIRawForm.Top = 10;
                    lObjActiveForm.Show();
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

        private void SetInternalParameters()
        {
            mObjFreightsParams = new FreightsParamsDTO();

            mObjFreightsParams.UserSign = 0;
            mObjFreightsParams.FormType = 0;
            mObjFreightsParams.Insurance = false;
            mObjFreightsParams.Internal = true;
            mObjFreightsParams.Loaded = false;
            mObjFreightsParams.SalesOrderLines = null;
            mObjFreightsParams.CardCode = string.Empty;
        }

        private bool FormExists(string pStrUniqueId)
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

    }
}
