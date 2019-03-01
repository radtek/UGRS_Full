﻿using SAPbouiCOM.Framework;
using System;
using UGRS.AddOn.Corrals.Forms;


namespace UGRS.AddOn.Corrals {

    class Menu {

        public void AddMenuItems() {
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "UGRS.AddOn.Corrals";
            oCreationPackage.String = "Corrales";
            oCreationPackage.Enabled = true;
            oCreationPackage.Position = -1;

            oMenus = oMenuItem.SubMenus;

            try {
                //  If the manu already exists this code will fail
                oMenus.AddEx(oCreationPackage);
            }
            catch {

            }

            try {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("UGRS.AddOn.Corrals");
                oMenus = oMenuItem.SubMenus;

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Corrals.frmDelivery";
                oCreationPackage.String = "Distribucíon de Alimento";
                oMenus.AddEx(oCreationPackage);

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Corrals.frmMassBilling";
                oCreationPackage.String = "Facturación Masiva";
                oMenus.AddEx(oCreationPackage);

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Corrals.frmTransfer";
                oCreationPackage.String = "Translado Corrales Subasta";
                oMenus.AddEx(oCreationPackage);


            }
            catch { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent) {
            BubbleEvent = true;

            try {
                if(pVal.BeforeAction) {
                    switch(pVal.MenuUID) {
                        case "UGRS.AddOn.Corrals.frmDelivery":
                        frmDelivery lObjfrmDelivery = new frmDelivery();
                        lObjfrmDelivery.UIAPIRawForm.Left = 500;
                        lObjfrmDelivery.UIAPIRawForm.Top = 10;
                        lObjfrmDelivery.Show();
                        break;

                        case "UGRS.AddOn.Corrals.frmMassBilling":
                        frmMassBilling lObjfrmMassBilling = new frmMassBilling();
                        lObjfrmMassBilling.UIAPIRawForm.Left = 500;
                        lObjfrmMassBilling.UIAPIRawForm.Top = 10;
                        lObjfrmMassBilling.Show();
                        break;

                        case "UGRS.AddOn.Corrals.frmTransfer":
                        frmTransfer lObjfrmTransfer = new frmTransfer();
                        lObjfrmTransfer.UIAPIRawForm.Left = 500;
                        lObjfrmTransfer.UIAPIRawForm.Top = 10;
                        lObjfrmTransfer.Show();
                        break;
                    }

                }
            }
            catch(Exception ex) {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }

        }
    }
}
