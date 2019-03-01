using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.AddOn.Finances.Forms;

namespace UGRS.AddOn.Finances
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
            oCreationPackage.UniqueID = "UGRS.AddOn.Finances";
            oCreationPackage.String = "Finanzas";
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;

            oMenus = oMenuItem.SubMenus;

            try
            {
                //  If the manu already exists this code will fail
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception e)
            {

            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("UGRS.AddOn.Finances");
                oMenus = oMenuItem.SubMenus;

                // Create sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Finances.Payments";
                oCreationPackage.String = "Recepción de pagos de clientes";
                oMenus.AddEx(oCreationPackage);

                // Create sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Finances.CheckGeneration";
                oCreationPackage.String = "Generación de pagos de subasta";
                oMenus.AddEx(oCreationPackage);

                // Create sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Finances.BouncedChecks";
                oCreationPackage.String = "Trato de cheques devueltos";
                oMenus.AddEx(oCreationPackage);

                // Create sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Finances.InvoiceTest";
                oCreationPackage.String = "Prueba de facturas";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { //  Menu already exists
                //Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Finances.Payments")
                {
                    Payments activeForm = new Payments();
                    activeForm.Show();
                }
                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Finances.CheckGeneration")
                {
                    CheckGeneration activeForm = new CheckGeneration();
                    activeForm.Show();
                }
                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Finances.BouncedChecks")
                {
                    BouncedChecks activeForm = new BouncedChecks();
                    activeForm.Show();
                }
                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Finances.InvoiceTest")
                {
                    InvoiceTest activeForm = new InvoiceTest();
                    activeForm.Show();
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

    }
}
