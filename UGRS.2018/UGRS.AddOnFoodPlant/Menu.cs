/*
 * Autor: LCC Abraham SaÚL Sandoval Meneses
 * Description: Transfer of Transit Inventory to Area Form
 * Date: 31/08/2018
 */


using SAPbouiCOM.Framework;
using System;
using UGRS.AddOnFoodPlant.Forms;


namespace UGRS.AddOnFoodPlant {

    class Menu {

        SAPbouiCOM.Item btnOpen;
        SAPbouiCOM.Menus oMenus = null;
        SAPbouiCOM.MenuItem oMenuItem = null;
        SAPbouiCOM.MenuCreationParams oCreationPackage = null;

        public void AddMenuItems() {

            oMenus = Application.SBO_Application.Menus;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));
            oMenuItem = Application.SBO_Application.Menus.Item("43520"); // moudles'
            //
            //SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);

            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_POPUP;
            oCreationPackage.UniqueID = "UGRS.AddOnFoodPlant";
            oCreationPackage.String = "Planta de Alimento";
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
                oMenuItem = Application.SBO_Application.Menus.Item("UGRS.AddOnFoodPlant");
                oMenus = oMenuItem.SubMenus;

                // Create s sub menus
                CreateSubMenu("UGRS.AddOnFoodPlant.Forms.frmTransfer", "Transferencias Pendientes");
                CreateSubMenu("UGRS.AddOnFoodPlant.Forms.frmItems", "Transferencia de Artículos");
                CreateSubMenu("UGRS.AddOnFoodPlant.Forms.frmProcess", "Proceso de Producción");

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
                        case "UGRS.AddOnFoodPlant.Forms.frmTransfer":
                        frmTransfer ofrmTransfer = new frmTransfer();
                        ShowForm(ofrmTransfer);

                        break;

                        case "UGRS.AddOnFoodPlant.Forms.frmItems":
                        frmItems ofrmItems = new frmItems("0");
                        ShowForm(ofrmItems);
                        break;

                        case "UGRS.AddOnFoodPlant.Forms.frmProcess":
                        frmProcess ofrmProcess = new frmProcess();
                        ShowForm(ofrmProcess);
                        break;
                    }
                }
                if(!pVal.BeforeAction) {
                    switch(pVal.MenuUID) {
                        case "3088":

                        SAPbouiCOM.Form form = Application.SBO_Application.Forms.ActiveForm;

                        btnOpen = form.Items.Add("btnOpen", SAPbouiCOM.BoFormItemTypes.it_BUTTON);
                        btnOpen.Top = form.Items.Item("2").Top;
                        btnOpen.Left = (form.Left / 2) + 150;
                        btnOpen.Width = 170;
                        (btnOpen.Specific as SAPbouiCOM.Button).Caption = "Abrir Tranferencia de Artículos";
                        btnOpen.LinkTo = "2";
                        (btnOpen.Specific as SAPbouiCOM.Button).ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.BtnOpen_ClickBefore);
                        //SetFilters();
                        break;
                    }
                }
            }
            catch(Exception ex) {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }

        }
        private void BtnOpen_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent) {
            BubbleEvent = true;

            SAPbouiCOM.Form form = Application.SBO_Application.Forms.ActiveForm;
            var docNum = ((SAPbouiCOM.EditText)form.Items.Item("11").Specific).Value;

            frmItems ofrmItems = new frmItems(docNum);
            ShowForm(ofrmItems);
        }

        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent) {
            BubbleEvent = true;

            try {

                if(FormUID.Equals(FormUID)) {

                    if(!pVal.BeforeAction) {
                        switch(pVal.EventType) {
                            case SAPbouiCOM.BoEventTypes.et_FORM_RESIZE:

                            if(!Object.ReferenceEquals(btnOpen, null)) {

                                SAPbouiCOM.Form form = Application.SBO_Application.Forms.ActiveForm;
                                btnOpen.Left = (form.Left / 2) + 150;
                                btnOpen.Top = form.Items.Item("2").Top;
                            }
                            break;
                        }
                    }
                }
            }

            catch(Exception ex) {
                if(!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }
        private void SetFilters() {

            SAPbouiCOM.EventFilters oFilters;
            SAPbouiCOM.EventFilter oFilter;
            oFilters = new SAPbouiCOM.EventFilters();
            SAPbouiCOM.Form form = Application.SBO_Application.Forms.ActiveForm;

            oFilter = oFilters.Add(SAPbouiCOM.BoEventTypes.et_FORM_RESIZE);
            oFilter.AddEx(form.TypeEx);

            oFilter = oFilters.Add(SAPbouiCOM.BoEventTypes.et_CLICK);
            oFilter.AddEx(form.TypeEx);

            oFilter = oFilters.Add(SAPbouiCOM.BoEventTypes.et_VALIDATE);
            oFilter.AddEx(form.TypeEx);

            oFilter = oFilters.Add(SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST);
            oFilter.AddEx(form.TypeEx);

            oFilter = oFilters.Add(SAPbouiCOM.BoEventTypes.et_FORM_LOAD);
            oFilter.AddEx(form.TypeEx);

            oFilter = oFilters.Add(SAPbouiCOM.BoEventTypes.et_FORM_CLOSE);
            oFilter.AddEx(form.TypeEx);

            oFilter = oFilters.Add(SAPbouiCOM.BoEventTypes.et_MENU_CLICK);
            oFilter.AddEx(form.TypeEx);

            SAPbouiCOM.Framework.Application.SBO_Application.SetFilter(oFilters);
        }

        public void CreateSubMenu(string id, string title) {
            oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
            oCreationPackage.UniqueID = id;
            oCreationPackage.String = title;
            oMenus.AddEx(oCreationPackage);
        }

        public void ShowForm(UserFormBase frm) {
            frm.UIAPIRawForm.Left = 500;
            frm.UIAPIRawForm.Top = 10;
            frm.Show();
        }
    }
}
