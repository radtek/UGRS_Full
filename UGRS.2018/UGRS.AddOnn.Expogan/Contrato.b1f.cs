using System;
using System.Collections.Generic;
using System.Xml;
using SAPbouiCOM;
using SAPbouiCOM.Framework;
using System.Globalization;
using System.Linq;
using System.Threading;
using UGRS.Core.SDK.UI;
using UGRS.Core.Services;
using UGRS.Core.Utility;
using UGRS.Core.SDK.DI.Expogan;
using UGRS.Core.SDK.DI.Expogan.DTO;
using UGRS.Core.SDK.DI.Expogan.Tables;
using UGRS.Core.SDK.DI;


namespace UGRS.AddOnn.Expogan
{
    [FormAttribute("UGRS.AddOnn.Expogan.Form1", "Contrato.b1f")]
    class Contrato : UserFormBase
    {
        //Colocarlo como varialbe global
        #region properties
        ExpoganServiceFactory mObjExpoganService = new ExpoganServiceFactory();

        int mIntRowSelected = 0;
        #endregion

        #region Contruct
        public Contrato()
        {
           
        }
        #endregion

        #region Initialize
        public override void OnInitializeComponent()
        {
            this.btnAdd = ((SAPbouiCOM.Button)(this.GetItem("btnAdd").Specific));
            this.btnAdd.ClickBefore += new SAPbouiCOM._IButtonEvents_ClickBeforeEventHandler(this.btnAdd_ClickBefore);
            this.lblBP = ((SAPbouiCOM.StaticText)(this.GetItem("lblBP").Specific));
            this.txtClient = ((SAPbouiCOM.EditText)(this.GetItem("txtClient").Specific));
            this.lblLevel = ((SAPbouiCOM.StaticText)(this.GetItem("lblLevel").Specific));
            this.cboNivel = ((SAPbouiCOM.ComboBox)(this.GetItem("cboNivel").Specific));
            this.cboNivel.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cboNivel_ComboSelectAfter);
            this.lblLocal = ((SAPbouiCOM.StaticText)(this.GetItem("lblLocal").Specific));
            this.cboLocal = ((SAPbouiCOM.ComboBox)(this.GetItem("cboLocal").Specific));
            this.mtxLocales = ((SAPbouiCOM.Matrix)(this.GetItem("mtxLocales").Specific));
            this.mtxLocales.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtxLocales_ValidateAfter);
            this.mtxLocales.ClickAfter += new SAPbouiCOM._IMatrixEvents_ClickAfterEventHandler(this.mtxLocales_ClickAfter);
            this.lblTotal = ((SAPbouiCOM.StaticText)(this.GetItem("lblTotal").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("btnCreateO").Specific));
            this.Button1.ClickAfter += new SAPbouiCOM._IButtonEvents_ClickAfterEventHandler(this.btnCreateO_ClickAfter);
            //     typeof(SAPbouiCOM.Framework.Application).SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(this.SBO_Application_ItemEvent);
            SAPbouiCOM.Framework.Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(this.SBO_Application_ItemEvent);
            SAPbouiCOM.Framework.Application.SBO_Application.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(this.SBO_Application_MenuEvent);
            this.txtBPName = ((SAPbouiCOM.EditText)(this.GetItem("txtBP_Name").Specific));
            this.txtTotal = ((SAPbouiCOM.EditText)(this.GetItem("txtTotal").Specific));
            this.lblNameBP = ((SAPbouiCOM.StaticText)(this.GetItem("lblNameBP").Specific));
            this.OnCustomInitialize();

        }

        private void SBO_Application_MenuEvent(ref MenuEvent pVal, out bool BubbleEvent)
        {
           BubbleEvent = true;
           try
           {

               if (pVal.MenuUID == "1293" && pVal.BeforeAction == true)//Borrar
               {
                   if (SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("¿Desea elimiar el item seleccionado?", 2, "Si", "No", "") == 1)
                   {
                       try
                       {
                           this.UIAPIRawForm.Freeze(true);
                            SAPbouiCOM.DataTable DtMatrixLocales = this.UIAPIRawForm.DataSources.DataTables.Item("DTLocales");
                            DtMatrixLocales.Rows.Remove(mIntRowSelected - 1);
                           txtTotal.Value = SumTotalPayment().ToString();
                           UIAPIRawForm.EnableMenu("8801", false);
                       }
                       catch (Exception ex)
                       {
                           this.UIAPIRawForm.Freeze(false);
                           LogService.WriteError("(SBO_Application_MenuEvent 1293 true): " + ex.Message);
                           LogService.WriteError(ex);
                       }
                       finally
                       {
                           this.UIAPIRawForm.Freeze(false);
                       }
                   }
                   else
                   {
                       BubbleEvent = false;
                   }
               }
           }
           catch (Exception ex)
           {
               LogService.WriteError("(SBO_Application_MenuEvent): " + ex.Message);
               LogService.WriteError(ex);
           }
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new ResizeAfterHandler(this.Form_ResizeAfter);

        }

        private void OnCustomInitialize()
        {
            try
            {
                //GetNewContractId();
                LoadChoseFromList();
                AddComboNivel();
                SetChooseToTxt();
                cboLocal.Item.Enabled = false;
                txtTotal.Item.Enabled = false;
                UIAPIRawForm.EnableMenu("1293", true);//Borrar
            }
            catch (Exception ex)
            {
                LogService.WriteError("(OnCustomInitialize): " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        #endregion
       
        #region Events
        private void SBO_Application_ItemEvent(string FormUID, ref ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                if (FormUID.Equals(this.UIAPIRawForm.UniqueID))
                {
                    if (pVal.EventType == BoEventTypes.et_FORM_CLOSE)
                    {
                        UIApplication.GetApplication().ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(this.SBO_Application_ItemEvent);

                    }

                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case BoEventTypes.et_CHOOSE_FROM_LIST:
                                ChooseFromListAfterEvent(pVal);
                                break;
                        }
                    }
                    else
                    {
                        switch (pVal.EventType)
                        {
                            case BoEventTypes.et_CHOOSE_FROM_LIST:
                                // ChooseFromListBeforeEvent(pVal);
                                break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                UIApplication.ShowError(string.Format("ItemEventException: {0}", ex.Message));
                LogService.WriteError("(SBO_Application_ItemEvent): " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void ChooseFromListAfterEvent(ItemEvent pObjValEvent)
        {
            try
            {
                if (pObjValEvent.Action_Success)
                {
                    SAPbouiCOM.IChooseFromListEvent lObjCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pObjValEvent;
                    if (lObjCFLEvento.SelectedObjects != null)
                    {
                        SAPbouiCOM.DataTable lObjDataTable = lObjCFLEvento.SelectedObjects;

                        if (lObjDataTable != null)
                        {

                            switch (lObjDataTable.UniqueID)
                            {

                                case "CFL_BP":
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = System.Convert.ToString(lObjDataTable.GetValue(0, 0));
                                    txtBPName.Item.Enabled = false;
                                    txtBPName.Value = System.Convert.ToString(lObjDataTable.GetValue(1, 0));

                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.WriteError("(ChooseFromListAfterEvent): " + ex.Message);
                LogService.WriteError(ex);
            }
        }

        private void btnAdd_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
                try
                {
                    this.UIAPIRawForm.Freeze(true);
                    if (ValidateFields())
                    {

                        SetDatatableValuesLocales();
                        BindMatrixLocales();
                        txtTotal.Value = SumTotalPayment().ToString();
                        FillLocations();
                    }
                }
                catch (Exception ex)
                {
                    UIApplication.ShowError("frm (btnAdd_ClickBefore) " + ex.Message);
                    LogService.WriteError("frm (btnAdd_ClickBefore) " + ex.Message);
                    LogService.WriteError(ex);

                }
                finally
                {
                    this.UIAPIRawForm.Freeze(false);
                }
         
        }

        private void cboNivel_ComboSelectAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                FillLocations();
            }
            catch (Exception ex)
            {

                LogService.WriteError("(cboNivel_ComboSelectAfter): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }


        }

        private void btnCreateO_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            bool lBolSuccess = false;
           
            string lStrDocEntry = string.Empty;
            if (ValidateCreationDocument())
            {
                try
                {
                    string lStrNewFolioID = GetNewContractId();
                    DIApplication.Company.StartTransaction();
                    lStrDocEntry = mObjExpoganService.GetPurchaseOrderService().CreateDocument(txtClient.Value, SumTotalPayment(), lStrNewFolioID);
                    if (!string.IsNullOrEmpty(lStrDocEntry))
                    {
                        lBolSuccess = SaveLocals(lStrDocEntry, lStrNewFolioID);
                    }

                }
                catch (Exception ex)
                {
                    lBolSuccess = false;
                    LogService.WriteError("(btnCreateO_ClickAfter): " + ex.Message);
                    LogService.WriteError(ex);
                }
                finally
                {
                    CloseTransaction(lBolSuccess, lStrDocEntry);

                }
            }
        }

        private void Form_ResizeAfter(SBOItemEventArg pVal)
        {
            mtxLocales.AutoResizeColumns();

        }

        private void mtxLocales_ClickAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                if (pVal.Row > 0)
                {
                    mtxLocales.SelectRow(pVal.Row, true, false);
                    mIntRowSelected = pVal.Row;
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowError(ex.Message);
                LogService.WriteError("(SBO_Application_ItemEvent): " + ex.Message);
                LogService.WriteError(ex);
            }


        }

        #endregion

        #region Methods

        #region SaveData
        private void CloseTransaction(bool pBolSuccess, string pStrDocEntry)
        {
            try
            {

                if (pBolSuccess)
                {
                    DIApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                    ClearControls();
                    UIApplication.ShowMessageBox(string.Format("Documento realizado correctamente"));
                    SAPbouiCOM.Framework.Application.SBO_Application.OpenForm((SAPbouiCOM.BoFormObjectEnum)17, "", pStrDocEntry);

                }
                else
                {
                    if (DIApplication.Company.InTransaction)
                    {
                        DIApplication.Company.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                    }
                }
            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(ex.Message);
                LogService.WriteError("(btnSave_ClickBefore): " + ex.Message);
                LogService.WriteError(ex);
            }
           
        }

        private string GetNewContractId()
        {

            string lStrNewFolio = "0";
            lStrNewFolio = mObjExpoganService.GetLocationService().GetLastContractId();
            if (string.IsNullOrEmpty(lStrNewFolio))
            {
                lStrNewFolio = "0";
            }
            else
            {
                lStrNewFolio = lStrNewFolio.Substring(9, lStrNewFolio.Length - 9);
            }
            return "EX_LOCAL_" + (Convert.ToInt32(lStrNewFolio) + 1);
        }

        private bool SaveLocals(string pStrDocEntry, string pStrNewFolioID)
        {
            bool lBolResult = true;
            List<Locations> lLstLocations = ReadDataTable(pStrDocEntry, pStrNewFolioID);
            foreach (Locations lObjLocations in lLstLocations)
            {
                int lIntResult = mObjExpoganService.GetLocationTableService().Add(lObjLocations);
                if (lIntResult != 0)
                {
                    lBolResult = false;
                }
            }
            
            return lBolResult;
        }
        #endregion

        #region Combobox
        private void AddComboNivel()
        {
            CleanComboBox(cboNivel);
            List<LevelDTO> Values = new List<LevelDTO>();
            Values = mObjExpoganService.GetLocationService().GetLevels().ToList();
            cboNivel.ValidValues.Add("", "");

            foreach (LevelDTO lStrIdRise in Values)
            {
                cboNivel.ValidValues.Add(lStrIdRise.IdLevel, lStrIdRise.Name);
            }
            cboNivel.Select(0, BoSearchKey.psk_Index);
            cboNivel.ExpandType = BoExpandType.et_ValueOnly;
        }

        private void FillLocations()
        {
            CleanComboBox(cboLocal);
            cboLocal.ValidValues.Add("", "");
            if (cboNivel.Value == "")
            {
                cboLocal.Item.Enabled = false;
            }
            else
            {
                cboLocal.Item.Enabled = true;
                ExpoganServiceFactory mObjExpoganService = new ExpoganServiceFactory();
                List<LocationDTO> lLstValues = new List<LocationDTO>();
                lLstValues = mObjExpoganService.GetLocationService().GetLocations(cboNivel.Value).ToList();
                List<Locations> lLstMatriz = ReadDataTable("", "");
                lLstValues = lLstValues.Where(x => !lLstMatriz.Any(y => y.LocalID == x.IdLocation)).ToList();
                foreach (LocationDTO lStrIdRise in lLstValues)
                {
                    //cboLocal.ValidValues.Add(lStrIdRise, "");
                    cboLocal.ValidValues.Add(lStrIdRise.IdLocation, lStrIdRise.Name);
                }
            }
            cboLocal.Select(0, BoSearchKey.psk_Index);
            cboLocal.ExpandType = BoExpandType.et_DescriptionOnly;
        }

        public static void CleanComboBox(dynamic oComboBox)
        {
            int i = 0;
            while (oComboBox.ValidValues.Count > 0)
            {
                oComboBox.ValidValues.Remove(i, SAPbouiCOM.BoSearchKey.psk_Index);
            }
        }
        #endregion

        #region Matrix
        private void SetDatatableValuesLocales()
        {

            ExpoganServiceFactory mObjExpoganService = new ExpoganServiceFactory();
            SAPbouiCOM.DataTable DtMatrixLocales = this.UIAPIRawForm.DataSources.DataTables.Item("DTLocales");
            //DtMatrixLocales.Rows.Clear();
            DtMatrixLocales.Rows.Add();
            int i = DtMatrixLocales.Rows.Count - 1;


            DtMatrixLocales.SetValue("#", i, DtMatrixLocales.Rows.Count);
            DtMatrixLocales.SetValue("Nivel", i, cboNivel.Value);
            string location_name = cboLocal.Selected.Description; // mObjExpoganService.GetLocationService().GetLocationName(cboLocal.Value).ToString();
            DtMatrixLocales.SetValue("Local", i, location_name);
            double total = mObjExpoganService.GetLocationService().GetCost(cboLocal.Value);
            DtMatrixLocales.SetValue("Importe", i, total);
            DtMatrixLocales.SetValue("LocalId", i, cboLocal.Selected.Value);
        }

        private void BindMatrixLocales()
        {
            mtxLocales.Columns.Item("#").DataBind.Bind("DTLocales", "#");
            mtxLocales.Columns.Item("Nivel").DataBind.Bind("DTLocales", "Nivel");
            mtxLocales.Columns.Item("Local").DataBind.Bind("DTLocales", "Local");
            mtxLocales.Columns.Item("Importe").DataBind.Bind("DTLocales", "Importe");
            mtxLocales.LoadFromDataSource();
            mtxLocales.AutoResizeColumns();
        }

        private double SumTotalPayment()
        {
            double lDblTotal = 0;
            SAPbouiCOM.DataTable DtMatrixLocales = this.UIAPIRawForm.DataSources.DataTables.Item("DTLocales");
            for (int i = 0; i < DtMatrixLocales.Rows.Count; i++)
            {
                lDblTotal += Convert.ToDouble(DtMatrixLocales.GetValue("Importe", i));
            }
            return lDblTotal;
        }

        //private void UpdateDatatableValuesLocales()
        //{
        //    try
        //    {
        //        int row = mtxLocales.GetCellFocus().rowIndex;
        //        if (Convert.ToString(((SAPbouiCOM.EditText)mtxLocales.Columns.Item("Importe").Cells.Item(row).Specific).Value) != string.Empty)
        //        {
        //        SAPbouiCOM.DataTable DtMatrixLocales = this.UIAPIRawForm.DataSources.DataTables.Item("DTLocales");
        //        DtMatrixLocales.Rows.Add();               
        //        DtMatrixLocales.SetValue("Importe", row, Convert.ToDouble(((SAPbouiCOM.EditText)mtxLocales.Columns.Item("Importe").Cells.Item(row).Specific).Value));
        //        txtTotal.Value = "0.00";
        //        txtTotal.Value = SumTotalPayment().ToString();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        UIApplication.ShowMessageBox(ex.Message);
        //        LogService.WriteError("(UpdateDatatableValuesLocales): " + ex.Message);
        //        LogService.WriteError(ex);
        //    }

        //}

        private List<Locations> ReadDataTable(string pStrDocEntry, string pStrContract)
        {
            List<Locations> lLstLocations = new List<Locations>();
            SAPbouiCOM.DataTable DtMatrixLocales = this.UIAPIRawForm.DataSources.DataTables.Item("DTLocales");
            for (int i = 0; i < DtMatrixLocales.Rows.Count; i++)
            {
                Locations lObjLocations = new Locations();
                lObjLocations.DocEntryO = pStrDocEntry;
                lObjLocations.ContractID = pStrContract;
                lObjLocations.LocalID = DtMatrixLocales.GetValue("LocalId", i).ToString();
                lObjLocations.Status = 0;
                lObjLocations.RowName = DtMatrixLocales.GetValue("Importe", i).ToString();

                lLstLocations.Add(lObjLocations);
            }


            return lLstLocations;
        }
        #endregion

        #region Validations
        private bool ValidateCreationDocument()
        {
            string lStrMessage = string.Empty;
            if (string.IsNullOrEmpty(txtClient.Value))
            {
                lStrMessage += " \n Socio de negocio";
            }
            SAPbouiCOM.DataTable DtMatrixLocales = this.UIAPIRawForm.DataSources.DataTables.Item("DTLocales");
            if (DtMatrixLocales.Rows.Count == 0)
            {
                lStrMessage += " \n Registros de locales";
            }

            if (!string.IsNullOrEmpty(lStrMessage))
            {
                UIApplication.ShowMessageBox("Favor de completar los siguientes campos: " + lStrMessage);
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool ValidateFields()
        {
            string lStrMessage = string.Empty;
            if (string.IsNullOrEmpty(cboNivel.Value))
            {
                lStrMessage += " \n Nivel";
            }
            if (string.IsNullOrEmpty(cboLocal.Value))
            {
                lStrMessage += " \n Local";
            }
            if (string.IsNullOrEmpty(txtClient.Value))
            {
                lStrMessage += " \n Socio de necocio";
                }
            if (!string.IsNullOrEmpty(lStrMessage))
            {
                UIApplication.ShowMessageBox("Favor de completar los siguientes campos: " + lStrMessage);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ClearControls()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                mIntRowSelected = 0;
                txtClient.Value = string.Empty;
                txtBPName.Value = string.Empty;
                txtTotal.Value = "0";
                AddComboNivel();
                FillLocations();
                SAPbouiCOM.DataTable DtMatrixLocales = this.UIAPIRawForm.DataSources.DataTables.Item("DTLocales");
                DtMatrixLocales.Rows.Clear();
                mtxLocales.LoadFromDataSource();
                
            }
            catch (Exception ex)
            {

                LogService.WriteError("(ClearControls): " + ex.Message);
                LogService.WriteError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        #endregion

        #region ChooseFromLists
        private void LoadChoseFromList()
        {
            ChooseFromList lObjCFLBP = InitChooseFromLists(false, "2", "CFL_BP", this.UIAPIRawForm.ChooseFromLists);
            AddConditionBP(lObjCFLBP);
        }

        private void SetChooseToTxt()
        {
            txtClient.DataBind.SetBound(true, "", "CFL_BP");
            txtClient.ChooseFromListUID = "CFL_BP";
            txtClient.ChooseFromListAlias = "CardCode";
        }

        public ChooseFromList InitChooseFromLists(bool pbolMultiselecction, string pStrType, string pStrID, SAPbouiCOM.ChooseFromListCollection pObjCFLs) //
        {
            SAPbouiCOM.ChooseFromList lObjoCFL = null;
            try
            {
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = (SAPbouiCOM.ChooseFromListCreationParams)UIApplication.GetApplication().CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams);

                oCFLCreationParams.MultiSelection = pbolMultiselecction;
                oCFLCreationParams.ObjectType = pStrType;
                oCFLCreationParams.UniqueID = pStrID;

                lObjoCFL = pObjCFLs.Add(oCFLCreationParams);

                this.UIAPIRawForm.DataSources.UserDataSources.Add(pStrID, SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 254);

            }
            catch (Exception ex)
            {
                UIApplication.ShowMessageBox(string.Format("InitCustomerChooseFromListException: {0}", ex.Message));
                LogService.WriteError("(InitChooseFromLists): " + ex.Message);
                LogService.WriteError(ex);
            }
            return lObjoCFL;
        }
        #endregion

        #region ConditionChooseFromLists
        private void AddConditionBP(ChooseFromList pCFL)
        {
            SAPbouiCOM.Condition lObjCon = null;
            SAPbouiCOM.Conditions lObjCons = null;
            lObjCons = pCFL.GetConditions();

            //DimCode
            lObjCon = lObjCons.Add();
            lObjCon.Alias = "CardType";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "C";

            pCFL.SetConditions(lObjCons);
        }

       
        #endregion

        #endregion

        #region Controls
        private SAPbouiCOM.Button btnAdd;
        private SAPbouiCOM.StaticText lblBP;
        private SAPbouiCOM.StaticText lblLevel;
        private SAPbouiCOM.ComboBox cboNivel;
        private SAPbouiCOM.StaticText lblLocal;
        private SAPbouiCOM.ComboBox cboLocal;
        private SAPbouiCOM.Matrix mtxLocales;
        private SAPbouiCOM.StaticText lblTotal;
        private SAPbouiCOM.EditText txtClient;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.EditText txtBPName;
        private SAPbouiCOM.EditText txtTotal;
        private StaticText lblNameBP;
        #endregion

        private void mtxLocales_ValidateAfter(object sboObject, SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);
                UpdateDatatable();
            }
            catch (Exception ex )
            {
                LogService.WriteError("(mtxLocales_ValidateAfter): " + ex.Message);
                        LogService.WriteError(ex);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
           
        }

        private void UpdateDatatable()
        {
            SAPbouiCOM.DataTable DtMatrixLocales = this.UIAPIRawForm.DataSources.DataTables.Item("DTLocales");
            double lDblNewValue = Convert.ToDouble((mtxLocales.Columns.Item("Importe").Cells.Item(mIntRowSelected).Specific as EditText).Value.Trim());

            DtMatrixLocales.SetValue("Importe", mIntRowSelected - 1, lDblNewValue);
            mtxLocales.LoadFromDataSource();
            txtTotal.Value = SumTotalPayment().ToString();
        }
     
       
    }
}