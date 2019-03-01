using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.AddOn.Auctions.Forms;

namespace UGRS.AddOn.Auctions
{
    class Menu
    {

        public void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "Subasta";
            oCreationPackage.String = "Subasta";
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;

            oMenus = oMenuItem.SubMenus;

            try
            {
                //  If the manu already exists this code will fail
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception)
            {

            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("Subasta");
                oMenus = oMenuItem.SubMenus;

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Auctions.frmPayment";
                oCreationPackage.String = "Cobros";
                oMenus.AddEx(oCreationPackage);

                // Create Mail sender sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Auctions.Forms.frmMailSender";
                oCreationPackage.String = "Envio de correos";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {

                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Auctions.frmPayment")
                {
                    //if (!FormExists("Pymnt"))
                    //{
                        frmPayment activeForm = new frmPayment();
                        activeForm.UIAPIRawForm.Left = 500;
                        activeForm.UIAPIRawForm.Top = 10;
                        activeForm.Show();
                    //}
                }

                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Auctions.Forms.frmMailSender")
                {
                    if (!FormExists("h1nz1"))
                    {
                        frmMailSender activeForm = new frmMailSender();
                        activeForm.UIAPIRawForm.Left = 500;
                        activeForm.UIAPIRawForm.Top = 10;
                        activeForm.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

        private bool FormExists(string pStrUniqueId)
        {
            try
            {
                Application.SBO_Application.Forms.Item(pStrUniqueId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

    }
}
