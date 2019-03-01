using System;
using SAPbouiCOM.Framework;
using UGRS.AddOn.Purchases.Forms;
using UGRS.Core.SDK.DI.Purchases.Enums;
using UGRS.Core.SDK.DI.Purchases;
using UGRS.Core.SDK.DI;

namespace UGRS.AddOn.Purchases
{
    class Menu
    {
        PurchasesServiceFactory mObjPurchasesServiceFactory = new PurchasesServiceFactory();
        public void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "UGRS.AddOn.Purchases";
            oCreationPackage.String = "Compras";
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
                oMenuItem = Application.SBO_Application.Menus.Item("UGRS.AddOn.Purchases");
                oMenus = oMenuItem.SubMenus;

                //// Create s sub menu
                //oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                //oCreationPackage.UniqueID = "UGRS.AddOn.Purchases.Form1";
                //oCreationPackage.String = "Notas";
                //oMenus.AddEx(oCreationPackage);

                //// Create s sub menu
                //oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                //oCreationPackage.UniqueID = "UGRS.AddOn.Purchases.frmPurchaseXML";
                //oCreationPackage.String = "XML";
                //oMenus.AddEx(oCreationPackage);

                // Create s sub menu

                DIApplication.DIConnect((SAPbobsCOM.Company)Application.SBO_Application.Company.GetDICompany());
                string lStrCostCenter = mObjPurchasesServiceFactory.GetPurchaseInvoiceService().GetCostCenter();
               /* if (mObjPurchasesServiceFactory.GetPurchasePermissionsService().GetPermissionType(lStrCostCenter, "U_GLO_Refund") != PermissionsEnum.Permission.None)
                {*/
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "UGRS.AddOn.Purchases.frmReceipts";
                    oCreationPackage.String = "Captura de comprobantes";
                    oMenus.AddEx(oCreationPackage);

                    // Create s sub menu
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "UGRS.AddOn.Purchases.frmSearchRefunds";
                    oCreationPackage.String = "Búsqueda de reembolsos";
                    oMenus.AddEx(oCreationPackage);
               /* }


                if (mObjPurchasesServiceFactory.GetPurchasePermissionsService().GetPermissionType(lStrCostCenter, "U_GLO_ExpeCheck") != PermissionsEnum.Permission.None)
                {*/

                    // Create s sub menu
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "UGRS.AddOn.Purchases.frmCheeckingCosts";
                    oCreationPackage.String = "Comprobación de gastos";
                    oMenus.AddEx(oCreationPackage);
               // }

                // Create s sub menu
                oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                oCreationPackage.UniqueID = "UGRS.AddOn.Purchases.frmXML";
                oCreationPackage.String = "Carga de xml";
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
                if (pVal.BeforeAction)
                {
                    switch (pVal.MenuUID)
                    {
                        //case "UGRS.AddOn.Purchases.Form1":
                        //    frmPurchaseNotes lObjfrmPurchaseNote = new frmPurchaseNotes(new "001", "CR_CORRA", "6010030002000", TypeEnum.Type.Refund, "2050060000000", "", true, "", "");
                        //    lObjfrmPurchaseNote.UIAPIRawForm.Left = 500;
                        //    lObjfrmPurchaseNote.UIAPIRawForm.Top = 10;
                        //    lObjfrmPurchaseNote.Show();
                        //    break;

                        //case "UGRS.AddOn.Purchases.frmPurchaseXML":
                        //    frmPurchaseXML lObjfrmPurchaseXML = new frmPurchaseXML("01", "CR_CORRA", "", "01", "", new Core.SDK.DI.Purchases.Tables.Vouchers());
                        //    lObjfrmPurchaseXML.UIAPIRawForm.Left = 500;
                        //    lObjfrmPurchaseXML.UIAPIRawForm.Top = 10;
                        //    lObjfrmPurchaseXML.Show();
                        //    break;

                        case "UGRS.AddOn.Purchases.frmReceipts":
                            frmReceipts lObjfrmReceipts = new frmReceipts(TypeEnum.Type.Refund);
                            lObjfrmReceipts.UIAPIRawForm.Left = 500;
                            lObjfrmReceipts.UIAPIRawForm.Top = 10;
                            lObjfrmReceipts.Show();
                            break;

                        case "UGRS.AddOn.Purchases.frmCheeckingCosts":
                            frmCheeckingCosts lObjfrmCheeckingCost = new frmCheeckingCosts();
                            lObjfrmCheeckingCost.UIAPIRawForm.Left = 500;
                            lObjfrmCheeckingCost.UIAPIRawForm.Top = 10;
                            lObjfrmCheeckingCost.Show();
                            break;

                        case "UGRS.AddOn.Purchases.frmSearchRefunds":
                            frmSearchRefunds lObjfrmSearchRefounds = new frmSearchRefunds();
                            lObjfrmSearchRefounds.UIAPIRawForm.Left = 500;
                            lObjfrmSearchRefounds.UIAPIRawForm.Top = 10;
                            lObjfrmSearchRefounds.Show();
                            break;

                        case "UGRS.AddOn.Purchases.frmXML":
                            frmPurchaseXML lObjfrmPurchaseXML = new frmPurchaseXML();
                            lObjfrmPurchaseXML.UIAPIRawForm.Left = 500;
                            lObjfrmPurchaseXML.UIAPIRawForm.Top = 10;
                            lObjfrmPurchaseXML.Show();
                            break;

                    }

                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }




    }
}
