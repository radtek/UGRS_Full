using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.AddOn.Machinery.Forms;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;

namespace UGRS.AddOn.Machinery
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
            oCreationPackage.UniqueID = "UGRS.AddOn.Machinery";
            oCreationPackage.String = "Maquinaria";
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
                LogService.WriteError(string.Format("[Menu - AddMenuItems: {0}]", e.Message));
            }

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenuItem = Application.SBO_Application.Menus.Item("UGRS.AddOn.Machinery");
                oMenus = oMenuItem.SubMenus;

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Machinery.Forms.MachineryForm";
                oCreationPackage.String = "Subidas";
                oMenus.AddEx(oCreationPackage);

                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Machinery.Forms.frmContracts";
                oCreationPackage.String = "Contratos";
                oMenus.AddEx(oCreationPackage);

                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Machinery.Forms.frmRiseSearch";
                oCreationPackage.String = "Búsqueda de contratos-subidas";
                oMenus.AddEx(oCreationPackage);

                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Machinery.Forms.frmRisesCommissions";
                oCreationPackage.String = "Comisiones maquinaria";
                oMenus.AddEx(oCreationPackage);

                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Machinery.Forms.frmStockTransfer";
                oCreationPackage.String = "Entrada de consumibles";
                oMenus.AddEx(oCreationPackage);
            }
            catch (Exception er)
            { //  Menu already exists
                LogService.WriteError(string.Format("[Menu - AddMenuItems: {0}]", er.Message));
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Machinery.Forms.MachineryForm")
                {
                    MachineryForm lObjMachineryForm = new MachineryForm();
                    lObjMachineryForm.Show();
                }

                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Machinery.Forms.frmRiseSearch")
                {
                    frmRiseSearch lObjFrmRiseSearch = new frmRiseSearch();
                    lObjFrmRiseSearch.Show();
                }

                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Machinery.Forms.frmRisesCommissions")
                {
                    frmRisesCommissions lObjfrmRisesCommissions = new frmRisesCommissions();
                    lObjfrmRisesCommissions.Show();
                }

                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Machinery.Forms.frmStockTransfer")
                {
                    frmStockTransfer lObjfrmStockTransfer = new frmStockTransfer();
                    lObjfrmStockTransfer.Show();
                }

                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Machinery.Forms.frmContracts")
                {
                    frmContracts lObjFrmContracts = new frmContracts();
                    lObjFrmContracts.Show();
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError(string.Format("[Menu - SBO_Application_MenuEvent: {0}]", ex.Message));

                if (ex.Message.Contains("Failed to create form. Please check the form attributes"))
                {
                    if (pVal.MenuUID == "UGRS.AddOn.Machinery.Forms.MachineryForm")
                    {
                        UIApplication.GetApplication().Forms.Item("frmRise").Close();
                    }
                    else if (pVal.MenuUID == "UGRS.AddOn.Machinery.Forms.frmRiseSearch")
                    {
                        UIApplication.GetApplication().Forms.Item("frmRSch").Close();
                    }
                    else if (pVal.MenuUID == "UGRS.AddOn.Machinery.Forms.frmRisesCommissions")
                    {
                        UIApplication.GetApplication().Forms.Item("frmRCom").Close();
                    }
                    else if (pVal.MenuUID == "UGRS.AddOn.Machinery.Forms.frmStockTransfer")
                    {
                        UIApplication.GetApplication().Forms.Item("frmStkTrn").Close();
                    }
                    else if (pVal.MenuUID == "UGRS.AddOn.Machinery.Forms.frmContracts")
                    {
                        UIApplication.GetApplication().Forms.Item("frmCont").Close();
                    }
                }
                else
                {
                    Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
                }
            }
        }

    }
}
