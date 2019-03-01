using System;
using System.Collections.Generic;
using System.Text;
using SAPbouiCOM.Framework;
using UGRS.AddOn.Permissions.Forms;

namespace UGRS.AddOn.Permissions
{
    class Menu
    {
         public string lStrTypeEx = "";
         public int lIntTypeCount = 0;

         public Menu()
         {

         }

        public void AddMenuItems()
        {

            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "UGRS.AddOn.Permissions";
            oCreationPackage.String = "Permisos";
            oCreationPackage.Enabled = true;
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
                oMenuItem = Application.SBO_Application.Menus.Item("UGRS.AddOn.Permissions");
                oMenus = oMenuItem.SubMenus;

                // Create s sub menu
                //oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                //oCreationPackage.UniqueID = "UGRS.AddOn.Permissions.Prefix";
                //oCreationPackage.String = "Prefijo de aretes";
                //oMenus.AddEx(oCreationPackage);

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Permissions.NC";
                oCreationPackage.String = "Notas de crédito";
                oMenus.AddEx(oCreationPackage);


                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Permissions.SO";
                oCreationPackage.String = "Ordenes de Venta Pendientes";
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
                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Permissions.Prefix")
                {
                    if (!FormExists(lStrTypeEx, lIntTypeCount))
                    {
                        //Prefixes activeForm = new Prefixes();
                        //activeForm.Show();
                        //lStrTypeEx = activeForm.UIAPIRawForm.TypeEx.ToString();
                        //lIntTypeCount = activeForm.UIAPIRawForm.TypeCount;

                    }
                }

                if (pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Permissions.NC")
                {
                    if (!FormExists(lStrTypeEx, lIntTypeCount))
                    {
                        FrmCreditN activeForm = new FrmCreditN();
                        activeForm.UIAPIRawForm.Left = 500;
                        activeForm.UIAPIRawForm.Top = 10;
                        activeForm.Show();
                        lStrTypeEx = activeForm.UIAPIRawForm.TypeEx.ToString();
                        lIntTypeCount = activeForm.UIAPIRawForm.TypeCount;

                    }
                }

                if(pVal.BeforeAction && pVal.MenuUID == "UGRS.AddOn.Permissions.SO") {
                    if(!FormExists(lStrTypeEx, lIntTypeCount)) {
                        FrmProcessORDR activeForm = new FrmProcessORDR();
                        activeForm.UIAPIRawForm.Left = 500;
                        activeForm.UIAPIRawForm.Top = 10;
                        activeForm.Show();
                        lStrTypeEx = activeForm.UIAPIRawForm.TypeEx.ToString();
                        lIntTypeCount = activeForm.UIAPIRawForm.TypeCount;

                    }
                }


            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

        private bool FormExists(string lStrTypex, int lIntTypeCount)
        {
            bool exist = false;
            try
            {
                var a = SAPbouiCOM.Framework.Application.SBO_Application.Forms.GetForm(lStrTypex, lIntTypeCount);
                exist = true;
            }
            catch (Exception )
            {
                exist = false;
            }

            return exist;
        }

    }
}
