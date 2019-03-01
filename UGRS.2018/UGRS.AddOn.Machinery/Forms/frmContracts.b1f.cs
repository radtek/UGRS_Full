using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UGRS.AddOn.Machinery.Enums;
using UGRS.Core.SDK.DI;
using UGRS.Core.SDK.DI.Machinery;
using UGRS.Core.SDK.DI.Machinery.DTO;
using UGRS.Core.SDK.DI.Machinery.Enums;
using UGRS.Core.SDK.UI;
using UGRS.Core.Utility;

namespace UGRS.AddOn.Machinery.Forms
{
    [FormAttribute("UGRS.AddOn.Machinery.Forms.frmContracts", "Forms/frmContracts.b1f")]
    class frmContracts : UserFormBase
    {
        #region Properties
        private MachinerySeviceFactory mObjMachinerySeviceFactory = new MachinerySeviceFactory();
        private int mIntRiseFolio;
        private string mStrClientCode = string.Empty;
        private UsersTypeEnum mEnumUserType;
        private ContractModeEnum mEnumContractMode;
        public ContractsDTO mObjCreatedContract = null;
        private List<DestinationAddressDTO> mLstDestinationAddressDTO = null;
        private string mStrMatrixSelected = string.Empty;
        private int mIntMatrixRowSelected = 0;
        public bool mBolFromMenu = false;
        #endregion

        #region Constructor
        public frmContracts()
        {
            LoadEvents();

            mLstDestinationAddressDTO = new List<DestinationAddressDTO>();

            mBolFromMenu = true;
            txtFolio.Item.Visible = false;
            lblFolio.Item.Visible = false;
            mEnumUserType = UsersTypeEnum.Machinery;
            mEnumContractMode = ContractModeEnum.Purchase;
            txtFolio.Item.Enabled = true;

            LoadInitialsControls();
        }

        public frmContracts(int pIntRiseFolio, string pStrClientCode, string pStrClientName, string pStrStartDate, string pStrEndDate, UsersTypeEnum pEnumUserType, ContractModeEnum pEnumContractMode)
        {
            LoadEvents();

            mIntRiseFolio = pIntRiseFolio;
            mEnumUserType = pEnumUserType;
            mEnumContractMode = pEnumContractMode;
            mStrClientCode = pStrClientCode;
            txtName.Value = pStrClientName;
            txtStartDate.Value = pStrStartDate;
            txtEndDate.Value = pStrEndDate;
            mLstDestinationAddressDTO = new List<DestinationAddressDTO>();

            LoadInitialsControls();
        }
        #endregion

        #region Initialize
        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.lblClient = ((SAPbouiCOM.StaticText)(this.GetItem("lblClient").Specific));
            this.lblName = ((SAPbouiCOM.StaticText)(this.GetItem("lblName").Specific));
            this.lblType = ((SAPbouiCOM.StaticText)(this.GetItem("lblType").Specific));
            this.lblConstructionType = ((SAPbouiCOM.StaticText)(this.GetItem("lblConst").Specific));
            this.lblPayment = ((SAPbouiCOM.StaticText)(this.GetItem("lblPaym").Specific));
            this.txtClient = ((SAPbouiCOM.EditText)(this.GetItem("txtClient").Specific));
            this.txtClient.KeyDownAfter += new SAPbouiCOM._IEditTextEvents_KeyDownAfterEventHandler(this.txtClient_KeyDownAfter);
            this.txtName = ((SAPbouiCOM.EditText)(this.GetItem("txtName").Specific));
            this.txtPayment = ((SAPbouiCOM.EditText)(this.GetItem("txtPaym").Specific));
            this.lblFolio = ((SAPbouiCOM.StaticText)(this.GetItem("lblFolio").Specific));
            this.lblDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblDate").Specific));
            this.txtFolio = ((SAPbouiCOM.EditText)(this.GetItem("txtFolio").Specific));
            this.txtDate = ((SAPbouiCOM.EditText)(this.GetItem("txtDate").Specific));
            this.lblStartDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblDateI").Specific));
            this.lblEndDate = ((SAPbouiCOM.StaticText)(this.GetItem("lblDateF").Specific));
            this.lblProperty = ((SAPbouiCOM.StaticText)(this.GetItem("lblProp").Specific));
            this.txtStartDate = ((SAPbouiCOM.EditText)(this.GetItem("txtDateI").Specific));
            this.txtEndDate = ((SAPbouiCOM.EditText)(this.GetItem("txtDateF").Specific));
            this.lblMunicipality = ((SAPbouiCOM.StaticText)(this.GetItem("lblMun").Specific));
            this.lblComite = ((SAPbouiCOM.StaticText)(this.GetItem("lblComite").Specific));
            this.lblTypeLine = ((SAPbouiCOM.StaticText)(this.GetItem("lblTypeL").Specific));
            this.lblArticle = ((SAPbouiCOM.StaticText)(this.GetItem("lblArt").Specific));
            this.mtxContractLines = ((SAPbouiCOM.Matrix)(this.GetItem("mtxCont").Specific));
            this.mtxContractLines.ValidateBefore += new SAPbouiCOM._IMatrixEvents_ValidateBeforeEventHandler(this.mtxContractLines_ValidateBefore);
            this.btnAdd = ((SAPbouiCOM.Button)(this.GetItem("btnAdd").Specific));
            this.lblSpecifications = ((SAPbouiCOM.StaticText)(this.GetItem("lblEspc").Specific));
            this.lblWellDiameter = ((SAPbouiCOM.StaticText)(this.GetItem("lblDiamP").Specific));
            this.txtWellDiameter = ((SAPbouiCOM.EditText)(this.GetItem("txtDiamP").Specific));
            this.lblPipeDiameter = ((SAPbouiCOM.StaticText)(this.GetItem("lblDiamT").Specific));
            this.txtPipeDiameter = ((SAPbouiCOM.EditText)(this.GetItem("txtDiamT").Specific));
            this.lblBomb = ((SAPbouiCOM.StaticText)(this.GetItem("lblBomb").Specific));
            this.txtBomb = ((SAPbouiCOM.EditText)(this.GetItem("txtBomb").Specific));
            this.lblImport = ((SAPbouiCOM.StaticText)(this.GetItem("lblImport").Specific));
            this.txtImport = ((SAPbouiCOM.EditText)(this.GetItem("txtImport").Specific));
            this.btnSave = ((SAPbouiCOM.Button)(this.GetItem("btnSaveC").Specific));
            this.cmbContractType = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbType").Specific));
            this.cmbConstructionType = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbConst").Specific));
            this.cmbMunicipality = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbMun").Specific));
            this.cmbCommittee = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbComm").Specific));
            this.cmbEqpType = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbEqpTyp").Specific));
            this.cmbArticles = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbArt").Specific));
            this.cmbProperties = ((SAPbouiCOM.ComboBox)(this.GetItem("cmbProp").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);
            this.CloseBefore += new SAPbouiCOM.Framework.FormBase.CloseBeforeHandler(this.Form_CloseBefore);

        }

        private void OnCustomInitialize()
        {
            UIAPIRawForm.EnableMenu("1293", true); //Borrar
        }
        #endregion

        #region Load & Unload Events
        private void LoadEvents()
        {
            Application.SBO_Application.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            Application.SBO_Application.RightClickEvent += new SAPbouiCOM._IApplicationEvents_RightClickEventEventHandler(this.SBO_Application_RightClickEvent_GoodIssue);
            Application.SBO_Application.MenuEvent += new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(this.SBO_Application_MenuEvent_GoodIssue);
        }

        private void UnLoadEvents()
        {
            Application.SBO_Application.ItemEvent -= new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SBO_Application_ItemEvent);
            Application.SBO_Application.RightClickEvent -= new SAPbouiCOM._IApplicationEvents_RightClickEventEventHandler(this.SBO_Application_RightClickEvent_GoodIssue);
            Application.SBO_Application.MenuEvent -= new SAPbouiCOM._IApplicationEvents_MenuEventEventHandler(this.SBO_Application_MenuEvent_GoodIssue);
        }
        #endregion

        #region Events
        private void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            //string y = pVal.CharPressed.ToString();
            try
            {
                if (FormUID.Equals(this.UIAPIRawForm.UniqueID))
                {
                    if (!pVal.BeforeAction)
                    {
                        switch (pVal.EventType)
                        {
                            case SAPbouiCOM.BoEventTypes.et_CLICK:
                                if (pVal.ItemUID.Equals("btnAdd"))
                                {
                                    AddContractLine();
                                }
                                if (pVal.ItemUID.Equals("btnSaveC"))
                                {
                                    SaveContract();
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_CHOOSE_FROM_LIST:
                                ChooseFromListAfterEvent(pVal);
                                break;
                            case SAPbouiCOM.BoEventTypes.et_COMBO_SELECT:
                                if (pVal.ItemUID.Equals("cmbMun"))
                                {
                                    LoadCommitteesCbo();
                                    txtImport.Value = string.Empty;
                                    ClearMatrix();
                                }
                                if (pVal.ItemUID.Equals("cmbType"))
                                {
                                    LoadEquipmentsTypesCbo();
                                    txtImport.Value = string.Empty;
                                    ClearMatrix();
                                }
                                if (pVal.ItemUID.Equals("cmbEqpTyp"))
                                {
                                    LoadArticlesCbo();
                                    txtImport.Value = string.Empty;
                                    ClearMatrix();
                                    BlockOrUnblockDiameterControls();
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_LOST_FOCUS:
                                if (pVal.ItemUID.Equals("mtxCont"))
                                {
                                    //if (pVal.ColUID.Equals("ColHPH") || pVal.ColUID.Equals("ColPrice"))
                                    //{
                                    //    CalculateLineTotal(pVal.Row);
                                    //}
                                    //else if (pVal.ColUID.Equals("ColTramo"))
                                    //{
                                    //    string lStrTramoSN = (mtxContractLines.Columns.Item(5).Cells.Item(pVal.Row).Specific as SAPbouiCOM.ComboBox).Value.Trim();

                                    //    if (!string.IsNullOrEmpty(lStrTramoSN))
                                    //        dtContractsLines.SetValue("ArtTramo", pVal.Row - 1, lStrTramoSN);
                                    //}
                                    //else if (pVal.ColUID.Equals("ColKM"))
                                    //{
                                    //    string lStrKM = (mtxContractLines.Columns.Item(6).Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();

                                    //    if (!string.IsNullOrEmpty(lStrKM))
                                    //        dtContractsLines.SetValue("ArtKM", pVal.Row - 1, int.Parse(lStrKM));
                                    //}
                                }
                                break;
                            case SAPbouiCOM.BoEventTypes.et_FORM_LOAD:
                                LoadInitialsControls();
                                break;
                            case SAPbouiCOM.BoEventTypes.et_FORM_CLOSE:
                                //UnLoadEvents();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(String.Format("[frmContracts - SBO_Application_ItemEvent] Error: {0}", ex.Message));

                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        private void SBO_Application_RightClickEvent_GoodIssue(ref SAPbouiCOM.ContextMenuInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmCont")
                {
                    mStrMatrixSelected = pVal.ItemUID;
                    mIntMatrixRowSelected = pVal.Row;
                }
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(String.Format("[frmContracts - SBO_Application_RightClickEvent_GoodIssue] Error: {0}", ex.Message));

                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        public void SBO_Application_MenuEvent_GoodIssue(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.MenuUID == "1293" && pVal.BeforeAction == true && UIApplication.GetApplication().Forms.ActiveForm.UniqueID == "frmCont") //Borrar
                {
                    if (SAPbouiCOM.Framework.Application.SBO_Application.MessageBox("¿Desea elimiar el registro seleccionado?", 2, "Si", "No", "") == 1)
                    {
                        if (mStrMatrixSelected == mtxContractLines.Item.UniqueID) //matrix empleados
                        {
                            dtContractsLines.Rows.Remove(mIntMatrixRowSelected - 1);

                            CalculateContractTotal();
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
                LogUtility.WriteError(String.Format("[frmContracts - SBO_Application_MenuEvent_GoodIssue] Error: {0}", ex.Message));

                if (!ex.Message.Contains("Form - Invalid Form"))
                    SAPbouiCOM.Framework.Application.SBO_Application.MessageBox(ex.Message);
            }
        }

        private void Form_CloseBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            UnLoadEvents();
        }

        private void mtxContractLines_ValidateBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (pVal.ColUID.Equals("ColHPH") || pVal.ColUID.Equals("ColPrice"))
                {
                    CalculateLineTotal(pVal.Row);
                }
                else if (pVal.ColUID.Equals("ColTramo"))
                {
                    string lStrTramoSN = (mtxContractLines.Columns.Item(5).Cells.Item(pVal.Row).Specific as SAPbouiCOM.ComboBox).Value.Trim();

                    if (!string.IsNullOrEmpty(lStrTramoSN))
                        dtContractsLines.SetValue("ArtTramo", pVal.Row - 1, lStrTramoSN);
                }
                else if (pVal.ColUID.Equals("ColKM"))
                {
                    string lStrKM = (mtxContractLines.Columns.Item(6).Cells.Item(pVal.Row).Specific as SAPbouiCOM.EditText).Value.Trim();

                    if (!string.IsNullOrEmpty(lStrKM))
                        dtContractsLines.SetValue("ArtKM", pVal.Row - 1, lStrKM);
                }

                mtxContractLines.LoadFromDataSource();
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(String.Format("[frmContracts - mtxContractLines_ValidateBefore] Error: {0}", ex.Message));
                this.UIAPIRawForm.Freeze(false);
                UIApplication.ShowError(ex.Message);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void ChooseFromListAfterEvent(SAPbouiCOM.ItemEvent pObjValEvent)
        {
            if (pObjValEvent.Action_Success)
            {
                SAPbouiCOM.IChooseFromListEvent lObjCFLEvento = (SAPbouiCOM.IChooseFromListEvent)pObjValEvent;
                if (lObjCFLEvento.SelectedObjects == null)
                    return;

                SAPbouiCOM.DataTable lObjDataTable = lObjCFLEvento.SelectedObjects;

                if (lObjDataTable.UniqueID == "CFLClient")
                {
                    mStrClientCode = Convert.ToString(lObjDataTable.GetValue(0, 0));
                    txtName.Value = lObjDataTable.GetValue(1, 0).ToString();
                    LoadDirectionsCbo();

                    this.UIAPIRawForm.DataSources.UserDataSources.Item(lObjDataTable.UniqueID).ValueEx = mStrClientCode;
                }
            }
        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            lblSpecifications.Item.Top = cmbArticles.Item.Top + cmbArticles.Item.Height + 7;
            lblWellDiameter.Item.Top = lblSpecifications.Item.Top + lblSpecifications.Item.Height + 7;
            txtWellDiameter.Item.Top = lblSpecifications.Item.Top + lblSpecifications.Item.Height + 7;
            lblPipeDiameter.Item.Top = lblWellDiameter.Item.Top + lblWellDiameter.Item.Height + 7;
            txtPipeDiameter.Item.Top = lblWellDiameter.Item.Top + lblWellDiameter.Item.Height + 7;
            lblBomb.Item.Top = lblPipeDiameter.Item.Top + lblPipeDiameter.Item.Height + 7;
            txtBomb.Item.Top = lblPipeDiameter.Item.Top + lblPipeDiameter.Item.Height + 7;
            lblImport.Item.Top = lblBomb.Item.Top + lblBomb.Item.Height + 7;
            txtImport.Item.Top = lblBomb.Item.Top + lblBomb.Item.Height + 7;

            mtxContractLines.AutoResizeColumns();
        }
        #endregion

        #region Functions
        public void LoadInitialsControls()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                txtFolio.Value = mIntRiseFolio == 0 ? string.Empty : mIntRiseFolio.ToString();
                txtDate.Value = DateTime.Now.ToString("dd/MM/yyyy");

                if (mEnumContractMode == ContractModeEnum.Purchase)
                {
                    cmbCommittee.Item.Visible = false;
                    lblComite.Item.Visible = false;
                }

                CreateContractsLinesDatatable();
                LoadChoosesFromList();
                SetCFLToTxt();
                LoadContractsTypesCbo();
                LoadConstructionTypesCbo();
                LoadMunicipalitiesCbo();
                LoadDirectionsCbo();

                if (!string.IsNullOrEmpty(mStrClientCode))
                    this.UIAPIRawForm.DataSources.UserDataSources.Item("CFLClient").ValueEx = mStrClientCode;
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmContracts - LoadInitialsControls] Error: {0}", lObjException.Message));
                Application.SBO_Application.MessageBox(string.Format("Error al cargar los controles iniciales: {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void CreateContractsLinesDatatable()
        {
            this.UIAPIRawForm.DataSources.DataTables.Add("DTContL");
            dtContractsLines = this.UIAPIRawForm.DataSources.DataTables.Item("DTContL");
            dtContractsLines.Columns.Add("#", SAPbouiCOM.BoFieldsType.ft_Integer);
            dtContractsLines.Columns.Add("ArtCode", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtContractsLines.Columns.Add("ArtDesc", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtContractsLines.Columns.Add("ArtHPH", SAPbouiCOM.BoFieldsType.ft_Price);
            dtContractsLines.Columns.Add("ArtPrice", SAPbouiCOM.BoFieldsType.ft_Price);
            dtContractsLines.Columns.Add("ArtTotal", SAPbouiCOM.BoFieldsType.ft_Price);
            dtContractsLines.Columns.Add("ArtTramo", SAPbouiCOM.BoFieldsType.ft_AlphaNumeric);
            dtContractsLines.Columns.Add("ArtKM", SAPbouiCOM.BoFieldsType.ft_Price);

            FillContractsLinesMatrix();
        }

        private void FillContractsLinesMatrix()
        {
            mtxContractLines.Columns.Item("#").DataBind.Bind("DTContL", "#");
            mtxContractLines.Columns.Item("ColDesc").DataBind.Bind("DTContL", "ArtDesc");
            mtxContractLines.Columns.Item("ColHPH").DataBind.Bind("DTContL", "ArtHPH");
            mtxContractLines.Columns.Item("ColPrice").DataBind.Bind("DTContL", "ArtPrice");
            mtxContractLines.Columns.Item("ColTotal").DataBind.Bind("DTContL", "ArtTotal");
            mtxContractLines.Columns.Item("ColTramo").DataBind.Bind("DTContL", "ArtTramo");
            mtxContractLines.Columns.Item("ColKM").DataBind.Bind("DTContL", "ArtKM");

            SAPbouiCOM.Column lObjColPrice = (SAPbouiCOM.Column)mtxContractLines.Columns.Item("ColPrice");
            SAPbouiCOM.Column lObjColKM = (SAPbouiCOM.Column)mtxContractLines.Columns.Item("ColKM");
            SAPbouiCOM.Column lObjColTramo = (SAPbouiCOM.Column)mtxContractLines.Columns.Item("ColTramo");
            if (mEnumUserType == UsersTypeEnum.Machinery)
            {
                lObjColPrice.Editable = false;
                lObjColKM.Editable = true;
                lObjColTramo.Editable = false;
            }
            else
            {
                lObjColPrice.Editable = true;
                lObjColKM.Editable = true;
                lObjColTramo.Editable = true;
            }

            mtxContractLines.AutoResizeColumns();
        }

        public void SaveContract()
        {
            try
            {
                if (ValidateSaveControls())
                {
                    Application.SBO_Application.SetStatusBarMessage("Verificar campos vacíos", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                if (dtContractsLines.Rows.Count <= 0)
                {
                    Application.SBO_Application.SetStatusBarMessage("No puede guardar el contrato sin líneas", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                int lIntValue = Application.SBO_Application.MessageBox("¿Desea guardar el contrato?", 1, "Aceptar", "Cancelar", "");

                if (lIntValue != 1)
                {
                    return;
                }

                this.UIAPIRawForm.Freeze(true);

                if (mEnumUserType == UsersTypeEnum.Machinery)
                {
                    SaveSalesOrder();
                }
                else if (mEnumUserType == UsersTypeEnum.Gremial)
                {
                    if (mEnumContractMode == ContractModeEnum.Purchase)
                    {
                        SavePurchaseOrder();
                    }
                    else
                    {
                        SaveSalesOrder();
                    }
                }
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmContracts - SaveContract] Error al guardar el contrato: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al guardar el contrato {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void SaveSalesOrder()
        {
            SAPbobsCOM.Documents lObjSalesOrder = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

            lObjSalesOrder.CardCode = mStrClientCode;
            lObjSalesOrder.DocDate = DateTime.Now;
            lObjSalesOrder.DocDueDate = DateTime.Parse(string.Format("{0}/{1}/{2}", txtEndDate.Value.Substring(0, 4), txtEndDate.Value.Substring(4, 2), txtEndDate.Value.Substring(6, 2)));
            lObjSalesOrder.TaxDate = DateTime.Parse(string.Format("{0}/{1}/{2}", txtStartDate.Value.Substring(0, 4), txtStartDate.Value.Substring(4, 2), txtStartDate.Value.Substring(6, 2)));
            lObjSalesOrder.UserFields.Fields.Item("U_MQ_TipoCont").Value = cmbContractType.Value;
            lObjSalesOrder.UserFields.Fields.Item("U_MQ_ObraType").Value = cmbConstructionType.Selected.Description;
            lObjSalesOrder.UserFields.Fields.Item("U_MQ_Rise").Value = txtFolio.Value;
            lObjSalesOrder.UserFields.Fields.Item("U_GLO_Municipality").Value = cmbMunicipality.Value;
            lObjSalesOrder.UserFields.Fields.Item("U_GR_Committe").Value = cmbCommittee.Value;
            lObjSalesOrder.UserFields.Fields.Item("U_MQ_DisPay").Value = int.Parse(txtPayment.Value.Trim());

            for (int i = 0; i < dtContractsLines.Rows.Count; i++)
            {
                lObjSalesOrder.Lines.SetCurrentLine(i);
                lObjSalesOrder.Lines.ItemCode = dtContractsLines.GetValue(1, i).ToString();
                lObjSalesOrder.Lines.Quantity = double.Parse(dtContractsLines.GetValue(3, i).ToString());
                lObjSalesOrder.Lines.UnitPrice = double.Parse(dtContractsLines.GetValue(4, i).ToString());
                lObjSalesOrder.Lines.UserFields.Fields.Item("U_GLO_Sections").Value = dtContractsLines.GetValue(6, i).ToString();
                lObjSalesOrder.Lines.UserFields.Fields.Item("U_MQ_Kilometers").Value = double.Parse(dtContractsLines.GetValue(7, i).ToString());

                lObjSalesOrder.Lines.Add();
            }

            if (lObjSalesOrder.Add() == 0)
            {
                int lIntDocEntry = int.Parse(DIApplication.Company.GetNewObjectKey());

                LogUtility.WriteSuccess(String.Format("[frmContracts - SaveSalesOrder] Orden de venta creada correctamente con el DocEntry {0} para la Subida: {1}", lIntDocEntry, txtFolio.Value));

                mObjCreatedContract = mObjMachinerySeviceFactory.GetContractsService().GetContract(lIntDocEntry);

                if (mBolFromMenu)
                {
                    CleanControls();
                    UIApplication.ShowSuccess(string.Format("Se creó correctamente el contrato {0}", mObjCreatedContract.DocNum));
                }
                else
                {
                    this.UIAPIRawForm.Close();
                }
            }
            else
            {
                if (DIApplication.Company.GetLastErrorCode() != 0)
                {
                    throw new Exception(string.Format("Error al generar el contrato: {0}", DIApplication.Company.GetLastErrorDescription()));
                }
            }
        }

        private void SavePurchaseOrder()
        {
            SAPbobsCOM.Documents lObjPurchaseOrder = (SAPbobsCOM.Documents)DIApplication.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);

            lObjPurchaseOrder.CardCode = mStrClientCode;
            lObjPurchaseOrder.DocDate = DateTime.Now;
            lObjPurchaseOrder.DocDueDate = DateTime.Now.AddDays(1);
            lObjPurchaseOrder.UserFields.Fields.Item("U_MQ_TipoCont").Value = cmbContractType.Value;
            lObjPurchaseOrder.UserFields.Fields.Item("U_MQ_ObraType").Value = cmbConstructionType.Value;
            lObjPurchaseOrder.UserFields.Fields.Item("U_MQ_Rise").Value = txtFolio.Value;
            lObjPurchaseOrder.UserFields.Fields.Item("U_GLO_Municipality").Value = cmbMunicipality.Value;
            lObjPurchaseOrder.UserFields.Fields.Item("U_GR_Committe").Value = cmbCommittee.Value;

            for (int i = 0; i < dtContractsLines.Rows.Count; i++)
            {
                lObjPurchaseOrder.Lines.SetCurrentLine(i);
                lObjPurchaseOrder.Lines.ItemCode = dtContractsLines.GetValue(1, i).ToString();
                lObjPurchaseOrder.Lines.Quantity = double.Parse(dtContractsLines.GetValue(3, i).ToString());
                lObjPurchaseOrder.Lines.UnitPrice = double.Parse(dtContractsLines.GetValue(4, i).ToString());
                lObjPurchaseOrder.Lines.UserFields.Fields.Item("U_GLO_Sections").Value = dtContractsLines.GetValue(6, i).ToString();
                lObjPurchaseOrder.Lines.UserFields.Fields.Item("U_MQ_Kilometers").Value = int.Parse(dtContractsLines.GetValue(7, i).ToString());

                lObjPurchaseOrder.Lines.Add();
            }

            if (lObjPurchaseOrder.Add() == 0)
            {
                int lIntDocEntry = int.Parse(DIApplication.Company.GetNewObjectKey());

                LogUtility.WriteSuccess(String.Format("[frmContracts - SavePurchaseOrder] Orden de compra creada correctamente con el DocEntry {0} para la Subida: {1}", lIntDocEntry, txtFolio.Value));

                /*mObjCreatedContract = mObjMachinerySeviceFactory.GetContractsService().GetContract(lIntDocEntry);

                this.UIAPIRawForm.Close();*/
            }
            else
            {
                if (DIApplication.Company.GetLastErrorCode() != 0)
                {
                    throw new Exception(string.Format("Error al generar el contrato: {0}", DIApplication.Company.GetLastErrorDescription()));
                }
            }
        }

        public void AddContractLine()
        {
            try
            {
                if (ValidateLinesControls())
                {
                    Application.SBO_Application.SetStatusBarMessage("Verificar campos vacíos", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                    return;
                }

                this.UIAPIRawForm.Freeze(true);

                dtContractsLines.Rows.Add();
                dtContractsLines.SetValue("#", dtContractsLines.Rows.Count - 1, dtContractsLines.Rows.Count);
                dtContractsLines.SetValue("ArtCode", dtContractsLines.Rows.Count - 1, cmbArticles.Selected.Value);
                dtContractsLines.SetValue("ArtDesc", dtContractsLines.Rows.Count - 1, cmbArticles.Selected.Description);
                dtContractsLines.SetValue("ArtHPH", dtContractsLines.Rows.Count - 1, 0);
                dtContractsLines.SetValue("ArtTotal", dtContractsLines.Rows.Count - 1, 0);
                dtContractsLines.SetValue("ArtTramo", dtContractsLines.Rows.Count - 1, "");
                dtContractsLines.SetValue("ArtKM", dtContractsLines.Rows.Count - 1, 0);
                if (mEnumUserType == UsersTypeEnum.Machinery) //Ventas
                {
                    dtContractsLines.SetValue("ArtPrice", dtContractsLines.Rows.Count - 1, mObjMachinerySeviceFactory.GetArticlesService().GetArticlePriceByCode(cmbArticles.Value));
                }
                else //Compras
                {
                    AddCboTramoSN();
                }

                mtxContractLines.LoadFromDataSource();
                mtxContractLines.AutoResizeColumns();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmContracts - AddContractLine] Error: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al agregar la línea del contrato {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void AddCboTramoSN()
        {
            SAPbouiCOM.Column lObjColumn = (SAPbouiCOM.Column)mtxContractLines.Columns.Item("ColTramo");
            SAPbouiCOM.Cells lObjCells = lObjColumn.Cells;

            if (cmbContractType.Value == "2") //Caminos (tramos)
            {
                List<SectionsDTO> lLstSectionsDTO = mObjMachinerySeviceFactory.GetSectionsService().GetSections(int.Parse(cmbMunicipality.Value));

                CleanColumnItems(lObjColumn);

                foreach (var lObjSection in lLstSectionsDTO)
                {
                    lObjColumn.ValidValues.Add(lObjSection.Code.ToString(), lObjSection.Name);
                }

                lObjColumn.DisplayDesc = true;
            }
            else if (cmbContractType.Value == "3") //Conaza (clientes)
            {
                List<ClientsDTO> lLstClientsDTO = mObjMachinerySeviceFactory.GetClientsService().GetClients();

                CleanColumnItems(lObjColumn);

                foreach (var lObjClient in lLstClientsDTO)
                {
                    lObjColumn.ValidValues.Add(lObjClient.CardCode, lObjClient.CardName);
                }

                lObjColumn.DisplayDesc = true;
            }
        }

        private void CalculateLineTotal(int pIntRow)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                string lStrHPH = (mtxContractLines.Columns.Item(2).Cells.Item(pIntRow).Specific as SAPbouiCOM.EditText).Value.Trim();
                string lStrPrice = (mtxContractLines.Columns.Item(3).Cells.Item(pIntRow).Specific as SAPbouiCOM.EditText).Value.Trim();

                if (string.IsNullOrEmpty(lStrHPH) || string.IsNullOrEmpty(lStrPrice))
                {
                    return;
                }

                double lDblLineTotal = double.Parse(lStrHPH) * double.Parse(lStrPrice);

                dtContractsLines.SetValue("ArtHPH", pIntRow - 1, lStrHPH);
                dtContractsLines.SetValue("ArtPrice", pIntRow - 1, lStrPrice);
                dtContractsLines.SetValue("ArtTotal", pIntRow - 1, lDblLineTotal);

                //(mtxContractLines.Columns.Item(4).Cells.Item(pIntRow).Specific as SAPbouiCOM.EditText).Value = lDblLineTotal.ToString();

                mtxContractLines.LoadFromDataSource();

                CalculateContractTotal();
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmContracts - CalculateLineTotal] Error al calcular el total de la línea: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al calcular el total de la línea {0}", lObjException.Message));
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void CalculateContractTotal()
        {
            double lDblContractTotal = 0; //string.IsNullOrEmpty(txtImport.Value) ? 0 : double.Parse(txtImport.Value.Substring(1));
            for (int i = 0; i < dtContractsLines.Rows.Count; i++)
            {
                string lStrLineTotal = dtContractsLines.GetValue(5, i).ToString();

                lDblContractTotal = lDblContractTotal + double.Parse(lStrLineTotal);
            }

            if (cmbEqpType.Value == "7") //Perforadora
            {
                string lStrBPP = txtBomb.Value;

                if (!string.IsNullOrEmpty(lStrBPP))
                {
                    lDblContractTotal = lDblContractTotal + double.Parse(lStrBPP);
                }
            }

            txtImport.Value = lDblContractTotal.ToString("C");
        }

        private void BlockOrUnblockDiameterControls()
        {
            if (!string.IsNullOrEmpty(cmbEqpType.Value))
            {
                if (mObjMachinerySeviceFactory.GetArticlesService().UseDrilling(cmbEqpType.Value))
                {
                    txtWellDiameter.Item.Enabled = true;
                    txtPipeDiameter.Item.Enabled = true;
                    txtBomb.Item.Enabled = true;

                    //Mostrar valor de bomba de tabla configuraciones
                    txtBomb.Value = mObjMachinerySeviceFactory.GetConfigurationsService().GetConfigurationByName(ConfigurationsEnum.BombPrice).Value;
                }
                else
                {
                    txtWellDiameter.Item.Enabled = false;
                    txtPipeDiameter.Item.Enabled = false;
                    txtBomb.Item.Enabled = false;

                    txtBomb.Value = string.Empty;
                }
                /*if (cmbEqpType.Value == "7") //Perforadora
                {
                    txtWellDiameter.Item.Enabled = true;
                    txtPipeDiameter.Item.Enabled = true;
                    txtBomb.Item.Enabled = true;

                    //Mostrar valor de bomba de tabla configuraciones
                    txtBomb.Value = mObjMachinerySeviceFactory.GetConfigurationsService().GetConfigurationByName(ConfigurationsEnum.BombPrice).Value;
                }
                else
                {
                    txtWellDiameter.Item.Enabled = false;
                    txtPipeDiameter.Item.Enabled = false;
                    txtBomb.Item.Enabled = false;

                    txtBomb.Value = string.Empty;
                }*/
            }
        }

        public void CleanControls()
        {
            ClearMatrix();
            //txtFolio.Value = "";
            txtPipeDiameter.Value = string.Empty;
            txtWellDiameter.Value = string.Empty;
            txtBomb.Value = string.Empty;
            txtImport.Value = string.Empty;
            txtPayment.Value = string.Empty;
        }

        public bool ValidateSaveControls()
        {
            bool lBolEmpty = false;

            if (string.IsNullOrEmpty(txtClient.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(cmbContractType.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(cmbConstructionType.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(txtPayment.Value))
            {
                lBolEmpty = true;
            }

            if (!string.IsNullOrEmpty(txtPayment.Value))
            {
                int n;
                if (!int.TryParse(txtPayment.Value, out n))
                {
                    throw new Exception("Solo puede ingresar valores numéricos en la forma de pago");
                }
                else
                {
                    if (n < 0)
                    {
                        throw new Exception("La forma de pago debe ser mayor a 0");
                    }
                }
            }

            if (!mBolFromMenu)
            {
                if (string.IsNullOrEmpty(txtFolio.Value))
                {
                    lBolEmpty = true;
                }
            }

            //if (mBolFromMenu)
            //{
            //    if (!string.IsNullOrEmpty(txtFolio.Value))
            //    {
            //        int n;
            //        if (!int.TryParse(txtFolio.Value, out n))
            //        {
            //            throw new Exception("Solo puede ingresar valores numéricos en el folio de la subida");
            //        }
            //        else
            //        {
            //            if (n <= 0)
            //            {
            //                throw new Exception("El folio de la subida debe ser mayor a 0");
            //            }
            //        }
            //    }
            //}

            if (string.IsNullOrEmpty(txtDate.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(txtStartDate.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(txtEndDate.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(cmbProperties.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(cmbMunicipality.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(cmbCommittee.Value) && cmbCommittee.Item.Visible)
            {
                lBolEmpty = true;
            }

            return lBolEmpty;
        }

        public bool ValidateLinesControls()
        {
            bool lBolEmpty = false;

            if (string.IsNullOrEmpty(cmbEqpType.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(cmbArticles.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(cmbContractType.Value))
            {
                lBolEmpty = true;
            }

            if (string.IsNullOrEmpty(cmbMunicipality.Value))
            {
                lBolEmpty = true;
            }

            return lBolEmpty;
        }

        public void LoadContractsTypesCbo()
        {
            try
            {
                List<ContractsTypesDTO> lLstContractsTypes = (mEnumUserType == UsersTypeEnum.Machinery)
                                                            ? mObjMachinerySeviceFactory.GetContractsService().GetContractsTypes().Where(x => x.Code == 1).ToList()
                                                            : mObjMachinerySeviceFactory.GetContractsService().GetContractsTypes().Where(x => x.Code != 1).ToList();

                foreach (var lObjContractType in lLstContractsTypes)
                {
                    cmbContractType.ValidValues.Add(lObjContractType.Code.ToString(), lObjContractType.Description);
                }

                SelectFirstValueCbo(cmbContractType);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmContracts - LoadContractsTypesCbo] Error: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al cargar el listado de tipos de contratos {0}", lObjException.Message));
            }
        }

        public void LoadConstructionTypesCbo()
        {
            try
            {
                List<ConstructionTypeDTO> lLstConstructionTypesDTO = mObjMachinerySeviceFactory.GetConstructionService().GetConstructionTypes();

                foreach (var lObjConstructionType in lLstConstructionTypesDTO)
                {
                    cmbConstructionType.ValidValues.Add(lObjConstructionType.Code.ToString(), lObjConstructionType.Name);
                }

                SelectFirstValueCbo(cmbConstructionType);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmContracts - LoadConstructionTypesCbo] Error: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al cargar el listado de tipos de construcción {0}", lObjException.Message));
            }
        }

        public void LoadMunicipalitiesCbo()
        {
            try
            {
                List<MunicipalitiesDTO> lLstMunicipalitiesDTO = mObjMachinerySeviceFactory.GetMunicipalitiesService().GetMunicipalities();

                foreach (var lObjMunicipality in lLstMunicipalitiesDTO)
                {
                    cmbMunicipality.ValidValues.Add(lObjMunicipality.Code.ToString(), lObjMunicipality.Name);
                }

                SelectFirstValueCbo(cmbMunicipality);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmContracts - LoadMunicipalitiesCbo] Error: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al cargar el listado de municipios {0}", lObjException.Message));
            }
        }

        public void LoadDirectionsCbo()
        {
            try
            {
                //if (mLstDestinationAddressDTO.Count == 0)
                mLstDestinationAddressDTO = mObjMachinerySeviceFactory.GetAddressService().GetDestinationAddressClient();

                if (string.IsNullOrEmpty(mStrClientCode))
                    return;

                CleanCboItems(cmbProperties);

                //mLstDestinationAddressDTO = mLstDestinationAddressDTO.Where(x => x.CardCode == mStrClientCode).ToList();

                foreach (var lObjDestinationAddress in mLstDestinationAddressDTO.Where(x => x.CardCode == mStrClientCode).ToList())
                {
                    cmbProperties.ValidValues.Add(lObjDestinationAddress.Address, lObjDestinationAddress.Address);
                }

                SelectFirstValueCbo(cmbProperties);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmContracts - LoadDirectionsCbo] Error: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al cargar el listado de propiedades {0}", lObjException.Message));
            }
        }

        public void LoadCommitteesCbo()
        {
            try
            {
                CleanCboItems(cmbCommittee);

                int lIntMunicipalityCode = int.Parse(cmbMunicipality.Value);
                List<CommitteesDTO> lLstCommitteesDTO = mObjMachinerySeviceFactory.GetCommitteesServices().GetCommitteesByMunCode(lIntMunicipalityCode);

                foreach (var lObjCommittee in lLstCommitteesDTO)
                {
                    cmbCommittee.ValidValues.Add(lObjCommittee.Code.ToString(), lObjCommittee.Name);
                }

                SelectFirstValueCbo(cmbCommittee);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmContracts - LoadCommitteesCbo] Error: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al cargar el listado de municipios {0}", lObjException.Message));
            }
        }

        public void LoadEquipmentsTypesCbo()
        {
            try
            {
                CleanCboItems(cmbEqpType);

                int lIntContractTypeCode = int.Parse(cmbContractType.Value);
                List<EquipmentDTO> lLstEquipmentDTO = mObjMachinerySeviceFactory.GetEquipmentsService().GetEquipmentTypesByContract(lIntContractTypeCode);

                foreach (var lObjEquipment in lLstEquipmentDTO)
                {
                    cmbEqpType.ValidValues.Add(lObjEquipment.TypeId, lObjEquipment.Name);
                }

                SelectFirstValueCbo(cmbEqpType);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmContracts - LoadEquipmentsTypesCbo] Error: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al cargar el listado de tipos de equipos {0}", lObjException.Message));
            }
        }

        public void LoadArticlesCbo()
        {
            try
            {
                CleanCboItems(cmbArticles);

                string lStrEquipmentTypeCode = cmbEqpType.Value;
                string lStrContractTypeCode = cmbContractType.Value;
                List<ArticlesDTO> lLstArticlesDTO = mObjMachinerySeviceFactory.GetArticlesService().GetArticlesByEquipmentType(lStrEquipmentTypeCode, lStrContractTypeCode);

                foreach (var lObjArticlesDTO in lLstArticlesDTO)
                {
                    if (!ExistValidValue(lObjArticlesDTO.ArticleCode))
                    {
                        cmbArticles.ValidValues.Add(lObjArticlesDTO.ArticleCode.ToString(), lObjArticlesDTO.Name);
                    }
                }

                SelectFirstValueCbo(cmbArticles);
            }
            catch (Exception lObjException)
            {
                LogUtility.WriteError(String.Format("[frmContracts - LoadArticlesCbo] Error: {0}", lObjException.Message));
                throw new Exception(string.Format("Error al cargar el listado de articulos {0}", lObjException.Message));
            }
        }

        public bool ExistValidValue(string pStrParam)
        {
            bool lBolResult = false;

            for (int i = 0; i < cmbArticles.ValidValues.Count; i++)
            {
                string lStrValidValue = cmbArticles.ValidValues.Item(i).Value;

                if (pStrParam == lStrValidValue)
                {
                    lBolResult = true;
                }
            }

            return lBolResult;
        }

        public void SelectFirstValueCbo(SAPbouiCOM.ComboBox pObjCbo)
        {
            pObjCbo.Item.DisplayDesc = true;

            if (pObjCbo.ValidValues.Count > 0)
                pObjCbo.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
        }

        public void CleanCboItems(SAPbouiCOM.ComboBox pObjCbo)
        {
            if (pObjCbo.ValidValues.Count > 0)
            {
                foreach (var item in pObjCbo.ValidValues)
                {
                    pObjCbo.ValidValues.Remove(pObjCbo.ValidValues.Count - 1, SAPbouiCOM.BoSearchKey.psk_Index);
                }

                pObjCbo.Item.Description = string.Empty;
                if (string.IsNullOrEmpty(pObjCbo.Value))
                {
                    pObjCbo.ValidValues.Add(string.Empty, string.Empty);
                }
                else
                {
                    pObjCbo.ValidValues.Add(string.Empty, string.Empty);
                    SelectFirstValueCbo(pObjCbo);
                    pObjCbo.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
                }
            }
        }

        public void CleanColumnItems(SAPbouiCOM.Column pObjCol)
        {
            if (pObjCol.ValidValues.Count > 0)
            {
                foreach (var item in pObjCol.ValidValues)
                {
                    pObjCol.ValidValues.Remove(pObjCol.ValidValues.Count - 1, SAPbouiCOM.BoSearchKey.psk_Index);
                }
            }
        }

        private void ClearMatrix()
        {
            if (!this.UIAPIRawForm.DataSources.DataTables.Item("DTContL").IsEmpty)
            {
                this.UIAPIRawForm.DataSources.DataTables.Item("DTContL").Rows.Clear();
                mtxContractLines.Clear();
            }
        }

        private void SetCFLToTxt()
        {
            txtClient.DataBind.SetBound(true, "", "CFLClient");
            txtClient.ChooseFromListUID = "CFLClient";
        }

        private void LoadChoosesFromList()
        {
            SAPbouiCOM.ChooseFromList lObjCFLClients = InitChooseFromLists(false, "2", "CFLClient", this.UIAPIRawForm.ChooseFromLists);
            AddConditionClientCFL(lObjCFLClients);
        }

        public SAPbouiCOM.ChooseFromList InitChooseFromLists(bool pbol, string pStrType, string pStrID, SAPbouiCOM.ChooseFromListCollection pObjCFLs) //
        {
            SAPbouiCOM.ChooseFromList lObjoCFL = null;
            try
            {
                SAPbouiCOM.ChooseFromListCreationParams oCFLCreationParams = null;
                oCFLCreationParams = (SAPbouiCOM.ChooseFromListCreationParams)UIApplication.GetApplication().CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_ChooseFromListCreationParams);

                oCFLCreationParams.MultiSelection = pbol;
                oCFLCreationParams.ObjectType = pStrType;
                oCFLCreationParams.UniqueID = pStrID;

                lObjoCFL = pObjCFLs.Add(oCFLCreationParams);

                this.UIAPIRawForm.DataSources.UserDataSources.Add(pStrID, SAPbouiCOM.BoDataType.dt_SHORT_TEXT, 254);
            }
            catch (Exception ex)
            {
                LogUtility.WriteError(String.Format("[frmContracts - InitChooseFromLists] Error: {0}", ex.Message));
                UIApplication.ShowMessageBox(string.Format("InitCustomerChooseFromListException: {0}", ex.Message));

            }
            return lObjoCFL;
        }

        private void AddConditionClientCFL(SAPbouiCOM.ChooseFromList pCFL)
        {
            SAPbouiCOM.Condition lObjCon = null;
            SAPbouiCOM.Conditions lObjCons = new SAPbouiCOM.Conditions();

            lObjCon = lObjCons.Add();
            lObjCon.Alias = "CardType";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;

            if (mEnumUserType == UsersTypeEnum.Machinery)
            {
                lObjCon.CondVal = "C";
            }
            else if (mEnumUserType == UsersTypeEnum.Gremial)
            {
                if (mEnumContractMode == ContractModeEnum.Purchase)
                {
                    lObjCon.CondVal = "C";
                }
                else
                {
                    lObjCon.CondVal = "S";
                }
            }

            lObjCon.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;

            lObjCon = lObjCons.Add();
            lObjCon.Alias = "validFor";
            lObjCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            lObjCon.CondVal = "Y";

            pCFL.SetConditions(lObjCons);
        }
        #endregion

        #region Controls
        private SAPbouiCOM.StaticText lblClient;
        private SAPbouiCOM.StaticText lblName;
        private SAPbouiCOM.StaticText lblType;
        private SAPbouiCOM.StaticText lblConstructionType;
        private SAPbouiCOM.StaticText lblPayment;
        private SAPbouiCOM.EditText txtClient;
        private SAPbouiCOM.EditText txtName;
        private SAPbouiCOM.EditText txtPayment;
        private SAPbouiCOM.StaticText lblFolio;
        private SAPbouiCOM.StaticText lblDate;
        private SAPbouiCOM.EditText txtFolio;
        private SAPbouiCOM.EditText txtDate;
        private SAPbouiCOM.StaticText lblStartDate;
        private SAPbouiCOM.StaticText lblEndDate;
        private SAPbouiCOM.StaticText lblProperty;
        private SAPbouiCOM.EditText txtStartDate;
        private SAPbouiCOM.EditText txtEndDate;
        private SAPbouiCOM.StaticText lblMunicipality;
        private SAPbouiCOM.StaticText lblComite;
        private SAPbouiCOM.StaticText lblTypeLine;
        private SAPbouiCOM.StaticText lblArticle;
        private SAPbouiCOM.Matrix mtxContractLines;
        private SAPbouiCOM.Button btnAdd;
        private SAPbouiCOM.StaticText lblSpecifications;
        private SAPbouiCOM.StaticText lblWellDiameter;
        private SAPbouiCOM.EditText txtWellDiameter;
        private SAPbouiCOM.StaticText lblPipeDiameter;
        private SAPbouiCOM.EditText txtPipeDiameter;
        private SAPbouiCOM.StaticText lblBomb;
        private SAPbouiCOM.EditText txtBomb;
        private SAPbouiCOM.StaticText lblImport;
        private SAPbouiCOM.EditText txtImport;
        private SAPbouiCOM.Button btnSave;
        private SAPbouiCOM.ComboBox cmbContractType;
        private SAPbouiCOM.ComboBox cmbConstructionType;
        private SAPbouiCOM.ComboBox cmbMunicipality;
        private SAPbouiCOM.ComboBox cmbCommittee;
        private SAPbouiCOM.ComboBox cmbEqpType;
        private SAPbouiCOM.ComboBox cmbArticles;
        private SAPbouiCOM.DataTable dtContractsLines;
        private SAPbouiCOM.ComboBox cmbProperties;
        #endregion

        private void txtClient_KeyDownAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            throw new System.NotImplementedException();

        }
    }
}
